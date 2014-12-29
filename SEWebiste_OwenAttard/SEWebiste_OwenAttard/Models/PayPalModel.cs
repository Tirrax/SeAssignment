using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SEWebiste_OwenAttard.Models
{
    public class ItemModel
    {
        public string item_name { get; set; }
        public decimal amount { get; set; }
        public int quantity { get; set; }
    }

    public class PayPalModel
    {
        public string cmd { get; set; }
        public string business { get; set; }
        public string no_shipping { get; set; }
        public string @return { get; set; }
        public string cancel_return { get; set; }
        public string notify_url { get; set; }
        public string currency_code { get; set; }
        public List<ItemModel> items = new List<ItemModel>(); 

        public string actionURL { get; set; }

        public PayPalModel(bool useSandBox)
        {
            cmd = "_cart";
            business = ConfigurationManager.AppSettings["business"];
            cancel_return = string.Format("http://{0}/Products/ShoppingCart", HttpContext.Current.Request.Url.Authority);
            @return = string.Format("http://{0}/Products/ShoppingCart", HttpContext.Current.Request.Url.Authority);

            if (useSandBox)
            {
                actionURL = ConfigurationManager.AppSettings["test_url"];
            }
            else
            {
                actionURL = ConfigurationManager.AppSettings["prod_url"];
            }

            notify_url = ConfigurationManager.AppSettings["notify_url"];
            currency_code = ConfigurationManager.AppSettings["currency_code"];
        }
    }
}