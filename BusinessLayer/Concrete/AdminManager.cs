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
    public class AdminManager : IAdminService
    {
        private readonly IAdminDal _adminDal;

        public AdminManager(IAdminDal adminDal)
        {
            _adminDal = adminDal;
        }

        public Admin GetAdmin(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            return _adminDal.Get(x => x.AdminUserName == username && x.AdminPassword == hashedPassword);
        }

        public void UpdateAdmin(Admin admin)
        {
            _adminDal.Update(admin);
        }

        public bool ValidateAdmin(string username, string password)
        {
            var admin = _adminDal.Get(x => x.AdminUserName == username);
            if(admin == null) return false;
            return VerifyPassword(password, admin.AdminPassword);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
