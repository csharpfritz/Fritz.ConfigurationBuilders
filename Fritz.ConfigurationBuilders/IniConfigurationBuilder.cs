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
using Microsoft.Configuration.ConfigurationBuilders;

namespace Fritz.ConfigurationBuilders
{

	public class IniConfigurationBuilder : KeyValueConfigBuilder
	{

		public const string locationTag = "location";
		public const string sectionTag = "inisection";
		private const string GlobalSection = "(global)";

		public string Location { get; private set; }
		public IniData IniData { get; private set; }

		public string IniSection { get; private set; } = GlobalSection;


		public override void Initialize(string name, NameValueCollection config)
		{

			base.Initialize(name, config);

			if (config[locationTag] == null || string.IsNullOrEmpty(config[locationTag]))
			{
				throw new ConfigurationErrorsException($"Missing {locationTag} attribute when initializing ConfigurationBuilder {name}");
			}
			this.Location = config[locationTag]; 

			if (config[sectionTag] != null)
			{
				this.IniSection = config[sectionTag];
			}

			var parser = new FileIniDataParser();
			this.IniData = parser.ReadFile(this.Location);

		}

		public override ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
		{

			var outList = new Dictionary<string, string>();

			var sectionToSearch = IniSection == GlobalSection ? IniData.Global : IniData.Sections[IniSection];
			
			foreach (var item in sectionToSearch)
			{
				outList.Add(item.KeyName, item.Value);
			}

			return outList;


		}

		public override string GetValue(string key)
		{

			if (!String.IsNullOrEmpty(IniData.Global[key])) {
				return IniData.Global[key];
			}

			var keySections = key.Split('_');
			var baseKey = keySections.Length > 1 ? keySections[1] : key;
			var section = keySections.Length > 1 ? keySections[0] : key;

			if (keySections.Length > 1 && !string.IsNullOrEmpty(IniData.Sections[section][baseKey]))
			{
				return IniData.Sections[section][baseKey];
			}

			return null;

		}



		private void ReplaceGreedy(XmlNode rawXml)
		{

			foreach (var item in IniData.Global)
			{

				if (rawXml.SelectSingleNode($"add[@key='{item.KeyName}']") != null)
				{
					rawXml.SelectSingleNode($"add[@key='{item.KeyName}']").Attributes["value"].Value = item.Value;
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

			// Apply the appSettings section only
			if (rawXml.LocalName == "appSettings" && IniData.Sections["appSettings"] != null)
			{

				foreach (var item in IniData.Sections["appSettings"])
				{

					if (rawXml.SelectSingleNode($"add[@key='{item.KeyName}']") != null)
					{
						rawXml.SelectSingleNode($"add[@key='{item.KeyName}']").Attributes["value"].Value = item.Value;
					}
					else
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
