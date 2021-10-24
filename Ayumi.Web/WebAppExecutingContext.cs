using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp {
    public sealed class WebAppExecutingContext {
        static WebAppExecutingContext currentCtx;

        WebAppExecutingContext() { }
        public String ControllerName { get; set; }
        public String ActionName { get; set; }
        public static WebAppExecutingContext Current => currentCtx ?? (currentCtx = new WebAppExecutingContext());
    }
}