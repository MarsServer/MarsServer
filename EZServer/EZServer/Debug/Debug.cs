using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;


namespace EZServer
{
    public class Debug
    {
        private static readonly ILogger _Log = LogManager.GetCurrentClassLogger();
        public static void Log(object message)
        {
           _Log.Debug(message);
        }

        public static void LogError(object message)
        {
            _Log.Error(message);
        }

        public static void LogWarning(object message)
        {
            _Log.Warn(message);
        }
    }
}
