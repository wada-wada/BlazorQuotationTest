namespace BlazorTest.Client.Models
{
    public class QuotationItem
    {
        public int Quotation_Item_Id { get; set; }
        public int Quotation_Id { get; set; }
        public string Product_Name { get; set; }
        public int Quantity { get; set; }
        public decimal Unit_Price { get; set; }
        public decimal Line_Total { get; set; }
        public DateTime Update_Date { get; set; }
        public bool IsNew { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
