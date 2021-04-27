using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Infrastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(ShoppingContext context) : base(context)
        {
        }
        public override Customer Update(Customer entity)
        {
            var customer = context.Customers
                .SingleOrDefault(c => c.CustomerId == entity.CustomerId);

            customer.Name = entity.Name;
            customer.PostalCode = entity.PostalCode;
            customer.ShippingAddress = entity.ShippingAddress;
            customer.City = entity.City;
            customer.Country = entity.Country;

            return base.Update(customer);
        }
    }
}
