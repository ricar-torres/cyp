using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;
using server.Entities;
using WebApi.Entities.Identity;
using WebApi.Entities;

namespace WebApi.Helpers {
	public static class DbContextExtension {

		public static bool AllMigrationsApplied(this DbContext context) {
			var applied = context.GetService<IHistoryRepository>()
				.GetAppliedMigrations()
				.Select(m => m.MigrationId);

			var total = context.GetService<IMigrationsAssembly>()
				.Migrations
				.Select(m => m.Key);

			return !total.Except(applied).Any();
		}

		public static void EnsureSeeded(this DataContext context) {

			try {

				context.Database.OpenConnection();

				#region Applications

				if (!context.Application.Any()) {
					var Applications = JsonConvert.DeserializeObject<List<Application>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "application.json"));
					context.AddRange(Applications);
					context.SaveChanges();
				}

				#endregion

				#region LoginProviders

				if (!context.LoginProvider.Any()) {
					var loginProviders = JsonConvert.DeserializeObject<List<LoginProvider>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "login_providers.json"));
					context.AddRange(loginProviders);
					context.SaveChanges();
				}

				#endregion

				#region AppUsers

				if (!context.AppUser.Any()) {
					//var _userService = new UserService(context);
					var users = JsonConvert.DeserializeObject<List<AppUser>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "users.json"));

					for (int i = 0; i < users.Count; i++) {
						byte[] passwordHash, passwordSalt;
						CreatePasswordHash("1234", out passwordHash, out passwordSalt);

						users[i].PasswordHash = passwordHash;
						users[i].PasswordSalt = passwordSalt;
					}

					context.AddRange(users);
					context.SaveChanges();
				}

				#endregion

				#region AppRoles

				var roles = JsonConvert.DeserializeObject<List<AppRole>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "Roles.json"));
				roles = roles.Where(x => !context.AppRole.Any(y => y.Id == x.Id)).ToList();
				context.AddRange(roles);
				_ = context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.AppRole ON");
				context.SaveChanges();
				_ = context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.AppRole OFF");

				#endregion

				#region UserRoles

				if (!context.UserRole.Any()) {
					var userRoles = JsonConvert.DeserializeObject<List<UserRole>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "users_roles.json"));
					context.AddRange(userRoles);
					context.SaveChanges();
				}

				#endregion

				#region Permissions

				var permission = JsonConvert.DeserializeObject<List<Permission>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "permissions.json"));
				permission = permission.Where(x => !context.Permission.Any(y => y.Id == x.Id)).ToList();
				context.AddRange(permission);
				context.SaveChanges();

				#endregion

				#region MenuItems

				var menuItem = JsonConvert.DeserializeObject<List<MenuItem>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "menu_items.json"));
				menuItem = menuItem.Where(x => !context.MenuItem.Any(y => y.Id == x.Id)).ToList();
				context.AddRange(menuItem);
				context.SaveChanges();

				#endregion

				#region RoleMenu

				var roleMenu = JsonConvert.DeserializeObject<List<RoleMenu>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "roles_menu_items.json"));
				roleMenu = roleMenu.Where(x => !context.RoleMenu.Any(y => y.RoleId == x.RoleId && y.MenuItemId == x.MenuItemId)).ToList();
				context.AddRange(roleMenu);
				_ = context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.RoleMenu ON");
				context.SaveChanges();
				_ = context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.RoleMenu OFF");

				#endregion

				#region MenuPermission

				var menuPermission = JsonConvert.DeserializeObject<List<MenuPermission>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "menu_permissions.json"));
				menuPermission = menuPermission.Where(x => !context.MenuPermission.Any(y => y.RoleMenuId == x.RoleMenuId && y.PermissionId == x.PermissionId)).ToList();
				context.AddRange(menuPermission);
				context.SaveChanges();

				#endregion

				#region TypeOfRelationship

				var typeOfRelationships = JsonConvert.DeserializeObject<List<TypeOfRelationship>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "types_relationship.json"));
				typeOfRelationships = typeOfRelationships.Where(x => !context.TypeOfRelationship.Any(y => y.Id == x.Id)).ToList();
				context.AddRange(typeOfRelationships);
				context.SaveChanges();

				#endregion



				#region InsuranceAddOns

				if (!context.InsuranceAddOns.Any())
				{
					var InsuranceAddOns = JsonConvert.DeserializeObject<List<InsuranceAddOns>>(File.ReadAllText("Seeds" + Path.DirectorySeparatorChar + "AddOns.json"));
					context.AddRange(InsuranceAddOns);
					_ = context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.insurance_addOns ON");
					context.SaveChanges();
					_ = context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.insurance_addOns OFF");
				}

				#endregion

			}
			finally {
				context.Database.CloseConnection();
			}

		}

		private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

			using(var hmac = new System.Security.Cryptography.HMACSHA512()) {
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

	}

}