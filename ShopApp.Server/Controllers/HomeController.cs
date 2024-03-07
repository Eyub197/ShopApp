using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Server.Data;
using ShopApp.Server.Models;

namespace ShopApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private ShopAppDbContext context;

        public HomeController(ShopAppDbContext _context)
        {
            context = _context;
        }

        [HttpGet(Name ="Index")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<Product>>> Index()
        {
            var products=await context.Products.ToListAsync();

            return Ok(products);
        }
    }
}
