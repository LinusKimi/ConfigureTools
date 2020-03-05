using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interface;

namespace Core
{
    class Logger : ILogger
    {
        private static NLog.Logger _logger;
        public void ShutDown() => NLog.LogManager.Shutdown();

        public void WriteToConsole(string msg) => _logger.Info(msg);

        public void WriteToLog(string msg) => _logger.Debug(msg);
    }
}
