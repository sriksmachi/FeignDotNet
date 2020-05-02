using HttpClientLibGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Runtime.InteropServices;

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
            Console.ReadKey();
        }
    }
}
