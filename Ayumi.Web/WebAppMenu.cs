using System;

namespace WebApp {
    public class WebAppMenu {
        public String MenuId { get; set; }
        public String ActivityId { get; set; }
        public String MenuName { get; set; }
        public String Link { get; set; }
        public String ParentId { get; set; }
        public Int32 Priority { get; set; }
        public Boolean IsShown { get; set; }
        public String Icon { get; set; }
    }
}