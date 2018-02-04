using Fritz.ConfigurationBuilders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Ini
{

	[TestFixture]
	public class StrictMergedSectionNames : BaseIniFixture
	{


		[Test]
		public void ShouldApplySectionName()
		{

			// Arrange
			var appSettings = new AppSettingsSection();
			appSettings.Settings.Add("appSettings_appsetting1", "inlineValue");
			appSettings.Settings.Add("appSettings_appsetting2", "inlineValue");
			var coll = new NameValueCollection()
			{
				{IniConfigurationBuilder.locationTag, iniFileLocation}
			};
			var sut = new IniConfigurationBuilder();
			sut.Initialize("test", coll);

			// Act
			sut.ProcessConfigurationSection(appSettings);

			// Assert
			Assert.AreEqual("value1", appSettings.Settings["appSettings_appsetting1"].Value, "Did not set the value properly on the value");
			Assert.AreEqual("value2", appSettings.Settings["appSettings_appsetting2"].Value, "Did not set the value properly on the value");

		}

		protected override string GetIniFilename()
		{
			return "manyAppSettings.ini";
		}
	}

}
