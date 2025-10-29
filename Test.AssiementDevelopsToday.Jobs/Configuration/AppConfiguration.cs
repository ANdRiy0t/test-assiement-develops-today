namespace Test.AssiementDevelopsToday.Jobs.Configuration;

public static class AppConfiguration
{
    public static string CsvInputPath = @"C:\Users\225588\RiderProjects\Test.AssiementDevelopsToday.Jobs\Test.AssiementDevelopsToday.Jobs\Files\sample-cab-data (1).csv";
    public static string DuplicatesOutputPath = @"C:\Users\225588\RiderProjects\Test.AssiementDevelopsToday.Jobs\Test.AssiementDevelopsToday.Jobs\Files\duplicates.csv";

    public static string ConnectionString { get; set; } =
        "Server=localhost;Database=TaxiDb;Trusted_Connection=True;TrustServerCertificate=True;";
    
    public static string TargetTable { get; set; } = "dbo.TripRecords";


    static AppConfiguration()
    {
        // TrySetValueFromEnvironment()
    }
    
    private static void TrySetValueFromEnvironment(string environmentKey, ref string value)
    {
        var environmentValue = Environment.GetEnvironmentVariable(environmentKey);
        if(string.IsNullOrEmpty(environmentValue))
            return;

        value = environmentValue;
    }
}