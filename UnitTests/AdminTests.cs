using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Phones()
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

            AdminController controller = new AdminController(mock.Object);

            List<Phone> result = ((IEnumerable<Phone>)controller.Index().ViewData.Model).ToList();

            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual(result[0].Mark, "Sony1");
            Assert.AreEqual(result[1].Mark, "Sony2");
        }

        [TestMethod]
        public void Can_Edit_Phone()
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

            AdminController controller = new AdminController(mock.Object);

            Phone phone1 = controller.Edit(1).ViewData.Model as Phone;
            Phone phone2 = controller.Edit(2).ViewData.Model as Phone;
            Phone phone3 = controller.Edit(3).ViewData.Model as Phone;

            Assert.AreEqual(1, phone1.PhoneId);
            Assert.AreEqual(2, phone2.PhoneId);
            Assert.AreEqual(3, phone3.PhoneId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Phone()
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

            AdminController controller = new AdminController(mock.Object);

            Phone result = controller.Edit(7).ViewData.Model as Phone;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            AdminController controller = new AdminController(mock.Object);

            Phone phone = new Phone { Mark = "Test" };

            ActionResult result = controller.Edit(phone);

            mock.Verify(m => m.SavePhone(phone));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Save_InValid_Changes()
        {
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            AdminController controller = new AdminController(mock.Object);

            Phone phone = new Phone { Mark = "Test" };

            controller.ModelState.AddModelError("error", "error");

            ActionResult result = controller.Edit(phone);

            mock.Verify(m => m.SavePhone(It.IsAny<Phone>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Phones()
        {
            Phone phone = new Phone { PhoneId = 2, Mark = "Телефон2" };
            
            // Организация - создание имитированного хранилища данных
            Mock<IPhoneRepository> mock = new Mock<IPhoneRepository>();
            mock.Setup(m => m.Phones).Returns(new List<Phone>
                {
                new Phone { PhoneId = 1, Mark = "Телефон1" },
                new Phone { PhoneId = 2, Mark = "Телефон2" },
                new Phone { PhoneId = 3, Mark = "Телефон3" },
                new Phone { PhoneId = 4, Mark = "Телефон4" },
                new Phone { PhoneId = 5, Mark = "Телефон5" },
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление игры
            controller.Delete(phone.PhoneId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Game
            mock.Verify(m => m.DeletePhone(phone.PhoneId));
        }
    }
}
