using BlazorTest.Server.Services;
using Microsoft.AspNetCore.Mvc;
using BlazorTest.Server.Models;

namespace BlazorTest.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotationsController : ControllerBase
    {
        private readonly QuotationService _quotationService;
        public QuotationsController(QuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuotations()
        {
            var quotations = await _quotationService.GetQuotaionsAsync();
            return Ok(quotations);
        }

        [HttpPost]
        public async Task<IActionResult> AddQuotation(Quotation quotation)
        {
            if(quotation == null)
            {
                return BadRequest();
            }
            await _quotationService.AddQuotationAsync(quotation);
            return Ok();
        }

        //削除処理
        [HttpDelete("{quotationId}/delete")]
        public async Task<IActionResult> DeleteQuotation(int quotationId)
        {
            bool isDeleted = await _quotationService.DeleteQuotationWithItems(quotationId);
            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
