namespace WindowsAzureStatus.Utils
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;

    public static class LinqObservableCollectionExtension
    {
        // DFB: found at Jimlynn's blog
        //http://jimlynn.wordpress.com/2008/12/09/using-observablecollection-with-linq/


        public static ObservableCollection<T> PopulateFrom<T>(this ObservableCollection<T> collection, IEnumerable<T> range)
        {
            foreach (var x in range)
            {
                collection.Add(x);
            }

            return collection;

        }
    }
}
