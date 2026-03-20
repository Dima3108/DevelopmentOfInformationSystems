//chace server
Dictionary<int,string>cahceStor=new Dictionary<int,string>();   
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
Random random = new Random();
app.MapGet("/data", (int id) =>
{
    if (cahceStor.ContainsKey(id))
    {
        return cahceStor[id];
    }
    else
    {
        if (cahceStor.Keys.Count > 100)
        {
            var keys_=cahceStor.Keys.ToList();
            for(int i=90;i<keys_.Count;i++)
                cahceStor.Remove(keys_[i]);
        }
        byte[]d=new byte[32];
        random.NextBytes(d);
        cahceStor[id]=Convert.ToBase64String(d);
        return $"данные отсутувуют для id={id}";
    }
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
