namespace WindowsAzureStatus.Models
{
    using Microsoft.Phone.Info; 

    public static class Phone
    {
        private static readonly int ANIDLength = 32;
        private static readonly int ANIDOffset = 2;
        public static string GetManufacturer()
        {
            string result = string.Empty;
            object manufacturer;
            if (DeviceExtendedProperties.TryGetValue("DeviceManufacturer", out manufacturer))
                result = manufacturer.ToString();

            return result;
        }

        //Note: to get a result requires ID_CAP_IDENTITY_DEVICE  
        // to be added to the capabilities of the WMAppManifest  
        // this will then warn users in marketplace  
        public static byte[] GetDeviceUniqueIDAsBtyeArray()
        {
            byte[] result = null;
            object uniqueId;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                result = (byte[])uniqueId;

            return result;
        }


        //Note: to get a result requires ID_CAP_IDENTITY_DEVICE  
        // to be added to the capabilities of the WMAppManifest  
        // this will then warn users in marketplace  
        //public static string GetDeviceUniqueID()
        //{
        //    byte[] result = GetDeviceUniqueIDAsBtyeArray();

        //    return result = this.GetDeviceUniqueIDAsBtyeArray;
        //}

        // NOTE: to get a result requires ID_CAP_IDENTITY_USER  
        //  to be added to the capabilities of the WMAppManifest  
        // this will then warn users in marketplace  
        public static string GetWindowsLiveAnonymousID()
        {
            string result = string.Empty;
            object anid;
            if (UserExtendedProperties.TryGetValue("ANID", out anid))
            {
                if (anid != null && anid.ToString().Length >= (ANIDLength + ANIDOffset))
                {
                    result = anid.ToString().Substring(ANIDOffset, ANIDLength);
                }
            }

            return result;
        }

        //#region EncodingType enum
        ///// <summary> 
        ///// Encoding Types. 
        ///// </summary> 
        //public enum EncodingType
        //{
        //    ASCII,
        //    Unicode,
        //    UTF7,
        //    UTF8
        //}
        //#endregion 

        //#region ByteArrayToString
        /// <summary> 
        /// Converts a byte array to a string using Unicode encoding. 
        /// </summary> 
        /// <param name="bytes">Array of bytes to be converted.</param> 
        /// <returns>string</returns> 
        //public static string ByteArrayToString(byte[] bytes)
        //{
        //    return ByteArrayToString(bytes, EncodingType.Unicode);
        //}
        /// <summary> 
        /// Converts a byte array to a string using specified encoding. 
        /// </summary> 
        /// <param name="bytes">Array of bytes to be converted.</param> 
        /// <param name="encodingType">EncodingType enum.</param> 
        /// <returns>string</returns> 
        //public static string ByteArrayToString(byte[] bytes, EncodingType encodingType)
        //{
        //    System.Text.Encoding encoding = null;
        //    switch (encodingType)
        //    {
        //        //case EncodingType.ASCII:
        //        //    encoding = new System.Text.ASCIIEncoding();
        //            //break;
        //        case EncodingType.Unicode:
        //            encoding = new System.Text.UnicodeEncoding();
        //            break;
        //        //case EncodingType.UTF7:
        //        //    encoding = new System.Text.UTF7Encoding();
        //            //break;
        //        case EncodingType.UTF8:
        //            encoding = new System.Text.UTF8Encoding();
        //            break;
        //    }
        //    return encoding.GetString(bytes);
        //}
        //#endregion 


    }  
}
