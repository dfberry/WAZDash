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
using Wp7AzureMgmt.Wp7ClientLibrary.Dashboard;
using Wp7AzureMgmt.Wp7ClientLibrary;
using System.Collections.Generic;
using MemoryDiagnostics;
using System.Diagnostics;
using System.Linq;
using WindowsAzureStatus.Models;
using System.ComponentModel;

namespace WindowsAzureStatus.ViewModels
{
    /// <summary>
    /// Data class
    /// </summary>
    public class DashboardModelView : INotifyPropertyChanged
    {
        /// <summary>
        /// Items back from web service
        /// </summary>
        public List<DashboardItem> Dashboard { get; set; }

        /// <summary>
        /// DateTime last fetch from web service
        /// </summary>
        public DateTime LastUpdate { get; set; }

        public DashboardModelView()
        {
            this.Dashboard = new List<DashboardItem>();
        }
#region Issues
        public List<String> GetAllItemIssuesByPubDate()
        {
            var sort = from tempfeed in this.Dashboard
                       where (tempfeed.FeedIssues != null)
                       from tempitem in tempfeed.FeedIssues
                       orderby tempitem.PubDate descending
                       select tempitem.Description;

            return sort.ToList();
        }
        public List<RSSFeedItemDetail> GetIssuesByPubDate2()
        {
            var sort = from tempfeed in this.Dashboard
                       where (tempfeed.FeedIssues != null)
                       from tempitem in tempfeed.FeedIssues
                       orderby tempitem.PubDate descending
                       select tempitem;

            return sort.ToList();
        }
        public List<RSSFeedItemDetail> GetIssuesBySearchTerm(string searchterm)
        {
            var sort = from tempfeed in this.Dashboard
                       where (tempfeed.FeedIssues != null)
                       from tempitem in tempfeed.FeedIssues
                       where (tempitem.Description.Contains(searchterm))
                       orderby tempitem.PubDate descending
                       select tempitem;

            return sort.ToList();
        }
        public int CountIssues()
        {
            var count = from tempfeed in this.Dashboard
                        where (tempfeed.FeedIssues != null) 
                        select tempfeed.FeedIssues;

            return count.Count();
        }
#endregion
#region Services
        public List<String> GetDistinctServices()
        {
            var sort = (from tempfeed in this.Dashboard
                       orderby tempfeed.FeedDefintion.ServiceName
                       select tempfeed.FeedDefintion.ServiceName).Distinct();

            return sort.ToList();
        }
        public List<ServiceLocationGroup2> GetServiceLocationGroups
        {
            get
            {
                //if (App.ViewModel.ShowAllItems)
                    return GetServiceLocationGroupsAll;
                //else
                //    return GetServiceLocationGroupsNotEmpty;
            }
        }

        //public List<ServiceLocationGroup2> GetServiceLocationGroupsNotEmpty
        //{
        //    get
        //    {
        //        var notEmpty = from item in this.GetServiceLocationGroupsAll
        //                       where (item.Issues != null) && (item.Issues.Count > 0)
        //                       select item;

        //        return notEmpty.ToList();
        //    }
        //}
        public List<ServiceLocationGroup2> GetServiceLocationGroupsAll
        {
            get
            {
                //var sort = from item in this.Dashboard
                //           orderby item.FeedDefintion.RegionLocation, item.FeedDefintion.ServiceName
                //           select item;

                //var group1 = from item in this.Dashboard
                //             orderby item.FeedDefintion.ServiceName, item.FeedDefintion.RegionLocation
                //             group item by item.FeedDefintion.ServiceName into g
                //             select new { ServiceName = g.Key, Items = g, Issues = g.SelectMany(location => location.FeedIssues.Select(feedissue => feedissue.Description)) };

                //var group3 = from service in group1
                //             select new ServiceLocationGroup2()
                //             {
                //                 ServiceName = service.ServiceName,
                //                 ServiceLocations = new List<ServiceLocation2>(service.Items.Select(location =>
                //                     new ServiceLocation2()
                //                     {
                //                         LocationName = location.FeedDefintion.RegionLocation,
                //                         Issues = new List<String>(location.FeedIssues.Select(feedissue => feedissue.Description))

                //                     })),

                //                 Issues = new List<String>(service.Issues)


                //             };

                var group2 = from item in this.Dashboard
                             where item.FeedIssues != null
                             orderby item.FeedDefintion.ServiceName, item.FeedDefintion.RegionLocation
                             group item by item.FeedDefintion.ServiceName into g
                             select new ServiceLocationGroup2()
                             {
                                 ServiceName = g.Key,
                                 ServiceLocations = new List<ServiceLocation2>(g.Select(location =>
                                     new ServiceLocation2()
                                     {
                                         LocationName = location.FeedDefintion.RegionLocation,
                                         Issues = new List<String>(location.FeedIssues.Select(feedissue => feedissue.Description))

                                     })),

                                 Issues = new List<String>(g.SelectMany(location => location.FeedIssues.Select(feedissue => feedissue.Description)))

                             };

                var alphasort = new List<ServiceLocationGroup2>(group2);

                return alphasort;
            }
        }
        public ServiceLocationGroup2 GetSingleServiceLocationGroup(String ServiceName)
        {
            var thisServiceLocationGroup = from item in this.GetServiceLocationGroupsAll
                                           where item.ServiceName == ServiceName
                                           select item;

            return thisServiceLocationGroup.FirstOrDefault();
        }
        public int GetServiceIssueCount(String ServiceName)
        {
            var thisServiceIssuesCount = from item in this.Dashboard
                                         where (item.FeedDefintion.ServiceName == ServiceName) && (item.FeedIssues!=null) 
                                          select item.FeedIssues.Count;

            return thisServiceIssuesCount.Sum();
        }
        #endregion
#region Locations
        public List<String> GetDistinctLocations()
        {
            var sort = (from tempfeed in this.Dashboard
                        orderby tempfeed.FeedDefintion.RegionLocation
                        select tempfeed.FeedDefintion.RegionLocation).Distinct();

            return sort.ToList();
        }
        public List<LocationServiceGroup2> GetLocationServiceGroups
        {
            get
            {
                //if (App.ViewModel.ShowAllItems)
                    return GetLocationServiceGroupsAll;
                //else
                //    return GetLocationServiceGroupsNotEmpty;
            }
        }

