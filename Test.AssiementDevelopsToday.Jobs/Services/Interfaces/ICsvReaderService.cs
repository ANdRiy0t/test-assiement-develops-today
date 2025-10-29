namespace Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

public interface ICsvReaderService
{
    Task<Dictionary<string, int>> ReadHeaderPositionsAsync(string path);
    IAsyncEnumerable<List<string>> ReadBatchesAsync(string path, int batchSize);
}