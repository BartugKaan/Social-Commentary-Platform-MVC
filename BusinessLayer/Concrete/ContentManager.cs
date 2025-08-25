using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ContentManager : IContentService
    {
        IContentDal _contentDal;

        public ContentManager(IContentDal contentDal)
        {
            _contentDal = contentDal;
        }

        public void AddContent(Content content)
        {
            _contentDal.Insert(content);
        }

        public void DeleteContent(Content content)
        {
            _contentDal.Delete(content);
        }

        public List<Content> GetAll()
        {
            return _contentDal.List();
        }

        public Content GetById(int id)
        {
            return _contentDal.Get(x => x.ContentId == id);
        }

        public List<Content> GetListById(int id)
        {
            return _contentDal.List(x => x.HeadingId == id);
        }

        public List<Content> GetAllByWriter(int writerId)
        {
            return _contentDal.List(x => x.WriterId == writerId);
        }

        public void UpdateContent(Content content)
        {
            _contentDal.Update(content);
        }
    }
}