        //public List<LocationServiceGroup2> GetLocationServiceGroupsNotEmpty
        //{
        //    get
        //    {
        //        var notEmpty = from item in this.GetLocationServiceGroupsAll
        //                       where (item.Issues != null) && (item.Issues.Count > 0)
        //                       select item;

        //        return notEmpty.ToList();
        //    }
        //}
        public List<LocationServiceGroup2> GetLocationServiceGroupsAll
        {
            get
            {
                //var sort = from item in this.Dashboard
                //           orderby item.FeedDefintion.RegionLocation, item.FeedDefintion.ServiceName
                //           select item;


                var group1 = from item in this.Dashboard
                             where (item.FeedIssues != null)
                             orderby item.FeedDefintion.RegionLocation, item.FeedDefintion.ServiceName
                             group item by item.FeedDefintion.RegionLocation into g
                             select new LocationServiceGroup2()
                             {
                                 LocationName = g.Key,
                                 LocationServices = new List<LocationService2>(g.Select(service =>
                                     new LocationService2()
                                     {
                                         ServiceName = service.FeedDefintion.ServiceName,
                                         Issues = new List<String>(service.FeedIssues.Select(feedissue => feedissue.Description))

                                     })),

                                 Issues = new List<String>(g.SelectMany(service => service.FeedIssues.Select(feedissue => feedissue.Description)))

                             };

                var alphasort = new List<LocationServiceGroup2>(group1);

                return alphasort;
            }
        }
        public LocationServiceGroup2 GetSingleLocationServiceGroup(String LocationName)
        {
            var thisLocationServiceGroup = from item in this.GetLocationServiceGroups
                                           where item.LocationName == LocationName
                                           select item;

            return thisLocationServiceGroup.FirstOrDefault();
        }
        public int GetLocationIssueCount(String LocationName)
        {
            var thisLocationIssuesCount = from item in this.Dashboard
                                where (item.FeedDefintion.RegionLocation == LocationName) && (item.FeedIssues!=null) 
                                select item.FeedIssues.Count;

            return thisLocationIssuesCount.Sum();
        }
#endregion
#region AsyncCall
        public void BeginGetDashboard(AsyncCallback callback, AsyncCallback exceptionCallback)
        {
            DashboardClient client = new DashboardClient(App.ViewModel.RESTServer);

            int IsTrialRequest;

            if (App.IsTrialApp)
                IsTrialRequest = 1;
            else
                IsTrialRequest = -1;

            DashboardRequest request = new DashboardRequest()
            {
                UserId = App.ViewModel.PhoneInfo.UserId,
                PhoneId = App.ViewModel.PhoneInfo.PhoneId,
                PhoneMaker = App.ViewModel.PhoneInfo.PhoneMaker,
                IssueAge = App.ViewModel.AppSettings.IssueAge,
                TrialRemaining = IsTrialRequest,
                AppVersion = App.AppVersion,
                FetchAllIncludingEmpties = Convert.ToInt16(App.ViewModel.GetAllItems),
                Callback = callback,
                ExceptionCallback = exceptionCallback
            };

            Dashboard.Clear();

            client.BeginGetFilledIssues(EndGetDashboard, GetDashboardExceptionsResponse<DashboardRequest>, request);
        }
        public void EndGetDashboard(IAsyncResult asynchronousResult)
        {
            DashboardRequest DashboardRequest = (DashboardRequest)asynchronousResult.AsyncState;

            this.Dashboard = DashboardRequest.DashboardResponse.List;

            LastUpdate = DateTime.UtcNow;
            App.ViewModel.IsDataLoaded = true;

            if (App.IsTrialApp)
            {
                App.ViewModel.Trial.TrialResponseRecieved = true;
                App.ViewModel.Trial.TrialResponseRemaining = DashboardRequest.TrialRemaining;
            }


            if (DashboardRequest.Callback != null)
            {
                // On Dispatcher There -- Caller Is Probably The GUI
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // since we are assigning the dashboard to the vm object, just return nothing here
                    DashboardRequest.Callback(null);
                });
            }
        }
        public void GetDashboardExceptionsResponse<T>(IAsyncResult asynchronousResult) where T : DashboardRequest
        {
            ExceptionReponse<T> exceptionResponse = (ExceptionReponse<T>)asynchronousResult.AsyncState;
            Exception exception = exceptionResponse.Exception;

            if (exception != null)
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    exceptionResponse.Request.ExceptionCallback(new WebServiceResponse<Exception>(exception));
                });
            }
            else
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    exceptionResponse.Request.ExceptionCallback(null);
                });
            }

            LastUpdate = DateTime.MinValue;
            App.ViewModel.IsDataLoaded = false;


        }
#endregion
#region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
#endregion
    }
}
