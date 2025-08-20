using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public void AddCategory(Category category)
        {
            _categoryDal.Insert(category);
        }

        public void CategoryDelete(Category category)
        {
            _categoryDal.Delete(category);
        }

        public void CategoryUpdate(Category category)
        {

            _categoryDal.Update(category);
        }

        public List<Category> GetAll()
        {
            return _categoryDal.List();
        }

        public Category GetById(int id)
        {
            return _categoryDal.Get(x => x.CategoryId == id);
        }

    }
}
