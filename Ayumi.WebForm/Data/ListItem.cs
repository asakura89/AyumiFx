using System;

namespace WebLib.Data
{
    [Serializable]
    public abstract class ListItem<N, V>
        where N : class
        where V: class
    {
        protected N Name { get; set; }
        protected V Value { get; set; }
    }
}
