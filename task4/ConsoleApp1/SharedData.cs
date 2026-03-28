namespace ConsoleApp1
{
    public class SharedData
    {
        public enum ReplicatType
        {
            Master=0,
            Slave=1
        };
        //public static int PORT = 17;
        public const int PORT = 17;
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
                        Console.WriteLine($"matser id='{r}'");
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
