namespace BlazorTest.Server.Models
{
    public class InvoiceModel
    {
        //初期値やnullの場合の値を定義
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; } = DateTime.MinValue;
        public string CustomerName { get; set; } = string.Empty;
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public decimal TotalAmount => Items?.Sum(item => item.Amount) ?? 0;
    }
}
