namespace WindowsAzureStatus.ViewModel
{

    using System;
    using System.Net;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using Wp7AzureMgmt.Wp7ClientLibrary;
    using Wp7AzureMgmt.Models;
    using System.ComponentModel;
    using Wp7AzureMgmt.Wp7ClientLibrary.Exceptions;
    using System.Collections.ObjectModel;
    using WindowsAzureStatus.Models;
    using WindowsAzureStatus.Utils;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Diagnostics;
    using MemoryDiagnostics;
//    using Coding4Fun.Phone.Controls.Data;

    public class ServiceDashboardModelView : INotifyPropertyChanged
    {
        //public ServiceDashboard Dashboard { get; set; }
        public ObservableCollection<Service> DashboardServices { get; set; }


        public String LastUpdate { get; set; }

        public ServiceDashboardModelView()
        {
            Debug.WriteLine(MemoryDiagnosticsHelper.GetCurrentMemoryUsage());
            this.DashboardServices = new ObservableCollection<Service>();
            Debug.WriteLine(MemoryDiagnosticsHelper.GetCurrentMemoryUsage());
        }
        public ObservableCollection<Service> DashboardServiceAlpha
        {
            get
            {
                //DFB: linq examples 
                //http://msdn.microsoft.com/en-us/vcsharp/aa336756

                var alphasort = new ObservableCollection<Service>().PopulateFrom(
                                        from item in this.DashboardServices
                                        orderby item.ServiceName
                                        select item);

                return alphasort;
            }
        }

        public ObservableCollection<Service> DashboardServiceSortServiceAlphaLocationAlpha()
        {

            //DFB: linq examples 
            //http://msdn.microsoft.com/en-us/vcsharp/aa336756

            var sort = from item in this.DashboardServices
                       orderby item.ServiceName, item.LocationName
                       select item;

            //var group1 = from item in this.DashboardServices
            //            group item by item.ServiceName into g
            //            select new { ServiceName = g.Key, Services = g };


            var alphasort = new ObservableCollection<Service>().PopulateFrom(sort);


            return alphasort;
        }
        public int CountIssues()
        {
            var count = from service in this.DashboardServices
                        from item in service.Items
                        select item.Issues;

            return count.Count();
        }


        // method so it doesn't wind up in isolated storage
        public int TotalIssueCount()
        {


            //int i = 0;

            //foreach (Service service in this.DashboardServices)
            //{
            //    i += service.IssueCount;
            //}

            //return i;
            return this.DashboardServices.Sum(service => service.IssueCount);
        }

        public ObservableCollection<String> GetAllIssues()
        {
            ObservableCollection<String> issues = new ObservableCollection<String>();

            foreach (Service service in this.DashboardServices)
            {
                foreach (ServiceItem item in service.Items)
                {
                    foreach (ServiceItemIssue issue in item.Issues)
                    {
                        issues.Add(issue.Description);
                    }
                }
            }
            return issues;

        }
        public ObservableCollection<ServiceItemIssue> GetAllItemIssuesByPubDate()
        {
            var sort = from tempfeed in this.DashboardServices
                       from tempitem in tempfeed.Items
                       from tempissue in tempitem.Issues
                       orderby tempissue.PubDateAsDate descending 
                       select tempissue;

            var alphasort = new ObservableCollection<ServiceItemIssue>().PopulateFrom(sort);


            return alphasort;
            //return alphasort.OrderByDescending(s => s.PubDateAsDate);
        }

