using HttpClientLibGenerator;
using System;

namespace CodeGenerator
{

    [HttpClientLibAttribute("test")]
    interface IUsersClient 
    {
        [RequestMapping("/api/items")]
        string Get();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world !");

            //TODO : Resolve using DI
            var usersClient = new UsersClient();

            var data = usersClient.Get();
            
            Console.ReadKey();
        }
    }
}
