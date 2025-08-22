using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class AdminAboutController : Controller
    {
        AboutManager manager = new AboutManager(new EFAboutDal());
        public ActionResult Index()
        {
            var aboutValues = manager.GetAll();
            return View(aboutValues);
        }

        [HttpGet]
        public ActionResult AddAbout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAbout(About about)
        {
            AboutValidatior validations = new AboutValidatior();
            ValidationResult result = validations.Validate(about);
            if (result.IsValid)
            {
                manager.AddAbout(about);
                return RedirectToAction("Index");
            }
            else
            {
                // TODO: It is not working properly Needs to be fixed
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                ViewBag.error = "Hata: Yeni Hakkımıza oluştururken Hata oluştu. Tüm gerekli alanları doldurunuz.";
            }
            return View();
        }

        public PartialViewResult AboutPartial()
        {
            return PartialView();
        }


        [HttpGet]
        public ActionResult EditAbout(int id)
        {
            var aboutValue = manager.GetById(id);
            return View(aboutValue);
        }


        [HttpPost]
        public ActionResult EditAbout(About about)
        {
            AboutValidatior validations = new AboutValidatior();
            ValidationResult result = validations.Validate(about);
            if (result.IsValid)
            {
                manager.UpdateAbout(about);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }


        public ActionResult DeleteAbout(int id)
        {
            var aboutValue = manager.GetById(id);
            manager.DeleteAbout(aboutValue);
            return RedirectToAction("Index");
        }
    }
}