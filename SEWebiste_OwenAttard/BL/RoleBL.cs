using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;

namespace BL
{
    public class RoleBL
    {
        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>Role.</returns>
        public Role GetRole(int roleId)
        {
            return new RoleRepository().GetRole(roleId);
        }

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>list of roles.</returns>
        public IQueryable<Role> GetUserRoles(string username)
        {
            return new RoleRepository().GetUserRoles(username);

        }

        /// <summary>
        /// Checks if user is in role
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="RoleName">the Role ID</param>
        /// <returns>true if user in role</returns>
        public bool IsUserInRole(string username, string RoleName)
        {
            return new RoleRepository().IsUserInRole(username, RoleName);

        }
    }
}
