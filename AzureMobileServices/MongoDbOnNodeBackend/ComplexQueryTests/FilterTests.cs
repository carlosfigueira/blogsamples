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
    public class FilterTests : QueryTestsBase
    {
        [ClassInitialize]
        public static void InitializeTests(TestContext context)
        {
            QueryTestsBase.InitializeOrders(context);
        }

        [TestMethod]
        public async Task Filter_True()
        {
            await TestFilter(o => true);
        }

        [TestMethod]
        public async Task Filter_False()
        {
            await TestFilter(o => false);
        }

        [TestMethod]
        public async Task Filter_StartsWith()
        {
            await TestFilter(o => o.Client.StartsWith("a"));
        }

        [TestMethod]
        public async Task Filter_DateTimeComparison()
        {
            await TestFilter(o => o.OrderDate > new DateTime(2014, 6, 18, 0, 0, 0, DateTimeKind.Utc));
        }

        [TestMethod]
        public async Task Filter_StringComparison()
        {
            var clientName = Orders[0].Client;
            await TestFilter(o => o.Client == clientName);
        }

        private async Task TestFilter(Expression<Func<Order, bool>> predicate)
        {
            var table = Client.GetTable<Order>();
            var items = await table.Where(predicate).OrderBy(o => o.Id).ToListAsync();
            var expected = Orders.Where(predicate.Compile()).OrderBy(o => o.Id).ToArray();
            ComparisonHelper.CompareAssert(expected, items);
        }
    }
}
