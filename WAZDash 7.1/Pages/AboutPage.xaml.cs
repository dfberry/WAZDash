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
using Microsoft.Phone.Shell;
using WindowsAzureStatus.Resources;
//using Coding4Fun.Phone.Controls.Data;
using WindowsAzureStatus.Utils;
using Microsoft.Phone.Tasks;
using System.Windows.Navigation;

using Microsoft.Phone.Controls;


namespace WindowsAzureStatus.Pages
{
    public partial class AboutPage : PhoneApplicationPage
    {
        bool ListLoaded = false;
        public AboutPage()
        {
            LittleWatson.CheckForPreviousException();
            InitializeComponent();

            textBlock1.Text = WindowsAzureStatus.Utils.ResourceStrings.Resource("LiteralVersion") + " " + App.AppVersion + "\n\n";
            ApplicationTitle.Text = App.AppName; 
        }
        #region Click
        private void buttonSupportEmail_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.EmailComposeTask emailComposeTask = new Microsoft.Phone.Tasks.EmailComposeTask();
            emailComposeTask.To = WindowsAzureStatus.Utils.ResourceStrings.Resource("SupportEmailToEmailAddress");
            emailComposeTask.Subject = WindowsAzureStatus.Utils.ResourceStrings.Resource("SupportEmailSubject");
            emailComposeTask.Body = WindowsAzureStatus.Utils.ResourceStrings.Resource("SupportEmailBody") +
                " " + WindowsAzureStatus.Utils.ResourceStrings.Resource("LiteralVersion") + " " + App.AppVersion;
             emailComposeTask.Show(); // Launches send mail screen
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Load();
        }
        private void buttonReview_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.MarketplaceReviewTask marketplaceReviewTask = new Microsoft.Phone.Tasks.MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
        #endregion
        private void Load()
        {
            //if (App.ViewModel.AppSettings.IssueAge == -1)
            //{
            //    DayRangeSelector.SelectedIndex = 4; // all
            //}
            //else
            //{
                switch (App.ViewModel.AppSettings.IssueAge)
                {
                    case 30:
                        DayRangeSelector.SelectedIndex = 0; // 30 days
                        break;
                    case 14:
                        DayRangeSelector.SelectedIndex = 1; // 14 days
                        break;
                    case 7:
                        DayRangeSelector.SelectedIndex = 2; // 7 days
                        break;
                    case 3:
                    case 0: // dayrange may be uninitialized but still set it to 3 days
                        DayRangeSelector.SelectedIndex = 3; // 3 days
                        break;
                    default:
                        DayRangeSelector.SelectedIndex = 3; // 3 days
                        break;
                }

            //}
            ListLoaded = true;
        }
        private void SaveIssueAge()
        {
            switch (DayRangeSelector.SelectedIndex)
            {
                //case 0:
                //    App.ViewModel.AppSettings.IssueAge = -1; // all days
                //    break;
                case 0:
                    App.ViewModel.AppSettings.IssueAge = 30; // 30 days
                    break;
                case 1:
                    App.ViewModel.AppSettings.IssueAge = 14; // 14 days
                    break;
                case 2:
                    App.ViewModel.AppSettings.IssueAge = 7; // 7 days
                    break;
                case 3:
                    App.ViewModel.AppSettings.IssueAge = 3; // 3 days
                    break;
                default:
                    App.ViewModel.AppSettings.IssueAge = 3; // 3 days
                    break;
            }
            App.ViewModel.RefreshData = true;

            App.ViewModel.ToIsolatedStorage();
        }
        private void DayRangeSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ListLoaded)
            {
                SaveIssueAge();
                NavigationService.Navigate(new Uri("/Pages/ServicesPage2.xaml", UriKind.Relative));
            }

        }

        private void buttonWebsite_Click(object sender, RoutedEventArgs e)
        {
            //WebBrowser web = new Microsoft.Phone.Controls.WebBrowser();
            //web.Navigate(new Uri("http://apps.berryintl.com/", UriKind.Absolute));


            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://apps.berryintl.com/", UriKind.Absolute);
            webBrowserTask.Show();

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            App.ViewModel.AppSettings.ShowAllItems = false;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            App.ViewModel.AppSettings.ShowAllItems = true;

        }

  


    }
}