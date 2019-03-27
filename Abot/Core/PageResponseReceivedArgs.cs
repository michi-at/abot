using Abot.Poco;
using System;
using System.Net;

namespace Abot.Core
{
    [Serializable]
    public class PageResponseReceivedArgs : EventArgs
    {
        public CrawledPage CrawledPage { get; private set; }
        public HttpWebResponse HttpWebResponse { get; private set; }

        public PageResponseReceivedArgs(CrawledPage crawledPage, HttpWebResponse response)
        {
            if (crawledPage == null || response == null)
                throw new ArgumentNullException("Event args");

            CrawledPage = crawledPage;
            HttpWebResponse = response;
        }
    }
}
