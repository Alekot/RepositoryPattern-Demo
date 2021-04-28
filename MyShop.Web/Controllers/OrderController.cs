using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Models;
using MyShop.Infrastructure;
using MyShop.Infrastructure.Repositories;
using MyShop.Web.Models;

namespace MyShop.Web.Controllers
{
    public class OrderController : Controller
    {
        //private readonly IRepository<Product> productRepo;
        //private readonly IRepository<Order> orderRepo;
        //private readonly IRepository<Customer> customerRepo;
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork unitOfWork
                                //,IRepository<Product> productRepo
                                //,IRepository<Order> orderRepo
                                //,IRepository<Customer> customerRepo
            )
        {
            this.unitOfWork = unitOfWork;
            //this.productRepo = productRepo;
            //this.orderRepo = orderRepo;
            //this.customerRepo = customerRepo;
        }

        public IActionResult Index()
        {
            var orders = unitOfWork.OrderRepo //orderRepo
                .Find(order => order.OrderDate > DateTime.UtcNow.AddDays(-1));

            return View(orders);
        }

        public IActionResult Create()
        {
            var products = unitOfWork.ProductRepo //productRepo
                .All();

            return View(products);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderModel model)
        {
            #region Model Validation
            if (!model.LineItems.Any()) return BadRequest("Please submit line items");

            if (string.IsNullOrWhiteSpace(model.Customer.Name)) return BadRequest("Customer needs a name");
            #endregion

            var customer = unitOfWork.CustomerRepo //customerRepo
                .Find(c => c.Name == model.Customer.Name)
                .FirstOrDefault();

            if (customer != null)
            {
                customer.Name = model.Customer.Name;
                customer.ShippingAddress = model.Customer.ShippingAddress;
                customer.City = model.Customer.City;
                customer.Country = model.Customer.Country;
                customer.PostalCode = model.Customer.PostalCode;

                unitOfWork.CustomerRepo.Update(customer);
                //customerRepo.Update(customer);
                //customerRepo.SaveChanges();
            }
            else
            {
                customer = new Customer
                {
                    Name = model.Customer.Name,
                    ShippingAddress = model.Customer.ShippingAddress,
                    City = model.Customer.City,
                    PostalCode = model.Customer.PostalCode,
                    Country = model.Customer.Country
                };
            }

            var order = new Order
            {
                LineItems = model.LineItems
                    .Select(line => new LineItem { ProductId = line.ProductId, Quantity = line.Quantity })
                    .ToList(),

                Customer = customer
            };

            //orderRepo.Add(order);
            unitOfWork.OrderRepo.Add(order);
            //orderRepo.SaveChanges();
            unitOfWork.SaveChanges();

            return Ok("Order Created");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
