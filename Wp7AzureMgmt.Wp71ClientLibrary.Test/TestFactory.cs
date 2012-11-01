// -----------------------------------------------------------------------
// <copyright file="TestFactory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Wp7AzureMgmt.Wp71ClientLibrary.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TestFactory
    {
        public string WebServer 
        {
            get
            {
                #if DEBUG
                    return "http://127.0.0.1:81";
                #else
                    return "https://wazup.berryintl.com";
                #endif
            }
        }
    }
}
