using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace ComplexQueryTests
{
    [DataTable("complexOrders")]
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

        public override string ToString()
        {
            return string.Format("Order[{0}-{1}-{2}]",
                Client,
                OrderDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                string.Join(", ", this.Items.ToList()));
        }

        public override bool Equals(object obj)
        {
            var other = obj as Order;
            if (other == null) return false;
            return this.Client == other.Client &&
                this.OrderDate == other.OrderDate &&
                AreEqual(this.Items, other.Items);
        }

        public override int GetHashCode()
        {
            int result = 0;
            if (this.Client != null) result ^= this.Client.GetHashCode();
            result ^= this.OrderDate.GetHashCode();
            if (this.Items != null)
            {
                foreach (var item in this.Items)
                {
                    if (item != null) result ^= item.GetHashCode();
                }
            }

            return result;
        }

        private static bool AreEqual(OrderItem[] expected, OrderItem[] actual)
        {
            if ((expected == null) != (actual == null)) return false;
            if (expected != null)
            {
                if (expected.Length != actual.Length) return false;
                for (var i = 0; i < expected.Length; i++)
                {
                    if (!expected[i].Equals(actual[i])) return false;
                }
            }

            return true;
        }
    }

    public class OrderItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        public override string ToString()
        {
            return string.Format("Item[{0}-{1}-{2}]", Name, Quantity, Price);
        }

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
