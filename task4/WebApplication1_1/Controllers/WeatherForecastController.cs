using Microsoft.AspNetCore.Mvc;

namespace WebApplication1_1.Controllers
{
    [ApiController]
    [Route("WeatherForecast")]
    public class WeatherForecastController : ControllerBase
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("RegistredSlave/{port}")]
        public string RegistredSlave(int port)
        {
            Console.WriteLine($"порт {port} зарегестрирован!");
            return "";
        }
        [HttpGet]
        [Route("GetId")]
        public string GetID()
        {
            return SharedData.ThisID;
        }
        [HttpGet]
        [Route("Get")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        [Route($"Index")]
        public IActionResult Index()
        {
            var htmlContent = System.IO.File.ReadAllText("/Controllers/weatherforecast.html");
            return Content(htmlContent,"text/html");
        }
        [HttpPost]
        [Route("savedata/{inpname}/{inpvalue}")]
        public IActionResult savedata(string inpname,string inpvalue)
        {
           return Redirect("/WeatherForecast/Index");
        }
    }
}
