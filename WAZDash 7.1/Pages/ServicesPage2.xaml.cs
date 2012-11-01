using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Wp7AzureMgmt.Models;
using Wp7AzureMgmt.Wp7ClientLibrary.Exceptions;
using System.Collections.ObjectModel;
using WindowsAzureStatus.Models;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Net.NetworkInformation;
using System.Windows.Navigation;
using System.Diagnostics;
using MemoryDiagnostics;
using Wp7AzureMgmt.Wp7ClientLibrary.Dashboard;

namespace WindowsAzureStatus.Pages
{
    public partial class ServicesPage2 : PhoneApplicationPage
    {
        public string error = String.Empty;

        //bool LoadDataFromWebService = false;
        bool Prep = false;

        /// <summary>
        /// Make sure only 1 request is sent at a time
        /// regardless of how many times user hits refresh
        /// </summary>
        bool PrepStarted = false;

        public ServicesPage2()
        {
            LittleWatson.CheckForPreviousException(); 
            InitializeComponent();

            PivotWindowsServiceDashboard.Title = App.AppName;
            App.ViewModel.SelectedObject = SelectedDashboardGroupObjectType.None;
           
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadPage();
        }
        #region page event model
        private void LoadPage()
        {

            Prep = false;

            //reset everytime we come back to this page
            App.ViewModel.SelectedName = String.Empty;
            App.ViewModel.SelectedObject = SelectedDashboardGroupObjectType.None;

            // these are filled in from iso storage
            // if they are empty, assume no iso storage
            // and run off to web service
            if ((App.ViewModel.DashboardModelView!= null) &&
            (App.ViewModel.DashboardModelView.Dashboard != null) &&
            (App.ViewModel.DashboardModelView.Dashboard.Count > 0))
            {
                if (App.ViewModel.RefreshData)
                {
                    PrepForAsyncWebServiceCall();
                }
            }
            else
            {
                // go grab data
                PrepForAsyncWebServiceCall();
            }

            // regardless of where data comes from,
            // load it onto page
            LoadUI();
        }
        #endregion
        private void LoadUI()
        {
            if (!Prep)
            {
                if ((App.ViewModel.DashboardModelView != null) &&
                    (App.ViewModel.DashboardModelView.CountIssues() > 0))
                {

                    BuildLocationsPivotUI();
                    BuildServicesPivotUI();
                    BuildIssuesPivotUI();

                    StackPanelLocationsCountText.Text = App.ViewModel.DashboardModelView.GetLocationServiceGroups.Count.ToString() + " locations found";
                    StackPanelServicesCountText.Text = App.ViewModel.DashboardModelView.GetServiceLocationGroups.Count.ToString() + " services found";
                    TotalIssueCount.Text = App.ViewModel.DashboardModelView.GetIssuesByPubDate2().Count.ToString() + " issues found";

                    if (App.ViewModel.AppSettings.IssueAge != -1)
                    {
                        StackPanelLocationsCountText.Text += " in last " + App.ViewModel.AppSettings.IssueAge.ToString() + " days";
                        StackPanelServicesCountText.Text += " in last " + App.ViewModel.AppSettings.IssueAge.ToString() + " days";
                        TotalIssueCount.Text += " in last " + App.ViewModel.AppSettings.IssueAge.ToString() + " days";
                    }
                    else
                    {
                        StackPanelLocationsCountText.Text += " in all days";
                        StackPanelServicesCountText.Text += " in all days";
                        TotalIssueCount.Text += " in last all days";
                    }
                }
                else
                {

                    // zero issues found

                    if (App.ViewModel.AppSettings.IssueAge != -1)
                    {
                        StackPanelLocationsCountText.Text = "0 locations found in last " + App.ViewModel.AppSettings.IssueAge.ToString() + " days";
                        StackPanelServicesCountText.Text = "0 services found in last " + App.ViewModel.AppSettings.IssueAge.ToString() + " days";
                        TotalIssueCount.Text = "0 issues found in last " + App.ViewModel.AppSettings.IssueAge.ToString() + " days";
                    }
                    else
                    {
                        StackPanelLocationsCountText.Text = "0 locations found in last in all days";
                        StackPanelServicesCountText.Text = "0 services found in last in all days";
                        TotalIssueCount.Text = "0 issues found in last in all days";
                    }
                }


                ApplicationBar.IsMenuEnabled = true;
                ApplicationBar.IsVisible = true;
                textBlockDataAge.Text = App.ViewModel.Updated;
            }

        }

      

