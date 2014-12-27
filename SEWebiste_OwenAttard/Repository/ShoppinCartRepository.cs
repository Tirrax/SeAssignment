using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;


namespace DataAccess
{
    public class ShoppinCartRepository : Connection
    {
        public ShoppinCartRepository()
            : base()
        {

        }

        public ShoppinCartRepository(DBTradersMarketEntities entity)
            : base(entity)
        {

        }


        public int GetShoppingCartItemCount(string Username)
        {
            try
            {
                return entity.ShoppingCarts.Where(x => x.Username == Username).Sum(x => x.Qty);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public ShoppingCart GetItem(string Username, int ProductID)
        {
            return entity.ShoppingCarts.SingleOrDefault(x => x.Username == Username && x.ProductID == ProductID);
        }

        public void RemoveItem(string Username, int ProductID)
        {
            ShoppingCart shop = GetItem(Username, ProductID);
            entity.ShoppingCarts.DeleteObject(shop);
            entity.SaveChanges();
        }

        public void AddShoppingCart(ShoppingCart cart)
        {

            entity.ShoppingCarts.AddObject(cart);
            entity.SaveChanges();

        }

        public void EditQuantity(string Username, int ProductID, int Qty)
        {
            ShoppingCart shop = GetItem(Username, ProductID);
            shop.Qty = Qty;
            entity.SaveChanges();
        }

        public IEnumerable<ShoppingCart> GetShoppingCartItems(string Username)
        {
            return entity.ShoppingCarts.Where(u => u.Username == Username);
        }

    }
}
