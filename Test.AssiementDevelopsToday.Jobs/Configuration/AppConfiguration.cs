namespace Test.AssiementDevelopsToday.Jobs.Configuration;

public static class AppConfiguration
{
    public static string CsvInputPath = Environment.GetEnvironmentVariable("CSV_INPUT_PATH") ??
                                        @"C:\Users\225588\RiderProjects\Test.AssiementDevelopsToday.Jobs\Test.AssiementDevelopsToday.Jobs\Files\sample-cab-data (1).csv";

    public static string DuplicatesOutputPath = Environment.GetEnvironmentVariable("CSV_OUTPUT_OUTPUT_PATH") ??
                                                @"C:\Users\225588\RiderProjects\Test.AssiementDevelopsToday.Jobs\Test.AssiementDevelopsToday.Jobs\Files\duplicates.csv";

    public static string ConnectionString { get; set; } = Environment.GetEnvironmentVariable("CONNECTION_STRING") ??
                                                          "Server=localhost;Database=TaxiDb;Trusted_Connection=True;TrustServerCertificate=True;";

    public static string TargetTable { get; set; } =
        Environment.GetEnvironmentVariable("TABLE_NAME") ?? "dbo.TripRecords";
}