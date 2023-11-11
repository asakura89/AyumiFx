using System;

namespace WebLib.Extensions.Model
{
    public class MainMenu
    {
        private String _id_module;
        private String _module_name;
        private String _link;
        private String _sub_module;

        public String id_module
        {
            get { return _id_module; }
            set { _id_module = value; }
        }

        public String module_name
        {
            get { return _module_name; }
            set { _module_name = value; }
        }

        public String link
        {
            get { return _link; }
            set { _link = value; }
        }

        public String sub_module
        {
            get { return _sub_module; }
            set { _sub_module = value; }
        }

        public MainMenu()
        {
            _id_module = String.Empty;
            _module_name = String.Empty;
            _link = String.Empty;
            _sub_module = String.Empty;
        }
    }
}
