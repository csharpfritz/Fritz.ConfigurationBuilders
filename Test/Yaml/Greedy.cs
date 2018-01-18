using Fritz.ConfigurationBuilders;
using Microsoft.Configuration.ConfigurationBuilders;
using NUnit.Framework;
using System.Collections.Specialized;
using System.Configuration;

namespace Test.Yaml
{
	[TestFixture]
	public class Greedy : BaseYamlFixture
	{

		[Test]
		public void ShouldAddMissingAppSettings()
		{

			// Arrange
			var appSettings = new AppSettingsSection();
			var coll = new NameValueCollection()
			{
				{YamlConfigurationBuilder.locationTag, configFileLocation},
				{YamlConfigurationBuilder.modeTag, KeyValueMode.Greedy.ToString() }
			};
			var sut = new YamlConfigurationBuilder();
			sut.Initialize("test", coll);

			// Act
			sut.ProcessConfigurationSection(appSettings);

			// Assert
			Assert.AreEqual(2, appSettings.Settings.Count, "Did not add more add nodes to appSettings");

			// Check for the other three settings
			for (var i = 1; i < 3; i++)
			{
				Assert.IsNotNull(appSettings.Settings[$"setting{i}"], $"Missing setting{i}");
			}


		}

		protected override string GetConfigFilename()
		{
			return "simple.yml";
		}
	}

}
