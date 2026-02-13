using System;
using System.Dynamic;
using System.IO;
using System.Xml;

namespace Haru.Console.Test {
    public class Program {
        const String AppName = "HaruTest";

        public static void Main(String[] args) {
            IStorage storage = new XmlStorage(AppName);
            storage.SetInt("a", 7);
            storage.SetInt("a", 15);
            storage.Set("b", "yuhuueo");
            storage.SetFloat("c", (Single) 14.6d);
            storage.SetFloat("b", (Single) 14.6d);
            storage.SetFloat("d", (Single) 46.89865m);
            storage.Set("b", "arererere");
            storage.SetInt("e", (Int32) 23L);
            storage.SetInt("f", 255_879_886);
            storage.SetDatetime("i", DateTime.Now);
            storage.SetTimespan("j", new TimeSpan(5, 3, 20, 40));

            String path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage.xml");
            String content = File.ReadAllText(path);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);

            dynamic readValue = new ExpandoObject();
            IStorage storage2 = new XmlStorage(path, AppName);
            readValue.A = storage2.GetInt("a");
            readValue.B = storage2.GetInt("b");
            readValue.C = storage2.GetFloat("c");
            readValue.D = storage2.GetFloat("d");
            readValue.E = storage2.GetInt("e");
            readValue.F = storage2.GetInt("f");
            readValue.I = storage2.GetDatetime("i");
            readValue.J = storage2.GetTimespan("j");

            System.Console.ReadLine();
        }
    }
}
