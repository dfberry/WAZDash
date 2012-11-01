

namespace WindowsAzureStatus.Models
{
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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Globalization;

    public enum SelectedDashboardGroupObjectType
    {
        None=0,
        LocationService,
        ServiceLocation,
        LocationServiceGroup,
        ServiceLocationGroup
    }

    public class LocationService : INotifyPropertyChanged
    {
        public int IssueCount { get; set; }

        public String ServiceName { get; set; }

        public ObservableCollection<String> Issues { get; set; }

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
    public class ServiceLocation : INotifyPropertyChanged
    {
        public int IssueCount { get; set; }

        public String LocationName { get; set; }

        public ObservableCollection<String> Issues { get; set; }

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
    public class LocationServiceGroup :INotifyPropertyChanged
    {
        public string LocationName { get; set; }

        public ObservableCollection<LocationService> LocationServices { get; set; }

        public int IssueCount { get; set; }

        public ObservableCollection<String> Issues { get; set; }

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
    public class ServiceLocationGroup : INotifyPropertyChanged
    {

        public string ServiceName { get; set; }

        //public ObservableCollection<String> Locations{ get; set; }

        public ObservableCollection<ServiceLocation> ServiceLocations { get; set; }

        public int IssueCount { get; set; }

        public ObservableCollection<String> Issues { get; set; }

        //public DateTime MostRecentIssueDate { get; set; }

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

    public class Service : INotifyPropertyChanged
    {
        public ObservableCollection<ServiceItem> Items { get; set; }


        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        public string LocationCode { get; set; }

        public string LocationName { get; set; }

        public int IssueCount { get; set; }

        public int ChannelCount { get; set; }

        public string LocationNameAndCount
        {
            get
            {
                return LocationName + " " + IssueCount.ToString();
            }
        }

        public Service()
        {
            Items = new ObservableCollection<ServiceItem>();
        }

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
    public class ServiceItem : INotifyPropertyChanged
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public string PubDate { get; set; }

        public DateTime PubDateAsDate { get; set; }

        public string LastBuildDate { get; set; }

        public string Copyright { get; set; }

        public ServiceImage Image { get; set; }

        public ObservableCollection<ServiceItemIssue> Issues { get; set; }

        public int IssueCount { get; set; }

        public ServiceItem()
        {
            Issues = new ObservableCollection<ServiceItemIssue>();
        }

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
    public class ServiceImage : INotifyPropertyChanged
    {
        public string RSSUrl { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

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
    public class ServiceItemIssue : INotifyPropertyChanged
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string PubDate { get; set; }

        public DateTime PubDateAsDate { get; set; }

        //public DateTime PubDateAsDate 
        //{
        //    get
        //    {
        //        if (!String.IsNullOrEmpty(PubDate))
        //        {

        //            string format1 = "yyyy-MM-dd HH:mm";

        //            string tempDateTimeString = this.PubDate.Replace("T", " ");
        //            tempDateTimeString = tempDateTimeString.Substring(0, 16);

        //            DateTime tempDateTime = DateTime.ParseExact(tempDateTimeString, format1, CultureInfo.InvariantCulture);

        //            return tempDateTime;

        //        }
        //        else
        //        {
        //            return new DateTime();
        //        }
        //    }

        //}

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
