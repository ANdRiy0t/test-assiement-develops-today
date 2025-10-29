using Test.AssiementDevelopsToday.Jobs.Entities;

namespace Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

public interface ITripProcessorService
{
    Task<List<TripRecord>> ProcessLinesAsync(List<string> lines, Dictionary<string, int> headerMap);
}