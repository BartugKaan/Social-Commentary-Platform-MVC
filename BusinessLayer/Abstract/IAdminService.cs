using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IAdminService
    {
        Admin GetAdmin(string username, string password);
        bool ValidateAdmin(string username, string password);
        void UpdateAdmin(Admin admin);
        string HashPassword(string password);
    }
}
