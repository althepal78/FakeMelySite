using Fake.Models;
using Fake.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Fake.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public void Initialize()
        {
            //implement migrations if none are applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {}

            if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User_Indi)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User_Comp)).GetAwaiter().GetResult();

                //if roles ar not create, then create an admin user
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "papichulo@test.com",
                    Email = "papichulo@test.com",
                    Name = "papi chulo",
                    PhoneNumber = "9787999999",
                    StreetAddress = "my stree434",
                    State = "FL",
                    PostalCode = "33614",
                    City = "Tampa"
                }, "adminUSER78$$").GetAwaiter().GetResult();

              

                //   site name for now https://melysundergroundcusine.azurewebsites.net/
             

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "albertcardonaii@gmail.com");
                _userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();

                user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "mister01610@yahoo.com");
                _userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();

                user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "melysundergroundcuisine@gmail.com");
                _userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();
                

            }
            return;
        }
    }
}
