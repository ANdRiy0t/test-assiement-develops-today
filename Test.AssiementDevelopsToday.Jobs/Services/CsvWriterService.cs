using System.Globalization;
using Test.AssiementDevelopsToday.Jobs.Entities;
using Test.AssiementDevelopsToday.Jobs.Helpers;
using Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

namespace Test.AssiementDevelopsToday.Jobs.Services;

public class CsvWriterService : ICsvWriterService
{
    public void WriteDuplicates(string path, List<DuplicateRecord> duplicates, params string[] headers)
    {
        CsvWriterHelper.WriteCsv(
            path,
            duplicates,
            headers,
            record =>
            [
                record.PickupDateTimeUtc.ToString("o", CultureInfo.InvariantCulture),
                record.DropoffDateTimeUtc.ToString("o", CultureInfo.InvariantCulture),
                record.PassengerCount.ToString(CultureInfo.InvariantCulture),
                record.TripDistance.ToString(CultureInfo.InvariantCulture),
                record.StoreAndFwdFlag?.Trim() ?? string.Empty,
                record.PULocationID.ToString(CultureInfo.InvariantCulture),
                record.DOLocationID.ToString(CultureInfo.InvariantCulture),
                record.FareAmount.ToString(CultureInfo.InvariantCulture),
                record.TipAmount.ToString(CultureInfo.InvariantCulture)
            ]
        );
    }
}