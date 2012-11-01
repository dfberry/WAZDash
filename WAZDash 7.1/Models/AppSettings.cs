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
using System.ComponentModel;

namespace WindowsAzureStatus.Models
{
    public class AppSettings : INotifyPropertyChanged
    {
        /// <summary>
        /// Issue Age retrieved from web service
        /// </summary>
        public int IssueAge { get; set; } //-1 == all

        /// <summary>
        /// Show empty and full nodes from web service
        /// </summary>
        public bool ShowAllItems { get; set; }

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
