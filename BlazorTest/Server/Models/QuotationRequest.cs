namespace BlazorTest.Server.Models
{
    public class QuotationRequest
    {
        public Quotation? Quotation { get; set; }
        public List<QuotationItem>? QuotationItems { get; set; }
    }
}
