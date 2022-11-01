using Npgsql;
using Polly;

namespace Discount.API.Extensions
{
    public class HostExtensions : IHostExtensions
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        public HostExtensions(ILogger<HostExtensions> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            MigrateDatabase();
        }
        public void MigrateDatabase()
        {
            try
            {
                _logger.LogInformation("Migrating postresql database.");

                var retry = Policy.Handle<NpgsqlException>()
                        .WaitAndRetry(
                            retryCount: 5,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                            onRetry: (exception, retryCount, context) =>
                            {
                                _logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                            });

                retry.Execute(() => ExecuteMigrations(_configuration));

                _logger.LogInformation("Migrated postresql database.");
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "An error occurred while migrating the postresql database");
            }

        }

        private void ExecuteMigrations(IConfiguration configuration)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            connection.Open();

            using var command = new NpgsqlCommand
            {
                Connection = connection
            };

            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                ProductId VARCHAR(50) NOT NULL,
                                                                Headline VARCHAR(100) NOT NULL,
                                                                Description TEXT,
                                                                CouponCode VARCHAR(15) NOT NULL,
                                                                Amount INT)";
           
            command.ExecuteNonQuery();


            command.CommandText = "INSERT INTO Coupon(ProductName, Description, ProductId, Headline, CouponCode, Amount) VALUES('IPhone X', 'IPhone Discount', '632b52550679f4f40cd35725', 'Mothers day promotion', 'OIS-UTW-JNG', 150);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, ProductId, Headline, CouponCode, Amount) VALUES('Samsung 10', 'Samsung Discount', '632b52550679f4f40cd35720', 'John Chilembwe day promotion', 'HGS-UWW-JNS', 100);";
            command.ExecuteNonQuery();
        }
    }
}
