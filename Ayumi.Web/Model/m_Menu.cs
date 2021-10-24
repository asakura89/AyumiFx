using System;

namespace WebApp.Model {
    public sealed class m_Menu {
        public Int32? Priority { get; set; }

        public Boolean? IsShown { get; set; }

        public String MenuId { get; set; }

        public String ActivityId { get; set; }

        public String MenuName { get; set; }

        public String Link { get; set; }

        public String ParentId { get; set; }

        public String Icon { get; set; }
    }
}