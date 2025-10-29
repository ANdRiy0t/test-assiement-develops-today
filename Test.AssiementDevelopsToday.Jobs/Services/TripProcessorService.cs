using System.Globalization;
using Test.AssiementDevelopsToday.Jobs.Entities;
using Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

namespace Test.AssiementDevelopsToday.Jobs.Services;

public class TripProcessorService : ITripProcessorService
{
    public Task<List<TripRecord>> ProcessLinesAsync(List<string> lines, Dictionary<string, int> headerMap)
    {
        var result = new List<TripRecord>(lines.Count);

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length < 9)
                continue;

            string Get(string key)
            {
                if (!headerMap.TryGetValue(key, out var idx)) return "";
                if (idx < 0 || idx >= parts.Length) return "";
                return parts[idx];
            }

            var rec = new TripRecord
            {
                PickupDateTimeUtc = ParseDate(Get("tpep_pickup_datetime")),
                DropoffDateTimeUtc = ParseDate(Get("tpep_dropoff_datetime")),
                PassengerCount = ParseInt(Get("passenger_count")),
                TripDistance = ParseDecimal(Get("trip_distance")),
                StoreAndFwdFlag = CleanStoreFlag(Get("store_and_fwd_flag")),
                PULocationID = ParseInt(Get("PULocationID")),
                DOLocationID = ParseInt(Get("DOLocationID")),
                FareAmount = ParseDecimal(Get("fare_amount")),
                TipAmount = ParseDecimal(Get("tip_amount"))
            };

            rec.PickupDateTimeUtc = rec.PickupDateTimeUtc.AddHours(5);
            rec.DropoffDateTimeUtc = rec.DropoffDateTimeUtc.AddHours(5);

            result.Add(rec);
        }

        return Task.FromResult(result);
    }

    private DateTime ParseDate(string raw)
    {
        DateTime dt;
        if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            return dt;

        return DateTime.MinValue;
    }

    private int ParseInt(string raw)
    {
        int v;
        if (int.TryParse(raw, out v))
            return v;
        return 0;
    }

    private decimal ParseDecimal(string raw)
    {
        decimal v;
        if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out v))
            return v;
        return 0m;
    }

    // Task #7 + #8
    private string CleanStoreFlag(string raw)
    {
        var trimmed = (raw ?? "").Trim(); // Task #8 (trim)
        if (trimmed == "N") return "No"; // Task #7
        if (trimmed == "Y") return "Yes"; // Task #7
        return trimmed;
    }
}