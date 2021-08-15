using System;

namespace Emi {
    public class EmitterEventArgs : EventArgs {
        public Object Context { get; }

        public EmitterEventArgs(Object context) {
            Context = context;
        }
    }
}