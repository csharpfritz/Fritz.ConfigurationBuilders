using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace Test.Ini
{
	public abstract class BaseIniFixture
	{

		protected string iniFileLocation { get; private set; }

		[OneTimeSetUp]
		public void Setup()
		{

			iniFileLocation = Path.GetTempFileName();

			using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.Ini.simple.ini"))
			{
				using (var file = new FileStream(iniFileLocation, FileMode.Create, FileAccess.Write))
				{
					resource.CopyTo(file);
				}
			}

		}

		[OneTimeTearDown]
		public void Teardown()
		{
			File.Delete(iniFileLocation);
		}

	}

}
