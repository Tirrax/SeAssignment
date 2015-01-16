using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BL;
using Common;
using SEWebiste_OwenAttard.Models;

namespace SEWebiste_OwenAttard.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        //
        public ActionResult ManageUsers()
        {
            UsersBL user = new UsersBL();

            if (!user.IsUserAdmin(User.Identity.Name))
                return RedirectToAction("Index", "Home");

            List<User> listUser = user.GetAllUsersExcept(User.Identity.Name).ToList();

            List<GeneralDetails> model = listUser.Select(curUser => new GeneralDetails()
            {
                Email = curUser.Email,
                Username = curUser.Username,
                IsAdmin = curUser.Roles.Any(x => x.RoleName == "Admin"),
                FristName = curUser.FirstName,
                LastName = curUser.LastName,
                MobileNumber = curUser.MobileNumber,
                Town = curUser.Town,
                address = curUser.Address

            }).ToList();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangeRole(string Username, int ID)
        {
            var serializer = new JavaScriptSerializer();
            try
            {

                bool  sucess  = new RoleBL().ChangeRole(Username, ID);
                var resultData = new { result = "Success", ret = sucess };
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


        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdateRole(string name, string OldName)
        {
            var serializer = new JavaScriptSerializer();
            try
            {

                bool sucess = new RoleBL().UpdateRoleByName(OldName, name) == 1;
                var resultData = new { result = "Success", ret = sucess };
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

        [HttpPost]
        [AllowAnonymous]
        public ActionResult DeleteRole(string name)
        {
            var serializer = new JavaScriptSerializer();
            try
            {

                int ret = new RoleBL().DeleteRoleByName(name);

                bool sucess = ret == 1;

                var resultData = new { result = "Success", ret = sucess };
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

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddRole(string name)
        {
            var serializer = new JavaScriptSerializer();
            try
            {
                int sucess = new RoleBL().AddRole(name);
                var resultData = new { result = "Success", ret = sucess };
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

        public ActionResult ManageRoles()
        {

            UsersBL user = new UsersBL();
            RoleBL role = new RoleBL();
            List<RolesDisp> model = role.GetAllRoles().Select(cur => new RolesDisp()
            {
                Name = cur.RoleName,
                RoleID = cur.RoleID
            }).ToList();

            return View(model);
        }

        public ActionResult PendingOrders()
        {
            var transactions =
                new TransactionBL().GetPendingTransactions().ToList();

            List<OrderModels> model = transactions.Select(curTrans => new OrderModels()
            {
                OrderId = curTrans.TransactionID,
                Date = curTrans.Date,
                ItemCount = transactions.Sum(x => x.TransactionDetails.Sum(y => y.Qty)),
                Total = transactions.Sum(x => x.TransactionDetails.Sum(y => y.Qty * y.Price))
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ApprovePendingOrder(int orderID)
        {
            var serializer = new JavaScriptSerializer();
            try
            {

                bool sucess = new TransactionBL().ApprovePament(orderID);
                var resultData = new { result = "Success", ret = sucess };
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

        public ActionResult ManageUserRoles()
        {

            UsersBL user = new UsersBL();
            RoleBL role = new RoleBL();

            if (!user.IsUserAdmin(User.Identity.Name))
                return RedirectToAction("Index", "Home");

            List<User> listUser = user.GetAllUsersExcept(User.Identity.Name).ToList();

            List<Role> Roles = role.GetAllRoles().ToList();

            RoleGeneral model = new RoleGeneral
            {
                RolesString = new List<string>(),
                RolesID = new List<int>(),
                Users = new List<UserRolesDisp>()
            };

            for (int i = 0; i < listUser.Count; i++)
            {
                UserRolesDisp tmp = new UserRolesDisp
                {
                    username = listUser[i].Username, 
                    RoleID = -1
                };

                if (listUser[i].Roles.Count != 0)
                    tmp.RoleID = listUser[i].Roles.ElementAt(0).RoleID;
            }

            for (int i = 0; i < Roles.Count; i++)
            {
                model.RolesString.Add(Roles[i].RoleName);
                model.RolesID.Add(Roles[i].RoleID);
            }


            return View(model);
        }

    }
}
