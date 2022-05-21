using System;
using System.Collections.Generic;

namespace Emi {
    public class EmitterEventArgs : EventArgs {
        public String EventName { get; set; }
        public IDictionary<String, Object> Data { get; set; } = new Dictionary<String, Object>();

        public EmitterEventArgs(String eventName) {
            if (String.IsNullOrEmpty(eventName))
                throw new ArgumentNullException(nameof(eventName));

            EventName = eventName;
        }

        public EmitterEventArgs(String eventName, params KeyValuePair<String, Object>[] data) {
            if (String.IsNullOrEmpty(eventName))
                throw new ArgumentNullException(nameof(eventName));

            EventName = eventName;

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(data));

            foreach (KeyValuePair<String, Object> dataItem in data)
                Data.Add(dataItem.Key, dataItem.Value);
        }
    }
}