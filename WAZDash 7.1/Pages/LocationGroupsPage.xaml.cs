

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

    public partial class LocationGroupsPage : PhoneApplicationPage
    {
        public LocationGroupsPage()
        {
            LittleWatson.CheckForPreviousException(); 
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            PivotWindowsGroup.Title = App.Name;
            
        }
        #region page event model
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(MemoryDiagnosticsHelper.GetCurrentMemoryUsage());

            if (App.ViewModel.SelectedObject == SelectedDashboardGroupObjectType.LocationServiceGroup)
            {
                Location.Text = App.ViewModel.SelectedLocationServiceGroup.LocationName;
                ServiceCountText.Text = App.ViewModel.SelectedLocationServiceGroup.LocationServices.Count + " services(s)";
                if (App.ViewModel.IssueAge != -1)
                {
                    ServiceCountText.Text += " in last " + App.ViewModel.IssueAge.ToString() + " days";
                }
                else
                {
                    ServiceCountText.Text += " in all days";
                }

                //DataContext = App.ViewModel;

                StackPanel containingStackPanel = new StackPanel();
                containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

                //ScrollerViewerList
                foreach (LocationService locationservice in App.ViewModel.SelectedLocationServiceGroup.LocationServices)
                {
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
                ScrollerViewerList.Content = containingStackPanel;

            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/ServicesPage.xaml?error=" + Uri.EscapeUriString("No Selected Object Found"), UriKind.Relative));

            }
            
            textBlockDataAge.Text = App.ViewModel.Updated;
            Debug.WriteLine(MemoryDiagnosticsHelper.GetCurrentMemoryUsage());

        }
        #endregion



    }
}