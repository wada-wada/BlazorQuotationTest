using BlazorTest.Client.Models;
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
    }
}
