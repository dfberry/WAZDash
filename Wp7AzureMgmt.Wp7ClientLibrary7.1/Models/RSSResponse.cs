using System;
using System.Net;


namespace Wp7AzureMgmt.Wp7ClientLibrary.Models
{
    public enum RSSResponseStatus
    {
        Success = 0,
        Exception
    }
    public class RSSResponse
    {
        public RSS Reader { get; set; }

        public string ServiceCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RSSResponseStatus Status { get; set; }
    }

}
