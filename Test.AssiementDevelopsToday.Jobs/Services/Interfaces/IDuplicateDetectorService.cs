using Test.AssiementDevelopsToday.Jobs.Entities;

namespace Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

public interface IDuplicateDetectorService
{
    void SplitUniqueAndDuplicates(List<TripRecord> input, out List<TripRecord> uniqueRecords,
        out List<DuplicateRecord> duplicates);
}