using System;

namespace Emi {
    public class EmitterException : Exception {
        public EmitterException(String message) : base(message) { }

        public EmitterException(String message, Exception innerException) : base(message, innerException) { }
    }
}