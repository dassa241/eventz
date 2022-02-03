using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventz.API.ErrorHandling
{
    public class LogNLog
    {
        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public LogNLog()
        {
        }

        public void Information(string message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }
    }
}
