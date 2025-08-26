using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface IMessageService
    {
        List<Message> GetListInbox();
        List<Message> GetListSendbox();
        void AddMessage(Message message);
        Message GetById(int id);
        void DeleteMessage(Message message);
        void UpdateMessage(Message message);
        
        // Writer-specific methods
        List<Message> GetWriterInbox(string writerEmail);
        List<Message> GetWriterSendbox(string writerEmail);
        List<Writer> GetWritersForMessaging();
    }
}
