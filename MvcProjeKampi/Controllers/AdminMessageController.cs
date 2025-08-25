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
    [AdminAuthorization]
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

        [HttpPost]
        public ActionResult NewMessage(Message message)
        {
            MessageValidator messageValidator = new MessageValidator();
            ValidationResult result = messageValidator.Validate(message);

            if (result.IsValid)
            {
                message.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                manager.AddMessage(message);
                return RedirectToAction("Sendbox");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }

        public ActionResult GetInboxMessageDetails(int id)
        {
            var values = manager.GetById(id);
            return View(values);
        }
    }
}