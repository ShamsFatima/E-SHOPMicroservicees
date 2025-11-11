//using Ordering.Domain.Models;
//using Ordering.Domain.ValueObjects;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Ordering.Infrastructure.Data.Extensions
//{
//    internal class InititalData
//    {
//        public static IEnumerable<Customer> Customers=>
//            new List<Customer> { Customer.Create("Mehmet","mehmet@gmail.com"), 
//                                Customer.Create("Mehmet1","mehmet1@gmail.com")};
//        public static IEnumerable<Product> Products=> new List<Product> 
//                              {
//                                Product.Create("IPhone",100),
//    Product.Create("Product 2",200),
//    Product.Create("Keyboard",90),
//    Product.Create("Monitor",250)
//        };
//        public static IEnumerable<Order> OrdersWithItems
//        {
//            get
//            {
//                var address1 = Address.Of(
//                    firstName: "John",
//                    lastName: "Doe",
//                    emailAddress: "john@example.com",
//                    Country: "USA",
//                    state: "New York",
//                    zipCode: "10001",
//                    addressLine: "123 Main St");

//                var address2 = Address.Of(
//                    firstName: "Jane",
//                    lastName: "Smith",
//                    emailAddress: "jane@example.com",
//                    Country: "USA",
//                    state: "California",
//                    zipCode: "90001",
//                    addressLine: "456 Park Ave");

//                // ✅ Create mock payment info (assuming you have Payment.Of)
//                var payment1 = Payment.Of("Visa", "John Doe", "4111111111111111", "12/27", 123);
//                var payment2 = Payment.Of("MasterCard", "Jane Smith", "5555555555554444", "09/28", 456);

//                // ✅ Create orders using existing customers
//                var customerList = Customers.ToList();
//                var productList = Products.ToList();

//                var order1 = Order.Create(
//    OrderId.Of(Guid.NewGuid()),
//    customerList[0].Id,
//    OrderName.Of("Ord_1"),
//    address1,
//    address1,
//    payment1);

//                order1.Add(productList[0].Id, 1, productList[0].Price);
//                order1.Add(productList[1].Id, 2, productList[1].Price);

//                var order2 = Order.Create(
//                    OrderId.Of(Guid.NewGuid()),
//                    customerList[1].Id,
//                    OrderName.Of("Ord_2"),
//                    address2,
//                    address2,
//                    payment2);

//                order2.Add(productList[2].Id, 1, productList[2].Price);
//                order2.Add(productList[3].Id, 1, productList[3].Price);

//                return new List<Order> { order1, order2 };

//            }
//        }
//    }
//}
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Extensions
{
    internal class InititalData
    {
        // ✅ Reflection helper to construct Address correctly
        private static Address CreateAddress(
            string firstName, string lastName, string email,
            string country, string state, string zip, string line)
        {
            var ctor = typeof(Address)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(c => c.GetParameters().Length == 7);

            return (Address)ctor.Invoke(new object[]
            {
                firstName, lastName, email, country, state, zip, line
            });
        }

        public static readonly List<Customer> Customers =
    new()
    {
        Customer.Create("Mehmet","mehmet@gmail.com"),
        Customer.Create("Mehmet1","mehmet1@gmail.com")
    };


        public static readonly List<Product> Products = new()
{
    Product.Create("IPhone",100),
    Product.Create("Product 2",200),
    Product.Create("Keyboard",90),
    Product.Create("Monitor",250)
};


        public static IEnumerable<Order> OrdersWithItems
        {
            get
            {
                // ✅ Use CreateAddress() instead of Address.Of()
                var address1 = CreateAddress("John", "Doe", "john@example.com", "USA", "New York", "10001", "123 Main St");
                var address2 = CreateAddress("Jane", "Smith", "jane@example.com", "USA", "California", "90001", "456 Park Ave");

                var payment1 = Payment.Of("John Doe", "4111111111111111", "12/27", "123", 1);
                var payment2 = Payment.Of("Jane Smith", "5555555555554444", "09/28", "456", 2);

                var customers = Customers.ToList();
                var products = Products.ToList();

                var order1 = Order.Create(
                    OrderId.Of(Guid.NewGuid()),
                    customers[0].Id,
                    OrderName.Of("Ord_1"),
                    address1,
                    address1,
                    payment1);

                order1.Add(products[0].Id, 1, products[0].Price);
                order1.Add(products[1].Id, 2, products[1].Price);

                var order2 = Order.Create(
                    OrderId.Of(Guid.NewGuid()),
                    customers[1].Id,
                    OrderName.Of("Ord_2"),
                    address2,
                    address2,
                    payment2);

                order2.Add(products[2].Id, 1, products[2].Price);
                order2.Add(products[3].Id, 1, products[3].Price);

                return new List<Order> { order1, order2 };
            }
        }
    }
}


