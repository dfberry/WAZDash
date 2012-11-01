namespace Wp7AzureMgmt.Wp7ClientLibrary.Models
{
    using System;
    using Wp7AzureMgmt.Models;
    using Wp7AzureMgmt.Wp7ClientLibrary.Models;
    
    public class RSSRequest : Request
    {
        /// <summary>
        /// Gets or sets the deployments returned from request
        /// </summary>
        public RSS Reader { get; set; }

        public string ServiceCode { get; set; }
    }
}
