using System;
using System.Collections.Generic;

namespace Ayumi.Configuration {
    public static class Feature {
        static readonly IDictionary<String, Boolean> features = new Dictionary<String, Boolean>();

        public static void Enable(String feature) {
            if (!features.ContainsKey(feature))
                features.Add(feature, true);
            else
                features[feature] = true;
        }

        public static void Disable(String feature) {
            if (!features.ContainsKey(feature))
                features.Add(feature, false);
            else
                features[feature] = false;
        }

        public static Boolean Enabled(String feature) => features.ContainsKey(feature) && features[feature];
    }
}
