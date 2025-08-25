using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class GallleryController : Controller
    {
        ImageFileManager imageFileManager = new ImageFileManager(new EFImageFileDal());


        public ActionResult Index()
        {
            var files = imageFileManager.GetList();
            return View(files);
        }
    }
}