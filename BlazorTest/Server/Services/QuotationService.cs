using Dapper;
using Npgsql;
using BlazorTest.Server.Models;
namespace BlazorTest.Server.Services
{
    public class QuotationService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public QuotationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Quotation>> GetQuotaionsAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM quotations";
                return await connection.QueryAsync<Quotation>(sql);
            }
        }

        public async Task AddQuotationAsync(Quotation quotation)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sql = "INSERT INTO quotation"
                        + "(quotation_id, customer_id, quotation_date, total_amount, status,"
                        + "expiration_date, created_date, update_date)"
                        + "VALUES"
                        + "(@quotation_id, @customer_id, @quotation_date, @total_amount, @status,"
                        + "@expiration_date, @created_date, @update_date)";
                await connection.ExecuteAsync(sql, quotation);
            }
        }
    }
}
