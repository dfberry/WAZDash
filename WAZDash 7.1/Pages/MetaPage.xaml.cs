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
using System.Windows.Navigation;
using WindowsAzureStatus.Models;

namespace WindowsAzureStatus.Pages
{
    public partial class MetaPage : PhoneApplicationPage
    {
        public MetaPage()
        {
            InitializeComponent();
            PivotWindowsServiceDashboard.Title = App.AppName;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadPage();
        }
        private void LoadPage()
        {
            LoadServiceList();
            LoadLocationList();
        }
        private void LoadServiceList()
        {
            StackPanel containingStackPanel = new StackPanel();
            containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

            int i = 0;

            foreach (String service in App.ViewModel.DashboardModelView.GetDistinctServices())
            {
                i++;

                TextBlock textblockissueTitle = new TextBlock();
                textblockissueTitle.Text = i.ToString() + " " + service + " (" + App.ViewModel.DashboardModelView.GetServiceIssueCount(service) + ")";
                textblockissueTitle.Tag = service;
                textblockissueTitle.TextWrapping = TextWrapping.Wrap;
                textblockissueTitle.Style = Resources["PhoneTextTitle2Style"] as Style;

                textblockissueTitle.KeyDown += new KeyEventHandler(textblockServiceName_KeyDown);
                textblockissueTitle.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(textblockServiceName_Tap);                
                
                containingStackPanel.Children.Add(textblockissueTitle);



            }
            ScrollerViewerListServices.Content = containingStackPanel;
            StackPanelServicesCountText.Text = i.ToString() + " services found";
        }
        private void LoadLocationList()
        {
            StackPanel containingStackPanel = new StackPanel();
            containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

            int i = 0;

            foreach (String location in App.ViewModel.DashboardModelView.GetDistinctLocations())
            {
                i++;

                TextBlock textblockLocationName = new TextBlock();
                textblockLocationName.Text = i.ToString() + " " + location + " (" + App.ViewModel.DashboardModelView.GetLocationIssueCount(location) + ")";
                textblockLocationName.Tag = location;
                textblockLocationName.TextWrapping = TextWrapping.Wrap;
                textblockLocationName.Style = Resources["PhoneTextTitle2Style"] as Style;

                textblockLocationName.KeyDown += new KeyEventHandler(textblockLocationName_KeyDown);
                textblockLocationName.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(textblockLocationName_Tap);

                containingStackPanel.Children.Add(textblockLocationName);

            }
            ScrollerViewerListLocations.Content = containingStackPanel;
            StackPanelLocationsCountText.Text = i.ToString() + " locations found";
        }

        private void textblockServiceName_KeyDown(object sender, KeyEventArgs e)
        {
            //App.ViewModel.SelectedName = ((sender as TextBlock).Tag.ToString());
            //App.ViewModel.SelectedObject = SelectedDashboardGroupObjectType.ServiceLocationGroup;
            //NavigationService.Navigate(new Uri("/Pages/ServiceGroupsPage2.xaml", UriKind.Relative));
            CheckIssues(((sender as TextBlock).Tag.ToString()), SelectedDashboardGroupObjectType.ServiceLocationGroup); 
        }
        private void textblockServiceName_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //App.ViewModel.SelectedName = ((sender as TextBlock).Tag.ToString());
            //App.ViewModel.SelectedObject = SelectedDashboardGroupObjectType.ServiceLocationGroup;
            //NavigationService.Navigate(new Uri("/Pages/ServiceGroupsPage2.xaml", UriKind.Relative));
            CheckIssues(((sender as TextBlock).Tag.ToString()), SelectedDashboardGroupObjectType.ServiceLocationGroup);
        }
        private void textblockLocationName_KeyDown(object sender, KeyEventArgs e)
        {
            //App.ViewModel.SelectedName = ((sender as TextBlock).Tag.ToString());
            //App.ViewModel.SelectedObject = SelectedDashboardGroupObjectType.LocationServiceGroup;
            //NavigationService.Navigate(new Uri("/Pages/LocationGroupsPage2.xaml", UriKind.Relative));
            CheckIssues(((sender as TextBlock).Tag.ToString()), SelectedDashboardGroupObjectType.LocationServiceGroup);

        }
        private void textblockLocationName_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //App.ViewModel.SelectedName = ((sender as TextBlock).Tag.ToString());
            //App.ViewModel.SelectedObject = SelectedDashboardGroupObjectType.LocationServiceGroup;
            //NavigationService.Navigate(new Uri("/Pages/LocationGroupsPage2.xaml", UriKind.Relative));
            CheckIssues(((sender as TextBlock).Tag.ToString()), SelectedDashboardGroupObjectType.LocationServiceGroup);

        }
        private void CheckIssues(string Name, SelectedDashboardGroupObjectType ObjectType)
        {
            switch(ObjectType)
            {
                case SelectedDashboardGroupObjectType.ServiceLocationGroup:
                    ServiceLocationGroup2 grp = App.ViewModel.DashboardModelView.GetSingleServiceLocationGroup(Name);
                    if ((grp==null) ||  (grp.Issues==null) || (grp.Issues.Count==0))
                    {
                        ErrorMsgForNoIssues(Name);
                    }
                    else
                    {
                        App.ViewModel.SelectedName = Name;
                        App.ViewModel.SelectedObject = ObjectType;
                        NavigationService.Navigate(new Uri("/Pages/ServiceGroupsPage2.xaml", UriKind.Relative));
                    }
                    break;
                case SelectedDashboardGroupObjectType.LocationServiceGroup:
                    LocationServiceGroup2 grp2 = App.ViewModel.DashboardModelView.GetSingleLocationServiceGroup(Name);
                    if ((grp2 == null) || (grp2.Issues == null) || (grp2.Issues.Count == 0))
                    {
                        ErrorMsgForNoIssues(Name);
                    }
                    else
                    {
                        App.ViewModel.SelectedName = Name;
                        App.ViewModel.SelectedObject = ObjectType;
                        NavigationService.Navigate(new Uri("/Pages/LocationGroupsPage2.xaml", UriKind.Relative));
                    }
                    break;
                default:
                    throw new Exception("MetaPage.xaml.cs:ErrorMessageNoIssues - ServiceObject invalid");
            }
        }
        private void ErrorMsgForNoIssues(String objectName)
        {
            MessageBox.Show("No issues found for " + objectName);
        }
    }
}