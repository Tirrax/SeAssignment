using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BL;
using Common;
using Repository;
using SEWebiste_OwenAttard.Models;

namespace SEWebiste_OwenAttard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            List<ProductModels> model = new ProductBL().GetLatestProducts().Select(cur => new ProductModels()
            {
                Desc = cur.Features,
                Name = cur.Name,
                price = cur.Price,
                ID =  cur.ProductID,
                ImgPath = "/ProductImages/" + cur.Image,
                Datelisted = cur.DateListed

            }).OrderByDescending(x => x.Datelisted).ToList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ContentResult GetCategories()
        {

            var serializer = new JavaScriptSerializer();
            try
            {
                List<Category> MainCat = new List<Category>();
                List<Category> SubCat = new List<Category>();

                new CategoriesRepository().GetAllCategories(ref MainCat, ref SubCat);

                List<CategoryModel> ModelMainCat = ConvertToCatModel(MainCat);
                List<CategoryModel> ModelSubCat = ConvertToCatModel(SubCat);

                serializer.MaxJsonLength = Int32.MaxValue;

                var resultData = new { result = "Success", main = ModelMainCat, Sub = ModelSubCat };
                var result = new ContentResult
                {
                    Content = serializer.Serialize(resultData),
                    ContentType = "application/json"
                };

                return result;

            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    Content = serializer.Serialize(new { error = "There was an error" }),
                    ContentType = "application/json"
                };
            }

        }


        public List<CategoryModel> ConvertToCatModel(List<Category> menus)
        {
            List<CategoryModel> model = new List<CategoryModel>();

            foreach (Category menu in menus)
            {
                CategoryModel currModel = new CategoryModel();
                currModel.ID = menu.CategoryID;
                currModel.Name = menu.Name;
                currModel.ParentID = menu.ParentID;

                model.Add(currModel);
            }

            return model;
        }
    }
}
