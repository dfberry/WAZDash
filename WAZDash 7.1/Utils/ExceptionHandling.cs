


namespace WindowsAzureStatus
{

    using System;
    using System.Net;
    using System.Windows;
    using System.IO.IsolatedStorage;
    using System.IO;
    using WindowsAzureStatus.Resources;
    using Microsoft.Phone.Tasks;
    //using Coding4Fun.Phone.Controls.Data;
    using WindowsAzureStatus.Utils;
    using Wp7AzureMgmt.Wp7ClientLibrary;
    using WindowsAzureStatus.Models;


    //DFB: stolen from Andy Pennell
    //http://blogs.msdn.com/b/andypennell/archive/2010/11/01/error-reporting-on-windows-phone-7.aspx

    public class WAZDashExceptionPost
    {
        public static void PostException(Exception ex, string extra)
        {
            // now post to Windows Azure Table Storage
            // phone id doesn't have  Microsoft.Phone.Info.UserExtendedProperties.GetValue("ANID")
            // because we have logon
            ExceptionRequest request = new ExceptionRequest()
            {
                Login = "anonymous",
                AppName = App.AppName,
                AppVersion = App.AppVersion,
                PhoneId = Phone.GetWindowsLiveAnonymousID() + "-" + Phone.GetManufacturer(),
                Exception = "[exception]:" + ex
                    + " [extra]: " + extra
                    + " [ex.Message]: " + ex.Message
                    + " [ex.StackTrace]: " + ex.StackTrace
            };

            // WWB: Create a client pointed at the live web service
            
            ClientException client = new ClientException(App.ViewModel.RESTServer);

            // WWB: Make an async request for deployment information, response data processed in Call Backs
            client.AppExceptionCall(null, null, request);
        }
    }

    public class LittleWatson
    {
        const string filename = "LittleWatson.txt";
        public static string completeExceptionText;
        public static Exception thisException;

        private static void CaptureException(Exception ex1, string text)
        {
            thisException = ex1;

            completeExceptionText = WindowsAzureStatus.Utils.ResourceStrings.Resource("ExceptionTextPartExtra") + text + "\n" +
                    ", " + WindowsAzureStatus.Utils.ResourceStrings.Resource("ExceptionTextPartAppVersionNumber") + App.AppVersion + "\n" +
                    ", " + WindowsAzureStatus.Utils.ResourceStrings.Resource("ExceptionTypeName") + ex1.GetType() + "\n" +
                    " , " + WindowsAzureStatus.Utils.ResourceStrings.Resource("ExceptionTextPartExceptionMessage") + ex1.Message + "\n" +
                    " , " + WindowsAzureStatus.Utils.ResourceStrings.Resource("ExceptionTextPartExceptionStackTrace") + ex1.StackTrace;

        }
        public static void EmailException(string contents)
        {
            //EmailComposeTask email = new EmailComposeTask();
            //email.To = WindowsAzureStatus.Utils.ResourceStrings.Resource("SupportEmailToEmailAddress");
            //email.Subject = WindowsAzureStatus.Utils.ResourceStrings.Resource("AppName") + WindowsAzureStatus.Utils.ResourceStrings.Resource("ExceptionSubjectTitle");
            //email.Body = contents;
            //email.Show();

        }
        private static void SafeDeleteExceptionFile(IsolatedStorageFile store)
        {
            store.DeleteFile(filename);

        }
        public static string ReturnException(Exception ex, string extra)
        {
            CaptureException(ex, extra);

            return completeExceptionText;
        }

        internal static void ReportException(Exception ex, string extra)
        {
            //using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    SafeDeleteExceptionFile(store);
            //    using (TextWriter output = new StreamWriter(store.CreateFile(filename)))
            //    {
            //        CaptureException(ex, extra);

            //        output.WriteLine(extra);
            //        output.WriteLine(ex.Message);
            //        output.WriteLine(ex.StackTrace);
            //    }
            //}
            WAZDashExceptionPost.PostException(ex, extra);
        }
 
        internal static void CheckForPreviousException()
        {
            try
            {
                string contents = null;
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(filename))
                    {
                        using (TextReader reader = new StreamReader(store.OpenFile(filename, FileMode.Open, FileAccess.Read, FileShare.None)))
                        {
                            contents = reader.ReadToEnd();
                        }
                        SafeDeleteExceptionFile(store);
                    }
                }
                if (contents != null)
                {
                    //if (MessageBox.Show(WindowsAzureStatus.Utils.ResourceStrings.Resource("ExceptionMsg1"), WindowsAzureStatus.Utils.ResourceStrings.Resource("ExceptionTypeTitle"), MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    //{
                    //    EmailException(contents);
                    //    SafeDeleteExceptionFile(IsolatedStorageFile.GetUserStoreForApplication()); // line added 1/15/2011
                        
                    //}
                    WAZDashExceptionPost.PostException(new Exception(contents), null);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                SafeDeleteExceptionFile(IsolatedStorageFile.GetUserStoreForApplication());
            }
        }


    }
}

