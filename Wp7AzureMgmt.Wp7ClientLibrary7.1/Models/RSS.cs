namespace Wp7AzureMgmt.Wp7ClientLibrary.Models
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot("rss")]
    public class RSS
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
        public RSSItem[] Item { get; set; }

    }

}
