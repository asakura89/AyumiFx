using System;
using System.IO;
using Reflx;

namespace Emi.Example {
    public class ClassA {
        event EventHandler<EmitterEventArgs> ClassAStart;
        event EventHandler<EmitterEventArgs> ClassAFinish;

        public ClassA() {
            ITypeAndAssemblyParser typeNAsmParser = TypeAndAssemblyParser.Instance;
            String emitterConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emitter_dotnetnative.config.xml");
            new XmlConfigEventRegistrar(typeNAsmParser, emitterConfig).Register(this);
        }

        public void TriggerStart() =>
            ClassAStart?.Invoke(this, new EmitterEventArgs(nameof(ClassA.ClassAStart)));

        public void TriggerFinish() =>
            ClassAFinish?.Invoke(this, new EmitterEventArgs(nameof(ClassA.ClassAFinish)));
    }
}