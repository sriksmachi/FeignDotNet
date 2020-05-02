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
    
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world !");
            GenerateHttpClient();
            Console.ReadKey();
        }

        private static void GenerateHttpClient()
        {
            
        }
    }
}
