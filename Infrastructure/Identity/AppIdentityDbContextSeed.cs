using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "yusuf",
                    Email = "yusuf@gmail.com",
                    UserName = "yusuf@gmail.com",
                    address = new Address()
                    {
                        FirstName="yusuf",
                        LastName="kamal",
                        Street="10 St",
                        City="Cairo",
                        State="Na",
                        ZipCode="123",
                    }
                };
                await userManager.CreateAsync(user,"Password123!");
            }
        }
    }
}
