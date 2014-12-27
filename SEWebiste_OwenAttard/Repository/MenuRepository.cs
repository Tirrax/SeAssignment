using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DataAccess
{
    public class MenuRepository : Connection
    {
        public MenuRepository()
            : base()
        {

        }

        public MenuRepository(DBTradersMarketEntities entity)
            : base(entity)
        {

        }

        public IEnumerable<Menu> GetMenusbyRole(int Role)
        {
            Role r = new RoleRepository().GetRole(Role);
            return r.Menus;
        }

    }
}
