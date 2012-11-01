

namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;
    using System.Net;

    public class DeploymentStatusString
    {
        public static string GetStatus(DeploymentStatus deploymentStatus)
        {
            switch (deploymentStatus)
            {
                case DeploymentStatus.Unset:
                    return "Unset";
                case DeploymentStatus.Delete:
                    return "Delete";
                case DeploymentStatus.Start:
                    return "Start";
                case DeploymentStatus.Stop:
                    return "Stop";
                case DeploymentStatus.Swap:
                    return "Swap";
                default:
                    return ""; //probably wrong thing to do here, throw exception?
            }

        }
    }

    public enum DeploymentStatus
    {
        Unset,
        Start,
        Stop, 
        Swap,
        Delete

    }
}
