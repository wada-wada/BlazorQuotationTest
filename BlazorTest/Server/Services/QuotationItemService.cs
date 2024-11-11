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
                var sql = "SELECT * FROM quotationitems WHERE quotation_id = @QuotationId ORDER BY quotation_item_id";
                return await connection.QueryAsync<QuotationItem>(sql, new { QuotationId = quotationId });
            }
        }

        public async Task<QuotationItem> GetQuotationItemLineByIdAsync(int quotationId, int quotationItemId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM quotationitems WHERE quotation_id = @QuotationId AND quotation_item_id = @QuotationItemId";
                return await connection.QueryFirstOrDefaultAsync<QuotationItem>(sql, new { QuotationId = quotationId, QuotationItemId = quotationItemId });
            }
        }

        public async Task<bool> UpdateQuotationAsync(QuotationItem updateQuotationItem)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sql = " UPDATE quotationitems SET "
                            + "  product_name = @Product_Name"
                            + ", quantity = @Quantity"
                            + ", unit_price = @Unit_Price"
                            + ", line_total = @Line_Total"
                            + ", update_date = CURRENT_TIMESTAMP"
                            + " WHERE quotation_item_id = @Quotation_Item_Id ";
                var rowsAffected = await connection.ExecuteAsync(sql, updateQuotationItem);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteQuotationItemAsync(int quotationItemId)
        {
            using (var connection = new NpgsqlConnection(_connectionString)) 
            {
                var sql = "DELETE FROM quotationitems WHERE quotation_item_id = @Quotation_Item_Id"; 
                var result = await connection.ExecuteAsync(sql, new { Quotation_Item_Id = quotationItemId });
                return result > 0;
            }

        }
    }
}
