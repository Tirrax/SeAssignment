using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using Common;
using Repository;
using SEWebiste_OwenAttard.Models;

namespace SEWebiste_OwenAttard.Controllers
{
    public class ProductsController : Controller
    {
        //
        // GET: /Products/
        public ActionResult ProductsDisplay(int ID)
        {
            ProductBL BL = new ProductBL();

            List<Product> ProductsList = BL.GetProductsByCat(ID).ToList();

            ProductDisplayModel prodModel = new ProductDisplayModel();

            foreach (Product product in ProductsList)
            {
                ProductModels curModel = new ProductModels
                {
                    Desc = product.Features,
                    ImgPath = "/ProductImages/" + product.Image,
                    ID = product.ProductID,
                    Name = product.Name,
                    price = product.Price
                }; 

                prodModel.productModels.Add(curModel);
            }

            prodModel.CatName = new CategoriesBL().GetCategoryName(ID);

            return View(prodModel);
        }

        public ActionResult ProductDetail(int ID)
        {
            ProductBL BL = new ProductBL();

            Product Products = BL.GetProductByID(ID);

            SingleProductDisplayModel prodModel = new SingleProductDisplayModel();

            ProductModels curModel = new ProductModels
            {
                Desc = Products.Features,
                ImgPath = "/ProductImages/" + Products.Image,
                ID = Products.ProductID,
                Name = Products.Name,
                price = Products.Price,
                Qty = Products.Qty
            };

            prodModel.productModels = curModel;

            prodModel.CatId = Products.CategoryID;
            prodModel.CatName = new CategoriesBL().GetCategoryName(Products.CategoryID);

            return View(prodModel);
        }



        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetCart()
        {
            if (!User.Identity.IsAuthenticated)
                return null;

            ShoppingCartBL ShopServ = new ShoppingCartBL();
            int count = ShopServ.GetShoppingCartItemCount(User.Identity.Name);
            return Json(new { result = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult AddToCart(int ProductID, int Qty)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new { error = "You are not logged in." }, JsonRequestBehavior.AllowGet);

                ProductBL productserv = new ProductBL();
                Product prod = productserv.GetProductByID(ProductID);
                if (prod.Qty < Qty)
                    return Json(new { error = "There is not enough stock" }, JsonRequestBehavior.AllowGet);

                ShoppingCartBL ShopServ = new ShoppingCartBL();

                ShoppingCart cart = new ShoppingCart
                {
                    Username = User.Identity.Name,
                    Qty = Qty,
                    ProductID = ProductID,
                    Total = prod.Price*Qty
                };

                ShopServ.AddShoppingCart(cart);
                return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { error = "There was an error whil adding to cart" }, JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize]
        public ActionResult ShoppingCart()
        {
            ShoppingCartBL ShopServ = new ShoppingCartBL();

            ShoppingcartModel model = new ShoppingcartModel
            {
                ShoppinCartItems = ShopServ.GetShoppingCartItems(User.Identity.Name)
            };

            return View(model);
        }



    }
}
