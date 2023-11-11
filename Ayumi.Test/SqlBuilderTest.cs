using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayumi.Data.Query;

namespace Ayumi.Test {
    public class SqlBuilderTest {
        var builder = new QueryCrafter()
            .Select("CompanyName", "ContactName")
            .From("Customers")
            .ToString();
    }
}
