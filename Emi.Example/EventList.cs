using System;

namespace Emi.Example {
    public class EventList {
        public void OnClassAStarted(Object source, EmitterEventArgs args) =>
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnClassAStarted));

        public void OnClassAFinished(Object source, EmitterEventArgs args) =>
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnClassAFinished));

        public void OnGlobalStarted(Object source, EmitterEventArgs args) =>
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnGlobalStarted));

        public void OnGlobalFinished(Object source, EmitterEventArgs args) =>
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnGlobalFinished));
    }
}