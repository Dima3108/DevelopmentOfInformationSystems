using Microsoft.AspNetCore.Mvc;

namespace WebApplication1R.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        /*  public IActionResult Index()
          {
              return View();
          }*/
        /// <summary>
        /// Возвращает данные пользователя по указанному ID 
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <returns> user info</returns>
        [HttpPost]
        [Route("/[Controller]/[Action]")]
        public JsonResult GetUserInfo(string userId)
        {
            return new JsonResult("no data");
        }
        /// <summary>
        /// Пытается добавить друга по id к указанному пользователю
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="frendId">идентификатор друга</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/[Controller]/[Action]")]
        public string AddFrend(string userId,string frendId)
        {
            bool stat = true;
            string mes = "";
            try
            {

            }
            catch
            {
                stat = false;
            }
            finally
            {
                mes= (stat==true )? "друг добавлен" : "не удалось добавить";
            }
            return mes;
        }
        /// <summary>
        /// Пытается удалить друга по id к указанному пользователю
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="frendId">идентификатор друга</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/[Controller]/[Action]")]
        public string DeleteFrend(string userId,string frendId) {
            bool stat = true;
            string mes = "";
            try
            {

            }
            catch
            {
                stat = false;
            }
            finally
            {
                mes = (stat == true) ? "друг удален" : "не удалось удалить";
            }
            return mes;
        }
    }
}
