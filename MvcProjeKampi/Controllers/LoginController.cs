using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
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
                AdminValidator validationRules = new AdminValidator();
                ValidationResult result = validationRules.Validate(admin);
                
                if (ModelState.IsValid && result.IsValid)
                {
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
                            ViewBag.ErrorMessage = "Kullanıcı bilgileri alınamadı";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Kullanıcı adı veya şifre yanlış";
                    }
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.ErrorMessage = "Giriş sırasında bir hata oluştu: " + ex.Message;
            }
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}