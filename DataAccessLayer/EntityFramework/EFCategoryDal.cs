using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;

namespace DataAccessLayer.EntityFramework
{
    public class EFCategoryDal: GenericRepository<Category>, ICategoryDal
    {

    }
}
