//請求書画面のWebAPIのセットアップ(請求書画面へのリンク機能追加)
using Microsoft.AspNetCore.Mvc;
using BlazorTest.Server.Models;

namespace BlazorTest.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceListLinkController : ControllerBase
    {
        private static List<InvoiceModel> Invoices = new List<InvoiceModel>
        {

                new InvoiceModel
                {
                    Id = 1,
                    InvoiceNumber = "INV-1001",
                    CustomerName = "山田太郎",
                    InvoiceDate = DateTime.Now,
                    Items = new List<InvoiceItem>
                    {
                        new InvoiceItem { Description = "商品A", Quantity = 2, UnitPrice = 1500m },
                        new InvoiceItem { Description = "商品B", Quantity = 1, UnitPrice = 2500m }
                    }
                },
                new InvoiceModel
                {
                    Id = 2,
                    InvoiceNumber = "INV-2001",
                    CustomerName = "鈴木花子",
                    InvoiceDate = DateTime.Now.AddDays(+2),
                    Items = new List<InvoiceItem>
                    {
                        new InvoiceItem { Description = "商品C", Quantity = 4, UnitPrice = 5000m },
                        new InvoiceItem { Description = "商品D", Quantity = 6, UnitPrice = 1000m }
                    }
                }

        };

        [HttpGet("{id}")]
        public ActionResult<InvoiceModel> GetInvoiceListLink(int id)
        {
            var invoiceLine = Invoices.First(x => x.Id == id);
            if(invoiceLine == null)
            {
                return NotFound();
            }
            return Ok(invoiceLine);
        }

        [HttpGet]
        public ActionResult<List<InvoiceModel>> GetInvoiceListLink()
        {
            return Ok(Invoices);
        }
    }
}
