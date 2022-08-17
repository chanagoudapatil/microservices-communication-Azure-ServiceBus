using Inventory.Data;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var data = MyData.Data;
            return Ok(data);
        }
    }
}
