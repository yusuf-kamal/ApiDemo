using ApiDemo.Response_Module;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context)
        {
            _context = context;
        }
        [HttpGet("Test Text")]
        [Authorize]
        public ActionResult<string> GetText()
        {
            return "Some Text";
        }

        [HttpGet("NotFound")]

        public ActionResult GetNotFoundRequest()
        {
            var anything = _context.Products.Find(1000);
            if(anything is null)
                return NotFound(new ApiResponse(400));
            return Ok();
        }

        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
    }
}
