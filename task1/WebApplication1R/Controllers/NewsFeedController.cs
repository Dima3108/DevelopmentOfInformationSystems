using Microsoft.AspNetCore.Mvc;

namespace WebApplication1R.Controllers
{
    [ApiController]

    public class NewsFeedController:Microsoft.AspNetCore.Mvc.ControllerBase

    {
        [HttpPost]
        public IActionResult CreateNews(string news,string userId)
        {
            return NotFound();
        }
    }
}
