using System.Data;
using Microsoft.Data.SqlClient;
using Test.AssiementDevelopsToday.Jobs.Configuration;
using Test.AssiementDevelopsToday.Jobs.Entities;
using Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

namespace Test.AssiementDevelopsToday.Jobs.Services;

public class SqlBulkWriterService : ISqlBulkWriterService
{
    private readonly string _connectionString = AppConfiguration.ConnectionString;

    public async Task WriteBatchAsync(IEnumerable<TripRecord> batch, string tableName)
    {
        var table = BuildDataTable(batch);

        if (table.Rows.Count == 0)
            return;

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
        bulkCopy.DestinationTableName = tableName;

        bulkCopy.ColumnMappings.Add("PickupUtc", "PickupUtc");
        bulkCopy.ColumnMappings.Add("DropoffUtc", "DropoffUtc");
        bulkCopy.ColumnMappings.Add("PassengerCount", "PassengerCount");
        bulkCopy.ColumnMappings.Add("TripDistance", "TripDistance");
        bulkCopy.ColumnMappings.Add("StoreAndFwdFlag", "StoreAndFwdFlag");
        bulkCopy.ColumnMappings.Add("PULocationID", "PULocationID");
        bulkCopy.ColumnMappings.Add("DOLocationID", "DOLocationID");
        bulkCopy.ColumnMappings.Add("FareAmount", "FareAmount");
        bulkCopy.ColumnMappings.Add("TipAmount", "TipAmount");

        await bulkCopy.WriteToServerAsync(table);
    }

    public async Task WriteBatchTransactionalAsync(IEnumerable<TripRecord> batch, string tableName,
        SqlTransaction externalTransaction)
    {
        var table = BuildDataTable(batch);

        if (table.Rows.Count == 0)
            return;

        using var bulkCopy = new SqlBulkCopy(
            externalTransaction.Connection!,
            SqlBulkCopyOptions.TableLock,
            externalTransaction);
        bulkCopy.DestinationTableName = tableName;

        bulkCopy.ColumnMappings.Add("PickupUtc", "PickupUtc");
        bulkCopy.ColumnMappings.Add("DropoffUtc", "DropoffUtc");
        bulkCopy.ColumnMappings.Add("PassengerCount", "PassengerCount");
        bulkCopy.ColumnMappings.Add("TripDistance", "TripDistance");
        bulkCopy.ColumnMappings.Add("StoreAndFwdFlag", "StoreAndFwdFlag");
        bulkCopy.ColumnMappings.Add("PULocationID", "PULocationID");
        bulkCopy.ColumnMappings.Add("DOLocationID", "DOLocationID");
        bulkCopy.ColumnMappings.Add("FareAmount", "FareAmount");
        bulkCopy.ColumnMappings.Add("TipAmount", "TipAmount");

        await bulkCopy.WriteToServerAsync(table);
    }

    private DataTable BuildDataTable(IEnumerable<TripRecord> batch)
    {
        var dt = new DataTable();

        dt.Columns.Add("PickupUtc", typeof(DateTime));
        dt.Columns.Add("DropoffUtc", typeof(DateTime));
        dt.Columns.Add("PassengerCount", typeof(int));
        dt.Columns.Add("TripDistance", typeof(decimal));
        dt.Columns.Add("StoreAndFwdFlag", typeof(string));
        dt.Columns.Add("PULocationID", typeof(int));
        dt.Columns.Add("DOLocationID", typeof(int));
        dt.Columns.Add("FareAmount", typeof(decimal));
        dt.Columns.Add("TipAmount", typeof(decimal));

        foreach (var tripRecord in batch)
        {
            var row = dt.NewRow();
            row["PickupUtc"] = tripRecord.PickupDateTimeUtc;
            row["DropoffUtc"] = tripRecord.DropoffDateTimeUtc;
            row["PassengerCount"] = tripRecord.PassengerCount;
            row["TripDistance"] = tripRecord.TripDistance;
            row["StoreAndFwdFlag"] = tripRecord.StoreAndFwdFlag;
            row["PULocationID"] = tripRecord.PULocationID;
            row["DOLocationID"] = tripRecord.DOLocationID;
            row["FareAmount"] = tripRecord.FareAmount;
            row["TipAmount"] = tripRecord.TipAmount;
            dt.Rows.Add(row);
        }

        return dt;
    }
}