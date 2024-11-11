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

        [HttpGet("{quotationId}")]
        public async Task<ActionResult<IEnumerable<QuotationItem>>>GetQuotationItems(int quotationId)
        {
            var items = await _quotationItemService.GetQuotationItemsAsync(quotationId);
            if(items == null) return NotFound();
            return Ok(items);
        }

        [HttpGet("{quotationId}/{quotationItemId}")]
        public async Task<IActionResult> GetQuotationItemLine(int quotationId, int quotationItemId)
        {
            var quotationItemLine = await _quotationItemService.GetQuotationItemLineByIdAsync(quotationId, quotationItemId);
            if(quotationItemLine == null)
                return NotFound();
            return Ok(quotationItemLine);
        }

        [HttpPut("{quotationId}/{quotationItemId}")]
        public async Task<IActionResult> UpdateQuotationItem(int quotationItemId, [FromBody] QuotationItem updatedQuotationItem)
        {
            if(quotationItemId != updatedQuotationItem.Quotation_Item_Id)
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
    }
}
