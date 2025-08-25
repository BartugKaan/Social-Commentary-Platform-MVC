using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcProjeKampi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Admin kullanıcısının olup olmadığını kontrol et
            EnsureAdminExists();
        }

        private void EnsureAdminExists()
        {
            try
            {
                var adminManager = new AdminManager(new EFAdminDal());
                
                // Herhangi bir admin var mı kontrol et
                using (var context = new DataAccessLayer.Concrete.Context())
                {
                    var adminExists = context.Admins.Any();
                    if (!adminExists)
                    {
                        // Test admin kullanıcısı oluştur
                        var testAdmin = new Admin
                        {
                            AdminUserName = "admin",
                            AdminPassword = adminManager.HashPassword("123456"), // BCrypt ile hashlenecek
                            AdminRole = "Admin"
                        };

                        context.Admins.Add(testAdmin);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // Loglama yapmak istersen burada yapabilirsin
                System.Diagnostics.Debug.WriteLine("Admin oluşturma hatası: " + ex.Message);
            }
        }
    }
}
