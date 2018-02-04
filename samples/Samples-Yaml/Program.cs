using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples_Yaml
{
	class Program
	{
		static void Main(string[] args)
		{

			var keys = ConfigurationManager.AppSettings.AllKeys;

			Console.WriteLine($"Here are my current {keys.Length}: AppSettings");

			foreach (var item in keys)
			{

				Console.WriteLine($"{item}: {ConfigurationManager.AppSettings[item]}");

			}

			Console.ReadLine();


		}
	}
}
