using Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

namespace Test.AssiementDevelopsToday.Jobs.Services;

public class CsvReaderService : ICsvReaderService
{
    
    public async Task<Dictionary<string, int>> ReadHeaderPositionsAsync(string path)
    {
        using var reader = new StreamReader(path);
        var headerLine = await reader.ReadLineAsync();
        if (string.IsNullOrWhiteSpace(headerLine))
            return new Dictionary<string, int>();

        var headers = headerLine.Split(',');
        var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < headers.Length; i++)
        {
            var col = headers[i].Trim();
            if (!string.IsNullOrEmpty(col))
                dict.TryAdd(col, i);
        }

        return dict;
    }
    
    public async IAsyncEnumerable<List<string>> ReadBatchesAsync(string path, int batchSize)
    {
        using var reader = new StreamReader(path);
        string? header = await reader.ReadLineAsync();

        var buffer = new List<string>(batchSize);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line == null)
                break;

            buffer.Add(line);

            if (buffer.Count >= batchSize)
            {
                yield return buffer;
                buffer = new List<string>(batchSize);
            }
        }

        if (buffer.Count > 0)
            yield return buffer;
    }
}