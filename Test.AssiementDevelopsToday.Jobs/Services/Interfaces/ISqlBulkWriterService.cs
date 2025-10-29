using Microsoft.Data.SqlClient;
using Test.AssiementDevelopsToday.Jobs.Entities;

namespace Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

public interface ISqlBulkWriterService
{
    Task WriteBatchAsync(IEnumerable<TripRecord> batch, string tableName);

    Task WriteBatchTransactionalAsync(IEnumerable<TripRecord> batch, string tableName, SqlTransaction externalTransaction);
}