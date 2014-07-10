using System;

namespace WebLib.Data
{
    public class ObjectRepoData : ListItem<String, Object>
    {
        public new String Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        public Object Type
        {
            get { return Value; }
            set { Value = value; }
        }
    }
}
