using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;
        private readonly IWriterDal _writerDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public MessageManager(IMessageDal messageDal, IWriterDal writerDal)
        {
            _messageDal = messageDal;
            _writerDal = writerDal;
        }

        public void AddMessage(Message message)
        {
            if (message.MessageDate == DateTime.MinValue)
            {
                message.MessageDate = DateTime.Now;
            }
            _messageDal.Insert(message);
        }

        public void DeleteMessage(Message message)
        {
            _messageDal.Delete(message);
        }

        public List<Message> GetListInbox()
        {
            return _messageDal.List(x => x.ReceiverMail == "admin@gmail.com");
        }

        public Message GetById(int id)
        {
            return _messageDal.Get(x => x.MessageId == id);
        }

        public List<Message> GetListSendbox()
        {
            return _messageDal.List(x => x.SenderMail == "admin@gmail.com");
        }

        public void UpdateMessage(Message message)
        {
            _messageDal.Update(message);
        }

        public List<Message> GetWriterInbox(string writerEmail)
        {
            if (string.IsNullOrEmpty(writerEmail))
                return new List<Message>();

            return _messageDal.List(x => x.ReceiverMail == writerEmail)
                             .OrderByDescending(x => x.MessageDate)
                             .ToList();
        }

        public List<Message> GetWriterSendbox(string writerEmail)
        {
            if (string.IsNullOrEmpty(writerEmail))
                return new List<Message>();

            return _messageDal.List(x => x.SenderMail == writerEmail)
                             .OrderByDescending(x => x.MessageDate)
                             .ToList();
        }

        public List<Writer> GetWritersForMessaging()
        {
            if (_writerDal == null)
                return new List<Writer>();

            return _writerDal.List(x => x.WriterStatus == true)
                            .OrderBy(x => x.WriterName)
                            .ThenBy(x => x.WriterSurname)
                            .ToList();
        }
    }
}
