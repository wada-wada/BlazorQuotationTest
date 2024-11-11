using BlazorTest.Client;
using BlazorTest.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<InvoiceServiceDummyValue>();//請求書（ダミーデータ）
builder.Services.AddScoped<InvoiceListSeriviceDummyValues>();//請求書一覧（ダミーデータ）
builder.Services.AddScoped<InvoiceServicesWebAPIDummy>();//請求書（WebAPI経由ダミーデータ）
builder.Services.AddScoped<InvoiceListServicesWebAPIDummy>();//請求書一覧（WebAPIダミーデータ）

await builder.Build().RunAsync();
