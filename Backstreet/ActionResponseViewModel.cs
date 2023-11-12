using System;

namespace Arvy {
    [Serializable]
    public class ActionResponseViewModel {
        public const String Info = "I";
        public const String Warning = "W";
        public const String Error = "E";
        public const String Success = "S";
        public String ResponseType { get; set; }
        public String Message { get; set; }

        public override String ToString() => ToString(true);

        public String ToString(Boolean alwaysReturn) {
            if (!alwaysReturn && ResponseType == Error)
                throw new InvalidOperationException(Message);

            return ResponseType + "|" + Message;
        }
    }
}