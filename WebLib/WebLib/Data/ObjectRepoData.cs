using System;

namespace WebLib.Data
{
    public class ObjectRepoData : ListItem<String, Object>
    {
        public String Name
        {
            get { return base.name; }
            set { base.name = value; }
        }

        public Object Type
        {
            get { return base.value; }
            set { base.value = value; }
        }
    }
}
