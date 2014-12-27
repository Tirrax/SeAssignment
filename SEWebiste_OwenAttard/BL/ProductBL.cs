using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Repository;

namespace BL
{
    public class ProductBL
    {

        public IEnumerable<Product> GetAllProducts()
        {
            return new ProductsRepository().GetAllProducts();
        }

        public Product GetProductByID(int ID)
        {
            return new ProductsRepository().GetProductByID(ID);
        }

        public bool ReduceQuantity(int ID, int Qty)
        {
            return new ProductsRepository().ReduceQuantity(ID, Qty);
        }

        public IEnumerable<Product> GetProductsByCat(int CategoryID)
        {
            return new ProductsRepository().GetProductsByCat(CategoryID);
        }

        public IEnumerable<Product> GetProductsByName(string Name)
        {
            return new ProductsRepository().GetProductsByName(Name);
        }

    }
}
