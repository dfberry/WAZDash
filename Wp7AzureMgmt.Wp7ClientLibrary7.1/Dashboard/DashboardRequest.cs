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
using System.Collections.Generic;

namespace Wp7AzureMgmt.Wp7ClientLibrary.Dashboard
{
    public class DashboardRequest : Request
    {
        /// <summary>
        /// Gets or sets the service dashboard returned from request
        /// </summary>
        public DashboardResponse DashboardResponse { get; set; }

        /// <summary>
        /// Request [1 == trial, -1 == not trial]
        /// Response [0=expired trial, >0 nonexpired trial, -1 = not trial]
        /// </summary>
        public int TrialRemaining { get; set; }

        public string AppVersion { get; set; }

        public int IssueAge { get; set; }

        public int FetchAllIncludingEmpties { get; set; }

        public String UserId { get; set; }

        public String PhoneId { get; set; }

        public String PhoneMaker { get; set; }

        /// <summary>
        /// Summary of data
        /// true == count of items since & most recent item - this is used by background agent for toast
        /// false == all items since - this is used by app
        /// </summary>
        public bool Summary { get; set; }

    }

}
