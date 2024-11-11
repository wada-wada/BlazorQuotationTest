using BlazorTest.Server.Models;
using Dapper;
using Npgsql;

namespace BlazorTest.Server.Services
{
    public class QuotationItemService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public QuotationItemService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Quotation>> GetQuotationsAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM quotations";
                return await connection.QueryAsync<Quotation>(sql);
            }
        }

        public async Task<IEnumerable<QuotationItem>> GetQuotationItemsAsync(int quotationId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM quotationitems WHERE quotation_id = @QuotationId";
                return await connection.QueryAsync<QuotationItem>(sql, new { QuotationId = quotationId });
            }
        }
    }
}
