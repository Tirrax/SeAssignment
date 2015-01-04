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
        public ActionResult ManageAdmins()
        {
            UsersBL user = new UsersBL();

            if (!user.IsUserAdmin(User.Identity.Name))
                return RedirectToAction("Index", "Home");

            List<User> listUser = user.GetAllUsersExcept(User.Identity.Name).ToList();

            List<GeneralDetails> model = listUser.Select(curUser => new GeneralDetails()
            {
                Email = curUser.Email,
                Username = curUser.Username,
                IsAdmin = curUser.Roles.Any(x => x.RoleName == "Admin")

            }).ToList();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult MakeAdmins(string Username, bool IsAdmin)
        {
            var serializer = new JavaScriptSerializer();
            try
            {

                bool  sucess  = new RoleBL().MakeAdmin(Username, IsAdmin);
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
    }
}
