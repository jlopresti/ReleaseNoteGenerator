using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Core;

namespace Ranger.NetCore.Common
{
    public sealed class Log4NetAdapter<T> : LogImpl
    {
        public Log4NetAdapter() : base(LogManager.GetLogger(typeof(T)).Logger) { }
    }
}
