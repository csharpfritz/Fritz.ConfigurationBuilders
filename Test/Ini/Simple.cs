using Fritz.ConfigurationBuilders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Test.Ini
{

	[TestFixture]
	public class Simple
	{

		private static readonly string iniFileLocation;

		static Simple()
		{

			iniFileLocation = Path.GetTempFileName();

		}

		[OneTimeSetUp]
		public void Setup()
		{
			
			using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.Ini.simple.ini"))
			{
				using (var file = new FileStream(iniFileLocation, FileMode.Create, FileAccess.Write))
				{
					resource.CopyTo(file);
				}
			}

		}

		[OneTimeTearDown]
		public void Teardown()
		{
			File.Delete(iniFileLocation);
		}

		[Test]
		public void AppliesSetting()
		{

			// Arrange
			var appSettings = new XmlDocument();
			appSettings.LoadXml(@"<appSettings><add key=""setting"" value=""inlineValue"" /></appSettings>");
			var coll = new NameValueCollection()
			{
				{IniConfigurationBuilder.locationTag, iniFileLocation}
			};
			var sut = new IniConfigurationBuilder();
			sut.Initialize("test", coll);

			// Act
			var outNode = sut.ProcessRawXml(appSettings.SelectSingleNode("appSettings"));

			// Assert
			Assert.AreEqual("value", outNode.SelectSingleNode("//add[@key='setting']").Attributes["value"].Value, "Did not set the value properly on the value");

		} 


	}

}
