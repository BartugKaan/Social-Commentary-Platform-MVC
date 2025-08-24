using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class AdminMessageController : Controller
    {
        MessageManager manager = new MessageManager(new EFMessageDal());

        public ActionResult Inbox()
        {
            var messageList = manager.GetListInbox();
            return View(messageList);
        }

        public ActionResult Sendbox()
        {
            var messageList = manager.GetListSendbox();
            return View(messageList);
        }

        [HttpGet]
        public ActionResult NewMessage() { 
            return View();
        }

        public ActionResult GetInboxMessageDetails(int id)
        {
            var values = manager.GetById(id);
            return View(values);
        }

        [HttpPost]
        public ActionResult NewMessage(Message message)
        {

            return View();
        }
    }
}