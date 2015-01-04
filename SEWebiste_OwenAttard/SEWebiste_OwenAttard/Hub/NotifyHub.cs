using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Common;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SEWebiste_OwenAttard.Observer;

namespace SEWebiste_OwenAttard
{

    public class HubHelper
    {
        

        private Hub hub;

        public HubHelper(Hub hub)
        {
            this.hub = hub;
        }

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            hub.Clients.All.broadcastMessage(name, message);
        }

        public void sendToSpecific(string ID)
        {
            // Call the broadcastMessage method to update clients            
            hub.Clients.Client(ID).broadcastMessage();
        }
    }

    public class NotifyHub : Hub
    {

        public static ProductObservable Observable = new ProductObservable();

        public void Notify(string name, string id)
        {
            UserObserver observer = new UserObserver()
            {
                ID = id,
                hub = new HubHelper(this)
            };

            Observable.AddObserver(observer);
        }

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            new HubHelper(this).Send(name, message);
        }

        public void sendToSpecific(string name, string message, string to)
        {

            new HubHelper(this).sendToSpecific(to);
        }

        public override Task OnDisconnected()
        {
            return Clients.All.disconnected();
        }




    }

}