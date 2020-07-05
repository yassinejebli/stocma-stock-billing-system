using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Models
{
    // Vous pouvez ajouter des données de profil pour l'utilisateur en ajoutant plus de propriétés à votre classe ApplicationUser ; consultez http://go.microsoft.com/fwlink/?LinkID=317594 pour en savoir davantage.
    public class ApplicationUser : IdentityUser
    {
        //public virtual ICollection<BonLivraison> BonLivraisons { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Notez qu'authenticationType doit correspondre à l'élément défini dans CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ajouter les revendications personnalisées de l’utilisateur ici
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUser> GetApplicationUsers()
        {
            return (DbSet<ApplicationUser>)Users;
        }

        public static ApplicationDbContext Create()
        {
            var context = new ApplicationDbContext();

            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            // RoleTypes is a class containing constant string values for different roles
            List<IdentityRole> identityRoles = new List<IdentityRole>();
            identityRoles.Add(new IdentityRole() {  Name = "Admin" });

            foreach (IdentityRole role in identityRoles)
            {
                if (manager.FindByName("Admin") == null)
                    manager.Create(role);
            }

            // Initialize default user
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            if (userManager.FindById("00000000-0000-0000-0000-000000000000") == null)
            {
                ApplicationUser admin = new ApplicationUser();
                admin.Id = "00000000-0000-0000-0000-000000000000";
                admin.Email = "admin";
                admin.UserName = "admin";
                userManager.Create(admin, "comciel123");
                userManager.AddToRole(admin.Id, "Admin");
            }

            return context;
        }
    }
}