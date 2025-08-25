using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface IWriterService
    {
        List<Writer> GetAll();
        void WriterAdd(Writer writer);
        void WriterDelete(Writer writer);
        void WriterUpdate(Writer writer);
        Writer GetById(int id);
        
        // Authentication metodları
        bool ValidateWriter(string email, string password);
        Writer GetWriter(string email, string password);
        Writer GetWriterByEmail(string email);
    }
}
