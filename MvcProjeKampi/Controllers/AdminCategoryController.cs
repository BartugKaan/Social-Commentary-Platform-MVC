using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class AdminCategoryController : Controller
    {
        CategoryManager categoryManager = new CategoryManager(new EFCategoryDal());
        public ActionResult Index()
        {
            var categoryValues = categoryManager.GetAll();
            return View(categoryValues);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            CategoryValidatior validation = new CategoryValidatior();
            ValidationResult result = validation.Validate(category);
            if (result.IsValid)
            {
                categoryManager.AddCategory(category);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            return View();
        }

        public ActionResult DeleteCategory(int id)
        {
            var categoryValue = categoryManager.GetById(id);
            if (categoryValue != null)
            {
                categoryManager.CategoryDelete(categoryValue);
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            var categoryValue = categoryManager.GetById(id);
            if (categoryValue != null)
            {
                return View(categoryValue);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            CategoryValidatior validation = new CategoryValidatior();
            ValidationResult result = validation.Validate(category);
            if (result.IsValid)
            {
                categoryManager.CategoryUpdate(category);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            return View(category);
        }
    }
}