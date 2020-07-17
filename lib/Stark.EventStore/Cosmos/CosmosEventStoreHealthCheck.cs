using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Stark.EventStore.Cosmos
{
    internal class CosmosEventStoreHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public CosmosEventStoreHealthCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "In this file we specifically want to catch any exception so we can report the service as unhealthy.")]
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var client = new CosmosClient(_connectionString);
                await client.ReadAccountAsync();
                return HealthCheckResult.Healthy("Database for Event Store responding.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Error reading from the Event Store.", ex);
            }
        }
    }
}
