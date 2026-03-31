using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Net;
using System.Net.Sockets;

namespace WebApplication1_1
{
    public class SharedData
    {
        private static Dictionary<string, string> _cache=new Dictionary<string, string>();
        public enum ReplicatType
        {
            Master=0,
            Slave=1
        };
        //public static int PORT = 17;
        public const int PORT = 5178;
        public static ReplicatType this_type { get; private set; }
        public static void SetThisReplica(ReplicatType type)
        {
            this_type = type;   
        }
        public static string ThisID { get; private set; }   
        public static void GenerateID()
        {
            byte[] j = new byte[256];
            DateTime now = DateTime.Now;    
            (new Random(now.Second*1000000+now.Millisecond+60*now.Minute*1000000)).NextBytes(j);
            ThisID=Convert.ToBase64String(j);   
        }
        //public static TcpListener listener;
        private static WebApplicationBuilder locServer;
        public static async Task InitTcpListener()
        {
            locServer = WebApplication.CreateBuilder();
            locServer.WebHost.ConfigureKestrel(options =>
            {
                options.ListenLocalhost(0);
            });
            var app=locServer.Build();
            app.MapPost("setinfo", (string token,string info) =>
            {
                Console.WriteLine(info);
                _cache[token] = info;
                return "succes";
            });
            app.MapGet("getinfo", (string token) =>
            {
                return _cache[token];
            });
            await app.RunAsync();
            Console.WriteLine("loc server run");
            var server = app.Services.GetRequiredService<IServer>();
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            int port_ = 0;
            // Iterate through the addresses to get the ports
            foreach (var address in addressFeature.Addresses)
            {
                Console.WriteLine($"Kestrel is listening on: {address}");
                // You can parse the URI to get the port number
                var uri = new Uri(address);
                Console.WriteLine($"Port: {uri.Port}");
                port_ = Convert.ToInt32(uri.Port);  
            }

            // listener = new TcpListener(0);
            // listener.Start(3);
            //int port_=locServer.WebHost.
            using (HttpClient cl=new HttpClient())
            {
                string r =await cl.GetStringAsync($"http://localhost:{PORT}/WeatherForecast/RegistredSlave?port={port_}");
                Console.WriteLine($"returned ='{r}'");
            }
            await app.WaitForShutdownAsync();
            await Task.Delay(-1);
        }
        public static bool CheckAvailability()
        {
            try
            {
               using (HttpClient client = new HttpClient())
                {
                  var r=  client.GetStringAsync($"http://localhost:{PORT}/WeatherForecast/GetID").Result;
                    if (Convert.FromBase64String(r).Length != Convert.FromBase64String(ThisID).Length)
                    {
                        return false;
                    }
                    else
                    {
                       // Console.WriteLine($"matser id='{r}'");
return true;
                    }
                }
                
            }
            catch
            {
                return false;
            }
        }

    }
}
