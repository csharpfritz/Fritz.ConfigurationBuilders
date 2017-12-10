using Microsoft.Configuration.ConfigurationBuilders;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Fritz.ConfigurationBuilders
{
    public class RssConfigurationBuilder : KeyValueConfigBuilder
    {
        private Uri m_feed;
        private const string FeedTag = "feed";
        private readonly Dictionary<string, string> m_feedData;

        public RssConfigurationBuilder()
        {
            m_feedData = new Dictionary<string, string>();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            if (string.IsNullOrEmpty(config[FeedTag]?.Trim()))
                throw new ConfigurationErrorsException($"Missing {FeedTag} attribute when initializing ConfigurationBuilder {name}");

            if (!Uri.TryCreate(config[FeedTag], UriKind.Absolute, out Uri uriResult) || (uriResult?.Scheme != Uri.UriSchemeHttp && uriResult?.Scheme != Uri.UriSchemeHttps))
                throw new UriFormatException("Invalid URL provided");

            m_feed = uriResult;
            if (!m_feed.IsFeedValid())
                throw new UriFormatException("URL Appears to be an invalid RSS Feed");

            GetSettingsFromFeed();
        }

        public override ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
        {
            var values = m_feedData;

            if (!string.IsNullOrEmpty(prefix))
                values = values.Where(k => k.Key.StartsWith(prefix)).ToDictionary(x => x.Key, x => x.Value);

            return values;
        }

        public override string GetValue(string key)
        {
            string settingsValue = null;

            if (m_feedData.ContainsKey(key))
            {
                settingsValue = m_feedData[key];
            }

            return settingsValue;
        }

        private void GetSettingsFromFeed()
        {
            try
            {
                using (var reader = XmlReader.Create(m_feed.AbsoluteUri))
                {
                    var loadedFeed = SyndicationFeed.Load(reader);
                    foreach (var syndicationItem in loadedFeed.Items)
                    {
                        if (!m_feedData.ContainsKey(syndicationItem.Title.Text))
                        {
                            if (syndicationItem.Summary == null) continue;
                            m_feedData[syndicationItem.Title.Text] = syndicationItem.Summary.Text;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    internal static class UriExtensions
    {
        public static bool IsFeedValid(this Uri uri)
        {
            bool isValid = true;

            try
            {
                using (XmlReader reader = XmlReader.Create(uri.AbsoluteUri))
                {
                    Rss20FeedFormatter formatter = new Rss20FeedFormatter();
                    formatter.ReadFrom(reader);
                }
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
