using System;
using System.Linq;

namespace Emi.Example {
    public class EventList {
        // .Net native event handling
        public void OnClassAStarted(Object source, EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnClassAStarted));
            Console.WriteLine("Data: " + args.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
        }

        public void OnClassAFinished(Object source, EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnClassAFinished));
            Console.WriteLine("Data: " + args.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
        }

        public void OnGlobalStarted(Object source, EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnGlobalStarted));
            Console.WriteLine("Data: " + args.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
        }

        public void OnGlobalFinished(Object source, EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(source.GetType().Name + "+" + nameof(OnGlobalFinished));
            Console.WriteLine("Data: " + args.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
        }

        // Event emitter event handling
        public void OnClassBStarted(EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(nameof(OnClassBStarted));
            Console.WriteLine("Data: " + args.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
        }

        public void OnClassBFinished(EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(nameof(OnClassBFinished));
            Console.WriteLine("Data: " + args.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
        }

        public void OnGlobalBStarted(EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(nameof(OnGlobalBStarted));
            Console.WriteLine("Data: " + args.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
        }

        public void OnGlobalBFinished(EmitterEventArgs args) {
            Console.WriteLine("Event Executed");
            Console.WriteLine(nameof(OnGlobalBFinished));
            Console.WriteLine("Data: " + args.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
        }
    }
}