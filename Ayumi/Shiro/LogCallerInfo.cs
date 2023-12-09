using System;
using System.Reflection;
using System.Text;
using Exy;
using log4net;
using Newtonsoft.Json;

namespace Shiro {
    public sealed class LogCallerInfo {
        public String CallerType { get; set; }
        public String CallerMember { get; set; }
    }
}
