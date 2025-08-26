using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;

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
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return null;
                }

                var admin = _adminDal.Get(x => x.AdminUserName == username);
                
                if (admin == null)
                {
                    return null;
                }

                if (VerifyPassword(password, admin.AdminPassword))
                {
                    return admin;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void UpdateAdmin(Admin admin)
        {
            if (admin != null)
            {
                if (!string.IsNullOrEmpty(admin.AdminPassword) && 
                    !admin.AdminPassword.StartsWith("$2a$"))
                {
                    admin.AdminPassword = HashPassword(admin.AdminPassword);
                }
                
                _adminDal.Update(admin);
            }
        }

        public bool ValidateAdmin(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return false;
                }

                var admin = _adminDal.Get(x => x.AdminUserName == username);
                
                if (admin == null)
                {
                    return false;
                }

                if (admin.AdminPassword.StartsWith("$2a$"))
                {
                    return VerifyPassword(password, admin.AdminPassword);
                }
                else
                {
                    return admin.AdminPassword == password;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty");
            }
            
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                {
                    return false;
                }

                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void HashExistingPasswords()
        {
            try
            {
                var admins = _adminDal.List();
                
                foreach (var admin in admins)
                {
                    if (!string.IsNullOrEmpty(admin.AdminPassword) && 
                        !admin.AdminPassword.StartsWith("$2a$"))
                    {
                        admin.AdminPassword = HashPassword(admin.AdminPassword);
                        _adminDal.Update(admin);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error hashing existing passwords: " + ex.Message);
            }
        }
    }
}
