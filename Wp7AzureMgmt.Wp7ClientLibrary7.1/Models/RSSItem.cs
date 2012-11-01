namespace Wp7AzureMgmt.Wp7ClientLibrary.Models
{
    using System;
    using System.Xml.Serialization;

    public class RSSItem
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("pubDate")]
        public string PubDate { get; set; }
    }
}
