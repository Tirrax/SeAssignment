using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

namespace SEWebiste_OwenAttard.Models
{
    public class ProductModels
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImgPath { get; set; }
        public decimal price { get; set; }
        public string Desc { get; set; }
        public int Qty{ get; set; }
        public bool HandleDeliveries { get; set; }
        public DateTime Datelisted { get; set; }
        //Categories
        public int SelectedCatID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Categories { get; set; }
    }

    public class ProductDisplayModel
    {
        public List<ProductModels> productModels = new List<ProductModels>();
        public string CatName;
    }

    public class SingleProductDisplayModel
    {
        public ProductModels productModels{ get; set; }
        public string CatName;
        public int CatId;
    }

    public class ShoppingcartModel
    {
        public IEnumerable<Common.ShoppingCart> ShoppinCartItems;

        public decimal total = 0;
    }
}