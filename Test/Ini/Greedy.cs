using Fritz.ConfigurationBuilders;
using Microsoft.Configuration.ConfigurationBuilders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Test.Ini
{


	public class Greedy : BaseIniFixture
	{

		[Test]
		public void ShouldAddMissingAppSettings()
		{

			// Arrange
			var appSettings = new AppSettingsSection();
			var coll = new NameValueCollection()
			{
				{IniConfigurationBuilder.locationTag, iniFileLocation},
				{IniConfigurationBuilder.modeTag, KeyValueMode.Greedy.ToString() }
			};
			var sut = new IniConfigurationBuilder();
			sut.Initialize("test", coll);

			// Act
			sut.ProcessConfigurationSection(appSettings);

			// Assert
			Assert.AreEqual(3, appSettings.Settings.Count, "Did not add more add nodes to appSettings");

			// Check for the other three settings
			for (int i=1;i<4;i++)
			{
				Assert.IsNotNull(appSettings.Settings[$"setting{i}"], $"Missing setting{i}");
			}


		}

		protected override string GetIniFilename()
		{
			return "manySettings.ini";
		}
	}

}
