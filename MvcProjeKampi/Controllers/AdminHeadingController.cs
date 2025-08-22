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
    public class AdminHeadingController : Controller
    {
        HeadingManager headingManager = new HeadingManager(new EFHeadingDal());
        CategoryManager categoryManager = new CategoryManager(new EFCategoryDal());
        WriterManager writerManager = new WriterManager(new EFWriterDal());

        public ActionResult Index()
        {
            var headingValues = headingManager.GetAll();
            return View(headingValues);
        }

        [HttpGet]
        public ActionResult AddHeading()
        {
            List<SelectListItem> valueCategory = (from x in categoryManager.GetAll()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryId.ToString()
                                                  }).ToList();
            ViewBag.ctg = valueCategory;
            List<SelectListItem> valueWriter = (from x in writerManager.GetAll()
                                                select new SelectListItem
                                                {
                                                    Text = x.WriterName,
                                                    Value = x.WriterId.ToString()
                                                }).ToList();
            ViewBag.wrt = valueWriter;
            return View();
        }

        [HttpPost]
        public ActionResult AddHeading(Heading heading)
        {
            HeadingValidator validationRules = new HeadingValidator();
            ValidationResult results = validationRules.Validate(heading);
            if (results.IsValid)
            {
                heading.HeadingDate =DateTime.Parse(DateTime.Now.ToShortDateString());
                headingManager.AddHeading(heading);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            List<SelectListItem> valueCategory = (from x in categoryManager.GetAll()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryId.ToString()
                                                  }).ToList();
            ViewBag.ctg = valueCategory;
            var headingValue = headingManager.GetById(id);
            return View(headingValue);
        }

        [HttpPost]
        public ActionResult EditHeading(Heading heading)
        {
            HeadingValidator validationRules = new HeadingValidator();
            ValidationResult result = validationRules.Validate(heading);
            if (result.IsValid)
            {
                headingManager.UpdateHeading(heading);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(heading);
        }

        public ActionResult DeleteHeading(int id)
        {
            var headingValue = headingManager.GetById(id);
            headingManager.ChangeStatus(headingValue);
            return RedirectToAction("Index");
        }
    }
}