        private void BuildLocationsPivotUI()
        {
            StackPanel containingStackPanel = new StackPanel();
            containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

            foreach (LocationServiceGroup2 locationservicegroup in App.ViewModel.DashboardModelView.GetLocationServiceGroups)
            {
                // adding stackpanel so customer has more space to click
                StackPanel childStackPanel = new StackPanel();
                childStackPanel.Tag = locationservicegroup.LocationName;

                TextBlock textblockLocationName = new TextBlock();
                childStackPanel.Children.Add(textblockLocationName);

                textblockLocationName.KeyDown += new KeyEventHandler(textblockLocationName_KeyDown);
                textblockLocationName.Tap +=new EventHandler<System.Windows.Input.GestureEventArgs>(textblockLocationName_Tap);

                textblockLocationName.Text = locationservicegroup.LocationName + " (" + locationservicegroup.Issues.Count + ")";
                textblockLocationName.Tag = locationservicegroup.LocationName;
                textblockLocationName.TextWrapping = TextWrapping.Wrap;
                textblockLocationName.Style = Resources["PhoneTextLargeStyle"] as Style;
                textblockLocationName.Foreground = Resources["PhoneAccentBrush"] as Brush;

                if ((locationservicegroup.LocationServices != null)&&
                    (locationservicegroup.LocationServices.Count>0))
                { 
                    foreach (LocationService2 locationservice in locationservicegroup.LocationServices)
                    {
                        TextBlock textblockServiceName = new TextBlock();

                        textblockServiceName.KeyDown +=new KeyEventHandler(textblockLocationName_KeyDown);
                        textblockServiceName.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(textblockLocationName_Tap);

                        textblockServiceName.Text = locationservice.ServiceName + " (" + locationservice.Issues.Count +")";
                        textblockServiceName.Tag = locationservicegroup.LocationName;
                        textblockServiceName.TextWrapping = TextWrapping.Wrap;
                        textblockServiceName.Style = Resources["PhoneTextNormalStyle"] as Style;
                        textblockServiceName.Foreground = Resources["PhoneSubtleBrush"] as Brush;

                        childStackPanel.Children.Add(textblockServiceName);
                    }
                }
                containingStackPanel.Children.Add(childStackPanel);
            }
            ScrollerViewerListLocations.Content = containingStackPanel;
        }

        private void BuildServicesPivotUI()
        {
            StackPanel containingStackPanel = new StackPanel();
            containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

            foreach (ServiceLocationGroup2 servicelocationgroup in App.ViewModel.DashboardModelView.GetServiceLocationGroups)
            {
                // adding stackpanel so customer has more space to click
                StackPanel childStackPanel = new StackPanel();
                childStackPanel.Tag = servicelocationgroup.ServiceName;
                    
                TextBlock textblockServiceName = new TextBlock();

                textblockServiceName.KeyDown += new KeyEventHandler(textblockServiceName_KeyDown);
                textblockServiceName.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(textblockServiceName_Tap);

                textblockServiceName.Text = servicelocationgroup.ServiceName + " (" + servicelocationgroup.Issues.Count + ")";
                textblockServiceName.Tag = servicelocationgroup.ServiceName;
                textblockServiceName.TextWrapping = TextWrapping.Wrap;
                textblockServiceName.Style = Resources["PhoneTextLargeStyle"] as Style;
                textblockServiceName.Foreground = Resources["PhoneAccentBrush"] as Brush;

                childStackPanel.Children.Add(textblockServiceName);

                if ((servicelocationgroup.ServiceLocations != null)&&
                    (servicelocationgroup.ServiceLocations.Count>0))
                {
                    foreach (ServiceLocation2 servicelocation in servicelocationgroup.ServiceLocations)
                    {
                        TextBlock textblockLocationName = new TextBlock();

                        textblockLocationName.KeyDown += new KeyEventHandler(textblockServiceName_KeyDown);
                        textblockLocationName.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(textblockServiceName_Tap);

                        textblockLocationName.Text = servicelocation.LocationName + " (" + servicelocation.Issues.Count + ")";
                        textblockLocationName.Tag = servicelocationgroup.ServiceName;
                        textblockLocationName.TextWrapping = TextWrapping.Wrap;
                        textblockLocationName.Style = Resources["PhoneTextNormalStyle"] as Style;
                        textblockLocationName.Foreground = Resources["PhoneSubtleBrush"] as Brush;

                        childStackPanel.Children.Add(textblockLocationName);
                    }
                }

                containingStackPanel.Children.Add(childStackPanel);
            }

            ScrollerViewerListServices.Content = containingStackPanel;
          
        }
        private void BuildIssuesPivotUI()
        {

            StackPanel containingStackPanel = new StackPanel();
            containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

            foreach (RSSFeedItemDetail issue in App.ViewModel.DashboardModelView.GetIssuesByPubDate2())
            {
                TextBlock textblockissueTitle = new TextBlock();
                textblockissueTitle.Text = issue.Title;
                textblockissueTitle.TextWrapping = TextWrapping.Wrap;
                textblockissueTitle.Style = Resources["PhoneTextNormalStyle"] as Style;
                textblockissueTitle.Foreground = Resources["PhoneAccentBrush"] as Brush;
                containingStackPanel.Children.Add(textblockissueTitle);

                //TextBlock textblockissuePubDate = new TextBlock();
                //// textblockissuePubDate.Text = issue.PubDateAsDate.ToShortDateString() + " " + issue.PubDateAsDate.ToShortTimeString();
                //textblockissuePubDate.Text = issue.PubDate;
                //textblockissuePubDate.TextWrapping = TextWrapping.Wrap;
                //textblockissuePubDate.Style = Resources["PhoneTextNormalStyle"] as Style;
                //textblockissuePubDate.Foreground = Resources["PhoneContrastForegroundBrush"] as Brush;
                //containingStackPanel.Children.Add(textblockissuePubDate);

                TextBlock textblockissueDescription = new TextBlock();
                textblockissueDescription.Text = issue.Description + "\n\n";
                textblockissueDescription.TextWrapping = TextWrapping.Wrap;
                textblockissueDescription.Style = Resources["PhoneTextNormalStyle"] as Style;
                //textblockissuePubDate.Foreground = Resources["PhoneSubtleBrush"] as Brush;
                containingStackPanel.Children.Add(textblockissueDescription);
            }
            ScrollerViewerListIssues.Content = containingStackPanel;
        }

