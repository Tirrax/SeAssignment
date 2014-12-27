using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;

namespace BL
{
    public class MenuBL
    {

        public IEnumerable<Menu> GetMenusbyRole(int Role)
        {
            return new MenuRepository().GetMenusbyRole(Role);
        }
    }
}
