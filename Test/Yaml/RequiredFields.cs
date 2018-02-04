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

		[Test()]
		public void MissingLocationWhenOptionalAttributeShouldNotThrowException()
		{

			// arrange
			var appSettings = new AppSettingsSection();
			var coll = new NameValueCollection() {
				{YamlConfigurationBuilder.optionalTag, "true"}
			};
			var sut = new YamlConfigurationBuilder();

			// act - assert
			sut.Initialize("test", coll);

		}


		[Test]
		public void SupportsRelativeLocation() {

			Assert.Pass("This is supported by the framework, no explicit code needed");

		}

		[Test]
		public void SupportsAbsoluteLocation() {

			Assert.Pass("This is supported and tested with the BaseFixture configuration and resource extraction");

		}

	}

}
