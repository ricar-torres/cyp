using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities.Identity;
using WebApi.Helpers;

namespace WebApi.Services
{

    public interface IMenuService
    {
        ICollection<MenuItem> GetMenuByUser(AppUser user,System.Func<MenuPermission, bool> filterFunc = null);
        ICollection<MenuItem> GetAllByUser(AppUser user);
        ICollection<MenuItem> GetViewableMenuItems(AppUser user);
        ICollection<MenuItem> GetCreateMenuItems(AppUser user);
        ICollection<MenuItem> GetDeleteMenuItems(AppUser user);
        ICollection<MenuItem> GetUpdateMenuItems(AppUser user);
        ICollection<MenuItem> GetUploadMenuItems(AppUser user);
        ICollection<MenuItem> GetPublishMenuItems(AppUser user);

        MenuItem Create(MenuItem menu);

    }

    public class MenuService : IMenuService
    {
        private DataContext _context;

        public MenuService(DataContext context)
        {
            _context = context;
        }

        public ICollection<MenuItem> GetMenuByUser(AppUser user,
        System.Func<MenuPermission, bool> filterFunc = null)
        {
            if (user == null)
            {
                return new Collection<MenuItem>();
            }

            // Get Ids.
            //var roleIds = user.Roles.Select(role => role.RoleId).ToList();
            var roleIds = _context.UserRole.Where(u => u.UserId == user.Id).Select(r => r.RoleId).ToList();

            var items = _context.MenuItem
                            .Include(m => m.Roles)
                                .ThenInclude(roles => roles.Permissions)
                                    .ThenInclude(permissions => permissions.Permission)
                                .Where(e => e.Roles.Any(roleMenu => roleIds.Contains(roleMenu.RoleId)))
                                .ToList();

            //var c = _context.RoleMenu.Include(rm => rm.Permissions).ToList();

            // Enable eager-loading to retrieve our permissions as well.
            //var items = _context.MenuItem
                //.Include(m => m.Roles).ThenInclude(roles => roles.Permissions)
                //.Where(e => e.Roles.Any(roleMenu => roleIds.Contains(roleMenu.RoleId)));

            ICollection<MenuItem> records;
            if (filterFunc == null)
            {
                records = items.Where(e => e.Roles.Any(f => f.Permissions.Any())).ToList();
            }
            else
            {
                records = items.Where(e => e.Roles.Any(f => f.Permissions.Any(filterFunc))).ToList();
            }

            return records;
        }

        public ICollection<MenuItem> GetAllByUser(AppUser user)
        {
            return GetMenuByUser(user);
        }

        public ICollection<MenuItem> GetViewableMenuItems(AppUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "View");
        }

        public ICollection<MenuItem> GetCreateMenuItems(AppUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "Create");
        }

        public ICollection<MenuItem> GetDeleteMenuItems(AppUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "Delete");
        }

        public ICollection<MenuItem> GetUpdateMenuItems(AppUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "Update");
        }

        public ICollection<MenuItem> GetUploadMenuItems(AppUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "Upload");
        }

        public ICollection<MenuItem> GetPublishMenuItems(AppUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "Publish");
        }

        public MenuItem Create(MenuItem menu)
        {

            return new MenuItem();

        }

    }
}
