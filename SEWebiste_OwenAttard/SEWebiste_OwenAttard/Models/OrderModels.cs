using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

namespace SEWebiste_OwenAttard.Models
{
    public class OrderModels
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public int ItemCount { get; set; }
        public decimal Total { get; set; }
    }

    public class OrderDetailsModels
    {
        public string Seller { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}