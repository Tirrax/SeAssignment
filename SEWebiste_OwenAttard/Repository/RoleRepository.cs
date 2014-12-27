using System;
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

    }
}
