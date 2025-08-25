using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using MvcProjeKampi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    [AdminAuthorization]
    public class AdminWriterController : Controller
    {
        WriterManager writerManager = new WriterManager(new EFWriterDal());
        public ActionResult Index()
        {
            var writerValues = writerManager.GetAll();
            return View(writerValues);
        }

        [HttpGet]
        public ActionResult AddWriter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddWriter(Writer writer)
        {
            WriterValiator validationRules = new WriterValiator();
            ValidationResult results = validationRules.Validate(writer);
            if (results.IsValid)
            {
                // WriterManager.WriterAdd metodu şifreyi otomatik olarak hash'ler
                writerManager.WriterAdd(writer);
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
        public ActionResult EditWriter(int id)
        {
            var writerValue = writerManager.GetById(id);
            return View(writerValue);
        }

        [HttpPost]
        public ActionResult EditWriter(Writer writer)
        {
            WriterValiator validationRules = new WriterValiator();
            ValidationResult results = validationRules.Validate(writer);
            if (results.IsValid)
            {
                // WriterManager.WriterUpdate metodu şifre değiştirilmişse hash'ler
                writerManager.WriterUpdate(writer);
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

        // Mevcut yazarların şifrelerini hash'lemek için yardımcı metot
        public ActionResult HashExistingPasswords()
        {
            var writers = writerManager.GetAll();
            foreach (var writer in writers)
            {
                // Eğer şifre hash'lenmemişse (BCrypt hash'i "$2a$" ile başlar)
                if (!string.IsNullOrEmpty(writer.WriterPassword) && !writer.WriterPassword.StartsWith("$2a$"))
                {
                    var originalPassword = writer.WriterPassword;
                    writer.WriterPassword = originalPassword; // WriterUpdate içinde hash'lenecek
                    writerManager.WriterUpdate(writer);
                }
            }
            
            ViewBag.Message = "Tüm yazarların şifreleri güvenli hale getirildi!";
            return RedirectToAction("Index");
        }
    }
}