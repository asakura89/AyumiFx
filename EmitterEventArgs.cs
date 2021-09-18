using System;
using System.Collections.Generic;

namespace Emi {
    public class EmitterEventArgs : EventArgs {
        public IDictionary<String, Object> Data { get; set; } = new Dictionary<String, Object>();
    }
}