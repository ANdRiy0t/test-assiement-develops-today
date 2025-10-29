using System.Globalization;
using Test.AssiementDevelopsToday.Jobs.Entities;

namespace Test.AssiementDevelopsToday.Jobs.Helpers;

public static class CsvWriterHelper
{
    public static void WriteCsv<TItem>(
        string path,
        IEnumerable<TItem> items,
        string[] headers,
        Func<TItem, string[]> selector)
    {
        using var w = new StreamWriter(path);

        w.WriteLine(string.Join(",", headers));

        foreach (var item in items)
        {
            var fields = selector(item);
            w.WriteLine(string.Join(",", fields));
        }
    }
}