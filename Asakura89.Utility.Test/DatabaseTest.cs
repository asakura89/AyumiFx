using System;
using System.Data;
using Asakura89.Utility.Data;
using NUnit.Framework;

namespace Asakura89.Utility.Test
{
    [TestFixture]
    public class DatabaseTest
    {
        [Test]
        public void ReturnDatatableTest()
        {
            const String CURRENT_CONN_STRING = @"Data Source=asmnetworklab\sqlexp08r2;Initial Catalog=estimasiBiaya;Persist Security Info=True;User ID=sa;Password=rara:{78};";
            using (Database db = Database.Open(CURRENT_CONN_STRING, ConnectionStringType.CONNECTION_STRING))
            {
                DataTable dt = db.Query("SELECT ip.* FROM identitasProyek ip");
                Assert.IsNotNull(dt);
            }
        }
    }
}