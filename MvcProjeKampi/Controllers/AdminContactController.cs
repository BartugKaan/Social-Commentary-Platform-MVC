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
    [AdminAuthorization]
    public class AdminContactController : Controller
    {

        ContactManager manager = new ContactManager(new EFContactDal());
        // GET: AdminContact
        public ActionResult Index()
        {
            var contactValues = manager.GetAll();
            return View(contactValues);
        }

        public ActionResult GetContactDetails(int id)
        {
            var contactValue = manager.GetById(id);
            return View(contactValue);
        }

        public PartialViewResult MessageListMenu()
        {
            return PartialView();
        }
    }
}