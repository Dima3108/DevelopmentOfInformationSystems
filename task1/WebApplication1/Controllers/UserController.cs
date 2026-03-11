using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    //[ApiController]
   // [Route("api/[controller]")]
   // [Produces("application/json")]
    public class UserController : Controller
    {
        /// <summary>
        /// Возвращает главную общую страницу
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
