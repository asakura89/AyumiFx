using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace Haru.Console.Test {
    public class Program {
        public static void Main(String[] args) {
            IStorage storage = new XmlStorage();
            storage.Set("a", 7);
            storage.Set("a", 15);
            storage.Set("b", "yuhuueo");
            storage.Set("c", 14.6d);
            storage.Set("b", 14.6d);
            storage.Set("d", 46.89865m);
            storage.Set("b", "arererere");
            storage.Set("e", 23L);
            storage.Set("f", 255_879_886);
            storage.Set("i", DateTime.Now);
            storage.Set("j", new TimeSpan(5, 3, 20, 40));
            storage.Set("g", new[] { "aru", "no", "ka", "na" });
            //storage.Set("h", new {
            //    Are = "arererere",
            //    Ano = "anonono",
            //    X = 17.5,
            //    Y = 40.0,
            //    Z = -23,
            //    Arr = new[] { "aru", "no", "ka", "na" }
            //});
            //storage.Set("k", new Dictionary<String, String> {
            //    ["key1"] = "value1",
            //    ["key2"] = "value2"
            //});
            //storage.Set("m", new Dictionary<String, Object> {
            //    ["key1"] = new {
            //        Are = "arererere",
            //        Ano = "anonono",
            //        X = 17.5,
            //        Y = 40.0,
            //        Z = -23,
            //        Arr = new[] { "aru", "no", "ka", "na" },
            //        Ignore = (Object) null,
            //        Emp = String.Empty
            //    },
            //    ["key2"] = new[] { "aru", "no", "ka", "na" }
            //});
            //storage.Set("l", new Hashtable {
            //    ["a"] = "e",
            //    ["e"] = "a",
            //    ["o"] = 723
            //});

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage.xml");
            //String content = File.ReadAllText(path);
            //var xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(content);

            dynamic readValue = new ExpandoObject();
            IStorage storage2 = new XmlStorage(path);
            readValue.A = storage2.Get<Int32>("a");
            readValue.B = storage2.Get<String>("b");
            readValue.C = storage2.Get<Double>("c");
            readValue.D = storage2.Get<Decimal>("d");
            readValue.E = storage2.Get<Int64>("e");
            readValue.F = storage2.Get<Int32>("f");
            readValue.G = storage2.Get<String[]>("g");
            readValue.I = storage2.Get<DateTime>("i");
            readValue.J = storage2.Get<TimeSpan>("j");

            System.Console.ReadLine();
        }
    }
}
