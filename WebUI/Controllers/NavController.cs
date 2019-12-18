using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private IPhoneRepository repository;

        public NavController (IPhoneRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> typeOfPhone = repository.Phones
                .Select(phone => phone.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView("FlexMenu", typeOfPhone);
        }
    }
}