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
    public class PagingTests : QueryTestsBase
    {
        [ClassInitialize]
        public static void InitializeTests(TestContext context)
        {
            QueryTestsBase.InitializeOrders(context);
        }

        [TestMethod]
        public async Task NoSkipTakeGetsAllItems()
        {
            var table = Client.GetTable<Order>();
            var items = await table.ToListAsync();
            ComparisonHelper.CompareAssert(Orders, items);
        }

        [TestMethod]
        public async Task SkipFewItems()
        {
            var table = Client.GetTable<Order>();
            var skip = Orders.Length / 2;
            var items = await table.Skip(skip).ToListAsync();
            ComparisonHelper.CompareAssert(Orders.Skip(skip).ToArray(), items);
        }

        [TestMethod]
        public async Task SkipAllItems()
        {
            var table = Client.GetTable<Order>();
            var skip = Orders.Length + 2;
            var items = await table.Skip(skip).ToListAsync();
            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public async Task TakeFewItems()
        {
            var table = Client.GetTable<Order>();
            var take = Orders.Length / 2;
            var items = await table.Take(take).OrderBy(o => o.Id).ToListAsync();
            ComparisonHelper.CompareAssert(Orders.Take(take).OrderBy(o => o.Id).ToArray(), items);
        }

        [TestMethod]
        public async Task TakeAllItems()
        {
            var table = Client.GetTable<Order>();
            var take = Orders.Length + 2;
            var items = await table.OrderBy(o => o.Id).Take(take).ToListAsync();
            ComparisonHelper.CompareAssert(Orders.OrderBy(o => o.Id), items);
        }

        [TestMethod]
        public async Task SkipAndTake()
        {
            var table = Client.GetTable<Order>();
            var take = Orders.Length / 3;
            var skip = Orders.Length / 3;
            var items = await table.OrderBy(o => o.Id).Take(take).Skip(skip).ToListAsync();
            ComparisonHelper.CompareAssert(Orders.OrderBy(o => o.Id).Skip(skip).Take(take).ToArray(), items);
        }
    }
}
