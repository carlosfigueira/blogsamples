using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MongoDbOnNetBackend
{
    public class OrderController : TableController<Order>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var connStringName = "mongodb";
            var dbName = "MyMongoLab";
            var collectionName = "orders";
            this.DomainManager = new MongoDomainManager<Order>(connStringName, dbName, collectionName, this.Request, this.Services);
        }

        [ExpandProperty("Items")]
        public IQueryable<Order> GetAllOrders()
        {
            return base.Query();
        }

        public Order GetOneOrder(string id)
        {
            var result = base.Lookup(id).Queryable.FirstOrDefault();
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                return result;
            }
        }

        public Task<Order> PostOrder(Order order)
        {
            return base.InsertAsync(order);
        }

        public Task DeleteOrder(string id)
        {
            return base.DeleteAsync(id);
        }

        public Task<Order> PatchOrder(string id, Delta<Order> patch)
        {
            return base.UpdateAsync(id, patch);
        }
    }
}