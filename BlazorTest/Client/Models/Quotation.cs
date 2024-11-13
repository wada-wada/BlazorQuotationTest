namespace BlazorTest.Client.Models
{
        public class Quotation
        {
            public int Quotation_Id { get; set; }
            public int Customer_Id { get; set; }
            public DateTime Quotation_Date { get; set; }
            public decimal Total_Amount { get; set; }
            public DateTime Expiration_Date { get; set; }
            public DateTime Created_Date { get; set; }
            public DateTime Update_Date { get; set; }
            public string Status { get; set; }
            public string Quotation_Name { get; set; }
            public bool IsNew { get; set; } = false;
        }
}
