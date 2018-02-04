using Fritz.ConfigurationBuilders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Yaml
{

	[TestFixture]
	public class WhenSectionConfigured : BaseYamlFixture
	{

		protected override string GetConfigFilename()
		{
			return "sections.yml";
		}

		[Test]
		public void ShouldReadOnlyFromThatSection()
		{

			// arrange
			var appSettings = new AppSettingsSection();
			appSettings.Settings.Add("setting1", "inlineValue");
			var coll = new NameValueCollection()
			{
				{YamlConfigurationBuilder.locationTag, configFileLocation},
				{YamlConfigurationBuilder.sectionTag, "appSettings" }
			};
			var sut = new YamlConfigurationBuilder();
			sut.Initialize("test", coll);

			// act
			sut.ProcessConfigurationSection(appSettings);

			// Assert
			Assert.AreEqual("value1", appSettings.Settings["setting1"].Value,
				"Did not load the value1 setting into the AppSettingsSection");

		}

		[Test]
		public void ShouldReadAllValuesFromOnlyTheSection() {

			// arrange
			var appSettings = new AppSettingsSection();
			appSettings.Settings.Add("setting1", "inlineValue");
			var coll = new NameValueCollection()
			{
				{YamlConfigurationBuilder.locationTag, configFileLocation},
				{YamlConfigurationBuilder.sectionTag, "appSettings"},
				{"mode", "greedy" }
			};
			var sut = new YamlConfigurationBuilder();
			sut.Initialize("test", coll);

			// act
			sut.ProcessConfigurationSection(appSettings);
			var keys = appSettings.Settings.AllKeys;

			// assert
			Assert.AreEqual(2, keys.Count(), "Did not find both keys");



		}

	}

}
