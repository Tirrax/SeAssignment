using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DataAccess
{
    public class Country
    {
        public string CountryId { get; set; }
        public string Title { get; set; }
    }

    public class UserRepository  : Connection
    {
        public UserRepository()
            : base()
        {

        }

        public UserRepository(DBTradersMarketEntities entity)
            : base(entity)
        {

        }

        public void AddNewUser(User user)
        {
            entity.Users.AddObject(user);
            entity.SaveChanges();
        }

        public bool DoesUsernameExist(string Username)
        {
            
            return entity.Users.Count(x => x.Username == Username) == 1;
           
        }

        public bool DoesEmailExist(string Email)
        {

            return entity.Users.Count(x => x.Email == Email) == 1;

        }

        public bool AuthenitcateUser(string Username, string Password)
        {
            return entity.Users.Count(x => x.Username == Username && x.Password == Password) == 1;
        }

        public User GetUserByUsername(string Username)
        {
            return entity.Users.SingleOrDefault(x => x.Username == Username);
        }

        public IEnumerable<User> GetAllUsersExcept(string username)
        {
            return entity.Users.Where(x => x.Username != username);
        }

        public static IEnumerable<Country> GetCountries()
        {
            return from ri in
                       from ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                       select new RegionInfo(ci.LCID)
                   group ri by ri.TwoLetterISORegionName into g
                   //where g.Key.Length == 2
                   select new Country
                   {
                       CountryId = g.Key,
                       Title = g.First().DisplayName
                   };
        }

        public bool IsUserAdmin(string username)
        {
            var singleOrDefault = entity.Users.SingleOrDefault(x => x.Username == username);

            return singleOrDefault != null && singleOrDefault.Roles.Any(x => x.RoleName == "Admin");
        }
    }
}
