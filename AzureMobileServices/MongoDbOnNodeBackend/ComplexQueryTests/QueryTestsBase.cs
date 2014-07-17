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
    public class QueryTestsBase
    {
        public const string CollectionName = "complexOrders";
        public static MobileServiceClient Client = new MobileServiceClient(
            "https://blog20140618.azure-mobile.net/",
            "umErdVDwyiFtEUjquGtoMPbEPmoFEr37"
        );

        protected static Order[] Orders;
        protected const int OrderCount = 20;

        [ClassInitialize]
        public static void InitializeOrders(TestContext ctx)
        {
            var table = Client.GetTable<Order>();
            var serverItems = table.ToListAsync().Result;
            if (serverItems.Count == OrderCount)
            {
                // TODO: validate that the first element is the same
                Console.WriteLine("Already populated");
                Orders = serverItems.ToArray();
            }
            else
            {
                Orders = CreateOrders();

                Client.InvokeApiAsync("clearCollection", HttpMethod.Post, new Dictionary<string, string>
                {
                    { "name", CollectionName }
                }).Wait();

                foreach (var order in Orders)
                {
                    table.InsertAsync(order).Wait();
                }
            }
        }

        private static Order[] CreateOrders()
        {
            var rndGen = new Random(1);
            return Enumerable.Range(1, OrderCount).Select(_ => new Order
            {
                Client = GetName(rndGen, 2),
                OrderDate = DateTime.UtcNow.Date.AddDays(-rndGen.Next(100)),
                Items = Enumerable.Range(0, rndGen.Next(4)).Select(__ => new OrderItem
                {
                    Name = GetName(rndGen, 1),
                    Quantity = rndGen.Next(1, 100),
                    Price = Math.Round(rndGen.NextDouble() * 100, 2)
                }).ToArray()
            }).ToArray();
        }

        private static string GetName(Random rndGen, int numberOfParts)
        {
            var consonants = "bcdfghjklmnpqrstvwxz";
            var vowels = "aeiouy";
            var sb = new StringBuilder();
            for (var part = 0; part < numberOfParts; part++)
            {
                if (part != 0)
                {
                    sb.Append(' ');
                }

                var size = rndGen.Next(2, 5);
                for (var i = 0; i < size; i++)
                {
                    sb.Append(consonants[rndGen.Next(consonants.Length)]);
                    sb.Append(consonants[rndGen.Next(vowels.Length)]);
                }
            }

            return sb.ToString();
        }
    }
}
