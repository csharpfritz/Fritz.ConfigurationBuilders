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

		private XmlDocument AppSettings;
		private NameValueCollection Settings;
		private XmlDocument ConnectionStrings;

		protected override void OneTimeSetup()
		{

			//AppSettings = new XmlDocument();
			//AppSettings.LoadXml(@"<appSettings><add key=""setting"" value=""inlineValue"" /></appSettings>");

			//ConnectionStrings = new XmlDocument();
			//ConnectionStrings.LoadXml(@"<connectionStrings></connectionStrings>");

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

		[Test]
		public void ShouldNotApplyAppSettingsToConnectionStrings()
		{

			// Arrange
			var connStrings = new ConnectionStringsSection();
			var sut = new IniConfigurationBuilder();
			sut.Initialize("test", Settings); 

			// Act
			sut.ProcessConfigurationSection(connStrings);

			// Assert  
			Assert.AreEqual(0, connStrings.ConnectionStrings.Count, "Added connectionstring when not directed to");


		}

		protected override string GetIniFilename()
		{
			return "manyAppSettings.ini";
		}

	}

}
