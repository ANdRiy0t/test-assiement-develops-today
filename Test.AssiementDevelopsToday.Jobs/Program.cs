using Microsoft.Extensions.DependencyInjection;
using Test.AssiementDevelopsToday.Jobs.Commands;
using Test.AssiementDevelopsToday.Jobs.Services;
using Test.AssiementDevelopsToday.Jobs.Services.Interfaces;

var services = new ServiceCollection();

services.AddSingleton<ICsvReaderService, CsvReaderService>();
services.AddSingleton<ITripProcessorService, TripProcessorService>();
services.AddSingleton<IDuplicateDetectorService, DuplicateDetectorService>();
services.AddSingleton<ISqlBulkWriterService, SqlBulkWriterService>();
services.AddSingleton<ICsvWriterService, CsvWriterService>();

services.AddSingleton<IProcessCsvFileCommand, ProcessCsvFileCommand>();

await using var provider = services.BuildServiceProvider();

var processCsvFileCommand = provider.GetRequiredService<IProcessCsvFileCommand>();
await processCsvFileCommand.ExecuteAsync();