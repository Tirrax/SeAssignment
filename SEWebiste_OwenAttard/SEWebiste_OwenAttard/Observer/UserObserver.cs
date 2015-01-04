using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SEWebiste_OwenAttard.Models;

namespace SEWebiste_OwenAttard.Observer
{
    public class UserObserver : IObserver
    {
        public string ID;
        public HubHelper hub;
     
        public void Update(List<ProductModels> products)
        {
            hub.sendToSpecific(ID, products);
        }
    }
}