//請求書サービス（ダミーデータ）
using BlazorTest.Client.Models;

namespace BlazorTest.Client.Services
{
    public class InvoiceServiceDummyValue
    {
        public InvoiceModel GetSampleInvoice()
        {
            return new InvoiceModel
            {
                InvoiceNumber = "INV-1001",
                InvoiceDate = DateTime.Now,
                CustomerName = "山田太郎",
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem { Description = "商品A", Quantity = 2, UnitPrice = 1500m },
                    new InvoiceItem { Description = "商品B", Quantity = 1, UnitPrice = 2500m }
                }
            };
        }

        public List<InvoiceModel> GetSampleInvoices()
        {
            return new List<InvoiceModel>
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
        }
    }
}
