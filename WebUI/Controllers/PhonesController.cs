using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class PhonesController : Controller
    {
        private IPhoneRepository repository;
        public int pageSize = 4;


        public PhonesController(IPhoneRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int page = 1)
        {
            PhonesListViewModel model = new PhonesListViewModel
            {
                Phones = repository.Phones
                .Where(b => category == null || b.Category == category)
                .OrderBy(phone => phone.PhoneId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                        repository.Phones.Count() : 
                        repository.Phones.Where(phone => phone.Category == category).Count()
                },
                CurrentCategory = category
            };

            return View(model);
        }

        public FileContentResult GetImage(int phoneId)
        {
            Phone phone = repository.Phones
                .FirstOrDefault(p => p.PhoneId == phoneId);

            if (phone != null)
            {
                return File(phone.ImageData, phone.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public ActionResult Info(int Id, string returnUrl)
        {
            Phone phone = repository.Phones
                .FirstOrDefault(p => p.PhoneId == Id);
            
            return View(phone);
        }
    }
}