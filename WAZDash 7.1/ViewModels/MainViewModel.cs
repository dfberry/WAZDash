namespace WindowsAzureStatus.ViewModel
{
    using System;
    using System.Net;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Windows;
    using System.Collections.ObjectModel;
    using Wp7AzureMgmt.Wp7ClientLibrary;
    using Wp7AzureMgmt.Models;
    using Wp7AzureMgmt.Wp7ClientLibrary.Exceptions;
    using WindowsAzureStatus;
    using System.IO.IsolatedStorage;
    using System.IO;
    using System.Xml.Serialization;
    using WindowsAzureStatus.Models;
    using Microsoft.Phone.Info;
    using WindowsAzureStatus.ViewModels;
    using MemoryDiagnostics; 

    public class MainViewModel : INotifyPropertyChanged
    {
        public PhoneInfo PhoneInfo { get; set; } 


        private const int cTimerWait = 300000;// 30 seconds

        /// <summary>
        /// Trial Information
        /// </summary>
        public WindowsAzureStatus.Models.AppTrial Trial { get; set; }         

        public bool RefreshData { get; set; }

        /// <summary>
        /// Retrieve empty and full nodes from web service
        /// </summary>
        public bool GetAllItems { get; set; }

        /// <summary>
        /// Any settings that user has to set to configure app
        /// </summary>
        public AppSettings AppSettings { get; set; } 


        //public DateTime LastSyncWithBerryInternationalWebService { get; set; }

        //public Settings AppSettings = new Settings();

        //public Guid PhoneAppUniqueGuid { get; set; }



        //public String MsgToUser { get; set; }

        //public Service SelectedService { get; set; }

        //public ServiceDashboardModelView ServiceDashboardModelView { get; set; }

        public DashboardModelView DashboardModelView { get; set; }

        public SelectedDashboardGroupObjectType SelectedObject = SelectedDashboardGroupObjectType.None;
        public String SelectedName { get; set; }


        public MainViewModel()
        {
            PhoneInfo = new PhoneInfo();
            DashboardModelView = new DashboardModelView();
            Trial = new WindowsAzureStatus.Models.AppTrial();
            AppSettings = new AppSettings();
            //SelectedService = new Service();

            GetAllItems = false;
            this.AppSettings.ShowAllItems = false;
            RefreshData = true;
            this.AppSettings.IssueAge = 3;
            
        }


        public bool IsDataLoaded
        {
            get;
            set;
        }

        public string RESTServer
        {
            get
            {
#if DEBUG
                return "http://localhost:50113";
                //return "https://wazup.berryintl.com";
#else
                return "https://wazup.berryintl.com";
#endif

            }
        }
        public string Updated
        {
            get
            {
                if (this.DashboardModelView.LastUpdate != DateTime.MinValue)
                {
                    // Set Focus To the Page
                    if (this.DashboardModelView.LastUpdate == DateTime.Now.Date)
                    {
                        //only show time
                        return "Updated: " + this.DashboardModelView.LastUpdate.ToString("t");
                    }
                    else
                    {
                        //show date
                        return "Updated: " + this.DashboardModelView.LastUpdate.ToString("MMM") + " " + this.DashboardModelView.LastUpdate.ToString("dd");
                    }

                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public void ToIsolatedStorage()
        {
            // Ensure that required application state is persisted here.
            //AppSettings.Save();

            // persist the data using isolated storage
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = new IsolatedStorageFileStream("wazdashdata.txt",
                                                              FileMode.Create,
                                                              FileAccess.Write,
                                                              store))
            {
                var serializer = new XmlSerializer(typeof(MainViewModel));
                serializer.Serialize(stream, this);
            }

            //AppSettings.Save();

        }

        //Note: to get a result requires ID_CAP_IDENTITY_DEVICE  
        // to be added to the capabilities of the WMAppManifest  
        // this will then warn users in marketplace  
        //public static byte[] GetDeviceUniqueID()
        //{
        //    byte[] result = null;
        //    object uniqueId;
        //    if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
        //        result = (byte[])uniqueId;

        //    return result;
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