using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterFeed
{
    public class Tweet
    {
        public string Date { get; set; }
        public TweetBody Body { get; set; }
        public string Id { get; set; }
        public User User { get; set; }
        public User RetweetUser { get; set; }
        private readonly bool isRetweet = false;
        public Tweet(RawTweet source)
        {
            Date = source.Date;
            Id = source.Id;
            User = source.User;
            var retweet = source.RetweetedStatus;
            if(retweet == null)
                Body = new TweetBody(source.Text, source.Urls);
            else
            {
                isRetweet = true;
                System.Diagnostics.Debug.Assert(source.Urls.Count == retweet.Urls.Count, "Retweet urls counts don't match. Base=" + source.Urls.Count + ", RT=" + retweet.Urls.Count);
                Body= new TweetBody(retweet.Text, retweet.Urls);
                RetweetUser = retweet.User;
            }
        }

        public override string ToString()
        {
            //if (RetweetedStatus != null)
            //    return User + " Retweeting " + RetweetedStatus.User + "\n" + Body + "\n" + Date + " - " + Id;
            return User + (isRetweet ? " Retweeting " + RetweetUser : "") + Environment.NewLine + Body + Environment.NewLine + Date + " - " + Id;
        }
        public class TweetBody : IEnumerable<TweetBody.ISegment>
        {
            private readonly string text;
            private readonly List<Url> urls;

            public abstract class ISegment
            {
                public string Text { get; private set; }

                protected ISegment(string text)
                {
                    Text = text;
                }

                public override string ToString()
                {
                    return Text;
                }
            }
            public class LinkSegment:ISegment
            {
                public LinkSegment(string text):base(text) { }
                public override string ToString()
                {
                    return "<" + Text + ">";
                }
            }           
            public class TextSegment:ISegment
            {
                public TextSegment(string text):base(text) { }
            }

            private List<ISegment> segments;// = new List<Segment>();

            public TweetBody(string text, List<Url> urls)
            {
                this.text = text;
                this.urls = urls;
            }

            internal async Task InitialiseSegmentsAsync()
            {
                if (segments == null)
                {
                    segments = new List<ISegment>();
                    //var remainingText = text;
                    var currentIndex = 0;
                    var iterator = urls.OrderBy(u => u.StartIndex).GetEnumerator();
                    //while(currentIndex <= text.Length)
                    //{
                       //if (iterator.MoveNext())
                       while (iterator.MoveNext())
                       {
                           var url = iterator.Current;
                           if (url.StartIndex > currentIndex)
                               segments.Add(new TextSegment(text.Substring(currentIndex, url.StartIndex - currentIndex)));
                           var resolvedLink = await UrlExpander.ResolveUrlShortenerAsync(url.Link);
                           segments.Add(new LinkSegment(resolvedLink));
                           currentIndex = url.EndIndex;
                       }
                       //else
                       //{
                           segments.Add(new TextSegment(text.Substring(currentIndex)));
                           //currentIndex = text.Length;
                       //}
                    //}


                    //const string urlStartDelimiter = @"http://t.co/";
                    //var parser = new Api.LineParser(text);
                    //while (!parser.Finished)
                    //{
                    //    var part = parser.GetUntil(urlStartDelimiter);
                    //    // TODO - de-escape html character code &amp; &gt; ...
                    //    if (!string.IsNullOrEmpty(part))
                    //        segments.Add(new Segment(part, false));

                    //    var link = parser.GetUntil(" ");
                    //    if (!string.IsNullOrWhiteSpace(link))
                    //    {
                    //        var resolvedLink = await UrlExpander.ResolveUrlShortenerAsync(link);
                    //        segments.Add(new Segment(resolvedLink, true));
                    //    }
                    //}
                }
            }

            public override string ToString()
            {
                var retval = new StringBuilder();
                //InitialiseSegmentsAsync();
                foreach (var segment in segments)
                {
                    //if (segment.Link)
                    //    retval.Append("<" + segment.Text + ">");
                    //else
                    //    retval.Append(segment.Text);
                    retval.Append(segment.ToString());
                }
                return retval.ToString();// +"\nOriginal text was: " + text;
            }
 

            public IEnumerator<ISegment> GetEnumerator()
            {
                Console.WriteLine("oops?");
                InitialiseSegmentsAsync();
                return segments.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

    }
}