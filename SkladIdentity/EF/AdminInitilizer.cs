using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SkladIdentity.Models;

namespace SkladIdentity.EF
{
    public class AdminInitilizer
    {
        public static async Task InitilizeAsync(UserManager<User> userManager)
        {
            string adminName = "Admin";
            string password = "Fb4a6a22_a";
            
            string localName = "Local";
            string localPassword = "1234567";
            
            if (await userManager.FindByNameAsync(adminName) == null)
            {
                var res = await userManager.CreateAsync(new User() { UserName = adminName }, password);
            }
            
            if (await userManager.FindByNameAsync(localName) == null)
            {
                var res = await userManager.CreateAsync(new User() { UserName = localName }, localPassword);
            }
        }
    }
}

