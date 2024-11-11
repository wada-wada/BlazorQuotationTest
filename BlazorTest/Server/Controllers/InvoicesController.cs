//請求書画面のWebAPIのセットアップ
using BlazorTest.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTest.Server.Controllers
{   
    [ApiController]
    [Route("api/Invoices")]

    public class InvoicesContoroller : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInvoices()
        {
            var invoices = new List<InvoiceModel>
            {
                new InvoiceModel
                {
                    InvoiceNumber= "INV-1001",
                    CustomerName = "山田太郎",
                    InvoiceDate = DateTime.Now,
                    Items = new List<InvoiceItem>
                    {
                        new InvoiceItem { Description = "商品A", Quantity = 2, UnitPrice = 1500m },
                        new InvoiceItem { Description = "商品B", Quantity = 1, UnitPrice = 2500m }
                    }
                }
            };
            return Ok(invoices);
        }
    }
}
