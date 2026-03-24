using Npgsql;

namespace ConsoleApp2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "Host=localhost;Username=postgres;Password=postpass;Database=Products";
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            const string tablename = "myschema2.accounts";
            using(var cmd = dataSource.CreateCommand($"SELECT COUNT(ID) FROM {tablename} ;"))
            {
                Console.WriteLine(Convert.ToInt32(await cmd.ExecuteScalarAsync()));
            }
        }
    }
}
