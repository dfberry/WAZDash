﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Scheduler;

namespace WindowsAzureStatus.Models
{
    public class BackgroundTask
    {
        public Boolean AgentsAreEnabled { get; set; }

        public PeriodicTask PeriodicTask { get; set; }
    }
}
