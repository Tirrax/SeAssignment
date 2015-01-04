using System;
using System.Collections.Generic;
using System.IO;
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

        public IEnumerable<Product> GetProductsBySeller(string Name)
        {
            return entity.Products.Where(x => x.Seller.ToLower().Contains(Name.ToLower()));
        }

        public bool UpdateProduct(Product NewProd)
        {
            Product old = GetProductByID(NewProd.ProductID);
            entity.Products.Attach(old);
            entity.Products.ApplyCurrentValues(NewProd);
            int ret = entity.SaveChanges();

            return ret >= 1;
        }

        public bool DeleteProduct(int ProdID)
        {
            Product old = GetProductByID(ProdID);

            if (new TransactionRepository().ProductBought(ProdID))
            {
                old.Deleted = true;
                return UpdateProduct(old);
            }

            
            entity.Products.DeleteObject(old);
            int ret = entity.SaveChanges();

            return ret >= 1;

        }

        public bool AddProduct(Product AddProduct)
        {
            entity.Products.AddObject(AddProduct);
            int ret = entity.SaveChanges();

            return ret >= 1;
        }

        public IEnumerable<Product> GetLatestProducts()
        {
            return entity.Products.OrderByDescending(x => x.DateListed).Take(6);
        }
    }
}
