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
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(ShoppingContext context) : base(context)
        {
        }
        public override Order Update(Order entity)
        {
            var order = context.Orders
                .SingleOrDefault(o => o.OrderId == entity.OrderId);

            
            //order.Customer = entity.Customer;
            //order.CustomerId = entity.CustomerId;
            order.OrderDate = entity.OrderDate;
            order.LineItems = entity.LineItems;

            return base.Update(order);
        }
        public override IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate)
        {
            return context.Orders
                .Include(o => o.LineItems)
                .ThenInclude(p => p.Product)
                .Where(predicate)
                .ToList();
        }
    }
}
