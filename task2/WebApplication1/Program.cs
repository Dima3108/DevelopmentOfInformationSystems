var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("введите порт");
int portId = Convert.ToInt32(Console.ReadLine());
// Add services to the container.
builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    var kestrelSection = context.Configuration.GetSection("Kestrel");
   /* serverOptions.Configure(kestrelSection).Endpoint("HTTP", listenOptions =>
    {
        listenOptions.ListenOptions.
    });*/
   serverOptions.Listen(System.Net.IPAddress.Any, portId);  
});
Console.WriteLine("¬ведите порт на который нужно отправл€ть запрос");
int respPOrt= Convert.ToInt32(Console.ReadLine());  
var app = builder.Build();

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

/*app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});*/
int thrId = Thread.GetCurrentProcessorId();
Thread reqThread = new Thread(() =>
{

Task.Run(async () =>

    {
string connection = $"http://localhost:{respPOrt}/info";
    using (HttpClient client = new HttpClient())
        { while (true)
                {
            try
            {
               
                    Console.WriteLine(await client.GetStringAsync(connection));

                    Task.Delay(100);
                
            }
            catch
            {
                Console.WriteLine("произошла ошибка");
            }}
        }
    }).Wait();
});
reqThread.IsBackground = true;
reqThread.Start();
app.MapGet("/info", () => $"service {portId} {DateTime.Now.ToString()}");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
