using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyShop.Domain.Models;
using MyShop.Infrastructure;
using MyShop.Infrastructure.Repositories;
using MyShop.Web.Controllers;
using MyShop.Web.Models;
using System;

namespace MyShop.Web.Tests
{
    [TestClass]
    public class OrderControllerTests
    {
        [TestMethod]
        public void CanCreateOrderWithCorrectModel()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            var productRepository = new Mock<IRepository<Product>>();
            var customerRepository = new Mock<IRepository<Customer>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var orderController = new OrderController(
                unitOfWork.Object
                //productRepository.Object,
                //orderRepository.Object,
                //customerRepository.Object
                );

            var createOrderModel = new CreateOrderModel
            {
                Customer = new CustomerModel
                {
                    Name = "Alek Kocevski",
                    ShippingAddress = "Elisie Popovski Marko 53/6",
                    City = "Skopje",
                    PostalCode = "1000",
                    Country = "Macedonia"
                },
                LineItems = new LineItemModel[]
                {
                    new LineItemModel {ProductId = Guid.NewGuid(), Quantity = 2},
                    new LineItemModel{ProductId= Guid.NewGuid(), Quantity = 15}
                }
            };

            orderController.Create(createOrderModel);
            //orderRepository.Verify(repository => repository.Add(It.IsAny<Order>()),
            //    Times.AtMostOnce());
            unitOfWork.Verify(repository => repository.OrderRepo.Add(It.IsAny<Order>()),
                Times.AtMostOnce());
        }
    }
}
