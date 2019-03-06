using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Benday.Presidents.Api.DataAccess
{
    public class PresidentsDbContextFactory : IDbContextFactory<PresidentsDbContext>
    {
        public PresidentsDbContext Create()
        {
            var environmentName =
                        Environment.GetEnvironmentVariable(
                            "Hosting:Environment");

            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName);
        }

        public PresidentsDbContext Create(DbContextFactoryOptions options)
        {
            return Create(
                options.ContentRootPath,
                options.EnvironmentName);
        }

        private PresidentsDbContext Create(string basePath, string environmentName)
        {
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString("default");

            if (String.IsNullOrWhiteSpace(connstr) == true)
            {
                throw new InvalidOperationException(
                    "Could not find a connection string named 'default'.");
            }
            else
            {
                return Create(connstr);
            }
        }

        private PresidentsDbContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(
                    $"{nameof(connectionString)} is null or empty.",
                    nameof(connectionString));

            var optionsBuilder =
                new DbContextOptionsBuilder<PresidentsDbContext>();

            Console.WriteLine("PresidentsDbContextFactory.Create(string): Connection string: {0}", 
                connectionString);

            optionsBuilder.UseSqlServer(connectionString);

            return new PresidentsDbContext(optionsBuilder.Options);
        }
    }
}