        #region WebService
        private void PrepForAsyncWebServiceCall()
        {
            Prep = true;

            if (this.PrepStarted == false)
            {

                this.PrepStarted = true;

                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    ApplicationBar.IsMenuEnabled = false;
                    ApplicationBar.IsVisible = false;

                    //DFB: if items in data and ui objects, get rid of them
                    App.ViewModel.DashboardModelView.Dashboard.Clear();
                    ScrollerViewerListServices.Content = null;
                    ScrollerViewerListLocations.Content = null;
                    ScrollerViewerListIssues.Content = null;

                    // signal user that something is happening
                    StackPanelServicesCountText.Text = "Loading...";
                    StackPanelLocationsCountText.Text = "Loading...";
                    TotalIssueCount.Text = "Loading...";
                    textBlockDataAge.Text = "Updating...";

                    // continue signaling by presenting progress bar at top of page
                    if (this.customIndeterminateProgressBar == null)
                    {
                        this.customIndeterminateProgressBar = new ProgressBar();
                    }
                    this.customIndeterminateProgressBar.IsIndeterminate = true;
                    this.customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Visible;


                    Focus();

                    BeginGetDashboard();
                }
                else
                {
                    MessageBox.Show(WindowsAzureStatus.Utils.ResourceStrings.Resource("AppErrorMsgNoNetworkConnection"));
                }
            }
            else
            {
                MessageBox.Show("Waiting for a request to return.");
            }
        }

        private void BeginGetDashboard()
        {

            // Pass call into View Model, who passes it to client library
            App.ViewModel.DashboardModelView.BeginGetDashboard(this.EndGetDashboard, this.HandleException);

            this.customIndeterminateProgressBar.Focus();
        }

        /// <summary>
        /// Callback From Successful Load Data Call
        /// </summary>
        /// <param name="asynchronousResult">Always Null</param>
        private void EndGetDashboard(IAsyncResult asynchronousResult)
        {
            App.ViewModel.RefreshData = false;
            Prep = false;

            // this date used in UI
            //App.ViewModel.LastSyncWithBerryInternationalWebService = DateTime.Now;
            App.ViewModel.DashboardModelView.LastUpdate = DateTime.UtcNow;

            // Hide Process Bar
            this.customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Collapsed;

            LoadUI();

            // this is the only time we go to iso storage 
            App.ViewModel.ToIsolatedStorage(); // save what we got so far

            this.Focus();
            this.PrepStarted = false;
        }

