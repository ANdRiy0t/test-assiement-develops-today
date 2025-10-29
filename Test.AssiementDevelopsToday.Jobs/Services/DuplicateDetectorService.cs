using Test.AssiementDevelopsToday.Jobs.Entities;
using Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

namespace Test.AssiementDevelopsToday.Jobs.Services;

public class DuplicateDetectorService : IDuplicateDetectorService
{
    public void SplitUniqueAndDuplicates(
        List<TripRecord> input,
        out List<TripRecord> uniqueRecords,
        out List<DuplicateRecord> duplicates
    )
    {
        var seenKeys = new HashSet<string>();
        uniqueRecords = new List<TripRecord>();
        duplicates = new List<DuplicateRecord>();

        foreach (var r in input)
        {
            var key = BuildKey(r);
            if (!seenKeys.Contains(key))
            {
                seenKeys.Add(key);
                uniqueRecords.Add(r);
            }
            else
            {
                duplicates.Add(new DuplicateRecord
                {
                    PickupDateTimeUtc = r.PickupDateTimeUtc,
                    DropoffDateTimeUtc = r.DropoffDateTimeUtc,
                    PassengerCount = r.PassengerCount,
                    TripDistance = r.TripDistance,
                    StoreAndFwdFlag = r.StoreAndFwdFlag,
                    PULocationID = r.PULocationID,
                    DOLocationID = r.DOLocationID,
                    FareAmount = r.FareAmount,
                    TipAmount = r.TipAmount
                });
            }
        }
    }

    private string BuildKey(TripRecord r)
    {
        return r.PickupDateTimeUtc.ToString("O")
               + "|" + r.DropoffDateTimeUtc.ToString("O")
               + "|" + r.PassengerCount;
    }
}