using Microsoft.AspNetCore.Mvc;

namespace WebApplication1R.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class NewsFeedController:Microsoft.AspNetCore.Mvc.ControllerBase

    {
        [HttpPost]
        [Route("/[Controller]/[Action]")]
        public string CreateNews(string news,string userId)
        {
            return "";
        }
    }
}
