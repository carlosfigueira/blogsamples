using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace MongoDbOnNetBackend
{
    public class Order : DocumentData
    {
        public DateTime OrderDate { get; set; }

        public string Client { get; set; }

        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public string Name { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }
    }
}