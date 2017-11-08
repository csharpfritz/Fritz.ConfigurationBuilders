using Fritz.ConfigurationBuilders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Ini
{

	[TestFixture]
	public class Initialize
	{
		private static readonly string fakeFileLocation = Path.GetTempFileName();

		[OneTimeTearDown]
		public void Teardown()
		{
			File.Delete(fakeFileLocation);
		}

		[Test]
		public void ThrowsExceptionWhenMissingLocation()
		{

			// Arrange
			var sut = new IniConfigurationBuilder();
			var coll = new NameValueCollection();

			// Act
			try
			{
				sut.Initialize("test", coll);
			} catch (ConfigurationErrorsException)
			{
				// do nothing, expected result
				return;
			}

			// Assert
			Assert.Fail("Did not throw a ConfigurationErrorsException for a missing location");

		}

		[Test]
		public void CaptureLocationAndNameInformation()
		{

			// Arrange
			var sut = new IniConfigurationBuilder();
			var coll = new NameValueCollection
			{
				{ IniConfigurationBuilder.locationTag, fakeFileLocation }
			};

			// Act
			sut.Initialize("test", coll);

			// Assert
			Assert.AreEqual("test", sut.Name, "Did not capture the name of the ConfigurationBuilder");
			Assert.AreEqual(fakeFileLocation, sut.Location, "Did not capture the ini file location");

		}

	}

}
