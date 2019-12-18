using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Domain.Abstract;
using WebUI.Controllers;
using System.Web.Mvc;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //огранизация
            Phone phone1 = new Phone { PhoneId = 1, Mark = "Phone1" };
            Phone phone2 = new Phone { PhoneId = 2, Mark = "Phone2" };

            Cart cart = new Cart();

            //действие
            cart.AddItem(phone1, 1);
            cart.AddItem(phone2, 1);
            List<CartLine> results = cart.Lines.ToList();

            //утверждение
            Assert.AreEqual(results.Count, 2);
            Assert.AreEqual(results[0].Phone, phone1);
            Assert.AreEqual(results[1].Phone, phone2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //огранизация
            Phone phone1 = new Phone { PhoneId = 1, Mark = "Phone1" };
            Phone phone2 = new Phone { PhoneId = 2, Mark = "Phone2" };

            Cart cart = new Cart();

            //действие
            cart.AddItem(phone1, 1);
            cart.AddItem(phone2, 1);
            cart.AddItem(phone1, 5);
            List<CartLine> results = cart.Lines.ToList();

            //утверждение
            Assert.AreEqual(results.Count, 2);
            Assert.AreEqual(results[0].Quantity, 6);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //огранизация
            Phone phone1 = new Phone { PhoneId = 1, Mark = "Phone1" };
            Phone phone2 = new Phone { PhoneId = 2, Mark = "Phone2" };
            Phone phone3 = new Phone { PhoneId = 3, Mark = "Phone3" };

            Cart cart = new Cart();

            //действие
            cart.AddItem(phone1, 1);
            cart.AddItem(phone2, 1);
            cart.AddItem(phone1, 5);
            cart.AddItem(phone3, 5);
            cart.RemoveLine(phone2);

            //утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Phone == phone2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //огранизация
            Phone phone1 = new Phone { PhoneId = 1, Mark = "Phone1", Price = 100 };
            Phone phone2 = new Phone { PhoneId = 2, Mark = "Phone2", Price = 55 };

            Cart cart = new Cart();

            //действие
            cart.AddItem(phone1, 1);
            cart.AddItem(phone2, 1);
            cart.AddItem(phone1, 5);
            decimal result = cart.ComputeTotalValue();

            //утверждение
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            //огранизация
            Phone phone1 = new Phone { PhoneId = 1, Mark = "Phone1", Price = 100 };
            Phone phone2 = new Phone { PhoneId = 2, Mark = "Phone2", Price = 55 };

            Cart cart = new Cart();

            //действие
            cart.AddItem(phone1, 1);
            cart.AddItem(phone2, 1);
            cart.AddItem(phone1, 5);
            cart.Clear();

            //утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        //добавление элемента в корзину
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone {PhoneId = 1, Mark = "Sony1", Category = "Category1"}
            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object, null);

            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Phone.PhoneId, 1);
        }

        //После добавления товара в корзину перенаправление на страницу корзины
        [TestMethod]
        public void Can_Phone_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone {PhoneId = 1, Mark = "Sony1", Category = "Category1"}
            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object, null);

            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();
            CartController target = new CartController(null, null);

            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();

            CartController controller = new CartController(null, mock.Object);

            ViewResult result = controller.Checkout(cart, shippingDetails);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Phone(), 1);

            CartController controller = new CartController(null, mock.Object);
            controller.ModelState.AddModelError("error", "error");

            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Phone(), 1);

            CartController controller = new CartController(null, mock.Object);

            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
