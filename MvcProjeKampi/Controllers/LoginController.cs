using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcProjeKampi.Controllers
{
    public class LoginController : Controller
    {
        private readonly AdminManager _adminManager;

        public LoginController()
        {
            _adminManager = new AdminManager(new EFAdminDal());
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Admin admin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Lütfen tüm gerekli alanları doğru şekilde doldurunuz.";
                    return View();
                }

                AdminValidator validationRules = new AdminValidator();
                ValidationResult result = validationRules.Validate(admin);
                
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                    return View();
                }

                if (_adminManager.ValidateAdmin(admin.AdminUserName, admin.AdminPassword))
                {

                    var adminUser = _adminManager.GetAdmin(admin.AdminUserName, admin.AdminPassword);
                    
                    if (adminUser != null)
                    {
                        Session["AdminId"] = adminUser.AdminId;
                        Session["AdminUserName"] = adminUser.AdminUserName;
                        
                        FormsAuthentication.SetAuthCookie(admin.AdminUserName, false);
                        
                        return RedirectToAction("Index", "AdminCategory");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Admin kullanıcı bilgileri alınamadı. Lütfen sistem yöneticisi ile iletişime geçiniz.";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Kullanıcı adı veya şifre hatalı.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Giriş sırasında bir hata oluştu. Lütfen tekrar deneyiniz.";
                
            }
            
            return View();
        }

        public ActionResult LogOut()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.Abandon();
            }
            catch (Exception ex)
            {
            }
            
            return RedirectToAction("Index", "Login");
        }

    }
}