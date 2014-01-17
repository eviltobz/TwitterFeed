using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterFeed
{
    public class Helpers
    {
        public static Func<DateTime> DatetimeProvider = () => DateTime.UtcNow;
 

        /// <summary>
        /// yeah, static with a datetime.now, ugh. it's temporary!
        /// </summary>
        /// <returns></returns>
        public static int CurrentTimestamp()
        {
            return ((int) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
        }

        public static string Render(IEnumerable<object> things, string suffix = "")
        {
            var retval = new StringBuilder();
            foreach (var thing in things)
            {
                retval.AppendLine(thing.ToString()+suffix);
            }
            return retval.ToString();
        }
    }
}
