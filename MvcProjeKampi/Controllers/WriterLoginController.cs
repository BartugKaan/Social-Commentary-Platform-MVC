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
                // Basit validation
                if (string.IsNullOrEmpty(writer.WriterName))
                {
                    ViewBag.ErrorMessage = "Ad alan� bo� olamaz!";
                    return View();
                }

                if (string.IsNullOrEmpty(writer.WriterSurname))
                {
                    ViewBag.ErrorMessage = "Soyad alan� bo� olamaz!";
                    return View();
                }

                if (string.IsNullOrEmpty(writer.WriterMail))
                {
                    ViewBag.ErrorMessage = "E-posta alan� bo� olamaz!";
                    return View();
                }

                if (string.IsNullOrEmpty(writer.WriterPassword))
                {
                    ViewBag.ErrorMessage = "�ifre alan� bo� olamaz!";
                    return View();
                }

                if (writer.WriterPassword.Length < 6)
                {
                    ViewBag.ErrorMessage = "�ifre en az 6 karakter olmal�d�r!";
                    return View();
                }

                // E-posta kontrol�
                var existingWriter = _writerManager.GetWriterByEmail(writer.WriterMail);
                if (existingWriter != null)
                {
                    ViewBag.ErrorMessage = "Bu e-posta adresi ile zaten bir hesap mevcut!";
                    return View();
                }

                // Varsay�lan de�erler
                if (string.IsNullOrEmpty(writer.WriterTitle))
                    writer.WriterTitle = "Yazar";
                
                if (string.IsNullOrEmpty(writer.WriterImage))
                    writer.WriterImage = "/AdminLTE-3.0.4/dist/img/user2-160x160.jpg";

                if (string.IsNullOrEmpty(writer.WriterAbout))
                    writer.WriterAbout = "Hen�z a��klama eklenmemi�.";

                // Kaydet
                _writerManager.WriterAdd(writer);
                
                TempData["SuccessMessage"] = "Kay�t ba�ar�l�! �imdi giri� yapabilirsiniz.";
                TempData["RegisteredEmail"] = writer.WriterMail;
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                ViewBag.ErrorMessage = "Kay�t s�ras�nda bir hata olu�tu!";
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