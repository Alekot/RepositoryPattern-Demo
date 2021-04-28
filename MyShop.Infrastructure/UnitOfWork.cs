using MyShop.Domain.Models;
using MyShop.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Infrastructure
{
    public interface IUnitOfWork
    {
        IRepository<Product> ProductRepo { get; }
        IRepository<Customer> CustomerRepo { get; }
        IRepository<Order> OrderRepo { get; }
        void SaveChanges();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private ShoppingContext context;
        public UnitOfWork(ShoppingContext context)
        {
            this.context = context;
        }

        private IRepository<Customer> customerRepo;

        public IRepository<Customer> CustomerRepo
        {
            get
            {
                if (customerRepo == null)
                {
                    customerRepo = new CustomerRepository(context);
                }
                return customerRepo;
            }
        }


        private IRepository<Product> productRepo;
        public IRepository<Product> ProductRepo
        {
            get
            {
                if (productRepo == null)
                {
                    productRepo = new ProductRepository(context);
                }
                return productRepo;
            }
        }

        private IRepository<Order> orderRepo;

        public IRepository<Order> OrderRepo
        {
            get
            {
                if(orderRepo == null)
                {
                    orderRepo = new OrderRepository(context);
                }
                return orderRepo;
            }
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
