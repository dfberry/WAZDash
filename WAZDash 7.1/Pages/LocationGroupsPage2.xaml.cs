

namespace WindowsAzureStatus.Pages
{
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
    using Microsoft.Phone.Net.NetworkInformation;
    using System.Diagnostics;
    using MemoryDiagnostics;
    using System.Windows.Navigation;

    public partial class LocationGroupsPage2 : PhoneApplicationPage
    {
        public LocationGroupsPage2()
        {
            LittleWatson.CheckForPreviousException(); 
            InitializeComponent();
            ApplicationTitle.Text = App.AppName;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadPage();
        }
        #region page event model
        private void LoadPage()
        {
            if (App.ViewModel.SelectedObject == SelectedDashboardGroupObjectType.LocationServiceGroup)
            {
                int i = 0;

                StackPanel containingStackPanel = new StackPanel();
                containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

                //ScrollerViewerList
                foreach (LocationService2 locationservice in App.ViewModel.DashboardModelView.GetSingleLocationServiceGroup(App.ViewModel.SelectedName).LocationServices)
                {
                    if (i == 0)
                    {
                        Location.Text = App.ViewModel.SelectedName;
                        i++;
                    }

                    TextBlock textblockServiceName = new TextBlock();
                    textblockServiceName.Text = locationservice.ServiceName;
                    textblockServiceName.TextWrapping = TextWrapping.Wrap;
                    textblockServiceName.Style = Resources["PhoneTextLargeStyle"] as Style;

                    containingStackPanel.Children.Add(textblockServiceName);

                    foreach (String issue in locationservice.Issues)
                    {
                        TextBlock textblockIssueName = new TextBlock();
                        textblockIssueName.TextWrapping = TextWrapping.Wrap;
                        textblockIssueName.Margin = new Thickness(10,0,0,20);
                        textblockIssueName.Text = issue;
                        textblockIssueName.Foreground = Resources["PhoneSubtleBrush"] as Brush;
                        containingStackPanel.Children.Add(textblockIssueName);
                    }
                }

                //ServiceCountText.Text = i.ToString() + " services(s)";
                //if (App.ViewModel.IssueAge != -1)
                //{
                //    ServiceCountText.Text += " in last " + App.ViewModel.IssueAge.ToString() + " days";
                //}
                //else
                //{
                //    ServiceCountText.Text += " in all days";
                //}

                ScrollerViewerList.Content = containingStackPanel;


            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/ServicesPage2.xaml?error=" + Uri.EscapeUriString("No Selected Object Found"), UriKind.Relative));

            }
            
            textBlockDataAge.Text = App.ViewModel.Updated;
        }
        #endregion



    }
}