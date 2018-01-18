using Microsoft.Configuration.ConfigurationBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.IO;
using YamlDotNet.RepresentationModel;
using System.Diagnostics;

namespace Fritz.ConfigurationBuilders
{
	public class YamlConfigurationBuilder : KeyValueConfigBuilder
	{

		public const string locationTag = "location";
		public const string optionalTag = "optional";
		public const string sectionTag = "section";

		private YamlDocument YamlDoc = null;

		public string Location { get; private set; }

		/// <summary>
		/// Is the file required?  If not, fail silently.  Defaults to FALSE
		/// </summary>
		public bool Optional { get; private set; } = false;

		public string Section { get; private set; } = "";

		public override void Initialize(string name, NameValueCollection config)
		{

			base.Initialize(name, config);

			this.Location = config[locationTag];
			if (bool.TryParse(config[optionalTag], out var optValue)) {

				this.Optional = optValue;

			} 

			// Fail quickly if the file presence is required
			if (!Optional && string.IsNullOrEmpty(this.Location))
			{

				throw new ArgumentNullException(nameof(locationTag),
					$"Missing required location attribute on {nameof(YamlConfigurationBuilder)}");

			} else if (Optional && string.IsNullOrEmpty(this.Location)) {

				return;

			}

			Section = config[sectionTag];

			using (var sr = new StreamReader(Location))
			{
				var parser = new YamlStream(); 
				parser.Load(sr); 
				YamlDoc = parser.Documents[0];
			}

		}

		public override ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
		{

			// Fail quickly if there is no YAML Doc
			if (YamlDoc == null) return new KeyValuePair<string, string>[] { };

			var topNode = YamlDoc.RootNode as YamlMappingNode;

			if (!string.IsNullOrEmpty(Section)) {
				topNode = (topNode as YamlMappingNode).Children
					.FirstOrDefault(n => n.Key is YamlScalarNode && ((YamlScalarNode)n.Key).Value == Section).Value as YamlMappingNode;
			}

			return topNode.Children.Select(c => new KeyValuePair<string, string>(
			(c.Key as YamlScalarNode).Value,
			(c.Value as YamlScalarNode).Value
			)).ToList();
			
		}

		public override string GetValue(string key)
		{

			// Fail quickly if there is no YAML Doc
			if (YamlDoc == null) return "";

			if (!string.IsNullOrEmpty(Section))
			{

				return GetValueFromSection(key, Section);

			}

			return GetValueFromKeyValuePair(key);

		}

		private string GetValueFromKeyValuePair(string key, YamlNode topMostNode = null)
		{

			topMostNode = topMostNode ?? YamlDoc.RootNode;

			var mappingNodes = topMostNode.AllNodes
				.Where(n => n.NodeType == YamlNodeType.Mapping)
				.Cast<YamlMappingNode>();

			var foundValue = string.Empty;
			foreach (var entry in mappingNodes)
			{

				var foundPair = entry.FirstOrDefault(mapNode => ((YamlScalarNode)mapNode.Key).Value == key);
				foundValue = ((YamlScalarNode)foundPair.Value).Value;

			}

			return foundValue;
		}

		private string GetValueFromSection(string key, string section)
		{

			var rootNode = (YamlMappingNode)YamlDoc.RootNode;

			var sectionNode = rootNode.Children
				.FirstOrDefault(n => n.Key is YamlScalarNode && ((YamlScalarNode)n.Key).Value == section).Value;

			return GetValueFromKeyValuePair(key, sectionNode);
			
		}
	}
}
