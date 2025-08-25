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
            // Önce username ile admin'i bulalım
            var admin = _adminDal.Get(x => x.AdminUserName == username);
            if (admin != null && VerifyPassword(password, admin.AdminPassword))
            {
                return admin;
            }
            return null;
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

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                // Eğer hash verification başarısız olursa (eski plain text şifreler için)
                // Plain text karşılaştırması yapalım
                return password == hashedPassword;
            }
        }
    }
}
