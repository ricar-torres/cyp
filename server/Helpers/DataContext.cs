using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApi.Entities.Identity;

namespace WebApi.Helpers
{
    //public class DataContext : IdentityDbContext<User, Role, int>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        #region TABLES

        public DbSet<Application> Application { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<AppRole> AppRole { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; }
        public DbSet<MenuPermission> MenuPermission { get; set; }
        public DbSet<LoginProvider> LoginProvider { get; set; }
        public DbSet<OneTimePassword> OneTimePassword { get; set; }

        #endregion

        #region FUNCTIONS

        //[DbFunction("IsValidIdentity", "dbo")]
        //public static int? IsValidIdentity(string firstName, string ssno, string type)
        //{
        //    throw new NotImplementedException();
        //}

        [DbFunction("IsValidIdentity")]
        public static int IsValidIdentity(string firstName, string ssno, string type) => throw new Exception();

        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
            .HasDbFunction(typeof(DataContext).GetMethod(nameof(IsValidIdentity)))
            .HasTranslation(
                args =>
                    SqlFunctionExpression.Create("IsValidIdentity", args, typeof(int), null));

            base.OnModelCreating(modelBuilder);

            new ApplicationMap(modelBuilder.Entity<Application>());
            new UserMap(modelBuilder.Entity<AppUser>());
            new RoleMap(modelBuilder.Entity<AppRole>());
            new MenuPermissionMap(modelBuilder.Entity<MenuPermission>());
            new UserRoleMap(modelBuilder.Entity<UserRole>());
            new MenuItemMap(modelBuilder.Entity<MenuItem>());
            new RoleMenuMap(modelBuilder.Entity<RoleMenu>());
            new PermissionMap(modelBuilder.Entity<Permission>());
            new LoginProviderMap(modelBuilder.Entity<LoginProvider>());
            new OneTimePasswordMap(modelBuilder.Entity<OneTimePassword>());
            
            //modelBuilder
            //    .HasDbFunction(typeof(DataContext).GetMethod(nameof(DataContext.IsValidIdentity)))
            //    .HasTranslation(args =>
            //    SqlFunctionExpression.Create("IsValidIdentity", args, typeof(int?), null));

        }

    }
}
