using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IHeadingService
    {
        List<Heading> GetAll();
        List<Heading> GetAllByWriter(int id);
        void AddHeading(Heading heading);
        Heading GetById(int id);
        void ChangeStatus(Heading heading);
        void UpdateHeading(Heading heading);
    }
}
