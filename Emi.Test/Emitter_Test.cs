using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Emi.Test {
    // NOTE: https://github.com/xunit/xunit/issues/1202
    // xUnit 2.2 min req is .Net 4.5.2
    // or downgrade to 2.1
    public class Emitter_Test {
        [Fact]
        public void On_EmptyFunc_ShouldContainsOne() {
            var emitter = new Emitter();
            emitter.On("test", arg => { });
            Assert.True(emitter.Count == 1);

            emitter.Emit("test", null);
            Assert.True(emitter.Count == 1);
        }

        [Fact]
        public void On_EmptyFuncWithArg_ShouldHaveArgValue() {
            var emitter = new Emitter();
            emitter.On("test", arg => { Assert.NotNull(arg); });
            emitter.Emit("test", new EmitterEventArgs("Context"));
        }

        [Fact]
        public void Once_EmptyFunc_ShouldContainsOneThenRemoved() {
            var emitter = new Emitter();
            emitter.Once("test", arg => { });
            Assert.True(emitter.Count == 1);

            emitter.Emit("test", null);
            Assert.True(emitter.Count == 0);
        }

        [Fact]
        public void Once_EmptyFuncWithArg_ShouldContainsOneThenRemoved() {
            var emitter = new Emitter();
            emitter.Once("test", arg => { Assert.NotNull(arg); });
            Assert.True(emitter.Count == 1);

            emitter.Emit("test", new EmitterEventArgs("Context"));
            Assert.True(emitter.Count == 0);
        }

        [Fact]
        public void Emit_DummyFunc_ShouldContainsTwo() {
            var emitter = new Emitter();
            emitter.On("test", arg => {
                Console.WriteLine("Start.");
                Thread.Sleep(3000);
                Console.WriteLine("Data: " + arg.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
                Thread.Sleep(3000);
                Console.WriteLine("Done.");
            });

            emitter.On("test-2", arg => {
                Console.WriteLine("Start Logging.");
                Thread.Sleep(1000);
                Console.WriteLine("Data: " + arg.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
                Thread.Sleep(1000);
                Console.WriteLine("Done Logging.");
            });

            Assert.True(emitter.Count == 2);

            emitter.Emit("test", new EmitterEventArgs("Context"));
            Assert.True(emitter.Count == 2);

            emitter.Emit("test-2", new EmitterEventArgs("Arg"));
            Assert.True(emitter.Count == 2);
        }

        [Fact]
        public void Emit_DummyFuncWithComplexArg_ShouldContainsOne() {
            var emitter = new Emitter();
            emitter.On("test", arg => {
                Console.WriteLine("Start.");
                Thread.Sleep(3000);
                Console.WriteLine("Data: " + arg.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
                Thread.Sleep(3000);
                Console.WriteLine("Done.");
            });

            emitter.On("test", arg => {
                Console.WriteLine("Start Logging.");
                Thread.Sleep(1000);
                Console.WriteLine("Data: " + arg.Data.Keys.Aggregate(String.Empty, (acc, curr) => curr + ", "));
                Thread.Sleep(1000);
                Console.WriteLine("Done Logging.");
            });

            Assert.True(emitter.Count == 1);

            emitter.Emit("test", new EmitterEventArgs("Arg"));
            Assert.True(emitter.Count == 1);
        }
    }
}