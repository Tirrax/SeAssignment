using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;

namespace DataAccess
{
    public class ProductsRepository: Connection
    {
        public ProductsRepository()
            : base()
        {

        }

        public ProductsRepository(DBTradersMarketEntities entity)
            : base(entity)
        {

        }

        public IEnumerable<Product> GetAllProducts()
        {
            return entity.Products.AsEnumerable();
        }

        public Product GetProductByID(int ID)
        {
            return entity.Products.SingleOrDefault(x => x.ProductID == ID);
        }



        public bool ReduceQuantity(int ID, int Qty)
        {
            Product prod = GetProductByID(ID);
            if (prod.Qty >= Qty)
            {
                prod.Qty -= Qty;
                entity.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Product> GetProductsByCat(int CategoryID)
        {
            return entity.Products.Where(x => x.CategoryID == CategoryID);   
        }

        public IEnumerable<Product> GetProductsByName(string Name)
        {
            return entity.Products.Where(x => x.Name.ToLower().Contains(Name.ToLower()));
        }
    }
}
