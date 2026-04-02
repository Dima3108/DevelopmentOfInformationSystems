using Microsoft.AspNetCore.Mvc;

namespace WebApplication1_1.Controllers
{
    [ApiController]
    [Route("WeatherForecast")]
    public class WeatherForecastController : ControllerBase
    {
        private static HashSet<int> sPorts = new HashSet<int>();

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
        [Route($"RegistredSlave")]
        public string RegistredSlave(int port)
        {
            Console.WriteLine($"порт {port} зарегестрирован!");
            sPorts.Add(port);
            return $"port is registred!";
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
        public async Task<IActionResult> Index()
        {
            
            var htmlContent =await  System.IO.File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory+@"\Controllers\weatherforecast.html");
            return Content(htmlContent,"text/html");
            
        }
        private readonly Random random = new Random();
        [HttpPost]
        [Route("savedata")]
        public IActionResult savedata(/*string inpname,string inpvalue*/)
        {
            string inpname =(string) HttpContext.Request.Form["inpname"];
            string inpvalue = (string)HttpContext.Request.Form["inpvalue"];
            Console.WriteLine($"['{inpname}']:'{inpvalue}'");
            int key_pos = random.Next(0, sPorts.Count);
            int port_ = sPorts.ToArray()[key_pos];
            using (HttpClient client = new HttpClient())
            {
                var values = new Dictionary<string, string>
{
    { $"token","'"+ inpname+"'" },
    { "info","'"+ inpvalue+"'" }
};
                var content = new FormUrlEncodedContent(values);
                var req = client.PostAsync($"http://localhost:{port_}/setinfo", content).Result;
                Console.WriteLine($"result='{req.Content.ReadAsStringAsync().Result}'");
            }
           return Redirect(@"Index");
        }
        [HttpGet]
        [Route("SlavesPorts")]
        public JsonResult SlavesPorts()
        {
            return new  JsonResult(sPorts.ToArray());
        }
    }
}
