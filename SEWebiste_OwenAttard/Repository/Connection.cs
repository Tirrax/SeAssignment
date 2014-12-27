using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Data.Common;

namespace DataAccess
{
    public class Connection
    {
        public  DBTradersMarketEntities entity { get; set; }
        public DbTransaction DbTransaction { get; set; }

        public Connection()
        {
            entity = new DBTradersMarketEntities();
        }

        public Connection(DBTradersMarketEntities entity)
        {
            this.entity = entity;
        }

    }
}
