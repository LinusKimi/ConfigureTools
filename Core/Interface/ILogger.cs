using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    interface ILogger
    {
        void WriteToLog(string msg);
        void WriteToConsole(string msg);
        void ShutDown();
    }
}
