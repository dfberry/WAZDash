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
using System.ComponentModel;

namespace WindowsAzureStatus.Models
{

    //public enum SelectedDashboardGroupObjectType
    //{
    //    None=0,
    //    LocationService,
    //    ServiceLocation,
    //    LocationServiceGroup,
    //    ServiceLocationGroup
    //}

    public class LocationService2 : INotifyPropertyChanged
    {
        
        public String ServiceName { get; set; }

        public List<String> Issues { get; set; }

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
    public class ServiceLocation2 : INotifyPropertyChanged
    {
        public String LocationName { get; set; }

        public List<String> Issues { get; set; }

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
    public class LocationServiceGroup2 : INotifyPropertyChanged
    {
        public string LocationName { get; set; }

        public List<LocationService2> LocationServices { get; set; }

        public List<String> Issues { get; set; }

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
    public class ServiceLocationGroup2 : INotifyPropertyChanged
    {

        public string ServiceName { get; set; }

        public List<ServiceLocation2> ServiceLocations { get; set; }

        public List<String> Issues { get; set; }

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
