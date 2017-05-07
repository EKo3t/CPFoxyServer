using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TemplateAuth.Models;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Data.Entity;
using System.Web;

namespace TemplateAuth
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser, string>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, string> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            //var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            var manager = new ApplicationUserManager(
                            new UserStore<ApplicationUser, ApplicationRole, string,
                                ApplicationUserLogin, ApplicationUserRole,
                                ApplicationUserClaim>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser, string>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var roleStore = new ApplicationRoleStore(db);
            var roleManager = new ApplicationRoleManager(roleStore);
            var userStore = new ApplicationUserStore(db);
            var userManager = new ApplicationUserManager(userStore);
            InitializeUser("admin@example.com", "Admin_1", "Admin", userManager, roleManager);
            InitializeUser("ekostukevich@gmail.com", "Ekost_1", "User", userManager, roleManager);
            InitializeUser("cococo@gmail.com", "Cococo_1", "User", userManager, roleManager);
            InitializeUser("bebebe@gmail.com", "Bebebe_1", "User", userManager, roleManager);
            InitializeUser("ololo@gmail.com", "Manager_1", "Manager", userManager, roleManager);
            //Create Role Admin if it does not exist
        }

        private static void InitializeUser(string name, string password, string roleName, 
            ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            ApplicationRole role = null;
            if (roleManager != null)
            {
                role = roleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new ApplicationRole(roleName);
                    var roleresult = roleManager.Create(role);
                }
            }

            var user = userManager == null ? null : userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (role != null && !rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }

    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new ApplicationRoleStore(context.Get<ApplicationDbContext>()));
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}
