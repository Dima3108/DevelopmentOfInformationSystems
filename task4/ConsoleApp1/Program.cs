using Timer = System.Timers.Timer;
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var timer = new Timer(2000);
            timer.AutoReset = true;
           
            timer.Elapsed += (source, e) =>
            {
                Console.WriteLine("timer tick");
                if (SharedData.CheckAvailability() == false)
                {




                    Console.WriteLine("this is master");
                  //  app.Run();
                }
            }; timer.Enabled = true;
            // timer.Start();
            //Task.Delay(-1);
            Console.ReadLine();
        }
    }
}
