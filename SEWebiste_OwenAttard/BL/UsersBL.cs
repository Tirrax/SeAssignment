using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Common;
using DataAccess;

namespace BL
{
    public class UsersBL
    {

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="user">The user details</param>
        /// <returns>Creation status</returns>
        public MembershipCreateStatus AddNewUser(User user)
        {
            UserRepository userRepo = new UserRepository();
            RoleRepository roles = new RoleRepository(userRepo.entity);
            if (userRepo.DoesUsernameExist(user.Username))
                return MembershipCreateStatus.DuplicateUserName;

            if (userRepo.DoesEmailExist(user.Email))
                return MembershipCreateStatus.DuplicateEmail;

            user.Roles.Add(roles.GetRole(2));
            userRepo.AddNewUser(user);
            return MembershipCreateStatus.Success;
        }

        /// <summary>
        /// Check if username exists
        /// </summary>
        /// <param name="Username">the username to check</param>
        /// <returns>true it exists</returns>
        public bool DoesUsernameExist(string Username)
        {
            UserRepository userRepo = new UserRepository();
            return userRepo.DoesUsernameExist(Username);
        }

        /// <summary>
        /// authenticate the user credetials
        /// </summary>
        /// <param name="Username">the username</param>
        /// <param name="Password">the password</param>
        /// <param name="code">the generated code</param>
        /// <returns>login response value</returns>
        public int AuthenitcateUser(string Username, string Password)
        {
            UserRepository userRepo = new UserRepository();

            if (!DoesUsernameExist(Username))
            {
                return (int)Common.GlobalDefines.LoginResponses.INVALIDUSERPASS;
            }

            bool ret = userRepo.AuthenitcateUser(Username, Password);

            if (!ret)
                return (int)Common.GlobalDefines.LoginResponses.INVALIDUSERPASS;
            

            return (int)Common.GlobalDefines.LoginResponses.SUCCESS;
        }

        /// <summary>
        /// Gets user details by username
        /// </summary>
        /// <param name="Username">the username</param>
        /// <returns>the user</returns>
        public User GetUserByUsername(string Username)
        {
            UserRepository userRepo = new UserRepository();
            return userRepo.GetUserByUsername(Username);
        }

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <returns>alist of countries</returns>
        public IEnumerable<Country> GetCountries()
        {
            return UserRepository.GetCountries();
        }
    }
}