        private void HandleException(IAsyncResult asynchronousResult)
        {
            //not sending to iso storage so next time we load, we will have old deployments if they existed
            //regardless of how someone hits an exception returned from async, it is all handled here
            App.ViewModel.RefreshData = false;
            this.PrepStarted = false;
            Exception exception = (Exception)asynchronousResult.AsyncState;
            string error = "";
            bool navigateAwayFromThisPage = false;

            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.IsVisible = true;

            // TODO Update GUI
            if (exception is Wp7AzureMgmt.Wp7ClientLibrary.Exceptions.WebServiceException<ServiceDashboardResponseStatus>)
            {
                // WWB: Exception Happened In Our Web Service 
                var status = ((WebServiceException<ServiceDashboardResponseStatus>)exception).Status;


                switch (status)
                {
                    case ServiceDashboardResponseStatus.Exception:
                        // WWB: Generic Catch All Exception
                        error = "WAZDash webservice: an error occured (1).";
                        //App.ViewModel.AppSettings.MsgToUser = error;
                        break;
                    case ServiceDashboardResponseStatus.MalformedRequest:
                        // WWB: Public Key Certificates Not Applid One or More Subscriptions
                        error = "WAZDash webservice: malformed request";
                        //App.ViewModel.AppSettings.MsgToUser = "WAZDash webservice: malformed request";
                        break;
                    case ServiceDashboardResponseStatus.IllegalRequest:
                        // DFB: the REST call was not legal
                        error = "WAZDash webservice: illegal request";
                        //App.ViewModel.AppSettings.MsgToUser = "WAZDash webservice: illegal request";
                        break;
                    case ServiceDashboardResponseStatus.IllegalResponse:
                        // DFB: the REST call was not legal
                        error = "WAZDash webservice: illegal response";
                        //App.ViewModel.AppSettings.MsgToUser = "WAZDash webservice: illegal response";
                        break;
                    case ServiceDashboardResponseStatus.ServerNotFound:
                        // DFB: the REST call was not legal
                        error = "WAZDash: web service not found";
                        //App.ViewModel.AppSettings.MsgToUser = "WAZDash: web service not found";
                        break;
                    default:
                        error = "WAZDash webservice: an error occured (2).";
                        //App.ViewModel.AppSettings.MsgToUser = error;
                        break;
                }
            }
            else if (exception is WebException)
            {
                // WWB: Exception Caused From Calling Web Service, i.e. Web Service Is Down, etc...
                // TODO: Communicate To User About Issue
                switch (((WebException)exception).Status)
                {
                    case WebExceptionStatus.RequestCanceled:
                        // WWB: This happens when we call Abort on the Request
                        // Usually from a Timeout
                        error = "WAZDash phone app: request canceled or timeout occurred.";
                        //App.ViewModel.AppSettings.MsgToUser = error;
                        break;
                    default:
                        if (exception.Message == "The remote server returned an error: NotFound.")
                        {
                            error = "WAZDash phone app: WAZDash server not found.";
                            //App.ViewModel.AppSettings.MsgToUser = error;
                            break;
                        }
                        else
                        {
                            error = "WAZDash phone app: an error occured (1).";
                            //App.ViewModel.AppSettings.MsgToUser = error;
                            break;
                        }
                }
            }
            else
            {
                // generic error meaning we don't know enough to give better message - always track these down
                error = "WAZDash: error occurred.";
                //App.ViewModel.AppSettings.MsgToUser = error;
            }
            this.customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Collapsed;

            textBlockDataAge.Text = "Updated: Error";
            StackPanelServicesCountText.Text = "0 services found";

            this.Focus();
            if (!navigateAwayFromThisPage)
            {
                if (error.Contains("WAZDash"))
                    MessageBox.Show(error);
                else
                    MessageBox.Show("An Error Occured (-1)");
            }
        }
        #endregion

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            PrepForAsyncWebServiceCall();
        }
        private void textblockServiceName_KeyDown(object sender, KeyEventArgs e)
        {
            App.ViewModel.SelectedName = ((sender as TextBlock).Tag.ToString());
            App.ViewModel.SelectedObject = SelectedDashboardGroupObjectType.ServiceLocationGroup;
            NavigationService.Navigate(new Uri("/Pages/ServiceGroupsPage2.xaml", UriKind.Relative));
        }
        private void textblockServiceName_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.SelectedName = ((sender as TextBlock).Tag.ToString());
            App.ViewModel.SelectedObject = SelectedDashboardGroupObjectType.ServiceLocationGroup;
            NavigationService.Navigate(new Uri("/Pages/ServiceGroupsPage2.xaml", UriKind.Relative));
        }
        private void textblockLocationName_KeyDown(object sender, KeyEventArgs e)
        {
            App.ViewModel.SelectedName = ((sender as TextBlock).Tag.ToString());
            App.ViewModel.SelectedObject = SelectedDashboardGroupObjectType.LocationServiceGroup;
            NavigationService.Navigate(new Uri("/Pages/LocationGroupsPage2.xaml", UriKind.Relative));
        }
        private void textblockLocationName_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.SelectedName = ((sender as TextBlock).Tag.ToString());
            App.ViewModel.SelectedObject = SelectedDashboardGroupObjectType.LocationServiceGroup;
            NavigationService.Navigate(new Uri("/Pages/LocationGroupsPage2.xaml", UriKind.Relative));
        }

        private void AppConfigButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AboutPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SearchPage.xaml", UriKind.Relative));
        }

        private void AppMetaData_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/MetaPage.xaml", UriKind.Relative));
        }

    }
}