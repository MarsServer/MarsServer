using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ExitGames.Logging;

using ExitGames.Logging.Log4Net;
using LogManager = ExitGames.Logging.LogManager;
using log4net;
using log4net.Config;

namespace MarsServer
{
    public class Debug
    {
        public readonly static Debug instance = new Debug ();
        private static readonly ILogger _Log = LogManager.GetCurrentClassLogger();
        public void Setup(MarsApplication loginApplication)
        {
            LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            GlobalContext.Properties["LogFileName"] = loginApplication.ApplicationName + "Log";// + System.DateTime.Now.ToString("yyyy-MM-dd");
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(loginApplication.BinaryPath, "log4net.config")));
            // 初始化GameServer

            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(loginApplication.ApplicationRootPath, "log");

            // log4net
            string path = Path.Combine(loginApplication.BinaryPath, "log4net.config");
            var file = new FileInfo(path);
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }
        }
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
