using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.Models;
using MyShop.Infrastructure;
using MyShop.Infrastructure.Repositories;

namespace MyShop.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IRepository<Customer> customerRepo;

        //private ShoppingContext context;

        public CustomerController(IRepository<Customer> customerRepo)
        {
            this.customerRepo = customerRepo;
            //context = new ShoppingContext();
        }

        public IActionResult Index(Guid? id)
        {
            if (id == null)
            {
                var customers = customerRepo.All(); //context.Customers.ToList();

                return View(customers);
            }
            else
            {
                var customer = customerRepo.Get(id.Value); //customerRepo.Find(c => c.CustomerId == id); //context.Customers.Find(id.Value);

                return View(new[] { customer });
            }
        }
    }
}
