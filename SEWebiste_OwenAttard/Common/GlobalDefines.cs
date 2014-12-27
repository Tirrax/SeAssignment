using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class GlobalDefines
    {
        public enum LoginResponses { SUCCESS = 0, INVALIDUSERPASS = 1, BLOCKED = 2 }
        public enum CheckOut { ALLCHECKOUT = 0, ERROR = -1, NOTENOUGHSTOCK = 1, NOITEMWASADDED = 2 }
    }
}
