

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

    public partial class ServiceGroupsPage2 : PhoneApplicationPage
    {
        public ServiceGroupsPage2()
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
            if (App.ViewModel.SelectedObject == SelectedDashboardGroupObjectType.ServiceLocationGroup)
            {

                int i = 0;

                StackPanel containingStackPanel = new StackPanel();
                containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

                //ScrollerViewerList
                foreach (ServiceLocation2 servicelocation in App.ViewModel.DashboardModelView.GetSingleServiceLocationGroup(App.ViewModel.SelectedName).ServiceLocations)
                {
                    if (i == 0)
                    {
                        Service.Text = App.ViewModel.SelectedName;
                    }
                    i++;

                    TextBlock textblockLocationName = new TextBlock();
                    textblockLocationName.Text = servicelocation.LocationName;
                    textblockLocationName.TextWrapping = TextWrapping.Wrap;
                    textblockLocationName.Style = Resources["PhoneTextLargeStyle"] as Style;

                    containingStackPanel.Children.Add(textblockLocationName);

                    foreach (String issue in servicelocation.Issues)
                    {
                        TextBlock textblockIssueName = new TextBlock();
                        textblockIssueName.TextWrapping = TextWrapping.Wrap;
                        textblockIssueName.Margin = new Thickness(10, 0, 0, 20);
                        textblockIssueName.Text = issue;
                        textblockIssueName.Foreground = Resources["PhoneSubtleBrush"] as Brush;
                        containingStackPanel.Children.Add(textblockIssueName);
                    }
                }
                //LocationCountText.Text = i.ToString() + " location(s) ";
                //if (App.ViewModel.IssueAge != -1)
                //{
                //    LocationCountText.Text += " in last " + App.ViewModel.IssueAge.ToString() + " days";
                //}
                //else
                //{
                //    LocationCountText.Text += " in all days";
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