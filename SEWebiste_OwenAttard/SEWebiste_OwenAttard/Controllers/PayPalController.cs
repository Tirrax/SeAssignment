using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

        public ActionResult ValidateCommand()
        {
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);
            var paypal = new PayPalModel(useSandbox);

            TransactionBL checkoutBL = new TransactionBL();
            List<TransactionDetail> ItemsAdded = new List<TransactionDetail>();
            int OrderID = 0;
            checkoutBL.Checkout(User.Identity.Name, ref ItemsAdded, ref OrderID);


            if (ItemsAdded.Count <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var curCart in ItemsAdded)
            {
                ItemModel model = new ItemModel()
                {
                    quantity = curCart.Qty,
                    item_name = curCart.Product.Name,
                    amount = curCart.Price
                };

                paypal.items.Add(model);
            }


            return View(paypal);
        }


        [AllowAnonymous]
        public ActionResult IPN()
        {

            // IPN code when hosted

            /*
            // Receive IPN request from PayPal and parse all the variables returned
            var formVals = new Dictionary<string, string>();
            formVals.Add("cmd", "_notify-validate");

            // if you want to use the PayPal sandbox change this from false to true
            string response = GetPayPalResponse(formVals, true);

            if (response == "VERIFIED")
            {
                string transactionID = Request["txn_id"];
                string sAmountPaid = Request["mc_gross"];
                string deviceID = Request["custom"];

                //validate the order
                Decimal amountPaid = 0;
                Decimal.TryParse(sAmountPaid, out amountPaid);

                if (sAmountPaid == "2.95")
                {
                    // take the information returned and store this into a subscription table
                    // this is where you would update your database with the details of the tran

                    return RedirectToAction("Index", "Home"); ;

                }
            }
            */
            return RedirectToAction("Index", "Home"); ;
        }

        private string GetPayPalResponse(Dictionary<string, string> formVals, bool useSandbox)
        {

            /*// Parse the variables
            // Choose whether to use sandbox or live environment
            string paypalUrl = useSandbox
                ? "https://www.sandbox.paypal.com/cgi-bin/webscr"
                : "https://www.paypal.com/cgi-bin/webscr";

            HttpWebRequest req = (HttpWebRequest) WebRequest.Create(paypalUrl);

            // Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            byte[] param = Request.BinaryRead(Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);

            StringBuilder sb = new StringBuilder();
            sb.Append(strRequest);

            foreach (string key in formVals.Keys)
            {
                sb.AppendFormat("&{0}={1}", key, formVals[key]);
            }
            strRequest += sb.ToString();
            req.ContentLength = strRequest.Length;

            string response = "";
            using (StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII))
            {

                streamOut.Write(strRequest);
                streamOut.Close();
                using (StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    response = streamIn.ReadToEnd();
                }
            }
            
            return response;*/
            return null;
        }
    }
}
