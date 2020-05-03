using HttpClientLibGenerator;
using System;
using System.Threading.Tasks;

namespace CodeGenerator
{

    [HttpClientLibAttribute("test")]
    interface IUsersClient 
    {
        [RequestMapping("/api/items")]
        Task<string> GetAsync();
    }

    class Program
    {
        async static Task Main(string[] args)
        {
            Console.WriteLine("hello world !");

            //TODO : Resolve using DI
            var usersClient = new UsersClient();

            var data = await usersClient.GetAsync();
            
            Console.ReadKey();
        }
    }
}
