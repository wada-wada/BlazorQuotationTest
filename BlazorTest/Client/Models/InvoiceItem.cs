//請求書内容モデル（ダミーデータ）
namespace BlazorTest.Client.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount => Quantity * UnitPrice;
    }
}
