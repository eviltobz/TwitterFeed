namespace TwitterFeed
{
    public class RateLimit
    {
        public string Method { get; set; }
        public int Limit { get; set; }
        public int Remaining { get; set; }
        public int Reset { get; set; }

        public override string ToString()
        {
            var currentTimestamp = Helpers.CurrentTimestamp();
            var seconds = Reset - currentTimestamp;
            var minutes = seconds/60;
            return "Method:" + Method + ", Limit:" + Limit + ", Remaining:" + Remaining + ", Reset:" + Reset + " (due in approx " + seconds + " seconds or " + minutes + " minutes)";
        }
    }
}
