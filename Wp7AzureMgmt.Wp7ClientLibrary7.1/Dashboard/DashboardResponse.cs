using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Wp7AzureMgmt.Models;
using System.ComponentModel;

namespace Wp7AzureMgmt.Wp7ClientLibrary.Dashboard
{

    public class DashboardResponse : Response, INotifyPropertyChanged
    {
        public List<DashboardItem> List { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class DashboardItem : INotifyPropertyChanged
    {
        public RssFeed FeedDefintion { get; set; }
        public List<RSSFeedItemDetail> FeedIssues { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class RSSFeedItemDetail : INotifyPropertyChanged
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string PubDate { get; set; }

        public string Status { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
    public class RssFeed : INotifyPropertyChanged
    {
        public String ServiceName { get; set; }
        public String RegionLocation { get; set; }
        public String ServiceId { get; set; }
        public String RegionId { get; set; }
        public String FeedCode { get; set; }
        public String RSSLink { get; set; }
        public DateTime DateStamp { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
