using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace ComplexQueryTests
{
    [TestClass]
    public class SelectTests : QueryTestsBase
    {
        [ClassInitialize]
        public static void InitializeTests(TestContext context)
        {
            QueryTestsBase.InitializeOrders(context);
        }

        [TestMethod]
        public async Task SelectAllUntyped()
        {
            var table = Client.GetTable<Order>();
            var items = await table.ReadAsync("$select=*&$top=1");
            Assert.IsInstanceOfType(items, typeof(JArray));
            JArray itemArray = (JArray)items;
            Assert.AreEqual(1, itemArray.Count);
            JObject item = itemArray[0] as JObject;
            Assert.IsNotNull(item["client"]);
            Assert.IsNotNull(item["orderDate"]);
            Assert.IsNotNull(item["items"]);
        }

        [TestMethod]
        public async Task SelectSomeUntyped()
        {
            var table = Client.GetTable<Order>();
            var items = await table.ReadAsync("$select=client&$top=1");
            Assert.IsInstanceOfType(items, typeof(JArray));
            JArray itemArray = (JArray)items;
            Assert.AreEqual(1, itemArray.Count);
            JObject item = itemArray[0] as JObject;
            Assert.IsNotNull(item["client"]);
            Assert.IsNull(item["orderDate"]);
            Assert.IsNull(item["items"]);
        }

        [TestMethod]
        public async Task SelectSomeTyped()
        {
            var table = Client.GetTable<Order>();
            var items = await table.ReadAsync("$select=client&$top=1");
            Assert.IsInstanceOfType(items, typeof(JArray));
            JArray itemArray = (JArray)items;
            Assert.AreEqual(1, itemArray.Count);
            JObject item = itemArray[0] as JObject;
            Assert.IsNotNull(item["client"]);
            Assert.IsNull(item["orderDate"]);
            Assert.IsNull(item["items"]);
        }
    }
}
