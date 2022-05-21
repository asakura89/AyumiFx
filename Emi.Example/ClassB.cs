using System;
using System.IO;
using Reflx;

namespace Emi.Example {
    public class ClassB {
        readonly IEmitter globalEmitter;

        public ClassB() {
            ITypeAndAssemblyParser typeNAsmParser = TypeAndAssemblyParser.Instance;
            String emitterConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emitter_emitter.config.xml");
            globalEmitter = new XmlConfigEmitterLoader(typeNAsmParser, emitterConfig).Load();
        }

        public void TriggerStart() =>
            globalEmitter.Emit("classb.classbstart", new EmitterEventArgs(nameof(ClassB.TriggerStart)));

        public void TriggerFinish() =>
            globalEmitter.Emit("ClassB:ClassBFinish", new EmitterEventArgs(nameof(ClassB.TriggerFinish)));
    }
}