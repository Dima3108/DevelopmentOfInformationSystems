using System.Linq;
using System.Data.Common;
using System.Data.SqlTypes;
namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] names = { "users1", "users2", "users3" };
            string[] mails = { "@mail.yand", "rt@mail.ru", "pr@mail.com" };
            string command = "INSERT INTO users(name,email) ";
            int LEN = 10000;
            for(int i = 0; i < LEN; i++)
            {
                command += $"('{names[i % names.Length]}{i}','us{i}{mails[i%mails.Length]}')";
                if (i != LEN)
                {
                    command += ",";
                }
                else
                {
                    command += ";";
                }
            }
            File.WriteAllText("com.sql",command);
        }
    }
}