        //v1
        public ObservableCollection<ServiceLocationGroup> ServiceLocationGroups
        {
            get
            {

                //DFB: linq examples 
                //http://msdn.microsoft.com/en-us/vcsharp/aa336756

                var sort = from item in this.DashboardServices
                           orderby item.ServiceName, item.LocationName
                           select item;

                var group1 = from item in this.DashboardServices
                             orderby item.ServiceName, item.LocationName
                             group item by item.ServiceName into g
                             select new ServiceLocationGroup()
                             {
                                 ServiceName = g.Key,
                                 //Locations = new ObservableCollection<String>().PopulateFrom(g.Select(service => service.LocationName)),
                                 IssueCount = g.Sum(service => service.IssueCount)
                             };

                var alphasort = new ObservableCollection<ServiceLocationGroup>().PopulateFrom(group1);

                return alphasort;
            }
        }
        //v2
        public ObservableCollection<ServiceLocationGroup> ServiceLocationGroups2
        {
            get
            {

                //DFB: linq examples 
                //http://msdn.microsoft.com/en-us/vcsharp/aa336756

                //var sort = from item in this.DashboardServices
                           //orderby item.ServiceName, item.LocationName
                           //select item;

                var group1 = from item in this.DashboardServices
                             orderby item.ServiceName, item.LocationName
                             group item by item.ServiceName into g
                             select new ServiceLocationGroup()
                             {
                                 ServiceName = g.Key,
                                 ServiceLocations = new ObservableCollection<ServiceLocation>().PopulateFrom(g.Select(service => new ServiceLocation() 
                                    { 
                                        LocationName = service.LocationName, 
                                        IssueCount = service.IssueCount,
                                        Issues = new ObservableCollection<string>().PopulateFrom(service.Items.SelectMany(item => item.Issues.Select(issue => issue.Description)))
                                 })),
                                 IssueCount = g.Sum(service => service.IssueCount),
                                 Issues = new ObservableCollection<String>().PopulateFrom(g.SelectMany(service => service.Items.SelectMany(item => item.Issues.Select(issue => issue.Description))))

                             };

                var alphasort = new ObservableCollection<ServiceLocationGroup>().PopulateFrom(group1);

                return alphasort;
            }
        }
        public ObservableCollection<LocationServiceGroup> LocationServiceGroups2
        {
            get
            {

                //DFB: linq examples 
                //http://msdn.microsoft.com/en-us/vcsharp/aa336756

                //var sort = from item in this.DashboardServices
                //orderby item.ServiceName, item.LocationName
                //select item;

                var group1 = from item in this.DashboardServices
                             orderby  item.LocationName, item.ServiceName
                             group item by item.LocationName into g
                             select new LocationServiceGroup()
                             {
                                 LocationName = g.Key,
                                 LocationServices = new ObservableCollection<LocationService>().PopulateFrom(g.Select(location => new LocationService()
                                 {
                                     ServiceName = location.ServiceName,
                                     IssueCount = location.IssueCount,
                                     Issues = new ObservableCollection<string>().PopulateFrom(location.Items.SelectMany(item => item.Issues.Select(issue => issue.Description)))
                                 })),
                                 IssueCount = g.Sum(service => service.IssueCount),
                                 Issues = new ObservableCollection<String>().PopulateFrom(g.SelectMany(location => location.Items.SelectMany(item => item.Issues.Select(issue => issue.Description))))

                             };

                var alphasort = new ObservableCollection<LocationServiceGroup>().PopulateFrom(group1);

                return alphasort;
            }
        }
        //public ObservableCollection<ServiceLocationGroup> ServiceLocationGroups3
        //{
        //    get
        //    {

        //        //DFB: linq examples 
        //        //http://msdn.microsoft.com/en-us/vcsharp/aa336756

        //        var sort = from item in this.DashboardServices
        //                   orderby item.ServiceName, item.LocationName
        //                   select item;

        //        var group1 = from item in this.DashboardServices
        //                     orderby item.ServiceName, item.LocationName
        //                     group item by item.ServiceName into g
        //                     select new ServiceLocationGroup()
        //                     {
        //                         ServiceName = g.Key,
        //                         ServiceLocations = new ObservableCollection<ServiceLocation>().PopulateFrom(g.Select(service => new ServiceLocation() { LocationName = service.LocationName, IssueCount = service.IssueCount })),
        //                         IssueCount = g.Sum(service => service.IssueCount),
        //                         Issues = new ObservableCollection<String>().PopulateFrom(g.SelectMany(service => service.Items.SelectMany(item => item.Issues.Select(issue => issue.Description))))

        //                     };

        //        var alphasort = new ObservableCollection<ServiceLocationGroup>().PopulateFrom(group1);

