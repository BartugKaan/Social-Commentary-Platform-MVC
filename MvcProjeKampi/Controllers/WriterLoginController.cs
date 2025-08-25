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
            // Basit validation
            if (string.IsNullOrEmpty(WriterMail))
            {
                ViewBag.ErrorMessage = "E-posta adresi boþ olamaz!";
                return View();
            }

            if (string.IsNullOrEmpty(WriterPassword))
            {
                ViewBag.ErrorMessage = "Þifre boþ olamaz!";
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
                        ViewBag.ErrorMessage = "Hesabýnýz aktif deðil veya bulunamadý!";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "E-posta adresiniz veya þifreniz hatalý!";
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.ErrorMessage = "Giriþ yapýlýrken bir hata oluþtu!";
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
                // Basit validation
                if (string.IsNullOrEmpty(writer.WriterName))
                {
                    ViewBag.ErrorMessage = "Ad alaný boþ olamaz!";
                    return View();
                }

                if (string.IsNullOrEmpty(writer.WriterSurname))
                {
                    ViewBag.ErrorMessage = "Soyad alaný boþ olamaz!";
                    return View();
                }

                if (string.IsNullOrEmpty(writer.WriterMail))
                {
                    ViewBag.ErrorMessage = "E-posta alaný boþ olamaz!";
                    return View();
                }

                if (string.IsNullOrEmpty(writer.WriterPassword))
                {
                    ViewBag.ErrorMessage = "Þifre alaný boþ olamaz!";
                    return View();
                }

                if (writer.WriterPassword.Length < 6)
                {
                    ViewBag.ErrorMessage = "Þifre en az 6 karakter olmalýdýr!";
                    return View();
                }

                // E-posta kontrolü
                var existingWriter = _writerManager.GetWriterByEmail(writer.WriterMail);
                if (existingWriter != null)
                {
                    ViewBag.ErrorMessage = "Bu e-posta adresi ile zaten bir hesap mevcut!";
                    return View();
                }

                // Varsayýlan deðerler
                if (string.IsNullOrEmpty(writer.WriterTitle))
                    writer.WriterTitle = "Yazar";
                
                if (string.IsNullOrEmpty(writer.WriterImage))
                    writer.WriterImage = "/AdminLTE-3.0.4/dist/img/user2-160x160.jpg";

                if (string.IsNullOrEmpty(writer.WriterAbout))
                    writer.WriterAbout = "Henüz açýklama eklenmemiþ.";

                // Kaydet
                _writerManager.WriterAdd(writer);
                
                TempData["SuccessMessage"] = "Kayýt baþarýlý! Þimdi giriþ yapabilirsiniz.";
                TempData["RegisteredEmail"] = writer.WriterMail;
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                ViewBag.ErrorMessage = "Kayýt sýrasýnda bir hata oluþtu!";
                return View();
            }
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