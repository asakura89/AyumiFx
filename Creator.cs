using System;

namespace Pocho {
    public class Creator<T> where T : class, new() {
        private readonly T poco;

        public Creator() {
            poco = new T();
        }

        public Creator<T> With(Action<T> action) {
            action(poco);
            return this;
        }

        public T Create() {
            return poco;
        }
    }
}