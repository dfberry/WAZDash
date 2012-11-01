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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Phone.Marketplace;
using System.Xml.Serialization;

using WindowsAzureStatus;
using WindowsAzureStatus.Utils;
using WindowsAzureStatus.ViewModel;
using MemoryDiagnostics;
using System.Diagnostics;


namespace WindowsAzureStatus
{
    public partial class App : Application
    {
        //public const string AppName = "WAZDash";

        public const string AppDescription = "Windows Azure Dashboard Service";

        public const string AppVersion = "1.4.5.1";
        
        public static NavigationMode CurrentNavigationDirection { get; set; }
        public static Boolean NavigationDetermined { get; set; }

        public bool ModelDataGrabbed = false;
        //private bool ModelDataSet = false;

        private static MainViewModel dashboardViewModel = null;
        private Exception lastExceptionThrown;


        public static string AppName
        {
            get
            {
                string tempName = Name;

                if (App.IsTrialApp)
                {
                    // set # of trial usage to MaxTrial
                    App.ViewModel.Trial.InitializeTrailValue(); 
                    
                    tempName += " Trial ";

                    if (App.ViewModel.Trial.TrialRemaining != -1)
                    {
                        tempName += "(" + (App.ViewModel.Trial.TrialRemaining) + ")";
                    }
                }

                return tempName;
            }
        }

        private static string Name
        {
            get
            {
                string appTitle = WindowsAzureStatus.Utils.ResourceStrings.Resource("AppName");

                return appTitle;
            }
        }

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (dashboardViewModel == null)
                    dashboardViewModel = new MainViewModel();

                return dashboardViewModel;
            }
            set
            {
                dashboardViewModel = value;
            }
        }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        // Declare the dataObject variable as a public member of the Application class.
        // Real applications will use a more complex data structure, such as an XML
        // document. The only requirement is that the object be serializable.
        public string AppDataObject;

        public static Boolean IsTrialApp
        {
            get
            {
#if TRIALVER
                return true;
#else
                var license = new Microsoft.Phone.Marketplace.LicenseInformation();
                return license.IsTrial();
#endif
            }

        }

        /// <summary>
        /// Gets the application.
        /// </summary>
        public new static App Current
        {
            get
            {
                return Application.Current as App;
            }
        }
        
        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
 
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            lastExceptionThrown = new Exception();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            //RootFrame.Navigating += new NavigatingCancelEventHandler(RootFrame_Navigating);
            NavigationDetermined = false;
            CurrentNavigationDirection = NavigationMode.New;

            // Use one of the two different mechanisms for re-directing the main page
            //SetupUriMapper();
           
        }


        public void FromBackgroundIsolatedStorage()
        {
            try
            {
                if (ModelDataGrabbed == false)
                {
                    // load the view model from isolated storage
                    using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                    using (var stream = new IsolatedStorageFileStream("wazdashbackgrounddata.txt", FileMode.OpenOrCreate, FileAccess.Read, store))
                    using (var reader = new StreamReader(stream))
                    {
                        if (!reader.EndOfStream)
                        {
                            var serializer = new XmlSerializer(typeof(MainViewModel));
                            ViewModel = (MainViewModel)serializer.Deserialize(reader);
                        }
                    }

                    ModelDataGrabbed = true;

                    // set the frame DataContext
                    RootFrame.DataContext = ViewModel;
                }
            }
            catch (Exception ex)
            {

                ModelDataGrabbed = false;
                RootFrame.DataContext = null;
            }
        }

        public void FromIsolatedStorage()
        {
            try
            {
                if (ModelDataGrabbed == false)
                {
                    // load the view model from isolated storage
                    using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                    using (var stream = new IsolatedStorageFileStream("wazdashdata.txt", FileMode.OpenOrCreate, FileAccess.Read, store))
                    using (var reader = new StreamReader(stream))
                    {
                        if (!reader.EndOfStream)
                        {
                            var serializer = new XmlSerializer(typeof(MainViewModel));
                            ViewModel = (MainViewModel)serializer.Deserialize(reader);
                        }
                    }

                    ModelDataGrabbed = true;

                    // set the frame DataContext
                    RootFrame.DataContext = ViewModel;
                }
            }
            catch (Exception ex)
            {

                ModelDataGrabbed = false;
                RootFrame.DataContext = null;
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            FromIsolatedStorage();
        }


        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved)
            {
                FromIsolatedStorage();
            }
        }


        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            //ToIsolatedStorage();
            
        }
        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            //ToIsolatedStorage();

        }
        // DFB: TODO/FIX Stub code to detect Device or Simulator
        // Stolen from Cory Smith (@smixx) http://www.innovativetechguy.com/?p=47
        // usage: App.IsRunningOnDevice
        public static bool IsRunningOnDevice
        {
            get { return Microsoft.Devices.DeviceType.Device == Microsoft.Devices.Environment.DeviceType; }
        }




        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
                      
            LittleWatson.ReportException((Exception)e.Exception, "App.xaml.cs:RootFrame_NavigationFailed "); 

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
                     
            LittleWatson.ReportException((Exception)e.ExceptionObject, "App.xaml.cs:Application_UnhandledException "); 
            
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;

            //DFB: Don't Refresh data until asked to
            //DFB: 2011/09/21 this is crashing - not sure why
            ViewModel.RefreshData = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}