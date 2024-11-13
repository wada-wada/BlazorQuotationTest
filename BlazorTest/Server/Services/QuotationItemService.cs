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

        public async Task<Quotation> GetQuotationAsync(int quotationId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM quotations WHERE quotation_id = @Quotation_Id";
                return await connection.QueryFirstOrDefaultAsync<Quotation>(sql, new {Quotation_Id = quotationId});
            }
        }

        public async Task<IEnumerable<QuotationItem>> GetQuotationItemsAsync(int quotationId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM quotationitems WHERE quotation_id = @Quotation_Id ORDER BY quotation_item_id";
                return await connection.QueryAsync<QuotationItem>(sql, new { Quotation_Id = quotationId });
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

        //明細を複数行更新する
        public async Task SaveQuotationItemsAsync(int quotationId, List<QuotationItem> items)
        {
            string sql = @"
                            UPDATE quotationitems
                            SET
                                product_name = @Product_Name
                                , quantity = @Quantity
                                , unit_price = @Unit_Price
                                , line_total = @Quantity * @Unit_Price
                                , update_date = CURRENT_TIMESTAMP
                            WHERE quotation_id = @Quotation_Id
                            AND quotation_item_id = @Quotation_Item_Id
                        ";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                foreach(var item in items)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("Quotation_Id", quotationId);
                    parameters.Add("Quotation_Item_Id", item.Quotation_Item_Id);
                    parameters.Add("Product_Name", item.Product_Name);
                    parameters.Add("Quantity", item.Quantity);
                    parameters.Add("Unit_Price", item.Unit_Price);

                    await connection.ExecuteAsync(sql, parameters);
                }
            }
        }

        //請求書名を更新する
        public async Task<bool> SaveQuotationNameAsync(int quotationId, Quotation quotation)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                string sql = @"
                                UPDATE quotations
                                SET
                                   quotation_name = @Quotation_Name
                                WHERE quotation_id = @Quotation_Id
                            ";
                var result = await connection.ExecuteAsync(sql, new 
                            { 
                                Quotation_Id = quotationId, 
                                Quotation_Name = quotation.Quotation_Name 
                            });
                return result > 0;
            }
        }

        //新しい明細を登録する
        public async Task InsertQuotationItemsAsync(int quotationId, List<QuotationItem> newItems)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        var sql = @"
                                    INSERT INTO quotationitems
                                        ( quotation_item_id, quotation_id, product_name, quantity, 
                                          unit_price, line_total, update_date )
                                    VALUES
                                        ((SELECT COALESCE(MAX(quotation_item_id), 0) + 1 FROM quotationitems), @Quotation_Id, @Product_Name, @Quantity, 
                                          @Unit_Price, @Quantity * @Unit_Price, CURRENT_DATE )
                                    ";

                        foreach (var item in newItems)
                        {
                            await connection.ExecuteAsync(sql, new
                            {
                                Quotation_Id = quotationId,
                                Product_Name = item.Product_Name,
                                Quantity = item.Quantity,
                                Unit_Price = item.Unit_Price,
                            }, transaction);
                        }

                        await transaction.CommitAsync();

                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        //トランザクションで更新、登録処理を行う
        public async Task<bool> SaveQuotationDetailsAsync(Quotation quotation, List<QuotationItem> quotationitems)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //Quotationの更新
                        var updateUotaionQuery = @"
                                                    UPDATE quotations
                                                    SET
                                                       quotation_name = @Quotation_Name
                                                    WHERE quotation_id = @Quotation_Id
                                                ";
                        await connection.ExecuteAsync(updateUotaionQuery, quotation, transaction);

                        //有効なQuotationItemをフィルタリングする
                        var validItems = quotationitems
                            .Where(i => !string.IsNullOrWhiteSpace(i.Product_Name)
                                        && i.Quantity > 0
                                        && i.Unit_Price > 0)
                            .ToList();

                        //QuotationItemの挿入と更新
                        var insertItemQuery = @"
                                                INSERT INTO quotationitems
                                                    ( quotation_item_id, quotation_id, product_name, quantity, 
                                                      unit_price, line_total, update_date )
                                                VALUES
                                                    ((SELECT COALESCE(MAX(quotation_item_id), 0) + 1 FROM quotationitems), @Quotation_Id, @Product_Name, @Quantity, 
                                                      @Unit_Price, @Quantity * @Unit_Price, CURRENT_DATE )
                                                ";

                        var updateItemQuery = @"
                                                UPDATE quotationitems
                                                SET
                                                    product_name = @Product_Name
                                                    , quantity = @Quantity
                                                    , unit_price = @Unit_Price
                                                    , line_total = @Quantity * @Unit_Price
                                                    , update_date = CURRENT_TIMESTAMP
                                                WHERE quotation_id = @Quotation_Id
                                                AND quotation_item_id = @Quotation_Item_Id
                                                ";

                        var deleteItemQuery = @"DELETE FROM quotationitems
                                                WHERE quotation_id = @Quotation_Id
                                                AND quotation_item_id = @quotation_item_id
                                                ";
                        foreach(var item in validItems)
                        {
                            if (item.IsDeleted)
                            {
                                await connection.ExecuteAsync(deleteItemQuery, item, transaction);
                            }
                            else if(item.Quotation_Item_Id == 0)
                            {
                                await connection.ExecuteAsync(insertItemQuery, item, transaction);
                            }
                            else
                            {
                                await connection.ExecuteAsync(updateItemQuery, item, transaction);
                            }
                        }

                        await transaction.CommitAsync();
                        return true;

                    }
                    catch(Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Transaction faild", ex);
                    }
                }
            }
        }
    }
}
