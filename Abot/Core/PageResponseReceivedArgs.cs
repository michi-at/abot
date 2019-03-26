using System;
using System.Net;

namespace Abot.Core
{
    [Serializable]
    public class PageResponseReceivedArgs : EventArgs
    {
        public HttpWebResponse HttpWebResponse { get; private set; }

        public PageResponseReceivedArgs(HttpWebResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("HttpWebResponse");

            HttpWebResponse = response;
        }
    }
}
