using System;

namespace Emi.Example {
    public class Program {
        public static void Main(String[] args) {
            var classA = new ClassA();
            classA.TriggerStart();
            classA.TriggerFinish();

            classA.TriggerStart();
            classA.TriggerFinish();

            Console.ReadLine();
        }
    }
}
