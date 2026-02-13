using System;
using System.IO;
using System.Xml;
using Xunit;

namespace Eksmaru.Test {
    public class XmlHelper_Load {
        String GetFilePath(String filename) =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", filename);

        [Fact]
        public void Load_IncorrectXmlContent_Error() {
            IXmlHelper helper = new XmlHelper();

            Assert
                .Throws<XmlException>(() =>
                    helper
                        .Load(@"<?xml version='1.0' encoding='utf-8'?>
<packages>
  <package id='xunit' version='2.4.1' targetFramework='net461'>
  <package id='xunit.abstractions' version='2.0.3' targetFramework='net461' />
  <package id='xunit.analyzers' version='0.10.0' targetFramework='net461' />
  <package id='xunit.assert' version='2.4.1' targetFramework='net461' />
  <package id='xunit.core' version='2.4.1' targetFramework='net461' />
  <package id='xunit.extensibility.core' version='2.4.1' targetFramework='net461' />
  <package id='xunit.extensibility.execution' version='2.4.1' targetFramework='net461' />
  <package id='xunit.runner.visualstudio' version='2.4.3' targetFramework='net461' developmentDependency='true' />
</packages>"));

            Assert
                .Throws<XmlException>(() =>
                    helper
                        .Load(@"<?xml version='1.0' encoding='utf-8'?>
<packages>
  <package id='xunit' version='2.4.1' targetFramework='net461' />
  <package id='xunit.abstractions' version='2.0.3' targetFramework='net461' />
  <package id='xunit.analyzers' version='0.10.0' targetFramework='net461' />
  <package id='xunit.assert' version='2.4.1' targetFramework='net461' />
  <package id='xunit.core' version='2.4.1'
  <package id='xunit.extensibility.core' version='2.4.1' targetFramework='net461' />
  <package id='xunit.extensibility.execution' version='2.4.1' targetFramework='net461' />
  <package id='xunit.runner.visualstudio' version='2.4.3' targetFramework='net461' developmentDependency='true' />
</packages>"));

            Assert
                .Throws<ArgumentException>(() =>
                    helper
                        .Load(GetFilePath(@"<?xml version='1.0' encoding='utf-8'?>
<packages>
  <package id='xunit' version='2.4.1' targetFramework='net461' />
  <package id='xunit.abstractions' version='2.0.3' targetFramework='net461' />
  <package id='xunit.analyzers' version='0.10.0' targetFramework='net461' />
  <package id='xunit.assert' version='2.4.1' targetFramework='net461' />
  <package id='xunit.core' version='2.4.1' target />
  <package id='xunit.extensibility.core' version='2.4.1' targetFramework='net461' />
  <package id='xunit.extensibility.execution' version='2.4.1' targetFramework='net461' />
  <package id='xunit.runner.visualstudio' version='2.4.3' targetFramework='net461' developmentDependency='true' />
</packages>")));
        }

        [Fact]
        public void Load_EmptyContent_ReturnsNull() {
            XmlDocument doc = new XmlHelper().Load("");
            Assert.Null(doc);
        }

        [Fact]
        public void Load_XmlContent_ReturnsXmlDoc() {
            XmlDocument doc = new XmlHelper().Load(@"<?xml version='1.0' encoding='utf-8'?>
<packages>
  <package id='xunit' version='2.4.1' targetFramework='net461' />
  <package id='xunit.abstractions' version='2.0.3' targetFramework='net461' />
  <package id='xunit.analyzers' version='0.10.0' targetFramework='net461' />
  <package id='xunit.assert' version='2.4.1' targetFramework='net461' />
  <package id='xunit.core' version='2.4.1' targetFramework='net461' />
  <package id='xunit.extensibility.core' version='2.4.1' targetFramework='net461' />
  <package id='xunit.extensibility.execution' version='2.4.1' targetFramework='net461' />
  <package id='xunit.runner.visualstudio' version='2.4.3' targetFramework='net461' developmentDependency='true' />
</packages>");

            Assert.NotNull(doc);
            Assert.True(doc.HasChildNodes);
        }
    }
}
