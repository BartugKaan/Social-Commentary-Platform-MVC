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
    public class WriterManager : IWriterService
    {
        IWriterDal _writerDal;

        public WriterManager(IWriterDal writerDal)
        {
            _writerDal = writerDal;
        }

        public List<Writer> GetAll()
        {
            return _writerDal.List();
        }

        public Writer GetById(int id)
        {
            return _writerDal.Get(x => x.WriterId == id);
        }

        public void WriterAdd(Writer writer)
        {
            try
            {
                // Şifreyi hashle
                writer.WriterPassword = HashPassword(writer.WriterPassword);
                writer.WriterStatus = true; // Yeni yazarları aktif yap
                _writerDal.Insert(writer);
            }
            catch (Exception ex)
            {
                throw new Exception("Yazar eklenirken hata oluştu: " + ex.Message);
            }
        }

        public void WriterDelete(Writer writer)
        {
            _writerDal.Delete(writer);
        }

        public void WriterUpdate(Writer writer)
        {
            // Eğer şifre değiştirilmişse hash'le
            if (!string.IsNullOrEmpty(writer.WriterPassword) && !writer.WriterPassword.StartsWith("$2a$"))
            {
                writer.WriterPassword = HashPassword(writer.WriterPassword);
            }
            _writerDal.Update(writer);
        }

        public Writer GetWriter(string email, string password)
        {
            try
            {
                var writer = _writerDal.Get(x => x.WriterMail == email && x.WriterStatus == true);
                if (writer != null && VerifyPassword(password, writer.WriterPassword))
                {
                    return writer;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool ValidateWriter(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return false;

                var writer = _writerDal.Get(x => x.WriterMail == email && x.WriterStatus == true);
                if (writer == null) 
                    return false;

                // Şifre kontrolü
                return VerifyPassword(password, writer.WriterPassword);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Writer GetWriterByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return null;
                    
                return _writerDal.Get(x => x.WriterMail == email && x.WriterStatus == true);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string HashPassword(string password)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }
            catch (Exception)
            {
                // BCrypt hatası durumunda basit hash kullan (geliştirme için)
                return password; // Geçici - güvenlik için üretimde kaldırılmalı
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Hash'lenmiş şifre kontrolü
                if (hashedPassword.StartsWith("$2a$"))
                {
                    return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                }
                else
                {
                    // Hash'lenmemiş şifre (eski kayıtlar için)
                    return password == hashedPassword;
                }
            }
            catch (Exception)
            {
                // Hata durumunda basit karşılaştırma
                return password == hashedPassword;
            }
        }
    }
}
