using System.Globalization;
using Test.AssiementDevelopsToday.Jobs.Entities;
using Test.AssiementDevelopsToday.Jobs.Helpers;

namespace Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

public interface ICsvWriterService
{
    void WriteDuplicates(string path, List<DuplicateRecord> duplicates, params string[] headers);
}