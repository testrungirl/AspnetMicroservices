
using Rebate.GRPC.Data;
using Dapper;
using System.Data;
using Rebate.GRPC.Models;
using Npgsql;

namespace Rebate.GRPC.Repositories
{
    public class RebateRepository : IRebateRepository
    {
        IConfiguration _configuration;
        public RebateRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<GenericResponse<bool>> CreateRebate(Coupon coupon)
        {
            try
            {
                using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                if (connection.State == ConnectionState.Closed)
                {
                    Console.WriteLine("Trying to open postgreSQL connection.");
                    connection.Open();
                    Console.WriteLine("Success opening postgreSQL connection.");
                }
                var affectedRows = await connection.ExecuteAsync("insert into Coupon (ProductName, Description, Amount) values (@ProductName, @Description, @Amount)",
                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
                if (affectedRows == 1)
                {
                    return new GenericResponse<bool> { Message = "Successfully created", Success = true };
                }
                return new GenericResponse<bool> { Message = "Could not perform Insert action", Success = false };
            }
            catch (Exception ex)
            {
                return new GenericResponse<bool> { Message = ex.Message, Success = false };
            }
        }

        public async Task<GenericResponse<bool>> DeleteRebate(string productName)
        {
            try
            {
                using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                if (connection.State == ConnectionState.Closed)
                {
                    Console.WriteLine("Trying to open postgreSQL connection.");
                    connection.Open();
                    Console.WriteLine("Success opening postgreSQL connection.");
                }
                var RowsCount = await connection.ExecuteAsync("delete from coupon where ProductName = @ProductName", new { ProductName = productName });
                if (RowsCount == 0)
                {
                    return new GenericResponse<bool> { Message = "Could not perform Delete action", Success = false };
                }

                return new GenericResponse<bool> { Message = "Successfully deleted", Success = true };
            }
            catch (Exception ex)
            {
                return new GenericResponse<bool> { Message = ex.Message, Success = false };
            }
        }

        public async Task<GenericResponse<Coupon>> GetRebate(string productName)
        {
            try
            {
                using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                if (connection.State == ConnectionState.Closed)
                {
                    Console.WriteLine("Trying to open postgreSQL connection.");
                    connection.Open();
                    Console.WriteLine("Success opening postgreSQL connection.");
                }
                var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("Select * from Coupon where ProductName = @ProductName", new { ProductName = productName });
                if (coupon == null)
                {
                    return new GenericResponse<Coupon>
                    {
                        Success = false,
                        Message = "Coupon does not exist with the name, " + productName,

                    };
                }
                return new GenericResponse<Coupon>
                {
                    Data = coupon,
                    Success = true,

                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<Coupon> { Message = ex.Message, Success = false };
            }
        }

        public async Task<GenericResponse<bool>> UpdateRebate(Coupon coupon)
        {
            try
            {
                using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                if (connection.State == ConnectionState.Closed)
                {
                    Console.WriteLine("Trying to open postgreSQL connection.");
                    connection.Open();
                    Console.WriteLine("Success opening postgreSQL connection.");
                }
                var affectedRows = await connection.ExecuteAsync("update Coupon set ProductName = @ProductName, Description = @Description, Amount=@Amount where Id = @Id",
                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });
                if (affectedRows == 1)
                {
                    return new GenericResponse<bool> { Message = "Successfully created", Success = true };
                }
                return new GenericResponse<bool> { Message = "Could not perform Insert action", Success = false };
            }
            catch (Exception ex)
            {
                return new GenericResponse<bool> { Message = ex.Message, Success = false };
            }
        }
    }
}
