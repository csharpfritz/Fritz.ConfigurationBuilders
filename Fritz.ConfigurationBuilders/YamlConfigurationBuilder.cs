using Microsoft.Configuration.ConfigurationBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fritz.ConfigurationBuilders
{
	public class YamlConfigurationBuilder : KeyValueConfigBuilder
	{

		public const string locationTag = "location";
		public string Location { get; private set; }


		public override ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
		{
			throw new NotImplementedException();
		}

		public override string GetValue(string key)
		{
			throw new NotImplementedException();
		}
	}
}
