using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace Test.Ini
{
	public abstract class BaseIniFixture
	{

		protected string iniFileLocation { get; private set; }

		protected abstract string GetIniFilename();

		protected virtual void OneTimeSetup() { }
		protected virtual void OneTimeTeardown() { }


		[OneTimeSetUp]
		public void Setup()
		{

			iniFileLocation = Path.GetTempFileName();
			var thisIniFile = GetIniFilename();

			using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Test.Ini.{thisIniFile}"))
			{
				using (var file = new FileStream(iniFileLocation, FileMode.Create, FileAccess.Write))
				{
					resource.CopyTo(file);
				}
			}

			OneTimeSetup();

		}

		[OneTimeTearDown]
		public void Teardown()
		{
			File.Delete(iniFileLocation);

			OneTimeTeardown();

		}

	}

}
