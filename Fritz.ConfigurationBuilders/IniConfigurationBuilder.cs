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
		public const string modeTag = "mode";

		public string Location { get; private set; }
		public IniData IniData { get; private set; }
		public KeyValueMode Mode { get; private set; } = KeyValueMode.Strict;

		public override void Initialize(string name, NameValueCollection config)
		{

			base.Initialize(name, config);

			if (config[locationTag] == null || string.IsNullOrEmpty(config[locationTag]))
			{
				throw new ConfigurationErrorsException($"Missing {locationTag} attribute when initializing ConfigurationBuilder {name}");
			}
			this.Location = config[locationTag];

			if (Enum.TryParse<KeyValueMode>(config[modeTag], out var result))
			{
				this.Mode = result;
			}

			var parser = new FileIniDataParser();
			this.IniData = parser.ReadFile(this.Location);

		}

		public override XmlNode ProcessRawXml(XmlNode rawXml)
		{

			var outNode = rawXml;

			switch (Mode)
			{
				case KeyValueMode.Strict:
					ReplaceStrict(rawXml);
					break;
				case KeyValueMode.Greedy:
					ReplaceGreedy(rawXml);
					break;
			}

			return outNode;

		}

		private void ReplaceGreedy(XmlNode rawXml)
		{

			foreach (var item in IniData.Global)
			{

				if (rawXml.SelectSingleNode($"add[@key='{item.KeyName}']") != null)
				{
					rawXml.SelectSingleNode($"add[@key='{item.KeyName}']").Value = item.Value;
				} else
				{
					var newElement = rawXml.OwnerDocument.CreateElement("add");
					newElement.Attributes.SetNamedItem(rawXml.OwnerDocument.CreateAttribute("key"));
					newElement.Attributes.SetNamedItem(rawXml.OwnerDocument.CreateAttribute("value"));
					newElement.Attributes["key"].Value = item.KeyName;
					newElement.Attributes["value"].Value = item.Value;
					rawXml.AppendChild(newElement);
				}

			}

		}

		private void ReplaceStrict(XmlNode rawXml)
		{
			var keys = rawXml.SelectNodes(@"add[@key]");
			for (var i = 0; i < keys.Count; i++)
			{

				var thisKey = keys[i].Attributes["key"].Value.ToLowerInvariant();

				if (this.IniData.Global[thisKey] != null)
				{
					keys[i].Attributes["value"].Value = this.IniData.Global[thisKey];
				}

			}
		}
	}

}
