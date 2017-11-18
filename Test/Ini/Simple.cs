using Fritz.ConfigurationBuilders;
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

	[TestFixture]
	public class Simple : BaseIniFixture
	{

		protected override string GetIniFilename()
		{
			return "simple.ini";
		}

		[Test]
		public void AppliesSetting()
		{

			// Arrange
			var appSettings = new AppSettingsSection();
			appSettings.Settings.Add("setting", "inlineValue");
			var coll = new NameValueCollection()
			{
				{IniConfigurationBuilder.locationTag, iniFileLocation}
			};
			var sut = new IniConfigurationBuilder();
			sut.Initialize("test", coll);

			// Act
			sut.ProcessConfigurationSection(appSettings);

			// Assert
			Assert.AreEqual("CorrectTestResult", appSettings.Settings["setting"].Value, "Did not set the value properly on the value");

		}


		[Test]
		public void DoesNotApplySettingIfNoSettingFound()
		{

			// Arrange
			var appSettings = new AppSettingsSection();
			appSettings.Settings.Add("NotFoundSetting", "inlineValue");
			var coll = new NameValueCollection()
			{
				{IniConfigurationBuilder.locationTag, iniFileLocation}
			};
			var sut = new IniConfigurationBuilder();
			sut.Initialize("test", coll);

			// Act
			sut.ProcessConfigurationSection(appSettings);

			// Assert
			Assert.AreEqual("inlineValue", appSettings.Settings["NotFoundSetting"].Value, "Changed the value when it shouldn't have");

		}


	}

}
