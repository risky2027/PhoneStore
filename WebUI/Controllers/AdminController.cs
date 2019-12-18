using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IPhoneRepository repository;

        public AdminController(IPhoneRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Phones);
        }

        public ViewResult Edit(int phoneId)
        {
            Phone phone = repository.Phones.FirstOrDefault(p => p.PhoneId == phoneId);
            return View(phone);
        }

        [HttpPost]
        //public ActionResult Edit(Phone phone, HttpPostedFileBase image = null)
        public ActionResult Edit(Phone phone, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    phone.ImageMimeType = image.ContentType;
                    phone.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(phone.ImageData, 0, image.ContentLength);
                }
                repository.SavePhone(phone);
                TempData["message"] = string.Format("Изменение информации о товаре \"{0} {1}\" сохранены", phone.Mark, phone.Model);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(phone);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Phone());
        }

        [HttpPost]
        public ActionResult Delete(int phoneId)
        {
            Phone deletedPhone = repository.DeletePhone(phoneId);
            if (deletedPhone!=null)
            {
                TempData["message"] = string.Format("Товар \"{0} {1}\" был удален", deletedPhone.Mark, deletedPhone.Model);
            }
            return RedirectToAction("Index");
        }

    }
}