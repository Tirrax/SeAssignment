using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using Common;
using Microsoft.AspNet.SignalR;
using Repository;
using SEWebiste_OwenAttard.Models;

namespace SEWebiste_OwenAttard.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ProductsController : Controller
    {

        public IEnumerable<System.Web.Mvc.SelectListItem> GetCategories()
        {
            List<Category> mainCategories = new List<Category>();
            List<Category> subCategories = new List<Category>();
            new CategoriesBL().GetAllCategories(ref mainCategories, ref subCategories);

            var Categories = subCategories.Select(x => new System.Web.Mvc.SelectListItem
            {
                Value = x.CategoryID.ToString(),
                Text = x.Name.ToString()
            }).OrderBy(x => x.Text);

            return new System.Web.Mvc.SelectList(Categories, "Value", "Text");
        }

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
            int count = 0;

            if (User.Identity.IsAuthenticated)
            {
                ShoppingCartBL ShopServ = new ShoppingCartBL();
                count = ShopServ.GetShoppingCartItemCount(User.Identity.Name);
            }

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


        [System.Web.Mvc.Authorize]
        public ActionResult ShoppingCart()
        {
            ShoppingCartBL ShopServ = new ShoppingCartBL();

            ShoppingcartModel model = new ShoppingcartModel
            {
                ShoppinCartItems = ShopServ.GetShoppingCartItems(User.Identity.Name)                
            };

            model.total = model.ShoppinCartItems.Sum(x => x.Qty*x.Product.Price);

            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult RemoveAll()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new { error = "You are not logged in." }, JsonRequestBehavior.AllowGet);

                ShoppingCartBL ShopServ = new ShoppingCartBL();
                List<ShoppingCart> items = ShopServ.GetShoppingCartItems(User.Identity.Name).ToList();

                foreach (ShoppingCart item in items)
                {
                    ShopServ.RemoveItem(User.Identity.Name, item.ProductID);
                }

                return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "There was an error while Editing Quantity" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult RemoveItem(int ProductID)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new { error = "You are not logged in." }, JsonRequestBehavior.AllowGet);

                ShoppingCartBL ShopServ = new ShoppingCartBL();
                ShopServ.RemoveItem(User.Identity.Name, ProductID);

                List<ShoppingCart> items = ShopServ.GetShoppingCartItems(User.Identity.Name).ToList();
                decimal tmpTotal = items.Sum(item => item.Qty*item.Product.Price);

                return Json(new { result = "Success", count = ShopServ.GetShoppingCartItems(User.Identity.Name).Count(), Total = tmpTotal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "There was an error while Editing Quantity" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult UpdateQuantity(int ProductID, int Qty)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new { error = "You are not logged in." }, JsonRequestBehavior.AllowGet);

                ShoppingCartBL ShopServ = new ShoppingCartBL();
                ProductBL productserv = new ProductBL();
                ShoppingCart cart = ShopServ.GetItem(User.Identity.Name, ProductID);

                if (Qty == cart.Qty)
                    return Json(new { result = "NoChange" }, JsonRequestBehavior.AllowGet);

                ShopServ.EditQuantity(User.Identity.Name, ProductID, Qty);

                List<ShoppingCart> items = ShopServ.GetShoppingCartItems(User.Identity.Name).ToList();

                decimal tmpTotal = items.Sum(item => item.Qty*item.Product.Price);

                return Json(new { result = "Success", Total = productserv.GetProductByID(ProductID).Price * Qty, TotalAll = tmpTotal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "There was an error while Editing Quantity" }, JsonRequestBehavior.AllowGet);
            }
        }


        
        public ActionResult AddProduct()
        {
            var model = new ProductModels()
            {
                Categories = GetCategories(),
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddProduct(ProductModels model, HttpPostedFileBase file)
        {
            bool Added = false;
            try
            {
                if (file != null)
                {
                    Guid g = Guid.NewGuid();
                    string GuidString = Convert.ToBase64String(g.ToByteArray());
                    GuidString = GuidString.Replace("=", "");
                    GuidString = GuidString.Replace("+", "");
                    GuidString = GuidString.Replace("/", "");
                    GuidString = GuidString.Replace("\\", "");
                    GuidString = GuidString.Replace(".", "");
                    string extension = Path.GetExtension(file.FileName);

                    string pic = GuidString + extension;
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/ProductImages"), pic);
                    // file is uploaded
                    file.SaveAs(path);

                    ProductBL productserv = new ProductBL();
                    Product prod = new Product()
                    {
                        Name = model.Name,
                        CategoryID = model.SelectedCatID,
                        Features = model.Desc,
                        Qty = model.Qty,
                        Price = model.price,
                        Image = pic,
                        Seller = User.Identity.Name,
                        DateListed = DateTime.Now,
                        HandleDeliveries = model.HandleDeliveries
                    };

                    Added = productserv.AddProduct(prod);
                    
                }
            }
            catch (Exception)
            {


            }
            finally
            {
                if (!Added)
                {
                    ModelState.AddModelError("", "Could not be added due to internal exception");
                    TempData["Success"] = false;
                }
                else
                {
                    TempData["Success"] = true;
                }

            }
            model.Categories = GetCategories();
            return View(model);
        }

        public ActionResult ManageProducts()
        {
            string username = User.Identity.Name;
            List<ProductModels> modelList = new ProductBL().GetProductsBySeller(username).Select(
                    cur => new ProductModels()
                    {
                        Desc = cur.Features,
                        ImgPath = cur.Image,
                        ID = cur.ProductID,
                        Name = cur.Name,
                        Qty = cur.Qty,
                        price = cur.Price,
                        HandleDeliveries = cur.HandleDeliveries
                    }
                ).ToList();

            return View(modelList);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DeleteProducts(int ProdID)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new { error = "You are not logged in." }, JsonRequestBehavior.AllowGet);

                ProductBL prodBL = new ProductBL();
                Product prod = prodBL.GetProductByID(ProdID);

                bool Success = prodBL.DeleteProduct(ProdID);
                
                if (Success)
                {
                    string path = System.IO.Path.Combine(
                        Server.MapPath("~/ProductImages"), prod.Image);

                    System.IO.File.Delete(path);

                }
                return Json(new { ret = Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ret = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult UpdateProduct(int ProdID, string ProductName, int Qty, decimal Price, string Desc, bool HandleDeliveries)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(new { error = "You are not logged in." }, JsonRequestBehavior.AllowGet);

                ProductBL prodBL = new ProductBL();

                Product prod =  prodBL.GetProductByID(ProdID);

                prod.Name = ProductName;
                prod.Qty = Qty;
                prod.Price = Price;
                prod.Features = Desc;
                prod.HandleDeliveries = HandleDeliveries;
                prodBL.UpdateProduct(prod);

                return Json(new { ret = true}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ret = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
