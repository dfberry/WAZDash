

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
    using Wp7AzureMgmt.Wp7ClientLibrary.Dashboard;

    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
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
            //if (App.ViewModel.SelectedObject == SelectedDashboardGroupObjectType.LocationServiceGroup)
            //{
            //    int i = 0;

            //    StackPanel containingStackPanel = new StackPanel();
            //    containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

            //    //ScrollerViewerList
            //    foreach (LocationService2 locationservice in App.ViewModel.DashboardModelView.GetSingleLocationServiceGroup(App.ViewModel.SelectedName).LocationServices)
            //    {
            //        TextBlock textblockServiceName = new TextBlock();
            //        textblockServiceName.Text = locationservice.ServiceName;
            //        textblockServiceName.TextWrapping = TextWrapping.Wrap;
            //        textblockServiceName.Style = Resources["PhoneTextLargeStyle"] as Style;

            //        containingStackPanel.Children.Add(textblockServiceName);

            //        foreach (String issue in locationservice.Issues)
            //        {
            //            TextBlock textblockIssueName = new TextBlock();
            //            textblockIssueName.TextWrapping = TextWrapping.Wrap;
            //            textblockIssueName.Margin = new Thickness(10,0,0,20);
            //            textblockIssueName.Text = issue;
            //            textblockIssueName.Foreground = Resources["PhoneSubtleBrush"] as Brush;
            //            containingStackPanel.Children.Add(textblockIssueName);
            //        }
            //    }

            //    ScrollerViewerList.Content = containingStackPanel;

            //}
            //else
            //{
            //    NavigationService.Navigate(new Uri("/Pages/ServicesPage2.xaml?error=" + Uri.EscapeUriString("No Selected Object Found"), UriKind.Relative));

            //}
            
            textBlockDataAge.Text = App.ViewModel.Updated;
        }
        #endregion

        private void TextBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) && (SearchTerm.Text.Trim().Length>0))
            {
                buttonSearch_Click(sender, e);
            }
        }
        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            StackPanel containingStackPanel = new StackPanel();
            containingStackPanel.Margin = new Thickness(10, 0, 0, 20);

            int i = 0;

            foreach (RSSFeedItemDetail issue in App.ViewModel.DashboardModelView.GetIssuesBySearchTerm(SearchTerm.Text.Trim()))
            {
                i++;

                TextBlock textblockissueTitle = new TextBlock();
                textblockissueTitle.Text = i.ToString() + " " + issue.Title;
                textblockissueTitle.TextWrapping = TextWrapping.Wrap;
                textblockissueTitle.Style = Resources["PhoneTextNormalStyle"] as Style;
                containingStackPanel.Children.Add(textblockissueTitle);

                TextBlock textblockissueDescription = new TextBlock();
                textblockissueDescription.Text = issue.Description + "\n\n";
                textblockissueDescription.TextWrapping = TextWrapping.Wrap;
                textblockissueDescription.Style = Resources["PhoneTextNormalStyle"] as Style;
                //textblockissuePubDate.Foreground = Resources["PhoneSubtleBrush"] as Brush;
                containingStackPanel.Children.Add(textblockissueDescription);
            }
            ScrollerViewerList.Content = containingStackPanel;

        }


    }
}