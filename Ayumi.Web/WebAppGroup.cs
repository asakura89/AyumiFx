using System;
using System.Collections.Generic;

namespace WebApp {
    public sealed class WebAppGroup {
        public String GroupId { get; set; }
        public String GroupDesc { get; set; }
        public IList<String> Users { get; set; }
        public IList<String> Activities { get; set; }
    }
}