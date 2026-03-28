using Timer = System.Timers.Timer;
namespace WebApplication1
{
    public class Program
    {
        private static Timer timer;
        private static WebApplication app;
        private static WebApplicationBuilder builder;
        private static bool THISISMASTER = false;
        public static void Main(string[] args)
        {
            timer = new Timer(20000);
            timer.AutoReset = true;
            timer.Elapsed += (source, e) =>
            {
                Console.WriteLine("timer tick");
                if (!THISISMASTER && SharedData.CheckAvailability() == false)
                {

                    THISISMASTER = true;
                    builder = WebApplication.CreateBuilder(args);
                    // Add services to the container.
                    builder.WebHost.ConfigureKestrel(options =>
                    {
                        options.ListenAnyIP(SharedData.PORT);
                    });
                    builder.Services.AddControllers();

                    app = builder.Build();

                    // Configure the HTTP request pipeline.

                    app.UseHttpsRedirection();

                    app.UseAuthorization();


                    app.MapControllers();


                    Console.WriteLine("this is master");
                    app.Run();
                }
            };
            SharedData.GenerateID();
            timer.Enabled = true;


            // timer.Start();
            timer.Enabled = true;
            Console.ReadLine();
            // app.Run();
        }
    }
}
