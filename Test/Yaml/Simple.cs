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
	public class Simple : BaseYamlFixture
	{
		protected override string GetConfigFilename()
		{
			return "simple.yml";
		}

		[Test]
		public void ShouldReadSettingsFromFile()
		{

			// arrange
			var appSettings = new AppSettingsSection();
			appSettings.Settings.Add("setting1", "inlineValue");
			var coll = new NameValueCollection()
			{
				{YamlConfigurationBuilder.locationTag, configFileLocation}
			};
			var sut = new YamlConfigurationBuilder();
			sut.Initialize("test", coll);

			// act
			sut.ProcessConfigurationSection(appSettings);

			// Assert
			Assert.AreEqual("value1", appSettings.Settings["setting1"].Value, 
				"Did not load the value1 setting into the AppSettingsSection");

		}

	}

}
