using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations.Design;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using Common;
using Microsoft.Ajax.Utilities;
using SEWebiste_OwenAttard.Models;

namespace SEWebiste_OwenAttard.Controllers
{
    public class PayPalController : Controller
    {
        public ActionResult RedirectFromPaypal()
        {
            return View();
        }

        public ActionResult NotifyFromPaypal()
        {
            return View();
        }
        public ActionResult ValidateCommand(string product, string totalPrice)
        {
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);
            var paypal = new PayPalModel(useSandbox);

            List<ShoppingCart> cart = new ShoppingCartBL().GetShoppingCartItems(User.Identity.Name).ToList();

            foreach (var curCart in cart)
            {
                ItemModel model = new ItemModel()
                {
                    quantity = curCart.Qty,
                    item_name = curCart.Product.Name,
                    amount = curCart.Product.Price
                };

                paypal.items.Add(model);
            }


            return View(paypal);
        }

    }
}
