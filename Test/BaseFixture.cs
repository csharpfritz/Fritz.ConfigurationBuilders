using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
	public abstract class BaseFixture
	{

		protected string configFileLocation { get; private set; }

		protected abstract string GetFolder();

		protected abstract string GetConfigFilename();

		protected virtual void OneTimeSetup() { }
		protected virtual void OneTimeTeardown() { }


		[OneTimeSetUp]
		public void Setup()
		{

			configFileLocation = Path.GetTempFileName();
			var thisIniFile = GetConfigFilename();

			using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Test.{GetFolder()}.{thisIniFile}"))
			{
				using (var file = new FileStream(configFileLocation, FileMode.Create, FileAccess.Write))
				{
					resource.CopyTo(file);
				}
			}

			OneTimeSetup();

		}

		[OneTimeTearDown]
		public void Teardown()
		{
			File.Delete(configFileLocation);

			OneTimeTeardown();

		}

	}
}
