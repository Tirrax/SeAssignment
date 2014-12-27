using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;

namespace BL
{
    public class ShoppingCartBL
    {
        /// <summary>
        /// Shopping cart item count
        /// </summary>
        /// <param name="Username">the username</param>
        /// <returns>no of items</returns>
        public int GetShoppingCartItemCount(string Username)
        {
            return new ShoppinCartRepository().GetShoppingCartItemCount(Username);
        }


        /// <summary>
        /// Gets an item by ID
        /// </summary>
        /// <param name="Username">the username</param>
        /// <param name="ProductID">the product id</param>
        /// <returns>the product</returns>
        public Common.ShoppingCart GetItem(string Username, int ProductID)
        {
            return new ShoppinCartRepository().GetItem(Username, ProductID);
        }


        /// <summary>
        /// Remove Item from cart
        /// </summary>
        /// <param name="Username">the username</param>
        /// <param name="ProductID">the product id</param>
        public void RemoveItem(string Username, int ProductID)
        {
            new ShoppinCartRepository().RemoveItem(Username, ProductID);
        }

        /// <summary>
        /// Adds item to shopping cart
        /// </summary>
        /// <param name="cart">the cart item</param>
        public void AddShoppingCart(Common.ShoppingCart cart)
        {
            ShoppingCart tmpcart = GetItem(cart.Username, cart.ProductID);
            if (tmpcart == null)
            {
                new ShoppinCartRepository().AddShoppingCart(cart);
            }
            else
            {
                EditQuantity(cart.Username, cart.ProductID, cart.Qty + tmpcart.Qty);
            }
        }

        /// <summary>
        /// Edits the quantity
        /// </summary>
        /// <param name="Username">the username</param>
        /// <param name="ProductID">the product id</param>
        /// <param name="Qty">to Quantity to increase</param>
        public void EditQuantity(string Username, int ProductID, int Qty)
        {
            new ShoppinCartRepository().EditQuantity(Username, ProductID, Qty);
        }

        /// <summary>
        /// Get all items in the shopping cart
        /// </summary>
        /// <param name="Username">The username</param>
        /// <returns>list of shopping cart items</returns>
        public IEnumerable<Common.ShoppingCart> GetShoppingCartItems(string Username)
        {
            return new ShoppinCartRepository().GetShoppingCartItems(Username);
        }
    }
}
