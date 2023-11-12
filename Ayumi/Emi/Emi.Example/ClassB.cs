using System;

namespace Emi.Example {
    public class ClassB {
        readonly Emitter globalEmitter;

        public ClassB() {
            globalEmitter =
                new XmlConfigEmitterLoader($"{AppDomain.CurrentDomain.BaseDirectory}\\emitter_emitter.config.xml")
                    .Load();
        }

        public void TriggerStart() =>
            globalEmitter.Emit("classb.classbstart", new EmitterEventArgs(nameof(ClassB.TriggerStart)));

        public void TriggerFinish() =>
            globalEmitter.Emit("ClassB:ClassBFinish", new EmitterEventArgs(nameof(ClassB.TriggerFinish)));
    }
}