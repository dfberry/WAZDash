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
using WindowsAzureStatus.Models;
using Microsoft.Phone.Scheduler;

namespace WindowsAzureStatus.ViewModels
{
    public class BackgroundTaskFactory
    {
        public string TaskName = App.AppName;

        public string TaskDescription = App.AppDescription;

        public BackgroundTask Create()
        {
            BackgroundTask backgroundTask = new BackgroundTask()
            {
                AgentsAreEnabled = true,
                PeriodicTask = new Microsoft.Phone.Scheduler.PeriodicTask(this.TaskName)
                {
                    Description = this.TaskDescription,
                }
            };
            
            // Place the call to Add in a try block in case the user has disabled agents.
            try
            {
                ScheduledActionService.Add(backgroundTask.PeriodicTask);
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show("Background agents for this application have been disabled by the user.");
                    backgroundTask.AgentsAreEnabled = false;
                }
            }

            return backgroundTask;
        }
        private void RemoveAgent()
        {
            try
            {
                ScheduledActionService.Remove(this.TaskName);
            }
            catch (Exception)
            {
            }
        }


    }
}