        //        return alphasort;
        //    }
        //}
        public ObservableCollection<LocationServiceGroup> LocationServiceGroups
        {
            get
            {

                //DFB: linq examples 
                //http://msdn.microsoft.com/en-us/vcsharp/aa336756

                var sort = from item in this.DashboardServices
                           orderby item.LocationName, item.ServiceName
                           select item;

                var group1 = from item in this.DashboardServices
                             orderby item.LocationName, item.ServiceName
                             group item by item.LocationName into g
                             select new LocationServiceGroup()
                             {
                                 LocationName = g.Key,
                                 LocationServices = new ObservableCollection<LocationService>().PopulateFrom(g.Select(service => 
                                     new LocationService() { 
                                         ServiceName = service.ServiceName, 
                                         IssueCount = service.IssueCount, 
                                         Issues = new ObservableCollection<String>().PopulateFrom(service.Items.SelectMany(item => item.Issues.Select(issue => issue.Description))) })),
                                 IssueCount = g.Sum(service => service.IssueCount),
                                 Issues = new ObservableCollection<String>().PopulateFrom(g.SelectMany(service => service.Items.SelectMany(item => item.Issues.Select(issue => issue.Description))))
                             };

                var alphasort = new ObservableCollection<LocationServiceGroup>().PopulateFrom(group1);

                return alphasort;
            }
        }
        //public ObservableCollection<ServiceLocationGroup> ServiceLocationGroupsSampleData
        //{
        //    get
        //    {
        //        var sampledataServiceLocationGroup = new ObservableCollection<ServiceLocationGroup>();

        //        sampledataServiceLocationGroup.Add(new ServiceLocationGroup() { ServiceName = "Serv1", IssueCount = 2, Locations = new ObservableCollection<String>() { "Asia", "Europe" } });
        //        sampledataServiceLocationGroup.Add(new ServiceLocationGroup() { ServiceName = "Serv2", IssueCount = 3, Locations = new ObservableCollection<String>() { "US", "Europe", "Asia" } });
        //        sampledataServiceLocationGroup.Add(new ServiceLocationGroup() { ServiceName = "Serv3", IssueCount = 1, Locations = new ObservableCollection<String>() { "Europe" } });

        //        return sampledataServiceLocationGroup;          
        //    }


        //}

        //public ObservableCollection<Service> DashboardServiceNameAlphaLocationNameAlpha()
        //{

        //    //DFB: linq examples 
        //    //http://msdn.microsoft.com/en-us/vcsharp/aa336756

        //    v

        //    var alphasort = new ObservableCollection<Service>().PopulateFrom(
        //                            from item in this.DashboardServices
        //                            orderby item.ServiceName, item.LocationName
        //                            select item);

        //    return alphasort;
        //}

