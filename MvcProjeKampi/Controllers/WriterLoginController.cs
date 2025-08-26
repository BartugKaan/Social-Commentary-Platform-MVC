using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcProjeKampi.Controllers
{
    public class WriterLoginController : Controller
    {
        private readonly WriterManager _writerManager;

        public WriterLoginController()
        {
            _writerManager = new WriterManager(new EFWriterDal());
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string WriterMail, string WriterPassword)
        {
            if (string.IsNullOrEmpty(WriterMail))
            {
                ViewBag.ErrorMessage = "E-posta adresi bo� olamaz!";
                return View();
            }

            if (string.IsNullOrEmpty(WriterPassword))
            {
                ViewBag.ErrorMessage = "�ifre bo� olamaz!";
                return View();
            }

            try
            {
                if (_writerManager.ValidateWriter(WriterMail, WriterPassword))
                {
                    var writerUser = _writerManager.GetWriterByEmail(WriterMail);
                    if (writerUser != null && writerUser.WriterStatus)
                    {
                        Session["WriterId"] = writerUser.WriterId;
                        Session["WriterName"] = writerUser.WriterName + " " + writerUser.WriterSurname;
                        Session["WriterMail"] = writerUser.WriterMail;

                        FormsAuthentication.SetAuthCookie(WriterMail, false);

                        return RedirectToAction("WriterProfile", "WriterPanel");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Hesab�n�z aktif de�il veya bulunamad�!";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "E-posta adresiniz veya �ifreniz hatal�!";
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.ErrorMessage = "Giri� yap�l�rken bir hata olu�tu!";
            }

            ViewBag.WriterMail = WriterMail;
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Writer writer)
        {
            try
            {
                WriterValiator validationRules = new WriterValiator();
                ValidationResult validationResults = validationRules.Validate(writer);

                var existingWriter = _writerManager.GetWriterByEmail(writer.WriterMail);
                if (existingWriter != null)
                {
                    ModelState.AddModelError("WriterMail", "Bu e-posta adresi ile zaten bir hesap mevcut!");
                    return View(writer);
                }

                if (!validationResults.IsValid)
                {
                    foreach (var error in validationResults.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return View(writer);
                }

                SetDefaultValues(writer);


                _writerManager.WriterAdd(writer);
                
                TempData["SuccessMessage"] = "Kay�t ba�ar�l�! �imdi giri� yapabilirsiniz.";
                TempData["RegisteredEmail"] = writer.WriterMail;
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                ViewBag.ErrorMessage = "Kay�t s�ras�nda bir hata olu�tu!";
                return View(writer);
            }
        }

        private void SetDefaultValues(Writer writer)
        {
            if (string.IsNullOrEmpty(writer.WriterTitle))
                writer.WriterTitle = "Yazar";
            
            if (string.IsNullOrEmpty(writer.WriterImage))
                writer.WriterImage = "/AdminLTE-3.0.4/dist/img/user2-160x160.jpg";

            if (string.IsNullOrEmpty(writer.WriterAbout))
                writer.WriterAbout = "Hen�z a��klama eklenmemi�.";
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "WriterLogin");
        }
    }
}