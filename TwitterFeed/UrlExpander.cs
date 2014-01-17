using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace TwitterFeed
{
    public class UrlExpander
    {
        private const int MaxUrlRedirectionDepth = 0;
        private static readonly HashSet<string> RedirectionHosts = new HashSet<string>{"t.co", "goo.gl", "ow.ly", "bit.ly"};

        public static async Task<string> ResolveUrlShortenerAsync(string url, int depth = 0)
        {
            var uri = new Uri(url);
            if (!RedirectionHosts.Contains(uri.Host))
            {
                Console.WriteLine("*** Url redirect host not recognised *** " + uri.Host + "  " + url);
                return (url);
            }
            var req = WebRequest.CreateHttp(uri);
            req.AllowAutoRedirect = false;
            req.Timeout = 2000;
            try
            {
                string redirectUrl;
                Console.WriteLine("About to try to resolve " + url);
                using (var webResponse = await req.GetResponseAsync())
                {
                    redirectUrl = webResponse.Headers["Location"] ?? url;
                }
                Console.WriteLine("--RESOLVED " + uri + " to " + redirectUrl + "  --  Depth=" + depth);
                if (redirectUrl != url && depth < MaxUrlRedirectionDepth)
                    return await ResolveUrlShortenerAsync(redirectUrl, ++depth);
                return redirectUrl;
            }
            catch (WebException)
            {
                Console.WriteLine("Exception resolving " + url);
                return url;
            }
        }

    }
}