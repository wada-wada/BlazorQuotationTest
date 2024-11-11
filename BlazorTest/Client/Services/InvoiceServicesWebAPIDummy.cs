using System.Net.Http.Json;
using BlazorTest.Client.Models;

namespace BlazorTest.Client.Services
{
    public class InvoiceServicesWebAPIDummy
    {
        private readonly HttpClient _httpClient;
        public InvoiceServicesWebAPIDummy(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<List<InvoiceModel>?> GetInvoicesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<InvoiceModel>>("api/Invoices");
        }

    }
}
