using System;

namespace RyaNG {
    public static class RyandomNumberGenerator {
        public const Int32 Feigenbaum = 46692; // https://en.wikipedia.org/wiki/Feigenbaum_constants

        public static Int32 TurnToRyandom(this Int32 lowerBound, Int32 upperBound) {
            Int32 seed = Guid.NewGuid().GetHashCode() % Feigenbaum;
            var rnd = new Random(seed);
            return rnd.Next(lowerBound, upperBound);
        }

        public static Int32 TurnToRyandom(this Int32 upperBound) {
            Int32 seed = Guid.NewGuid().GetHashCode() % Feigenbaum;
            var rnd = new Random(seed);
            return rnd.Next(0, upperBound);
        }
    }
}