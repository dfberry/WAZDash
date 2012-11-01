using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WindowsAzureStatus.Resources;

namespace WindowsAzureStatus.Utils
{
    public class ResourceStrings : WindowsAzureStatus.Resources.Resource
    {

        /// <summary>
        /// Replaces string placeholders with correct strings. Both strings are kept in the string resource file.
        /// 
        /// Current replacements are:
        /// 
        /// [AppWebsiteURL] = location of webservice that populates app data
        /// [Companyname] = app manufacturer company name
        /// [YearDate] = year used for copyright 
        /// [CompanyURL] = url of app manufacturer
        /// [MicrosoftParentAzureProductName] = current name of Windows Azure
        /// [MicrosoftParentPhoneProductName] = current name of Windows Phone 7
        /// [AppAuthorName] = current app author(s)
        /// [AppName] = phone app name
        /// [NowDateTime] = shortdate + shorttime
        /// </summary>


        public static string Resource(string stringToFix)
        {
            string stringWorkingVersion = ResourceManager.GetString(stringToFix, Culture); 

            stringWorkingVersion = ReplaceCompanyName(stringWorkingVersion);
            stringWorkingVersion = ReplaceYearDate(stringWorkingVersion);
            stringWorkingVersion = ReplaceCompanyURL(stringWorkingVersion);
            stringWorkingVersion = ReplaceWebsiteURL(stringWorkingVersion);
            stringWorkingVersion = ReplaceMicrosoftParentProductName(stringWorkingVersion);
            stringWorkingVersion = ReplaceMicrosoftParentPhoneProductName(stringWorkingVersion);
            stringWorkingVersion = ReplaceAppAuthorName(stringWorkingVersion);
            stringWorkingVersion = ReplaceAppName(stringWorkingVersion);
            stringWorkingVersion = ReplaceEmailSupport(stringWorkingVersion);
            stringWorkingVersion = ReplaceDateTimeNow(stringWorkingVersion);

            return stringWorkingVersion;
        }

        public static string ReplaceCompanyName(string stringToFix)
        {
            return stringToFix.Replace("[Companyname]", AppCompanyName);
        }
        public static string ReplaceYearDate(string stringToFix)
        {
            return stringToFix.Replace("[YearDate]", DateTime.Today.ToString("yyyy"));
        }
        public static string ReplaceCompanyURL(string stringToFix)
        {
            return stringToFix.Replace("[CompanyURL]", AppCompanyURL);
        }
        public static string ReplaceWebsiteURL(string stringToFix)
        {
            return stringToFix.Replace("[AppWebsiteURL]", AppAzureCloudLocation);
        }
        public static string ReplaceMicrosoftParentProductName(string stringToFix)
        {
            return stringToFix.Replace("[MicrosoftParentAzureProductName]", MicrosoftParentAzureProductName);
        }
        public static string ReplaceMicrosoftParentPhoneProductName(string stringToFix)
        {
            return stringToFix.Replace("[MicrosoftParentPhoneProductName]", MicrosoftParentPhoneProductName);
        }
        public static string ReplaceAppAuthorName(string stringToFix)
        {
            return stringToFix.Replace("[AppAuthorName]", AppAuthorName);
        }
        public static string ReplaceAppName(string stringToFix)
        {
            return stringToFix.Replace("[AppName]", AppName);
        }          
        public static string ReplaceEmailSupport(string stringToFix)
        {
            return stringToFix.Replace("[EmailSupport]", SupportEmailToEmailAddress);
        }
        public static string ReplaceDateTimeNow(string stringToFix)
        {
            return stringToFix.Replace("[NowDateTime]", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
        }
    }
}
