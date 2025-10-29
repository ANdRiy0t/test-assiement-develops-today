using Test.AssiementDevelopsToday.Jobs.Configuration;
using Test.AssiementDevelopsToday.Jobs.Entities;
using Test.AssiementDevelopsToday.Jobs.Helpers;
using Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

namespace Test.AssiementDevelopsToday.Jobs.Commands
{
    public interface IProcessCsvFileCommand
    {
        Task ExecuteAsync();
    }

    public class ProcessCsvFileCommand : IProcessCsvFileCommand
    {
        private readonly int _maxParallelWorkers = 10;
        private readonly int _batchSize = 10000;

        private readonly ICsvReaderService _csvReaderService;
        private readonly ITripProcessorService _tripProcessorService;
        private readonly IDuplicateDetectorService _duplicateDetectorService;
        private readonly ISqlBulkWriterService _sqlBulkWriterService;
        private readonly ICsvWriterService _csvWriterService;
        
        private readonly string[] _headers = {
            "pickup_datetime_utc",
            "dropoff_datetime_utc",
            "passenger_count",
            "trip_distance",
            "store_and_fwd_flag",
            "PULocationID",
            "DOLocationID",
            "fare_amount",
            "tip_amount"
        };
        
        public ProcessCsvFileCommand(
            ICsvReaderService csvReaderService,
            ITripProcessorService tripProcessorService,
            IDuplicateDetectorService duplicateDetectorService,
            ISqlBulkWriterService sqlBulkWriterService,
            ICsvWriterService csvWriterService)
        {
            _csvReaderService = csvReaderService;
            _tripProcessorService = tripProcessorService;
            _duplicateDetectorService = duplicateDetectorService;
            _sqlBulkWriterService = sqlBulkWriterService;
            _csvWriterService = csvWriterService;
        }

        public async Task ExecuteAsync()
        {
            var allProcessed = new List<TripRecord>(capacity: 100_000);
            var headerMapping = await _csvReaderService.ReadHeaderPositionsAsync(AppConfiguration.CsvInputPath);
            
            await foreach (var batchLines in _csvReaderService.ReadBatchesAsync(AppConfiguration.CsvInputPath, _batchSize))
            {
                if (batchLines == null || batchLines.Count == 0)
                    continue;
                
                var miniBatches = SplitLines(batchLines, _maxParallelWorkers);
                var workerTasks = new List<Task<List<TripRecord>>>(miniBatches.Count);
                
                for (int i = 0; i < miniBatches.Count; i++)
                {
                    var linesChunk = miniBatches[i];
                    workerTasks.Add(_tripProcessorService.ProcessLinesAsync(linesChunk, headerMapping));
                }

                var results = await Task.WhenAll(workerTasks);
                foreach (var processedList in results)
                {
                    if (processedList != null && processedList.Count > 0)
                        allProcessed.AddRange(processedList);
                }
            }

            if (allProcessed.Count == 0)
                return;

            _duplicateDetectorService.SplitUniqueAndDuplicates(
                allProcessed,
                out var uniqueRecords,
                out var duplicates
            );

            if (duplicates.Count > 0)
                _csvWriterService.WriteDuplicates(AppConfiguration.DuplicatesOutputPath, duplicates, _headers);

            if (uniqueRecords.Count > 0)
                await _sqlBulkWriterService.WriteBatchAsync(uniqueRecords, AppConfiguration.TargetTable);
        }

        private List<List<string>> SplitLines(List<string> src, int parts)
        {
            var result = new List<List<string>>();
            if (src.Count == 0)
                return result;

            int chunkSize = Math.Max(1, src.Count / parts);
            for (int i = 0; i < src.Count; i += chunkSize)
            {
                var chunk = src.Skip(i).Take(chunkSize).ToList();
                result.Add(chunk);
            }
            return result;
        }
    }
}
