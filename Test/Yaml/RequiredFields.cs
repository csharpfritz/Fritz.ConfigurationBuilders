using Fritz.ConfigurationBuilders;
using NUnit.Framework;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Test.Yaml
{

	[TestFixture]
	public class RequiredFields
	{

		[Test()]
		public void MissingLocationAttributeShouldThrowException()
		{

			// arrange
			var appSettings = new AppSettingsSection();
			var coll = new NameValueCollection();
			var sut = new YamlConfigurationBuilder();

			// act - assert
			Assert.Throws<ArgumentNullException>(() => {
					sut.Initialize("test", coll);
				},
				"Did not throw an exception due to a missing config file location");

		}

	}

}
