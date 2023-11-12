using System;

namespace Emi.Example {
    public class ClassA {
        event EventHandler<EmitterEventArgs> ClassAStart;
        event EventHandler<EmitterEventArgs> ClassAFinish;

        public ClassA() {
            new XmlConfigEventRegistrar($"{AppDomain.CurrentDomain.BaseDirectory}\\emitter_dotnetnative.config.xml")
                .Register(this);
        }

        public void TriggerStart() =>
            ClassAStart?.Invoke(this, new EmitterEventArgs(nameof(ClassA.ClassAStart)));

        public void TriggerFinish() =>
            ClassAFinish?.Invoke(this, new EmitterEventArgs(nameof(ClassA.ClassAFinish)));
    }
}