namespace BlazorTest.Client.Models
{
    public class QuotationRequest
    {
        public Quotation? Quotation { get; set; }
        public List<QuotationItem>? QuotationItems { get; set; }
    }
}
