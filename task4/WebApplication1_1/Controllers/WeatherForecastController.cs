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
            if (sPorts.Contains(port) == false)
            {
                Console.WriteLine($"порт {port} зарегестрирован!");
                sPorts.Add(port);

                return $"port is registred!";
            }
            else
            {
                return $"the port is already registered";
            }
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
                 string apof = '"'.ToString();
                var values = new Dictionary<string, string>
{
    { $"token",apof+ inpname+apof },
    { "info","'"+ inpvalue+"'" }
};
                var content = new FormUrlEncodedContent(values);
                string query = $"?token={apof}{inpname}{apof}&info={apof}{inpvalue}{apof}";
                var req = client.PostAsync($"http://localhost:{port_}/setinfo"+query,null).Result;
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
        [HttpGet]
        [Route("GetAllData")]
        public string GetAllData()
        {
          var slavesContents=new Models.UserDataModel[sPorts.Count][];
            int[] ports_ = sPorts.ToArray();
            Task.Run(async () => {
                for (int i = 0; i < ports_.Length; i++)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var res = await client.GetStringAsync($"http://localhost:{ports_[i]}/getall");
                        slavesContents[i] = System.Text.Json.JsonSerializer.Deserialize<Models.UserDataModel[]>(res);
                       /* if (res != null)
                        {
                            await Task.Run(() =>
                            {
                                foreach(var item in res)
                                    slavesContents[i].Add(item.Key, item.Value);    
                            });
                        }*/
                    }
                }
            }).Wait();
            var total = new List<Models.UserDataModel>();
            foreach(var slav in slavesContents)
            {
                foreach(var item in slav)
                    total.Add(item);
            }
            return System.Text.Json.JsonSerializer.Serialize(total.ToArray());
        }
    }
}
