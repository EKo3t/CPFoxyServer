using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using TemplateAuth.Models.Vehicle;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateAuth.Models
{
    public class ApplicationUserLogin : IdentityUserLogin<string> { }
    public class ApplicationUserClaim : IdentityUserClaim<string> { }
    public class ApplicationUserRole : IdentityUserRole<string> { }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin,
        ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }       
        // Add any custom Role properties/code here
    }


    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.Initialize(force: true);
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<UserInfo> UserInfoes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<CarMark> CarMarks { get; set; }
        public DbSet<UserToInfo> UserToInfoes { get; set; }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole,
            ApplicationUserClaim>, IUserStore<ApplicationUser, string>, IDisposable
    {
        public ApplicationUserStore()
            : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }


    public class ApplicationRoleStore : RoleStore<ApplicationRole, string, ApplicationUserRole>, IQueryableRoleStore<ApplicationRole, string>,
            IRoleStore<ApplicationRole, string>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }
}