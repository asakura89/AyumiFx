using System;
using Nvy;

namespace Backstreet {
    public class CommandLineArgument : NameValueItem {
        public CommandLineArgument(String name) : this(name, String.Empty) { }

        public CommandLineArgument(String name, String value) : base(name, value) { }

        public override String ToString() {
            String name = Name.Trim();
            String value = Value.Trim();

            if (String.IsNullOrEmpty(value))
                return name;

            return $"{name} {value}";
        }
    }
}