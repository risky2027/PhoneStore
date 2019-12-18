using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using Domain.Abstract;

namespace WebUI.Controllers
{
    public class CompanyController : Controller
    {
        private IOrderProcessor orderProcessor;

        public CompanyController (IOrderProcessor processor)
        {
            orderProcessor = processor;
        }

        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }

        public ActionResult FeedBack()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FeedBack(FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                orderProcessor.ProccessFeedBack(feedBack);
                
                return RedirectToAction("Completed", "Company");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Completed()
        {
            return View();
        }
    }
}