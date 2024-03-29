﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Ayumi.Data;
using Ayumi.Test.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ayumi.Test
{
    [TestClass]
    public class DatabaseTest
    {
        private const String Provider = "System.Data.SQLite";
        private readonly String ConnectionString = "DataSource=" + AppDomain.CurrentDomain.BaseDirectory + "\\Data\\test.db;Password=databossytest;Version=3;Compress=True;UTF8Encoding=True;Page Size=1024;FailIfMissing=False;Read Only=False;Pooling=True;Max Pool Size=100;";

        public DatabaseTest()
        {
            InitializeSqliteDbProvider();
        }

        private void InitializeSqliteDbProvider()
        {
            try
            {
                var configDs = ConfigurationManager.GetSection("system.data") as DataSet;
                if (configDs != null)
                    configDs.Tables[0]
                        .Rows.Add("SQLite Data Provider", ".Net Framework Data Provider for SQLite",
                            Provider, "System.Data.SQLite.SQLiteFactory, System.Data.SQLite");
            }
            catch { }
        }

        [TestMethod]
        public void QueryTest()
        {
            IList<Product> pList = null;
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                pList = db.Query<Product>("SELECT * FROM Product").ToList();

            Assert.IsNotNull(pList);
            Assert.IsTrue(pList.Any());
            Assert.IsTrue(pList.Count > 1);
        }

        [TestMethod]
        public void QueryWParamTest()
        {
            const String categoryId = "CAT28789";
            IList<Product> pList = null;
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                pList = db
                    .Query<Product>("SELECT * FROM Product WHERE CategoryId = @0", categoryId)
                    .ToList();

            Assert.IsNotNull(pList);
            Assert.IsTrue(pList.Any());
            Assert.IsTrue(pList.Count > 1);
            Assert.AreEqual(pList[0].CategoryId, categoryId);
        }

        [TestMethod]
        public void QueryDataSetTest()
        {
            DataSet ds = null;
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                ds = db.QueryDataSet("SELECT * FROM Category; SELECT * FROM Product;");

            Assert.IsNotNull(ds);
            Assert.IsTrue(ds.Tables.Count == 2);
            Assert.IsTrue(ds.Tables[0].Rows.Count > 0);
            Assert.IsTrue(ds.Tables[1].Rows.Count > 0);
        }

        [TestMethod]
        public void QueryDataSetWParamTest()
        {
            const String categoryId = "CAT28789";
            DataSet ds = null;
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                ds = db.QueryDataSet("SELECT * FROM Category; SELECT * FROM Product; SELECT COUNT(0) FROM Product WHERE CategoryId = @0", categoryId);

            Assert.IsNotNull(ds);
            Assert.IsTrue(ds.Tables.Count == 3);
            Assert.IsTrue(ds.Tables[0].Rows.Count > 0);
            Assert.IsTrue(ds.Tables[1].Rows.Count > 0);
            Assert.IsTrue(ds.Tables[2].Rows.Count == 1);
        }

        [TestMethod]
        public void QueryDataTableTest()
        {
            DataTable dt = null;
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                dt = db.QueryDataTable("SELECT * FROM Product");

            Assert.IsNotNull(dt);
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [TestMethod]
        public void QueryDataTableWParamTest()
        {
            const String productId = "PROD07341";
            DataTable dt = null;
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                dt = db.QueryDataTable("SELECT * FROM Product WHERE [Id] = @0", productId);

            Assert.IsNotNull(dt);
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [TestMethod]
        public void QueryScalarTest()
        {
            const String categoryId = "CAT28789";
            Int64 productCount = 0;
            Boolean isExists = false;
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
            {
                // NOTE: EXISTS and COUNT in sqlite return object {long} type and value is case-sensitive
                productCount = db.QueryScalar<Int64>("SELECT COUNT(0) FROM Product WHERE CategoryId = @0", categoryId);
                isExists = Convert.ToBoolean(
                    db.QueryScalar<Int64>("SELECT EXISTS (SELECT * FROM sqlite_master WHERE type = 'table' AND name = @0);", "Product"));
            }

            Assert.IsTrue(productCount != 0);
            Assert.IsTrue(productCount > 1);

            Assert.IsTrue(isExists);
        }

        [TestMethod]
        public void QuerySingleTest()
        {
            const String productId = "PROD07341";
            ProductViewModel pVM = null;
            var query = new StringBuilder()
                .Append("SELECT p.[Id], p.[Name], c.[Desc] CategoryJ ")
                .Append("FROM Product p JOIN Category c ON c.[Id] = p.CategoryId ")
                .Append("WHERE p.[Id] = @0")
                .ToString();

            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                pVM = db.QuerySingle<ProductViewModel>(query, productId);

            Assert.IsNotNull(pVM);
            Assert.AreEqual(pVM.Id, productId);
            Assert.IsFalse(String.IsNullOrEmpty(pVM.CategoryJ));
        }

        [TestMethod]
        public void WithTransactionTest()
        {
            
        }
    }
}
