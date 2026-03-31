using Timer = System.Timers.Timer;
namespace WebApplication1_1
{
    public class Program
    {
        private static bool IsInit = false;
        private static WebApplicationBuilder? builder = null;
        private static WebApplication? app;
        private static Task serverTask;
        public static void Main(string[] args)
        {
            builder = WebApplication.CreateBuilder(args);

            SharedData.GenerateID();
            var timer = new Timer(2000);
            timer.AutoReset = true;
            timer.Elapsed += (source, e) =>
            {
                //Console.WriteLine("timer tick");
                bool r = SharedData.CheckAvailability();
                if (!IsInit && r == false)
                {
                    SharedData.SetThisReplica(SharedData.ReplicatType.Master);
                    IsInit = true;

                    // Add services to the container.
                    #region SETPORT
                    builder.WebHost.ConfigureKestrel(opt =>
                    {
                        opt.ListenLocalhost(SharedData.PORT);
                    });
                    #endregion
                    builder.Services.AddControllers();

                    app = builder.Build();

                    // Configure the HTTP request pipeline.

                    // app.UseAuthorization();


                    // app.MapControllers("{}/{}");
                    app.MapControllerRoute(name:default,
                        pattern: "{controller=WeatherForecast}/{action=Get}");
                    Console.WriteLine("this is master");
                    app.Run();
                }
                else if (r == true && IsInit == false)
                {
                    SharedData.SetThisReplica(SharedData.ReplicatType.Slave);
                    serverTask = Task.Run(async () =>await SharedData.InitTcpListener());
                }
                timer.Stop();
            };
            timer.Enabled = true;
            // timer.Start();
            Console.ReadLine();
            // app.Run();
        }
    }
}
