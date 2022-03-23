using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkladIdentity.Models;

namespace SkladIdentity.EF
{
    public class EFIdentityContext:IdentityDbContext<User>
    {
        public EFIdentityContext(DbContextOptions<EFIdentityContext> options) : base(options)
        {
            Database.EnsureCreated();

        }
    }
}