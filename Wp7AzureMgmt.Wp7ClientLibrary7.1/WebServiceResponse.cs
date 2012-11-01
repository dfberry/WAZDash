


namespace Wp7AzureMgmt.Wp7ClientLibrary
{
    using System;
    using System.Threading;

    public class WebServiceResponse<T> : IAsyncResult
    {
        private T state = default(T);
        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        public WebServiceResponse(T state)
        {
            this.state = state;
        }

        public object AsyncState
        {
            get
            {
                return (this.state);
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return (manualResetEvent);
            }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsCompleted
        {
            get
            {
                return (true);
            }
        }
    }
}
