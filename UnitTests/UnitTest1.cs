using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using Domain.Entities;
using System.Collections.Generic;
using WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.HtmlHelpers;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone {PhoneId = 1, Mark = "Sony1"},
                new Phone {PhoneId = 2, Mark = "Sony2"},
                new Phone {PhoneId = 3, Mark = "Sony3"},
                new Phone {PhoneId = 4, Mark = "Sony4"},
                new Phone {PhoneId = 5, Mark = "Sony5"}
            });

            PhonesController controller = new PhonesController(mock.Object);
            controller.pageSize = 3;

            PhonesListViewModel result = (PhonesListViewModel)controller.List(null, 2).Model;

            List<Phone> phones = result.Phones.ToList();
            Assert.IsTrue(phones.Count == 2);
            Assert.AreEqual(phones[0].Mark, "Sony4");
            Assert.AreEqual(phones[1].Mark, "Sony5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Огранизация
            HtmlHelper myHelper = null;
            PagingInfo paginInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Действие
            MvcHtmlString result = myHelper.PageLinks(paginInfo, pageUrlDelegate);
            
            //Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone {PhoneId = 1, Mark = "Sony1"},
                new Phone {PhoneId = 2, Mark = "Sony2"},
                new Phone {PhoneId = 3, Mark = "Sony3"},
                new Phone {PhoneId = 4, Mark = "Sony4"},
                new Phone {PhoneId = 5, Mark = "Sony5"}
            });

            PhonesController controller = new PhonesController(mock.Object);
            controller.pageSize = 3;

            PhonesListViewModel result = (PhonesListViewModel)controller.List(null, 2).Model;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }

        //ошибка!
        [TestMethod]
        public void Can_Filter_Phones()
        {
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone {PhoneId = 1, Mark = "Sony1", Category = "Category1"},
                new Phone {PhoneId = 2, Mark = "Sony2", Category = "Category2"},
                new Phone {PhoneId = 3, Mark = "Sony3", Category = "Category1"},
                new Phone {PhoneId = 4, Mark = "Sony4", Category = "Category3"},
                new Phone {PhoneId = 5, Mark = "Sony5", Category = "Category2"}
            });

            PhonesController controller = new PhonesController(mock.Object);
            controller.pageSize = 3;

            List <Phone> result = ((PhonesListViewModel)controller.List("Category2", 1).Model).Phones.ToList();
            
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Mark == "Category2" && result[0].Category == "Category2");
            Assert.IsTrue(result[1].Mark == "Category5" && result[1].Category == "Category2");
        }
        
        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone {PhoneId = 1, Mark = "Sony1", Category = "Category1"},
                new Phone {PhoneId = 2, Mark = "Sony2", Category = "Category2"},
                new Phone {PhoneId = 3, Mark = "Sony3", Category = "Category1"},
                new Phone {PhoneId = 4, Mark = "Sony4", Category = "Category3"},
                new Phone {PhoneId = 5, Mark = "Sony5", Category = "Category2"}
            });

            NavController target = new NavController(mock.Object);

            List<string> result = ((IEnumerable<string>)target.Menu().Model).ToList();

            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0], "Category1");
            Assert.AreEqual(result[1], "Category2");
            Assert.AreEqual(result[2], "Category3");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone {PhoneId = 1, Mark = "Sony1", Category = "Category1"},
                new Phone {PhoneId = 2, Mark = "Sony2", Category = "Category2"},
                new Phone {PhoneId = 3, Mark = "Sony3", Category = "Category1"},
                new Phone {PhoneId = 4, Mark = "Sony4", Category = "Category3"},
                new Phone {PhoneId = 5, Mark = "Sony5", Category = "Category2"}
            });

            NavController target = new NavController(mock.Object);

            string categoryToSelect = "Category2";

            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Phone_Count()
        {
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
            {
                new Phone {PhoneId = 1, Mark = "Sony1", Category = "Category1"},
                new Phone {PhoneId = 2, Mark = "Sony2", Category = "Category2"},
                new Phone {PhoneId = 3, Mark = "Sony3", Category = "Category1"},
                new Phone {PhoneId = 4, Mark = "Sony4", Category = "Category3"},
                new Phone {PhoneId = 5, Mark = "Sony5", Category = "Category2"}
            });

            PhonesController controller = new PhonesController(mock.Object);
            controller.pageSize = 3;

            int res1 = ((PhonesListViewModel)controller.List("Category1").Model).PagingInfo.TotalItems;
            int res2 = ((PhonesListViewModel)controller.List("Category2").Model).PagingInfo.TotalItems;
            int res3 = ((PhonesListViewModel)controller.List("Category3").Model).PagingInfo.TotalItems;
            int resAll = ((PhonesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
