using Fritz.ConfigurationBuilders;
using Microsoft.Configuration.ConfigurationBuilders;
using NUnit.Framework;
using System.Collections.Specialized;
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

			AppSettings = new XmlDocument();
			AppSettings.LoadXml(@"<appSettings><add key=""setting"" value=""inlineValue"" /></appSettings>");

			ConnectionStrings = new XmlDocument();
			ConnectionStrings.LoadXml(@"<connectionStrings></connectionStrings>");

			Settings = new NameValueCollection()
			{
				{IniConfigurationBuilder.locationTag, iniFileLocation},
				{IniConfigurationBuilder.modeTag, KeyValueMode.Greedy.ToString() }
			};

		}


		[Test]
		public void ShouldApplyAppSettingsToAppSettings()
		{

			// Arrange
			var sut = new IniConfigurationBuilder();
			sut.Initialize("test", Settings);

			// Act
			var outNode = sut.ProcessRawXml(AppSettings.SelectSingleNode("appSettings"));

			// Assert
			Assert.AreEqual(5, outNode.SelectNodes(@"//add").Count, "Did not add more add nodes to appSettings");

			// Check for the other three settings
			Assert.IsNotNull(outNode.SelectSingleNode($@"//add[@key='setting2']"), $"Missing setting2");
			for (int i = 1; i < 4; i++)
			{
				Assert.IsNotNull(outNode.SelectSingleNode($@"//add[@key='appsetting{i}']"), $"Missing appsetting{i}");
			}


		}

		[Test]
		public void ShouldNotApplyAppSettingsToConnectionStrings()
		{

			// Arrange
			var sut = new IniConfigurationBuilder();
			sut.Initialize("test", Settings);

			// Act
			var outNode = sut.ProcessRawXml(AppSettings.SelectSingleNode("appSettings"));

			// Assert
			Assert.AreEqual(5, outNode.SelectNodes(@"//add").Count, "Did not add more add nodes to appSettings");

			// Check for the other three settings
			Assert.IsNotNull(outNode.SelectSingleNode($@"//add[@key='setting2']"), $"Missing setting2");
			for (int i = 1; i < 4; i++)
			{
				Assert.IsNotNull(outNode.SelectSingleNode($@"//add[@key='appsetting{i}']"), $"Missing appsetting{i}");
			}


		}

		protected override string GetIniFilename()
		{
			return "manyAppSettings.ini";
		}

	}

}
