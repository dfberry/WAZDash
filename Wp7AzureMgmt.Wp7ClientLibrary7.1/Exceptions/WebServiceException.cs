

namespace Wp7AzureMgmt.Wp7ClientLibrary.Exceptions
{
    using System;
    using Wp7AzureMgmt.Models;

    public class WebServiceException<T> : Exception
    {
        public T Status { get; set; }
    }
}
