using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace CRUDTests
{
    [DataTable("orders")]
    public class Order
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("client")]
        public string Client { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("items")]
        public OrderItem[] Items { get; set; }
    }

    public class OrderItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        public override bool Equals(object obj)
        {
            OrderItem other = obj as OrderItem;
            if (other == null) return false;
            return this.Name == other.Name && this.Price == other.Price && this.Quantity == other.Quantity;
        }

        public override int GetHashCode()
        {
            var result = this.Price.GetHashCode() ^ this.Quantity.GetHashCode();
            if (this.Name != null)
            {
                result ^= this.Name.GetHashCode();
            }

            return result;
        }
    }
}
