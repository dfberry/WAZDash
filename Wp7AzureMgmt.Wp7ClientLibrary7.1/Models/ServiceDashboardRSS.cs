namespace Wp7AzureMgmt.Security.Model
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("rss", IsNullable = false)]
    public class ServiceDashboardRSS
    {
        [XmlElement("channel")]
        public ServiceDashboardChannel[] RSSChannels { get; set; }
    }


    [Serializable]
    [XmlRoot("channel", IsNullable = false)]
    public class ServiceDashboardChannel
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("language")]
        public string Language { get; set; }

        [XmlElement("pubDate")]
        public string PubDate { get; set; }

        [XmlElement("lastBuildDate")]
        public string LastBuildDate { get; set; }

        [XmlElement("copyright")]
        public string Copyright { get; set; }

        [XmlElement("image")]
        public string Image { get; set; }

        [XmlElement("item")]
        public ServiceDashboardChannelItem[] Item { get; set; }

    }

    [Serializable]
    [XmlRoot("item", IsNullable = false)]
    public class ServiceDashboardChannelItem
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("pubDate")]
        public string PubDate { get; set; }
    }
}