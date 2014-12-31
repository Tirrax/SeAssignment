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
    public class OrdersController : Controller
    {
        //
        // GET: /Orders/

        public ActionResult OrderHistory()
        {

            var transactions =
                new TransactionBL().GetTransactionBetweenDates(User.Identity.Name, DateTime.Now,
                    DateTime.Now.AddYears(-1)).ToList();

            List<OrderModels> model = transactions.Select(curTrans => new OrderModels()
            {
                OrderId = curTrans.TransactionID,
                Date = curTrans.Date,
                ItemCount = transactions.Sum(x => x.TransactionDetails.Sum(y => y.Qty)),
                Total = transactions.Sum(x => x.TransactionDetails.Sum(y => y.Qty*y.Price))
            }).ToList();

            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public ContentResult GetOrderDetail(int ID)
        {
            var serializer = new JavaScriptSerializer();
            try
            {

                var transactions = new TransactionBL().GetTransactionDetails(ID).ToList();

                List<OrderDetailsModels> model = transactions.Select(curTrans => new OrderDetailsModels()
                {
                    Price = curTrans.Price,
                    Qty = curTrans.Qty,
                    Total = curTrans.Price * curTrans.Qty,
                    ProductName = curTrans.Product.Name,
                    Seller = curTrans.Product.Seller

                }).ToList();


                serializer.MaxJsonLength = Int32.MaxValue;

                var resultData = new { result = "Success", ret = model };
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
