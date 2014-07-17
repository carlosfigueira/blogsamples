using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComplexQueryTests
{
    public static class ComparisonHelper
    {
        public static void CompareAssert(IEnumerable<Order> expected, IEnumerable<Order> actual)
        {
            var differences = new List<string>();
            if (!Compare(expected, actual, differences))
            {
                var sb = new StringBuilder();
                sb.AppendLine("Error comparing order collections:");
                for (var i = differences.Count - 1; i >= 0; i--)
                {
                    sb.AppendLine(differences[i]);
                }

                Assert.Fail(sb.ToString().Trim());
            }
        }

        public static bool Compare(IEnumerable<Order> expected, IEnumerable<Order> actual, List<string> differences)
        {
            if ((expected == null) != (actual == null))
            {
                differences.Add("One order is null, other is not");
                return false;
            }

            if (expected != null)
            {
                var expectedOrders = expected.ToArray();
                var actualOrders = actual.ToArray();
                if (expectedOrders.Length != actualOrders.Length)
                {
                    differences.Add("Expected value has " + expectedOrders.Length + " orders while actual has " +
                        actualOrders.Length);
                    return false;
                }

                for (var i = 0; i < expectedOrders.Length; i++)
                {
                    if (!Compare(expectedOrders[i], actualOrders[i], differences))
                    {
                        differences.Add("Error comparing order at index " + i);
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool Compare(Order expected, Order actual, List<string> differences)
        {
            if ((expected == null) != (actual == null))
            {
                differences.Add("One order is null, other is not");
                return false;
            }

            if (expected != null)
            {
                if (expected.Client != actual.Client)
                {
                    differences.Add("Expected client (" + expected.Client + ") != actual (" + actual.Client + ")");
                    return false;
                }

                if (expected.OrderDate != actual.OrderDate)
                {
                    differences.Add("Expected order date (" + expected.OrderDate + ") != actual (" + actual.OrderDate + ")");
                    return false;
                }

                if ((expected.Items == null) != (actual.Items == null))
                {
                    differences.Add("Expected items " + (expected.Items == null ? "is" : "is not") +
                        "null while actual items " + (actual.Items == null ? "is" : "is not") + "null");
                    return false;
                }

                if (expected.Items != null)
                {
                    if (expected.Items.Length != actual.Items.Length)
                    {
                        differences.Add("Expected items has " + expected.Items.Length + " while actual has " +
                            actual.Items.Length);
                        return false;
                    }

                    for (var i = 0; i < expected.Items.Length; i++)
                    {
                        if (!Compare(expected.Items[i], actual.Items[i], differences))
                        {
                            differences.Add("Difference in item " + i);
                            return false;
                        }
                    }
                }
            }
            Assert.AreEqual(expected == null, actual == null, "One is null, the other is not");
            Assert.AreEqual(expected.Client, actual.Client, "Clients are different: " + expected.Client + " != " + actual.Client);

            return true;
        }

        private static bool Compare(OrderItem expected, OrderItem actual, List<string> differences)
        {
            if ((expected == null) != (actual == null))
            {
                differences.Add("One order item is null, other is not");
                return false;
            }

            if (expected != null)
            {
                if (expected.Name != actual.Name)
                {
                    differences.Add("Expected item name (" + expected.Name + ") != actual (" + actual.Name + ")");
                    return false;
                }

                if (expected.Quantity != actual.Quantity)
                {
                    differences.Add("Expected item quantity (" + expected.Quantity + ") != actual (" + actual.Quantity + ")");
                    return false;
                }

                const double AcceptableDifference = 0.000001;
                if (Math.Abs(expected.Price - actual.Price) > AcceptableDifference)
                {
                    differences.Add("Expected item price (" + expected.Price + ") != actual (" + actual.Price + ")");
                    return false;
                }
            }

            return true;
        }
    }
}
