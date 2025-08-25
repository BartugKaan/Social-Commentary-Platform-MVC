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
    public class AdminContentController : Controller
    {
        // GET: AdminContent

        ContentManager manager = new ContentManager(new EFContentDal());

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ContentByHeading(int id)
        {
            var contentValues = manager.GetListById(id);
            return View(contentValues);
        }
    }
}