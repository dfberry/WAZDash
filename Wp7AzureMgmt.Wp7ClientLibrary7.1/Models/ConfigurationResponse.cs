


namespace Wp7AzureMgmt.Models
{
    using System;

    public enum ConfigurationResponseStatus
    {
        Success = 0,
        Exception
    }

    public class ConfigurationResponse
    {
        public Configuration Configuration { get; set; }
        public ConfigurationResponseStatus Status { get; set; }
    }
}