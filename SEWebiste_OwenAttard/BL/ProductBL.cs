using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;

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

        public IEnumerable<Product> GetProductsBySeller(string Name)
        {
            return new ProductsRepository().GetProductsBySeller(Name);
        }

        public bool UpdateProduct(Product NewProd)
        {
            return new ProductsRepository().UpdateProduct(NewProd);
        }

        public bool DeleteProduct(int ProdID)
        {
            return new ProductsRepository().DeleteProduct(ProdID);
        }

        public bool AddProduct(Product AddProduct)
        {
            return new ProductsRepository().AddProduct(AddProduct);
        }

        public IEnumerable<Product> GetLatestProducts()
        {
            return new ProductsRepository().GetLatestProducts();
        }

    }
}
