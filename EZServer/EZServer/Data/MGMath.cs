using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZServer
{
    public class MGMath
    {
        /// <summary>
        /// Get now time millseconds.
        /// </summary>
        static public long getCurrentTimeMillseconds()
        {
            DateTime minValue = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime();
            TimeSpan timeSpan = (DateTime.Now - minValue);
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}
