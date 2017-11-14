using Fritz.ConfigurationBuilders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
			var appSettings = new XmlDocument();
			appSettings.LoadXml(@"<appSettings><add key=""setting"" value=""inlineValue"" /></appSettings>");
			var coll = new NameValueCollection()
			{
				{IniConfigurationBuilder.locationTag, iniFileLocation},
				{IniConfigurationBuilder.modeTag, KeyValueMode.Greedy.ToString() }
			};
			var sut = new IniConfigurationBuilder();
			sut.Initialize("test", coll);

			// Act
			var outNode = sut.ProcessRawXml(appSettings.SelectSingleNode("appSettings"));

			// Assert
			Assert.AreNotEqual(1, outNode.SelectNodes(@"//add").Count, "Did not add more add nodes to appSettings");

		}

		protected override string GetIniFilename()
		{
			return "manySettings.ini";
		}
	}

}
