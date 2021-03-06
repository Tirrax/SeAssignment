﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;

namespace DataAccess
{
    public class RoleRepository : Connection
    {
        public RoleRepository()
            : base()
        {

        }

        public RoleRepository(DBTradersMarketEntities entity)
            : base(entity)
        {

        }


        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>Role.</returns>
        public Role GetRole(int roleId)
        {
            return entity.Roles.SingleOrDefault(r => r.RoleID == roleId);
        }

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>IQueryable{Role}.</returns>
        public IQueryable<Role> GetUserRoles(string username)
        {
            User user = new UserRepository().GetUserByUsername(username);
            return user.Roles.AsQueryable();

        }

        public bool IsUserInRole(string username, string RoleName)
        {
            User user = new UserRepository().GetUserByUsername(username);
            return user.Roles.Any(x => x.RoleName == RoleName);

        }

        public bool ChangeRole(string username, int ID)
        {
            User user = new UserRepository(entity).GetUserByUsername(username);
            Role role = GetRole(ID);

            if (user.Roles.Count != 0)
            {
                user.Roles.Clear();   
            }

            user.Roles.Add(role);
            int ret  = entity.SaveChanges();
            return ret >= 1;
        }

        public int AddRole(Role role)
        {
            entity.Roles.AddObject(role);
            entity.SaveChanges();
            return role.RoleID;
        }

        public Role GetRoleByName(string Name)
        {
            return entity.Roles.SingleOrDefault(x => x.RoleName == Name);
        }

        public int UpdateRole(string oldRole ,string NewRole)
        {
            Role old = GetRoleByName(oldRole);

            old.RoleName = NewRole;
            return entity.SaveChanges();
        }

        public int DeleteRole(string name)
        {
            Role role = GetRoleByName(name);

            if (role == null)
                return -1;

            entity.Roles.DeleteObject(role);
            return entity.SaveChanges();
        }

        public IQueryable<Role> GetAllRoles()
        {
            return entity.Roles;
        }


    }
}
