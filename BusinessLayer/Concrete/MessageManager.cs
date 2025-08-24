using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {
        IMessageDal _messageDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public void AddMessage(Message message)
        {
            _messageDal.Insert(message);
        }

        public void DeleteMessage(Message message)
        {
            _messageDal.Delete(message);
        }

        public List<Message> GetListInbox()
        {
            return _messageDal.List(x=> x.ReceiverMail=="admin@gmail.com");
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
    }
}
