using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
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
        HeadingManager headingManager = new HeadingManager(new EFHeadingDal());
        WriterManager writerManager = new WriterManager(new EFWriterDal());
        ContentManager contentManager = new ContentManager(new EFContentDal());
        
        public ActionResult WriterProfile()
        {
            string mail = (string)Session["WriterMail"];
            int writerId = (int)Session["WriterId"];
            
            ViewBag.mail = mail;
            
            // İstatistikler için veriler
            var headingCount = headingManager.GetAllByWriter(writerId).Count();
            var contentCount = contentManager.GetAllByWriter(writerId).Count();
            
            ViewBag.HeadingCount = headingCount;
            ViewBag.ContentCount = contentCount;
            
            var writerProfile = writerManager.GetWriterByEmail(mail);
            return View(writerProfile);
        }

        public ActionResult WriterHeading()
        {
            int writerId = (int)Session["WriterId"];
            var values = headingManager.GetAllByWriter(writerId);
            return View(values);
        }
    }
}