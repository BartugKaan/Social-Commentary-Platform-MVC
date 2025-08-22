using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class HeadingManager : IHeadingService
    {
        IHeadingDal _headingDal;

        public HeadingManager(IHeadingDal headingDal)
        {
            _headingDal = headingDal;
        }

        public void AddHeading(Heading heading)
        {
            _headingDal.Insert(heading);
        }

        public void ChangeStatus(Heading heading)
        {
            bool currentStatus = heading.HeadingStatus;
            heading.HeadingStatus = !currentStatus;
            _headingDal.Update(heading);
        }

        public List<Heading> GetAll()
        {
            return _headingDal.List();
        }

        public Heading GetById(int id)
        {
            return _headingDal.Get(x => x.HeadingId == id);
        }

        public void UpdateHeading(Heading heading)
        {
            _headingDal.Update(heading);
        }
    }
}
