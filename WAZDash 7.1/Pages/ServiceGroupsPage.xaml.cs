

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

    public partial class ServiceGroupsPage : PhoneApplicationPage
    {
        public ServiceGroupsPage()
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

                if (App.ViewModel.SelectedObject == SelectedDashboardGroupObjectType.ServiceLocationGroup)
                {

                    Service.Text = App.ViewModel.SelectedServiceLocationGroup.ServiceName;

                    LocationCountText.Text = App.ViewModel.SelectedServiceLocationGroup.ServiceLocations.Count + " location(s) ";
                    if (App.ViewModel.IssueAge != -1)
                    {
                        LocationCountText.Text += " in last " + App.ViewModel.IssueAge.ToString() + " days";
                    }
                    else
                    {
                        LocationCountText.Text += " in all days";
                    }

                    //DataContext = App.ViewModel;

                    StackPanel containingStackPanel = new StackPanel();
                    containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

                    //ScrollerViewerList
                    foreach (ServiceLocation servicelocation in App.ViewModel.SelectedServiceLocationGroup.ServiceLocations)
                    {
                        TextBlock textblockLocationName = new TextBlock();
                        textblockLocationName.Text = servicelocation.LocationName;
                        textblockLocationName.TextWrapping = TextWrapping.Wrap;
                        textblockLocationName.Style = Resources["PhoneTextLargeStyle"] as Style;

                        containingStackPanel.Children.Add(textblockLocationName);

                        foreach (String issue in servicelocation.Issues)
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