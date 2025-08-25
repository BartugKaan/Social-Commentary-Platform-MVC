using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface IContentService
    {
        List<Content> GetAll();
        List<Content> GetListById(int id);
        List<Content> GetAllByWriter(int writerId);
        void AddContent(Content content);
        Content GetById(int id);
        void DeleteContent(Content content);
        void UpdateContent(Content content);
    }
}
