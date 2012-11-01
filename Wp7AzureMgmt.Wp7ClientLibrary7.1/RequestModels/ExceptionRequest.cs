
namespace Wp7AzureMgmt.Wp7ClientLibrary
{

    using System;
    using System.Threading;
    using Wp7AzureMgmt.Models;

    public class ExceptionRequest : Request
    {
        /// <summary>
        /// Unique Appname
        /// </summary>
        public String AppName { get; set; }

        /// <summary>
        /// Unique Appname
        /// </summary>
        public String AppVersion { get; set; }

        /// <summary>
        /// UserPhoneId = Phone.GetManufacturer() + " " + Phone.GetWindowsLiveAnonymousID();
        /// </summary>
        public String PhoneId { get; set; }

        /// <summary>
        /// Complete exception details including stacktrace
        /// </summary>
        public String Exception { get; set; }

        /// <summary>
        /// username passed in Request from app
        /// can be empty for some apps
        /// </summary>
        public string Login { get; set; }
    }
}
