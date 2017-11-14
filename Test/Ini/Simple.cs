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

	[TestFixture]
	public class Simple : BaseIniFixture
	{

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
