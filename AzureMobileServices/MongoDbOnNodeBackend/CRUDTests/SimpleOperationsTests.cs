using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.MobileServices;

namespace CRUDTests
{
    [TestClass]
    public class SimpleOperationsTests
    {
        public static MobileServiceClient Client = new MobileServiceClient(
            "https://blog20140618.azure-mobile.net", "umErdVDwyiFtEUjquGtoMPbEPmoFEr37");
        public static IMobileServiceTable<Order> Table = Client.GetTable<Order>();

        [TestMethod]
        public async Task TestInsertWithNoId()
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Client = "John Doe",
                Items = new OrderItem[]
                {
                    new OrderItem { Name = "Bread", Quantity = 1, Price = 1.99 }
                }
            };

            await Table.InsertAsync(order);
            Assert.IsNotNull(order.Id);

            var inserted = await Table.LookupAsync(order.Id);
            Assert.AreEqual(order.OrderDate.ToUniversalTime(), inserted.OrderDate.ToUniversalTime());
            Assert.AreEqual(order.Id, inserted.Id);
            Assert.AreEqual(order.Client, inserted.Client);
            CollectionAssert.AreEqual(order.Items, inserted.Items);

            // Cleanup
            await Table.DeleteAsync(order);
        }


        [TestMethod]
        public async Task TestInsertWithId()
        {
            var id = Guid.NewGuid().ToString("N");
            var order = new Order
            {
                Id = id,
                OrderDate = DateTime.UtcNow,
                Client = "John Doe",
                Items = new OrderItem[]
                {
                    new OrderItem { Name = "Bread", Quantity = 1, Price = 1.99 }
                }
            };

            await Table.InsertAsync(order);
            Assert.AreEqual(id, order.Id);

            // Cleanup
            await Table.DeleteAsync(order);
        }

        [TestMethod]
        public async Task TestDelete()
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Client = "John Doe",
                Items = new OrderItem[]
                {
                    new OrderItem { Name = "Bread", Quantity = 1, Price = 1.99 }
                }
            };

            await Table.InsertAsync(order);
            var id = order.Id;

            await Table.DeleteAsync(order);

            try
            {
                await Table.LookupAsync(id);
                Assert.Fail("Lookup should have failed");
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, e.Response.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestUpdateMongoSuppliedId()
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Client = "John Doe",
                Items = new OrderItem[]
                {
                    new OrderItem { Name = "Bread", Quantity = 1, Price = 1.99 }
                }
            };

            await Table.InsertAsync(order);
            var id = order.Id;

            order.Client = "Jane Roe";
            var today = DateTime.UtcNow.Date;
            order.OrderDate = today;
            order.Items[0].Name = "White bread";
            await Table.UpdateAsync(order);

            var updated = await Table.LookupAsync(id);
            Assert.AreEqual("Jane Roe", updated.Client);
            Assert.AreEqual(today.ToUniversalTime(), updated.OrderDate.ToUniversalTime());
            Assert.AreEqual("White bread", updated.Items[0].Name);

            // Cleanup
            await Table.DeleteAsync(updated);
        }
    }
}
