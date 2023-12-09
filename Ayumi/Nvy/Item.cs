namespace Nvy {
    public class Item<N, V>
        where N : class
        where V : class {

        protected N InternalName { get; set; }
        protected V InternalValue { get; set; }
    }
}