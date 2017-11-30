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
		private YamlDocument YamlDoc;

		public string Location { get; private set; }

		public override void Initialize(string name, NameValueCollection config)
		{

			base.Initialize(name, config);

			this.Location = config[locationTag];

			if (string.IsNullOrEmpty(this.Location))
			{
				throw new ArgumentNullException(nameof(locationTag),
					$"Missing required location attribute on {nameof(YamlConfigurationBuilder)}");
			}

			using (var sr = new StreamReader(Location))
			{
				var parser = new YamlStream();
				parser.Load(sr);
				YamlDoc = parser.Documents[0];
			}

		}

		public override ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
		{

			var matchingNode = YamlDoc.RootNode.AllNodes.Skip(1).First();
			var mappingNodes = YamlDoc.RootNode.AllNodes
				.Where(n => n.NodeType == YamlNodeType.Mapping)
				.Cast<YamlMappingNode>();

			var outList = new List<KeyValuePair<string, string>>();
			foreach (var entry in mappingNodes)
			{

				foreach (var mapNode in entry)
				{

					outList.Add(new KeyValuePair<string, string>(
						((YamlScalarNode)mapNode.Key).Value,
						(((YamlScalarNode)mapNode.Value).Value)));

				}


			}

			return outList;
		}

		public override string GetValue(string key)
		{

			var matchingNode = YamlDoc.RootNode.AllNodes.Skip(1).First();
			var mappingNodes = YamlDoc.RootNode.AllNodes
				.Where(n => n.NodeType == YamlNodeType.Mapping)
				.Cast<YamlMappingNode>();

			string foundValue = string.Empty;
			foreach (var entry in mappingNodes)
			{

				var foundPair = entry.FirstOrDefault(mapNode => ((YamlScalarNode)mapNode.Key).Value == key);
				foundValue = ((YamlScalarNode)foundPair.Value).Value;


				//foreach (var mapNode in entry)
				//{
				//	Console.Out.WriteLine(((YamlScalarNode)mapNode.Key).Value);
				//	Console.Out.WriteLine(((YamlScalarNode)mapNode.Value).Value);
				//}

			}

			return foundValue;

		}
	}
}
