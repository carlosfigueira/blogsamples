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
    public class OrderingTests : QueryTestsBase
    {
        [ClassInitialize]
        public static void InitializeTests(TestContext context)
        {
            QueryTestsBase.InitializeOrders(context);
        }

        [TestMethod]
        public async Task SimpleOrderBy()
        {
            var table = Client.GetTable<Order>();
            var items = await table.OrderBy(o => o.OrderDate).ToListAsync();
            ComparisonHelper.CompareAssert(Orders.OrderBy(o => o.OrderDate), items);
        }

        [TestMethod]
        public async Task OrderByDescending()
        {
            var table = Client.GetTable<Order>();
            var items = await table.OrderByDescending(o => o.OrderDate).ToListAsync();
            ComparisonHelper.CompareAssert(Orders.OrderByDescending(o => o.OrderDate), items);
        }
    }
}
