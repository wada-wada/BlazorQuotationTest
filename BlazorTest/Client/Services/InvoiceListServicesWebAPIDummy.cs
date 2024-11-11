using System.Net.Http.Json;
using BlazorTest.Client.Models;

namespace BlazorTest.Client.Services
{
    public class InvoiceListServicesWebAPIDummy
    {
        private readonly HttpClient _httpClient;
        public InvoiceListServicesWebAPIDummy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //null許容に変更
        public async Task<List<InvoiceModel>?> GetInvoiceListAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<InvoiceModel>>("api/InvoiceList");
        }
    }
}
