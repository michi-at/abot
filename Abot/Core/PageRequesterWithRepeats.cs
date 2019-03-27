using Abot.Poco;
using log4net;
using System;
using System.Net;

namespace Abot.Core
{
    [Serializable]
    public class PageRequesterWithRepeats : PageRequester
    {
        protected ILog _logger = LogManager.GetLogger("AbotLogger");

        public PageRequesterWithRepeats(CrawlConfiguration config) : this(config, null)
        {

        }

        public PageRequesterWithRepeats(CrawlConfiguration config, IWebContentExtractor contentExtractor) : base(config, contentExtractor)
        {

        }

        public override CrawledPage MakeRequest(Uri uri, Func<CrawledPage, CrawlDecision> shouldDownloadContent)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            CrawledPage crawledPage = new CrawledPage(uri);

            for (int i = 1; i <= _config.NumberOfRecurrentRequests; ++i)
            {
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                try
                {
                    request = BuildRequestObject(uri);
                    crawledPage.RequestStarted = DateTime.Now;

                    FirePageRequestSentEvent(crawledPage, request);

                    response = (HttpWebResponse)request.GetResponse();
                    ProcessResponseObject(response);
                }
                catch (WebException e)
                {
                    crawledPage.WebException = e;

                    if (e.Response != null)
                        response = (HttpWebResponse)e.Response;

                    _logger.DebugFormat("Error occurred requesting url [{0}]", uri.AbsoluteUri);
                    _logger.Debug(e);
                }
                catch (Exception e)
                {
                    _logger.DebugFormat("Error occurred requesting url [{0}]", uri.AbsoluteUri);
                    _logger.Debug(e);
                }
                finally
                {
                    try
                    {
                        crawledPage.HttpWebRequest = request;
                        crawledPage.RequestCompleted = DateTime.Now;
                        if (response != null)
                        {
                            FirePageResponseReceivedEvent(crawledPage, response);

                            crawledPage.HttpWebResponse = new HttpWebResponseWrapper(response);
                            CrawlDecision shouldDownloadContentDecision = shouldDownloadContent(crawledPage);
                            if (shouldDownloadContentDecision.Allow && i == 1) // download if allowed and it's first request
                            {
                                crawledPage.DownloadContentStarted = DateTime.Now;
                                crawledPage.Content = _extractor.GetContent(response);
                                crawledPage.DownloadContentCompleted = DateTime.Now;
                            }
                            else
                            {
                                _logger.DebugFormat("Links on page [{0}] not crawled, [{1}]", crawledPage.Uri.AbsoluteUri, shouldDownloadContentDecision.Reason);
                            }

                            response.Close();//Should already be closed by _extractor but just being safe
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.DebugFormat("Error occurred finalizing requesting url [{0}]", uri.AbsoluteUri);
                        _logger.Debug(e);
                    }
                }
            }

            return crawledPage;
        }
    }
}
