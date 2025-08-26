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
    [WriterAuthorization]
    public class WriterPanelController : Controller
    {
        private readonly HeadingManager _headingManager;
        private readonly WriterManager _writerManager;
        private readonly ContentManager _contentManager;
        private readonly CategoryManager _categoryManager;
        private readonly MessageManager _messageManager;
        
        public WriterPanelController()
        {
            _headingManager = new HeadingManager(new EFHeadingDal());
            _writerManager = new WriterManager(new EFWriterDal());
            _contentManager = new ContentManager(new EFContentDal());
            _categoryManager = new CategoryManager(new EFCategoryDal());
            _messageManager = new MessageManager(new EFMessageDal(), new EFWriterDal());
        }
        
        public ActionResult WriterProfile()
        {
            string mail = (string)Session["WriterMail"];
            int writerId = (int)Session["WriterId"];
            
            ViewBag.mail = mail;
            
            var headingCount = _headingManager.GetAllByWriter(writerId).Count();
            var contentCount = _contentManager.GetAllByWriter(writerId).Count();
            
            ViewBag.HeadingCount = headingCount;
            ViewBag.ContentCount = contentCount;
            
            var writerProfile = _writerManager.GetWriterByEmail(mail);
            return View(writerProfile);
        }

        public ActionResult WriterHeading()
        {
            int writerId = (int)Session["WriterId"];
            var values = _headingManager.GetAllByWriter(writerId);
            return View(values);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            string mail = (string)Session["WriterMail"];
            var writer = _writerManager.GetWriterByEmail(mail);
            
            if (writer == null)
            {
                return RedirectToAction("WriterProfile");
            }
            
            return View(writer);
        }

        [HttpPost]
        public ActionResult EditProfile(Writer writer)
        {
            try
            {
                int currentWriterId = (int)Session["WriterId"];
                writer.WriterId = currentWriterId;
                
                WriterValiator validationRules = new WriterValiator();
                ValidationResult validationResults = validationRules.Validate(writer);

                if (!validationResults.IsValid)
                {
                    foreach (var error in validationResults.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return View(writer);
                }

                _writerManager.WriterUpdate(writer);
                
                Session["WriterName"] = writer.WriterName + " " + writer.WriterSurname;
                
                TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi!";
                return RedirectToAction("WriterProfile");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Profil güncellenirken bir hata oluştu!";
                return View(writer);
            }
        }

        public ActionResult MyContents()
        {
            int writerId = (int)Session["WriterId"];
            var contents = _contentManager.GetAllByWriter(writerId);
            return View(contents);
        }

        [HttpGet]
        public ActionResult AddHeading()
        {
            PopulateCategoryDropDown();
            return View();
        }

        [HttpPost]
        public ActionResult AddHeading(Heading heading)
        {
            try
            {
                int writerId = (int)Session["WriterId"];
                
                heading.WriterId = writerId;
                heading.HeadingDate = DateTime.Now;
                heading.HeadingStatus = true;

                _headingManager.AddHeading(heading);
                
                TempData["SuccessMessage"] = "Başlık başarıyla eklendi!";
                return RedirectToAction("WriterHeading");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Başlık eklenirken bir hata oluştu!";
                PopulateCategoryDropDown();
                return View(heading);
            }
        }

        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            var heading = _headingManager.GetById(id);
            int currentWriterId = (int)Session["WriterId"];
            
            if (heading == null || heading.WriterId != currentWriterId)
            {
                TempData["ErrorMessage"] = "Başlık bulunamadı veya bu başlığı düzenleme yetkiniz yok!";
                return RedirectToAction("WriterHeading");
            }
            
            PopulateCategoryDropDown();
            return View(heading);
        }

        [HttpPost]
        public ActionResult EditHeading(Heading heading)
        {
            try
            {
                int currentWriterId = (int)Session["WriterId"];
                var existingHeading = _headingManager.GetById(heading.HeadingId);
                
                if (existingHeading == null || existingHeading.WriterId != currentWriterId)
                {
                    TempData["ErrorMessage"] = "Bu başlığı düzenleme yetkiniz yok!";
                    return RedirectToAction("WriterHeading");
                }

                heading.WriterId = currentWriterId;
                heading.HeadingDate = existingHeading.HeadingDate;
                
                _headingManager.UpdateHeading(heading);
                
                TempData["SuccessMessage"] = "Başlık başarıyla güncellendi!";
                return RedirectToAction("WriterHeading");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Başlık güncellenirken bir hata oluştu!";
                PopulateCategoryDropDown();
                return View(heading);
            }
        }

        public ActionResult DeleteHeading(int id)
        {
            try
            {
                var heading = _headingManager.GetById(id);
                int currentWriterId = (int)Session["WriterId"];
                
                if (heading == null || heading.WriterId != currentWriterId)
                {
                    TempData["ErrorMessage"] = "Başlık bulunamadı veya bu başlığı silme yetkiniz yok!";
                    return RedirectToAction("WriterHeading");
                }

                heading.HeadingStatus = !heading.HeadingStatus;
                _headingManager.UpdateHeading(heading);
                
                string statusText = heading.HeadingStatus ? "aktif" : "pasif";
                TempData["SuccessMessage"] = $"Başlık başarıyla {statusText} hale getirildi!";
                
                return RedirectToAction("WriterHeading");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İşlem sırasında bir hata oluştu!";
                return RedirectToAction("WriterHeading");
            }
        }

        public ActionResult ContentByHeading(int id)
        {
            var heading = _headingManager.GetById(id);
            int currentWriterId = (int)Session["WriterId"];
            
            if (heading == null || heading.WriterId != currentWriterId)
            {
                TempData["ErrorMessage"] = "Başlık bulunamadı veya bu başlığın içeriklerini görme yetkiniz yok!";
                return RedirectToAction("WriterHeading");
            }
            
            var contents = _contentManager.GetListById(id);
            ViewBag.HeadingName = heading.HeadingName;
            ViewBag.HeadingId = id;
            
            return View(contents);
        }

        #region Messages

        public ActionResult Messages()
        {
            string writerEmail = (string)Session["WriterMail"];
            var inbox = _messageManager.GetWriterInbox(writerEmail);
            var sendbox = _messageManager.GetWriterSendbox(writerEmail);
            
            ViewBag.InboxCount = inbox.Count;
            ViewBag.SendboxCount = sendbox.Count;
            
            return View(inbox);
        }

        public ActionResult Inbox()
        {
            string writerEmail = (string)Session["WriterMail"];
            var messages = _messageManager.GetWriterInbox(writerEmail);
            return View(messages);
        }

        public ActionResult Sendbox()
        {
            try
            {
                string writerEmail = (string)Session["WriterMail"];
                
                if (string.IsNullOrEmpty(writerEmail))
                {
                    TempData["ErrorMessage"] = "Session hatası! Lütfen tekrar giriş yapınız.";
                    return RedirectToAction("Index", "WriterLogin");
                }
                
                var messages = _messageManager.GetWriterSendbox(writerEmail);
                return View(messages);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Gönderilen mesajlar yüklenirken hata oluştu: " + ex.Message;
                return View(new List<Message>());
            }
        }

        [HttpGet]
        public ActionResult NewMessage()
        {
            try
            {
                // Session kontrolü
                if (Session["WriterMail"] == null || Session["WriterId"] == null)
                {
                    TempData["ErrorMessage"] = "Session süresi dolmuş. Lütfen tekrar giriş yapınız.";
                    return RedirectToAction("Index", "WriterLogin");
                }
                
                PopulateWritersDropDown();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Sayfa yüklenirken hata oluştu: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult NewMessage(Message message)
        {
            try
            {
                string senderEmail = (string)Session["WriterMail"];
                
                if (string.IsNullOrEmpty(message.ReceiverMail) || 
                    string.IsNullOrEmpty(message.Subject) || 
                    string.IsNullOrEmpty(message.MessageContent))
                {
                    ViewBag.ErrorMessage = "Lütfen tüm alanları doldurunuz!";
                    PopulateWritersDropDown();
                    return View(message);
                }

                message.SenderMail = senderEmail;
                message.MessageDate = DateTime.Now;
                
                _messageManager.AddMessage(message);
                
                TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi!";
                return RedirectToAction("Sendbox");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Mesaj gönderilirken bir hata oluştu: " + ex.Message;
                if (ex.InnerException != null)
                {
                    ViewBag.ErrorMessage += " Inner: " + ex.InnerException.Message;
                }
                PopulateWritersDropDown();
                return View(message);
            }
        }

        public ActionResult GetMessageDetails(int id)
        {
            var message = _messageManager.GetById(id);
            string currentWriterEmail = (string)Session["WriterMail"];
            
            if (message == null || 
                (message.SenderMail != currentWriterEmail && message.ReceiverMail != currentWriterEmail))
            {
                TempData["ErrorMessage"] = "Mesaj bulunamadı veya bu mesajı görme yetkiniz yok!";
                return RedirectToAction("Messages");
            }
            
            return View(message);
        }

        #endregion

        #region Content Management

        [HttpGet]
        public ActionResult AddContent(int? headingId)
        {
            if (!headingId.HasValue)
            {
                TempData["ErrorMessage"] = "Geçersiz başlık ID'si!";
                return RedirectToAction("WriterHeading");
            }

            var heading = _headingManager.GetById(headingId.Value);
            int currentWriterId = (int)Session["WriterId"];
            
            if (heading == null || heading.WriterId != currentWriterId)
            {
                TempData["ErrorMessage"] = "Başlık bulunamadı veya bu başlık için içerik ekleme yetkiniz yok!";
                return RedirectToAction("WriterHeading");
            }
            
            ViewBag.HeadingId = headingId.Value;
            ViewBag.HeadingName = heading.HeadingName;
            
            return View();
        }

        [HttpPost]
        public ActionResult AddContent(Content content)
        {
            try
            {
                int currentWriterId = (int)Session["WriterId"];
                
                var heading = _headingManager.GetById(content.HeadingId);
                if (heading == null || heading.WriterId != currentWriterId)
                {
                    TempData["ErrorMessage"] = "Bu başlık için içerik ekleme yetkiniz yok!";
                    return RedirectToAction("WriterHeading");
                }

                ContentValidator validationRules = new ContentValidator();
                ValidationResult validationResults = validationRules.Validate(content);

                if (!validationResults.IsValid)
                {
                    foreach (var error in validationResults.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    ViewBag.HeadingId = content.HeadingId;
                    ViewBag.HeadingName = heading.HeadingName;
                    return View(content);
                }

                content.WriterId = currentWriterId;
                content.ContentDate = DateTime.Now;
                content.ContentStatus = true;

                _contentManager.AddContent(content);
                
                TempData["SuccessMessage"] = "İçerik başarıyla eklendi!";
                return RedirectToAction("ContentByHeading", new { id = content.HeadingId });
            }
            catch (Exception ex)
            {
                var heading = _headingManager.GetById(content.HeadingId);
                ViewBag.ErrorMessage = "İçerik eklenirken bir hata oluştu: " + ex.Message;
                if (ex.InnerException != null)
                {
                    ViewBag.ErrorMessage += " Inner: " + ex.InnerException.Message;
                }
                ViewBag.HeadingId = content.HeadingId;
                ViewBag.HeadingName = heading?.HeadingName ?? "";
                return View(content);
            }
        }

        [HttpGet]
        public ActionResult EditContent(int id)
        {
            var content = _contentManager.GetById(id);
            int currentWriterId = (int)Session["WriterId"];
            
            if (content == null || content.WriterId != currentWriterId)
            {
                TempData["ErrorMessage"] = "İçerik bulunamadı veya bu içeriği düzenleme yetkiniz yok!";
                return RedirectToAction("WriterHeading");
            }
            
            ViewBag.HeadingName = content.Heading.HeadingName;
            
            return View(content);
        }

        [HttpPost]
        public ActionResult EditContent(Content content)
        {
            try
            {
                int currentWriterId = (int)Session["WriterId"];
                var existingContent = _contentManager.GetById(content.ContentId);
                
                if (existingContent == null || existingContent.WriterId != currentWriterId)
                {
                    TempData["ErrorMessage"] = "Bu içeriği düzenleme yetkiniz yok!";
                    return RedirectToAction("WriterHeading");
                }

                ContentValidator validationRules = new ContentValidator();
                ValidationResult validationResults = validationRules.Validate(content);

                if (!validationResults.IsValid)
                {
                    foreach (var error in validationResults.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    ViewBag.HeadingName = existingContent.Heading.HeadingName;
                    return View(content);
                }

                content.WriterId = currentWriterId;
                content.ContentDate = existingContent.ContentDate; // Keep original date
                content.HeadingId = existingContent.HeadingId; // Ensure heading doesn't change
                
                _contentManager.UpdateContent(content);
                
                TempData["SuccessMessage"] = "İçerik başarıyla güncellendi!";
                return RedirectToAction("ContentByHeading", new { id = content.HeadingId });
            }
            catch (Exception ex)
            {
                var existingContent = _contentManager.GetById(content.ContentId);
                ViewBag.ErrorMessage = "İçerik güncellenirken bir hata oluştu!";
                ViewBag.HeadingName = existingContent?.Heading?.HeadingName ?? "";
                return View(content);
            }
        }

        public ActionResult DeleteContent(int id)
        {
            try
            {
                var content = _contentManager.GetById(id);
                int currentWriterId = (int)Session["WriterId"];
                
                if (content == null || content.WriterId != currentWriterId)
                {
                    TempData["ErrorMessage"] = "İçerik bulunamadı veya bu içeriği silme yetkiniz yok!";
                    return RedirectToAction("WriterHeading");
                }

                // Toggle status instead of deleting
                content.ContentStatus = !content.ContentStatus;
                _contentManager.UpdateContent(content);
                
                string statusText = content.ContentStatus ? "aktif" : "pasif";
                TempData["SuccessMessage"] = $"İçerik başarıyla {statusText} hale getirildi!";
                
                return RedirectToAction("ContentByHeading", new { id = content.HeadingId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İşlem sırasında bir hata oluştu!";
                return RedirectToAction("WriterHeading");
            }
        }

        #endregion

        #region Private Helpers

        private void PopulateCategoryDropDown()
        {
            var categories = _categoryManager.GetAll().Where(c => c.CategoryStatus).ToList();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");
        }

        private void PopulateWritersDropDown()
        {
            try
            {
                var writers = _messageManager.GetWritersForMessaging();
                string currentWriterEmail = (string)Session["WriterMail"];
                
                if (writers == null || writers.Count == 0)
                {
                    ViewBag.ReceiverMail = new SelectList(new List<SelectListItem>(), "Value", "Text");
                    ViewBag.ErrorMessage = "Mesaj gönderebileceğiniz aktif yazar bulunamadı!";
                    return;
                }
                
                var writerList = writers.Where(w => w.WriterMail != currentWriterEmail)
                                       .Select(w => new SelectListItem
                                       {
                                           Value = w.WriterMail,
                                           Text = $"{w.WriterName} {w.WriterSurname} ({w.WriterMail})"
                                       }).ToList();
                
                if (writerList.Count == 0)
                {
                    ViewBag.ReceiverMail = new SelectList(new List<SelectListItem>(), "Value", "Text");
                    ViewBag.ErrorMessage = "Mesaj gönderebileceğiniz başka yazar bulunamadı!";
                    return;
                }
                
                ViewBag.ReceiverMail = new SelectList(writerList, "Value", "Text");
            }
            catch (Exception ex)
            {
                ViewBag.ReceiverMail = new SelectList(new List<SelectListItem>(), "Value", "Text");
                ViewBag.ErrorMessage = "Yazarlar yüklenirken bir hata oluştu!";
            }
        }

        #endregion
    }
}