        public void BeginGetServiceDashboard(AsyncCallback callback, AsyncCallback exceptionCallback)
        {
            ClientServiceDashboard client = new ClientServiceDashboard(App.ViewModel.RESTServer);

            ServiceDashboardRequest request = new ServiceDashboardRequest()
            {
                
                User = App.ViewModel.UserPhoneId,
                IssueAge = App.ViewModel.IssueAge,
                AppVersion = App.AppVersion,
                FetchAllIncludingEmpties = Convert.ToInt16(App.ViewModel.GetAllItems),
                Callback = callback,
                ExceptionCallback = exceptionCallback
            };

            Debug.WriteLine(MemoryDiagnosticsHelper.GetCurrentMemoryUsage());

            DashboardServices.Clear();

            Debug.WriteLine(MemoryDiagnosticsHelper.GetCurrentMemoryUsage());

            client.GetAll(EndGetServiceDashboard, EndGetServiceDashboardExceptions<ServiceDashboardRequest>, request);
        }
        public void EndGetServiceDashboard(IAsyncResult asynchronousResult)
        {
            ServiceDashboardRequest ServiceDashboardRequest = (ServiceDashboardRequest)asynchronousResult.AsyncState;

            Debug.WriteLine(MemoryDiagnosticsHelper.GetCurrentMemoryUsage());

            //go through each item and traslate it to our model
            foreach (ServiceDashboardFeed feed in ServiceDashboardRequest.Dashboard.DashboardFeeds)
            {
                var translatedService = new Service();
                int issueCount = 0;
                int channelCount = 0;

                // still have to figure out how to NOT pass null back from REST 
                if (feed != null)
                {

                    translatedService.LocationCode = feed.LocationCode;
                    translatedService.LocationName = feed.LocationName;
                    translatedService.ServiceCode = feed.ServiceCode;
                    translatedService.ServiceName = feed.ServiceName;

                    foreach (ServiceDashboardChannel channel in feed.FeedResults)
                    {
                        channelCount++;

                        var translatedChannel = new ServiceItem();


                        translatedChannel.Copyright = channel.Copyright;
                        translatedChannel.Description = channel.Description;
                        translatedChannel.Language = channel.Language;
                        translatedChannel.LastBuildDate = channel.LastBuildDate;
                        translatedChannel.Link = channel.Link;
                        translatedChannel.PubDate = channel.PubDate;

                        translatedChannel.Image = new ServiceImage()
                        {
                            ImageUrl = channel.Image.ImageUrl,
                            RSSUrl = channel.Image.RSSUrl,
                            Title = channel.Image.Title
                        };

                        //translatedChannel.PubDateAsDate = DateTime.ParseExact(translatedChannel.PubDate, @"m\/d\/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                        translatedChannel.Title = channel.Title;

                        foreach (ServiceDashboardChannelItemDetail channelItemDetail in channel.Items)
                        {
                            var translatedIssue = new ServiceItemIssue();

                            translatedIssue.Description = channelItemDetail.Description.Replace("<br />", "");
                            translatedIssue.Title = channelItemDetail.Title;
                            translatedIssue.PubDate = channelItemDetail.PubDate;
                            translatedIssue.PubDateAsDate = channelItemDetail.PubDateAsDate;

                            translatedChannel.Issues.Add(translatedIssue);

                            issueCount++;
                        }

                        translatedService.Items.Add(translatedChannel);



                    }
                    translatedService.IssueCount = issueCount;
                    translatedService.ChannelCount = channelCount;

                    //DFB: just want to make sure that there is always only 1 channel per feed
                    // othersise we have to behave different which isn't coded right now
                    if (channelCount > 1)
                    {
                        throw new Exception("channelCount > 1 for" + feed.ServiceName + " " + feed.LocationName);
                    }

                    if (App.ViewModel.GetAllItems == false)
                    {
                        if (issueCount > 0)
                        {
                            // Invoke To Dispatcher Thread
                            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                this.DashboardServices.Add(translatedService);
                            });
                        }
                    }
                    else
                    {
                        // Invoke To Dispatcher Thread
                        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            this.DashboardServices.Add(translatedService);
                        });
                    }
                }


            }
            Debug.WriteLine(MemoryDiagnosticsHelper.GetCurrentMemoryUsage());
            Debug.WriteLine("Count of Service Issues=" + this.CountIssues());

            LastUpdate = DateTime.Now.ToString();
            App.ViewModel.IsDataLoaded = true;


            if (ServiceDashboardRequest.Callback != null)
            {
                // On Dispatcher There -- Caller Is Probably The GUI
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // since we are assigning the dashboard to the vm object, just return nothing here
                    ServiceDashboardRequest.Callback(null);
                });
            }
        }
        public void EndGetServiceDashboardExceptions<T>(IAsyncResult asynchronousResult) where T : ServiceDashboardRequest
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

            LastUpdate = "<error>";
            App.ViewModel.IsDataLoaded = false;

            //if (exception is WebServiceException<ServiceDashboardResponseStatus>)
            //{
            //    // WWB: Exception Happend In Our Web Service 
            //    var status = ((WebServiceException<ServiceDashboardResponseStatus>)exception).Status;
            //    //switch (status)
            //    //{
            //    //    default:
            //    //        throw new InvalidOperationException(String.Format("Unrecognized ServiceDashboard Response Status: {0}", status), exception);

            //        // WWB: On Dispatcher here -- Caller Is Probably The GUI
            //        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            //        {
            //            exceptionResponse.Request.ExceptionCallback(new (WebServiceException<ServiceDashboardResponseStatus> (exception));
            //        });
            //    //}
            //}
            //else if (exception is WebException)
            //{
            //    // WWB: Exception Caused From Calling Web Service, i.e. Web Service Is Down, etc...
            //    // TODO: Communicate To User About Issue
            //    switch (((WebException)exception).Status)
            //    {
            //        case WebExceptionStatus.RequestCanceled:
            //            // WWB: This happens when we call Abort on the Request
            //            // Usually from a Timeout
            //            break;
            //    }
            //}
            //else
            //{
            //    // TODO: Communicate To User About Issue
            //}

            //// WWB: Callback to the dispatcher and let it know that there is an exception
            //if (exceptionResponse.Request.ExceptionCallback != null)
            //{
            //    // WWB: On Dispatcher here -- Caller Is Probably The GUI
            //    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            //    {
            //        exceptionResponse.Request.ExceptionCallback(new WebServiceResponse<Exception>(exception));
            //    });
            //}

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
