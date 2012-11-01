using System;
using System.Net;
using System.Windows;

using Microsoft.Phone.Controls;

namespace WindowsAzureStatus.Pages
{
    public partial class AppConfigPage : PhoneApplicationPage
    {
        bool ListLoaded = false;

        public AppConfigPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.ViewModel.IssueAge == -1)
            {
                DayRangeSelector.SelectedIndex = 0; // all
            }
            else
            {
                switch (App.ViewModel.IssueAge)
                {
                    case 30:
                        DayRangeSelector.SelectedIndex = 1; // 30 days
                        break;
                    case 14:
                        DayRangeSelector.SelectedIndex = 2; // 14 days
                        break;
                    case 7:
                        DayRangeSelector.SelectedIndex = 3; // 7 days
                        break;
                    case 3:
                    case 0: // dayrange may be uninitialized but still set it to 3 days
                        DayRangeSelector.SelectedIndex = 4; // 3 days
                        break;
                }

            }
            ListLoaded = true;
        }
        private void SaveIssueAge()
        {
            switch (DayRangeSelector.SelectedIndex)
            {
                case 0:
                    App.ViewModel.IssueAge = -1; // all days
                    break;
                case 1:
                    App.ViewModel.IssueAge = 30; // 30 days
                    break;
                case 2:
                    App.ViewModel.IssueAge = 14; // 14 days
                    break;
                case 3:
                    App.ViewModel.IssueAge = 7; // 7 days
                    break;
                case 4:
                    App.ViewModel.IssueAge = 3; // 3 days
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



    }
}