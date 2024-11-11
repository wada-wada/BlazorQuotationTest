using BlazorTest.Client;
using BlazorTest.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<InvoiceServiceDummyValue>();//�������i�_�~�[�f�[�^�j
builder.Services.AddScoped<InvoiceListSeriviceDummyValues>();//�������ꗗ�i�_�~�[�f�[�^�j
builder.Services.AddScoped<InvoiceServicesWebAPIDummy>();//�������iWebAPI�o�R�_�~�[�f�[�^�j
builder.Services.AddScoped<InvoiceListServicesWebAPIDummy>();//�������ꗗ�iWebAPI�_�~�[�f�[�^�j

await builder.Build().RunAsync();
