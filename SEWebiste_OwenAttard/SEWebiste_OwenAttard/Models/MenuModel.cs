using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEWebiste_OwenAttard.Models
{
    public class MenuModel
    {

        public string ActionName { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string MenuID { get; set; }

    }

    public class CategoryModel
    {

        public int ID{ get; set; }
        public string Name { get; set; }
        public int? ParentID { get; set; }
        
    }
}