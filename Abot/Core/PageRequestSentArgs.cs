using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Abot.Poco;

namespace Abot.Core
{
    [Serializable]
    public class PageRequestSentArgs : EventArgs
    {
        public CrawledPage CrawledPage { get; private set; }
        public HttpWebRequest HttpWebRequest { get; private set; }

        public PageRequestSentArgs(CrawledPage crawledPage, HttpWebRequest request)
        {
            if (crawledPage == null || request == null)
                throw new ArgumentNullException("Event args");

            CrawledPage = crawledPage;
            HttpWebRequest = request;
        }
    }
}
