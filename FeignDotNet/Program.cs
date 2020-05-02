using Refit;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RefitSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var usersClient = RestService.For<IUsersClient>("http://localhost:3000");

            var response = usersClient.Get().GetAwaiter().GetResult();

            Console.ReadKey();
        }
    }

    public interface IUsersClient
    {
        [Get("/users")]
        Task<List<User>> Get();
    }

    public class User
    {
       public int Id { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string Status { get; set; }
    }
}
