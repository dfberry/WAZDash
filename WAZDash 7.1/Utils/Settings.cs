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
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace WindowsAzureStatus
{

    public class Settings : IsoStoreAppSettings
    {
        //DFB-todo: rip out hard coded strings

        //#region User
        /// <summary>
        /// 
        /// </summary>
        //string _User = string.Empty;
        //public string User
        //{
        //    get
        //    {
        //        return base.GetValueOrDefault<string>(
        //            "User",
        //            _User
        //            );
        //    }
        //    set
        //    {
        //        base.AddOrUpdateValue("User", value);
        //    }
        //}
        //#endregion

        //#region Password
        ///// <summary>
        ///// 
        ///// </summary>
        //string _Password = string.Empty;
        //public string Password
        //{
        //    get
        //    {
        //        return base.GetValueOrDefault<string>(
        //            "Password",
        //            _Password
        //            );
        //    }
        //    set
        //    {
        //        base.AddOrUpdateValue("Password", value);
        //    }
        //}
        //#endregion

        //#region TrialUsageCount
        ///// <summary>
        ///// 
        ///// </summary>
        //int _TrialUsageCount = 0;
        //public int TrialUsageCount
        //{
        //    get
        //    {
        //        return base.GetValueOrDefault<int>(
        //            "TrialUsageCount",
        //            _TrialUsageCount
        //            );
        //    }
        //    set
        //    {
        //        base.AddOrUpdateValue("TrialUsageCount", value);
        //    }
        //}
        //#endregion

        //#region TrialExpired
        ///// <summary>
        ///// 
        ///// </summary>
        //bool _TrialExpired = false;
        //public bool TrialExpired
        //{
        //    get
        //    {
        //        return base.GetValueOrDefault<bool>(
        //            "TrialExpired",
        //            _TrialExpired
        //            );
        //    }
        //    set
        //    {
        //        base.AddOrUpdateValue("TrialExpired", value);
        //    }
        //}
        //#endregion

        #region MsgToUser
        /// <summary>
        /// 
        /// </summary>
        string _MsgToUser = string.Empty;
        public string MsgToUser
        {
            get
            {
                return base.GetValueOrDefault<string>(
                    "MsgToUser",
                    _MsgToUser
                    );
            }
            set
            {
                base.AddOrUpdateValue("MsgToUser", value);
            }
        }
        #endregion
    
    }

}
