//請求書画面のWebAPIのセットアップ
using BlazorTest.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTest.Server.Controllers
{   
    [ApiController]
    [Route("api/Invoices")]

    public class InvoicesLinkContoroller : ControllerBase
    {
        private static List<InvoiceModel> InvoiceDetail = new List<InvoiceModel>()
        {

                new InvoiceModel
                {
                    Id =1,
                    InvoiceNumber= "INV-1001",
                    CustomerName = "山田太郎",
                    InvoiceDate = DateTime.Now,
                    Items = new List<InvoiceItem>
                    {
                        new InvoiceItem { Id = 1, Description = "商品A", Quantity = 2, UnitPrice = 1500m },
                        new InvoiceItem { Id = 2, Description = "商品B", Quantity = 1, UnitPrice = 2500m }
                    }
                },
                new InvoiceModel
                {
                    Id = 2,
                    InvoiceNumber = "INV-2001",
                    CustomerName = "鈴木花子A",
                    InvoiceDate = DateTime.Now.AddDays(+2),
                    Items = new List<InvoiceItem>
                    {
                        new InvoiceItem { Id = 1, Description = "商品C", Quantity = 4, UnitPrice = 5000m },
                        new InvoiceItem { Id = 2, Description = "商品D", Quantity = 6, UnitPrice = 1000m }
                    }
                }
        };

        [HttpGet("{id}")]
        public ActionResult<InvoiceModel> GetInvoiceDetail(int id)
        {
            var invoiceDetail = InvoiceDetail.FirstOrDefault(x => x.Id == id);
            if(invoiceDetail == null)
            {
                return NotFound();
            }
            return Ok(invoiceDetail);
        } 
    }
}
