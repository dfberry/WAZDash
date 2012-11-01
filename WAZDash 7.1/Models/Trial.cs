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
using System.ComponentModel;

namespace WindowsAzureStatus.Models
{
    public class AppTrial : INotifyPropertyChanged
    {

        private const int MaxTrial = 50;

        // isolated storage on phone captured last call
        private int TrialIsoStorageRemaining { get; set; }
        
        // response back from client library - do not alter these values
        public int TrialResponseRemaining { get; set; }
        public Boolean TrialResponseRecieved { get; set; }

        public AppTrial()
        {
            //treat this app as non-trial until we learn different
            TrialIsoStorageRemaining = -1;
            TrialResponseRemaining = -1;
            TrialResponseRecieved = false;

        }
        //redirect to buy page if istrial and trial is expired
        public Boolean IsRedirectForExpiredTrial 
        {
            get
            {
                if ((App.IsTrialApp) && (IsTrialExpired))
                    return true;
                else
                    return false;
            }
        }

        public string IsTrialAsString
        {
            get
            {
                String trialtemp = String.Empty;

                if (App.IsTrialApp)
                {
                    if (IsTrialExpired)
                    {
                        trialtemp = "Expired Trial";
                    }
                    else
                    {
                        trialtemp = "Unexpired Trial";
                    }
                }
                return trialtemp;
            }

        }

        // trial expired is based on last call captured in isolated settings
        // if isolated settings doesn't say expired,
        // then we go out to website, and base expiration on response
        public Boolean IsTrialExpired
        {
            get
            {
                if ((App.IsTrialApp) && (TrialResponseRecieved) && (TrialResponseRemaining == 0))
                    return true;
                else
                    return false;
            }
        }
        // this will begin with the isolated storage value
        // then every response will contain a value
        public int TrialRemaining
        {
            get
            {
                // -1 == not a trial app so doesn't matter
                if (!App.IsTrialApp)
                    return -1;
                else
                {
                    if (!TrialResponseRecieved)
                    {
                        return TrialIsoStorageRemaining;
                    }
                    else
                    {
                        return TrialResponseRemaining;
                    }
                }
            }
        }
        public void InitializeTrailValue()
        {
            // App is a trial version and never been set before
            // should only happen first time through
            if ((App.IsTrialApp) && (TrialIsoStorageRemaining== - 1))
            {
                TrialIsoStorageRemaining = MaxTrial;
            }
        }
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

}
