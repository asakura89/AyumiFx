using System;

namespace WebLib.Data
{
    [Serializable]
    public class ListItem<N,V>
        where N : class
        where V: class
    {
        private N _name;
        private V _value;

        protected N name
        {
            get { return _name; }
            set { _name = value; }
        }

        protected V value
        {
            get { return _value; }
            set { _value = value; }
        }

        public ListItem(N name, V value)
        {
            this.name = name;
            this.value = value;
        }

        public ListItem() : this(default(N), default(V)) { }
    }
}
