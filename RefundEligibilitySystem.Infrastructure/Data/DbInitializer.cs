using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RefundEligibilitySystem.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RefundDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<RefundDbContext>>();

        var connectionString = db.Database.GetConnectionString();
        var masterConnectionString = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = "master"
        }.ConnectionString;

        int retryCount = 0;
        const int maxRetries = 15;
        while (retryCount < maxRetries)
        {
            try
            {
                using var testConnection = new Microsoft.Data.SqlClient.SqlConnection(masterConnectionString);
                await testConnection.OpenAsync();
                logger.LogInformation("SQL Server is ready.");
                break;
            }
            catch (Exception ex)
            {
                retryCount++;
                logger.LogWarning("SQL Server not ready yet (attempt {Retry}/{Max}): {Message}",
                    retryCount, maxRetries, ex.Message);

                if (retryCount == maxRetries)
                    throw new Exception("SQL Server did not become ready in time.", ex);

                await Task.Delay(3000);
            }
        }

        using (var masterConnection = new Microsoft.Data.SqlClient.SqlConnection(masterConnectionString))
        {
            await masterConnection.OpenAsync();
            var dropAndCreateSql = @"
                IF EXISTS (SELECT * FROM sys.databases WHERE name = 'RefundSystemDB')
                BEGIN
                    ALTER DATABASE RefundSystemDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    DROP DATABASE RefundSystemDB;
                END
                CREATE DATABASE RefundSystemDB;";

            using var command = new Microsoft.Data.SqlClient.SqlCommand(dropAndCreateSql, masterConnection);
            await command.ExecuteNonQueryAsync();
            logger.LogInformation("Database 'RefundSystemDB' dropped and recreated successfully.");
        }

        var databaseFolder = "/app/Database/Scripts";
        var schemaSql = await File.ReadAllTextAsync(Path.Combine(databaseFolder, "Schema.sql"));
        var approvalApplicationSP = await File.ReadAllTextAsync(Path.Combine(databaseFolder, "ApproveApplication.sql"));
        var calculateRefundSP = await File.ReadAllTextAsync(Path.Combine(databaseFolder, "CalculateRefund.sql"));
        var seedSql = await File.ReadAllTextAsync(Path.Combine(databaseFolder, "SeedData.sql"));

        using var transaction = await db.Database.BeginTransactionAsync();
        try
        {
            await db.Database.ExecuteSqlRawAsync(schemaSql);
            await db.Database.ExecuteSqlRawAsync(approvalApplicationSP);
            await db.Database.ExecuteSqlRawAsync(calculateRefundSP);
            await db.Database.ExecuteSqlRawAsync(seedSql);
            await transaction.CommitAsync();
            logger.LogWarning("FULL DATABASE RESET COMPLETED SUCCESSFULLY.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "FATAL ERROR during Database Initialization!");
            await transaction.RollbackAsync();
            throw;
        }
    }
}