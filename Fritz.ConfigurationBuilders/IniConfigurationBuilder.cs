using IniParser;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IniParser.Model;

namespace Fritz.ConfigurationBuilders
{

	public class IniConfigurationBuilder : ConfigurationBuilder
	{

		public const string locationTag = "location";

		public string Location { get; private set; }
		public IniData IniData { get; private set; }

		public override void Initialize(string name, NameValueCollection config)
		{

			base.Initialize(name, config);

			if (config[locationTag] == null || string.IsNullOrEmpty(config[locationTag]))
			{
				throw new ConfigurationErrorsException($"Missing {locationTag} attribute when initializing ConfigurationBuilder {name}");
			}
			this.Location = config[locationTag];

			var parser = new FileIniDataParser();
			this.IniData = parser.ReadFile(this.Location);

		}

		public override XmlNode ProcessRawXml(XmlNode rawXml)
		{

			var outNode = rawXml;

			var keys = rawXml.SelectNodes(@"add[@key]");
			for (var i = 0; i < keys.Count; i++)
			{

				var thisKey = keys[i].Attributes["key"].Value.ToLowerInvariant();

				if (this.IniData.Global[thisKey] != null)
				{
					keys[i].Attributes["value"].Value = this.IniData.Global[thisKey];
				}

			}

			return outNode;

		}



	}

}
