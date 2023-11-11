using System;

namespace WebLib.Configuration
{
    public class DefaultConfiguration
    {
        public String AppName { get; set; }
        public String AppKey { get; set; }
        public String Domain { get; set; }
        public String ADPath { get; set; }
        public String FilePath { get; set; }
        public Boolean IsDebugMode { get; set; }
    }
}