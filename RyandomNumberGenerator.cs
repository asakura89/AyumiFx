using System;

namespace RyaNG {
    public static class RyandomNumberGenerator {
        // https://en.wikipedia.org/wiki/Feigenbaum_constants
        public const Int32 Feigenbaum = 46692;

        public static Int32 TurnToRyandom(this Int32 lowerBound, Int32 upperBound) {
            Int32 seed = Guid.NewGuid().GetHashCode() % Feigenbaum;
            var rnd = new Random(seed);
            return rnd.Next(lowerBound, upperBound);
        }

        public static Int32 TurnToRyandom(this Int32 upperBound) => 0.TurnToRyandom(upperBound);
    }
}