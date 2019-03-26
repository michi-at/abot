using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Abot.Core
{
    [Serializable]
    public class PageRequestSentArgs : EventArgs
    {
        public HttpWebRequest HttpWebRequest { get; private set; }

        public PageRequestSentArgs(HttpWebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("HttpWebRequest");

            HttpWebRequest = request;
        }
    }
}
