using System;

namespace Emi.Example {
    public class Program {
        public static void Main(String[] args) {
            Console.WriteLine(".Net native event handling");
            var classA = new ClassA();
            classA.TriggerStart();
            classA.TriggerFinish();

            classA.TriggerStart();
            classA.TriggerFinish();

            Console.WriteLine("Event emitter way event handling");
            var classB = new ClassB();
            classB.TriggerStart();
            classB.TriggerFinish();

            classB.TriggerStart();
            classB.TriggerFinish();

            Console.ReadLine();
        }
    }
}
