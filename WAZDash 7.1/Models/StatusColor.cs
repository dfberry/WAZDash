namespace WindowsAzureStatus.Models
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Media;

    public static class StatusColor
    {
        //this class must return a valid object for a FILL attribute of a rectangle

        public static Brush SolidColorBrush(string status)
        {
            SolidColorBrush tempBrush = new SolidColorBrush(StatusStringToColorObject(status));
                
            return tempBrush;
        }
        public static Brush LinearGradientColorBrush(string status)
        {
            LinearGradientBrush tempBrush = new LinearGradientBrush();
            tempBrush.StartPoint = new Point(0, 0);
            tempBrush.EndPoint = new Point(10, 10);
            tempBrush.Opacity = .5;

            tempBrush.MappingMode = BrushMappingMode.Absolute;
            tempBrush.SpreadMethod = GradientSpreadMethod.Repeat;

            GradientStop stop1 = new GradientStop();
            stop1.Color = StatusStringToColorObject(status);
            tempBrush.GradientStops.Add(stop1);

            GradientStop stop2 = new GradientStop();
            stop2.Color = StatusStringToColorObject(status);
            stop2.Offset = 0.49;
            tempBrush.GradientStops.Add(stop2);

            GradientStop stop3 = new GradientStop();
            stop3.Color = Colors.Transparent;
            stop3.Offset = 0.51;
            tempBrush.GradientStops.Add(stop3);

            GradientStop stop4 = new GradientStop();
            stop3.Color = Colors.Transparent;
            stop3.Offset = 1;
            tempBrush.GradientStops.Add(stop4);

            return tempBrush;
        }
        public static Color StatusStringToColorObject(string status)
        { 
            switch (status)
            {
                case "Running":
                case "Created":
                case "Ready":
                    return Colors.Green;
                case "Stopped":
                case "Disabled":
                case "Suspending":
                case "Suspended":
                case "SuspendedTransitioning":
                case "Deleting":
                    return Colors.Red;
                case "Stopping":
                    return Colors.Orange;
                case "Starting":
                case "Initializing":
                case "Preparing":
                case "Waiting":
                case "Deploying":
                case "RunningTransitioning":
                    return Colors.Blue;
                default:
                    return Colors.Gray; //error case - shouldn't happen

            }
        }
    }
}
