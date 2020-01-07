using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Proiect_FlickR.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<SelectListItem> AllRoles { get; internal set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet <Category> Categories { get; set; }
        public DbSet <Picture> Pictures { get; set; }
        // public DbSet<ApplicationUser> Users { get; set; } 
        public IEnumerable<SelectListItem> AllRoles { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Proiect_FlickR.Models.Album> Albums { get; set; }

        //public System.Data.Entity.DbSet<Proiect_FlickR.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<Proiect_FlickR.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<Proiect_FlickR.Models.Picture> Pictures { get; set; }
    }
}