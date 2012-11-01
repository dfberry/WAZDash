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
using System.Runtime.Serialization.Json;
using System.IO;
using Wp7AzureMgmt.Models;
using System.Diagnostics;

namespace Wp7AzureMgmt.Wp7ClientLibrary.Utilities
{
    public class JsonResponse
    {
        public static T Parse<T>(HttpWebResponse httpWebResponse) where T : Response, new()
        {
            if (httpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));

                if (httpWebResponse.ContentLength > int.MaxValue)
                {
                    throw new IndexOutOfRangeException(String.Format("Response From Web Service Exceeds {0}", int.MaxValue));
                }

                // WWB: Read The Data From The Response Stream
                int length = (int)httpWebResponse.ContentLength;
                byte[] data = new byte[length];
                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    responseStream.Read(data, 0, length);
                }

                // WWB: In A Memory Stream Convert the Json
                using (MemoryStream stream = new MemoryStream(data))
                {
                    Debug.WriteLine(stream.ToString());

                    T response = (T)(deserializer.ReadObject(stream));

                    //// WWB: Check The Response For Errors
                    //if (response.ResponseStatus != Wp7AzureMgmt.Models.ResponseStatus.Success)
                    //{
                    //    var exception = new WebServiceException<Wp7AzureMgmt.Models.ResponseStatus>()
                    //    {
                    //        Status = response.ResponseStatus
                    //    };

                    //    throw exception;
                    //}

                    return response;
                }
            }
            else
            {
                T ErrorResponse = new T()
                {
                    Status = Wp7AzureMgmt.Models.ResponseStatus.IllegalResponse
                };

                return ErrorResponse;
            }
        }
    }
}
