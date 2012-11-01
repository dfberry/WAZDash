using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wp7AzureMgmt.Wp7ClientLibrary;
using Wp7AzureMgmt.Wp7ClientLibrary.Dashboard;


namespace Wp7AzureMgmt.Wp71ClientLibrary.Test
{
    [TestClass()]
    public class ClientDashboardTest : TestFactory
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


    }
}
