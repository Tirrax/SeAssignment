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

        public bool MakeAdmin(string username, bool IsAdmin)
        {
            return new RoleRepository().MakeAdmin(username, IsAdmin);
        }


        public int AddRole(string name)
        {
            if (name == string.Empty)
                throw new ArgumentException("The parameter was empty");

            if (ContainsUnicodeCharacter(name))
                throw new ArgumentException("Parameter has non-Ascii Char");

            Role role = new Role()
            {
                RoleName = name
            };

            return new RoleRepository().AddRole(role);
        }

        public Role GetRoleByName(string name)
        {

            if (name == string.Empty)
                throw new ArgumentException("The parameter was empty");

            if (ContainsUnicodeCharacter(name))
                throw new ArgumentException("Parameter has non-Ascii Char");

            return new RoleRepository().GetRoleByName(name);
        }

        public int UpdateRoleByName(string Oldname, string NewName)
        {
           
            if (Oldname == "" || NewName == "")
                throw new ArgumentException("The parameter was empty");

            if (ContainsUnicodeCharacter(Oldname) || ContainsUnicodeCharacter(NewName))
                throw new ArgumentException("Parameter has non-Ascii Char");

            int ret = new RoleRepository().UpdateRole(Oldname, NewName);

            if (ret == 0)
                return -1;

            return 1;
        }

        public int DeleteRoleByName(string name)
        {
            if (name == string.Empty)
                throw new ArgumentException("The parameter was empty");

            if (ContainsUnicodeCharacter(name))
                throw new ArgumentException("Parameter has non-Ascii Char");

            return new RoleRepository().DeleteRole(name);
        }

        public IQueryable<Role> GetAllRoles()
        {
            return new RoleRepository().GetAllRoles();
        }

        public bool ContainsUnicodeCharacter(string input)
        {
            string sOut = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(input));
            return sOut != input;
        }
    }
}
