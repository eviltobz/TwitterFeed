using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterFeed;

namespace TwitterFeedHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var feed = new TwitterFeed.Api();
            var selection = "";

            while (!selection.StartsWith("q"))
            {
                Console.WriteLine("Current timestamp: " + Helpers.CurrentTimestamp());
                //Console.WriteLine(Helpers.Render(feed.GetRateLimitsAsync(Api.RateLimitResources.statuses, Api.RateLimitResources.application)));
                Console.WriteLine("console app needs some async love");
                Console.WriteLine("---");
                Console.WriteLine(feed.GetHomeTimelineAsync(50));
                Console.WriteLine("---");
                //System.Threading.Thread.Sleep(2000);
                //Console.WriteLine(feed.GetRateLimits(Api.RateLimitResources.statuses, Api.RateLimitResources.application));
                //Console.WriteLine(feed.GetFeed());
                Console.WriteLine("enter \"q\" to quit...");
                selection = Console.ReadLine();
            }
        }
    }
}
