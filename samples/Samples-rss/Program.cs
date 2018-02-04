using System;
using System.Configuration;

namespace Samples_rss
{
    class Program
    {
        static void Main(string[] args)
        {
            var keys = ConfigurationManager.AppSettings.AllKeys;

            Console.WriteLine("Here are my current AppSettings:");

            foreach (var item in keys)
            {
                Console.WriteLine($"{item}: {ConfigurationManager.AppSettings[item]}");
            }

            Console.ReadLine();
        }
    }
}
