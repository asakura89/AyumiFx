using System;

namespace Emi.Example {
    public class EventList {
        // .Net native event handling
        public void OnClassAStarted(Object source, EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnClassAStarted));
            Console.WriteLine("Context: " + args.Context);
        }

        public void OnClassAFinished(Object source, EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnClassAFinished));
            Console.WriteLine("Context: " + args.Context);
        }

        public void OnGlobalStarted(Object source, EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnGlobalStarted));
            Console.WriteLine("Context: " + args.Context);
        }

        public void OnGlobalFinished(Object source, EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnGlobalFinished));
            Console.WriteLine("Context: " + args.Context);
        }

        // Event emitter event handling
        public void OnClassBStarted(EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(nameof(OnClassBStarted));
            Console.WriteLine("Context: " + args.Context);
        }

        public void OnClassBFinished(EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(nameof(OnClassBFinished));
            Console.WriteLine("Context: " + args.Context);
        }

        public void OnGlobalBStarted(EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(nameof(OnGlobalBStarted));
            Console.WriteLine("Context: " + args.Context);
        }

        public void OnGlobalBFinished(EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(nameof(OnGlobalBFinished));
            Console.WriteLine("Context: " + args.Context);
        }
    }
}