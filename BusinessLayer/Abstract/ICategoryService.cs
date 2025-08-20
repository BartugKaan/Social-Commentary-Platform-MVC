using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        void AddCategory(Category category);
        Category GetById(int id);
        void CategoryDelete(Category category);
        void CategoryUpdate(Category category);
    }
}
