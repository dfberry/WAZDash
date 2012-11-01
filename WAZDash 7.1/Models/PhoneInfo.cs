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
    public class PhoneInfo : INotifyPropertyChanged
    {

        public String UserId { get; set; }

        public String PhoneId { get; set; }

        public String PhoneMaker { get; set; }

        public PhoneInfo()
        {
            this.UserId = Phone.GetWindowsLiveAnonymousID();
            this.PhoneId = Phone.GetDeviceUniqueIDAsBtyeArray().ToString();
            this.PhoneMaker = Phone.GetManufacturer();

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
}
