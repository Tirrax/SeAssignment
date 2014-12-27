using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BL;
using Common;
using SEWebiste_OwenAttard.Models;

namespace SEWebiste_OwenAttard.Controllers
{
    public class MenuController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public ContentResult GetMenus()
        {
            var serializer = new JavaScriptSerializer();
            try
            {

                MenuBL Serv = new MenuBL();
                RoleBL servRoles = new RoleBL();
                List<Menu> menus = new List<Menu>();
                if (User.Identity.IsAuthenticated)
                {
                    if (servRoles.IsUserInRole(User.Identity.Name, "Admin"))
                    {
                        menus = Serv.GetMenusbyRole(1).ToList();
                    }
                    else
                    {
                        menus = Serv.GetMenusbyRole(2).ToList();
                    }
                }
                else
                {
                    menus = Serv.GetMenusbyRole(3).ToList();
                }

                List<MenuModel> ModelMenus = ConvertToMenuModel(menus);

                serializer.MaxJsonLength = Int32.MaxValue;

                var resultData = new { result = "Success", ret = ModelMenus };
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
                    Content = serializer.Serialize(new { error = "There was an error while adding to cart" }),
                    ContentType = "application/json"
                };
            }

        }

        public List<MenuModel> ConvertToMenuModel(List<Menu> menus)
        {
            List<MenuModel> model = new List<MenuModel>();

            foreach (Menu menu in menus)
            {
                MenuModel currModel = new MenuModel();
                currModel.ActionName = menu.ActionName;
                currModel.ControllerName = menu.ControllerName;
                currModel.MenuName = menu.Name;
                currModel.MenuID = menu.HTMLID;
                model.Add(currModel);
            }

            return model;
        }

    }
}
