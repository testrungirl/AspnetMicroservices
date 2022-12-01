using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Rebate.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Migrating Postgre Sql database");
                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();
                    command.CommandText = @"Create table Coupon(Id Serial Primary Key,
                                            ProductName varchar(24) not null,
                                            description Text, Amount Int)";
                    command.ExecuteNonQuery();
                    command.CommandText = "insert into coupon (productname, description, amount) values ('IPhone X', 'IPhone Discount', 150);";
                    command.ExecuteNonQuery();
                    command.CommandText = "insert into coupon (productname, description, amount) values ('Samsung 10', 'Samsung Discount', 100);";
                    command.ExecuteNonQuery();
                    logger.LogInformation("Migrated postgre sql database");
                } 
                catch(Exception ex)
                {
                    logger.LogError(ex, "An error occured while migrating the postgre sql database");
                    if(retryForAvailability < 25)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
                return host;
            }

        }
    }
}
