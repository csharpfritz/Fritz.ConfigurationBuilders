using Fritz.ConfigurationBuilders;
using Microsoft.Configuration.ConfigurationBuilders;
using NUnit.Framework;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;

namespace Test.Ini
{
	[TestFixture]
	public class GreedyWithIniSections : BaseIniFixture
	{

		private NameValueCollection Settings;

		protected override void OneTimeSetup()
		{

			Settings = new NameValueCollection()
			{
				{IniConfigurationBuilder.locationTag, iniFileLocation},
				{IniConfigurationBuilder.modeTag, KeyValueMode.Greedy.ToString() },
				{IniConfigurationBuilder.sectionTag, "appSettings" }
			};

		}


		[Test]
		public void ShouldApplyAppSettingsToAppSettings()
		{

			// Arrange
			var appSettings = new AppSettingsSection();
			var sut = new IniConfigurationBuilder();
			sut.Initialize("test", Settings);

			// Act
			sut.ProcessConfigurationSection(appSettings);

			// Assert
			Assert.AreEqual(3, appSettings.Settings.Count, "Did not add more add nodes to appSettings");

		}

		protected override string GetIniFilename()
		{
			return "manyAppSettings.ini";
		}

	}

}
