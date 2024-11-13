using BlazorTest.Server.Models;
using BlazorTest.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTest.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotationItemsController : ControllerBase
    {
        private readonly QuotationItemService _quotationItemService;
        public QuotationItemsController(QuotationItemService service)
        {
            _quotationItemService = service;
        }

        //該当のquotationsテーブルの情報
        [HttpGet("{quotationId}/quotationinfo")]
        public async Task<ActionResult> GetQuotation(int quotationId)
        {
            var quotation = await _quotationItemService.GetQuotationAsync(quotationId);
            if (quotation == null)
                return NotFound();
            return Ok(quotation);
        }

        [HttpGet("{quotationId}")]
        public async Task<ActionResult<IEnumerable<QuotationItem>>> GetQuotationItems(int quotationId)
        {
            var items = await _quotationItemService.GetQuotationItemsAsync(quotationId);
            if (items == null) return NotFound();
            return Ok(items);
        }

        [HttpGet("{quotationId}/{quotationItemId}")]
        public async Task<IActionResult> GetQuotationItemLine(int quotationId, int quotationItemId)
        {
            var quotationItemLine = await _quotationItemService.GetQuotationItemLineByIdAsync(quotationId, quotationItemId);
            if (quotationItemLine == null)
                return NotFound();
            return Ok(quotationItemLine);
        }

        [HttpPut("{quotationId}/{quotationItemId}")]
        public async Task<IActionResult> UpdateQuotationItem(int quotationItemId, [FromBody] QuotationItem updatedQuotationItem)
        {
            if (quotationItemId != updatedQuotationItem.Quotation_Item_Id)
                return BadRequest();

            var result = await _quotationItemService.UpdateQuotationAsync(updatedQuotationItem);
            if (!result)
                return StatusCode(500, "Error updating quotationitems");

            return NoContent();
        }

        [HttpDelete("{quotationId}/{quotationItemId}")]
        public async Task<IActionResult> DeletreQuotationItem(int quotationItemId)
        {
            var result = await _quotationItemService.DeleteQuotationItemAsync(quotationItemId);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        //複数の明細を更新
        [HttpPut("{QuotationId}/saveQuotationItems")]
        public async Task<IActionResult> UpdateSaveQuotationItems(int quotationId, [FromBody] List<QuotationItem> items)
        {
            if (items == null || !items.Any())
            {
                return BadRequest("明細行が空です");
            }

            await _quotationItemService.SaveQuotationItemsAsync(quotationId, items);
            return NoContent();
        }

        //請求書名を更新
        [HttpPut("{QuotationId}/saveQuotationName")]
        public async Task<IActionResult> UpdateSaveQuotationName(int quotationId, [FromBody] Quotation quotation)
        {
            if (quotationId != quotation.Quotation_Id)
                return BadRequest();

            var result = await _quotationItemService.SaveQuotationNameAsync(quotationId, quotation);
            if (!result)
                return StatusCode(500, "Error updating quotationitemsName");
            return NoContent();
        }

        //新しい請求書明細を登録
        [HttpPost("{QuotationId}/createNewQuotationItems")]
        public async Task<IActionResult> CreateQuotationItems(int quotationId, [FromBody] List<QuotationItem> newItems)
        {
            if(newItems == null || !newItems.Any())
            {
                return BadRequest("No new items to create.");
            }

            var newItemsToCreate = newItems.Where(i => !string.IsNullOrWhiteSpace(i.Product_Name) && i.Quantity > 0 && i.Unit_Price > 0).ToList();
            if (!newItemsToCreate.Any())
            {
                return BadRequest("No Valid items to create.");
            }

            await _quotationItemService.InsertQuotationItemsAsync(quotationId, newItems);
            return Ok();
        }

        //更新、登録をトランザクションで行う
        [HttpPost("transaction")]
        public async Task<IActionResult> SaveQuotationDetails([FromBody] QuotationRequest quotationRequest)
        {
            if(quotationRequest == null || quotationRequest.Quotation == null || quotationRequest.QuotationItems == null) 
            {
                return BadRequest("QuotationRequest, Quotation or QuotationItems cannot be null.");
            }
            var result = await _quotationItemService.SaveQuotationDetailsAsync(quotationRequest.Quotation, quotationRequest.QuotationItems);
            return result ? Ok() : StatusCode(500, "Erroe in saving data");
        }
    }
}
