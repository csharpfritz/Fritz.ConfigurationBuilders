using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace Test.Ini
{
	public abstract class BaseIniFixture : BaseFixture
	{

		protected string iniFileLocation { get { return configFileLocation; } }

		protected override string GetConfigFilename()
		{
			return GetIniFilename();
		}

		protected abstract string GetIniFilename();

		protected override string GetFolder()
		{
			return "Ini";
		}

	}

}
