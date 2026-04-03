using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;

namespace WebApplication1_1
{
    public class SharedData
    {
        private static Dictionary<string, string> _cache = new Dictionary<string, string>();
        public enum ReplicatType
        {
            Master = 0,
            Slave = 1
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
            (new Random(now.Second * 1000000 + now.Millisecond + 60 * now.Minute * 1000000)).NextBytes(j);
            ThisID = Convert.ToBase64String(j);
        }
        //public static TcpListener listener;
        private static WebApplicationBuilder locServer;
        private static WebApplication locapp;
        private static int port_;
        public static void SendPortToMaster()
        {
            using (HttpClient cl = new HttpClient())
            {
                var res = cl.GetAsync($"http://localhost:{PORT}/WeatherForecast/RegistredSlave?port={port_}").Result;
                var r = res.Content.ReadAsStringAsync().Result;
                if (r == "port is registred!")
                {
                    Console.WriteLine($"returned ='{r}'");
                }

            }
        }
        public static void InitTcpListener()
        {
            //  return Task.Run( () =>
            // {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endpoint = new IPEndPoint(ip, 0);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endpoint);

            // Получаем назначенный системой порт
            IPEndPoint boundEndPoint = (IPEndPoint)socket.LocalEndPoint;
            Console.WriteLine($"Доступный порт: {boundEndPoint.Port}");
            port_ = boundEndPoint.Port;

            locServer = WebApplication.CreateBuilder();
            locServer.WebHost.UseUrls("http://*:0");
            Console.WriteLine("server create");

            /*locServer.Services.Configure<SocketTransportOptions>(options =>
            {
                options.CreateBoundListenSocket = (enpoint) =>
                {
                    return socket;
                };
            });*/
            Console.WriteLine("server port config");
            locapp = locServer.Build();
            Console.WriteLine("sever build");
            /*var server = locapp.Services.GetRequiredService<IServer>();
                            var addressFeature = server.Features.Get<IServerAddressesFeature>();
                          */
            // Iterate through the addresses to get the ports


            locapp.MapPost("setinfo", (string token, string info) =>
            {
                Console.WriteLine(info);
                _cache[token] = info;
                return $"succes {SharedData.ThisID}";
            });
            locapp.MapGet("getinfo", (string token) =>
            {
                return _cache[token];
            });
            locapp.MapGet("getall", () =>
            {
                var arr_ = _cache.ToArray();
                Models.UserDataModel[]mod=new Models.UserDataModel[arr_.Length];
                Parallel.For(0, arr_.Length, i =>
                {
                    mod[i] = new Models.UserDataModel(arr_[i].Key, arr_[i].Value);
                });
                return System.Text.Json.JsonSerializer.Serialize(mod);
            });
            locapp.Start();
            Console.WriteLine("loc server run");
            var server = locapp.Services.GetRequiredService<IServer>();
            var addressFeature = server.Features.Get<IServerAddressesFeature>();

            foreach (var address in addressFeature.Addresses)
            {
                Console.WriteLine($"Kestrel is listening on: {address}");
                var uri = new Uri(address);
                port_ = uri.Port;
            }
            SendPortToMaster();
            // listener = new TcpListener(0);
            // listener.Start(3);
            //int port_=locServer.WebHost.

            locapp.WaitForShutdown();
            // await Task.Delay(-1);
            //  });

        }
        public static bool CheckAvailability()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var r = client.GetStringAsync($"http://localhost:{PORT}/WeatherForecast/GetID").Result;
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
