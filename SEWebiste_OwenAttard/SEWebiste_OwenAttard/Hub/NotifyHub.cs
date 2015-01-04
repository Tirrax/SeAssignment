using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Common;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SEWebiste_OwenAttard.Models;
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

        public void sendToSpecific(string ID, List<ProductModels> models)
        {
            // Call the broadcastMessage method to update clients            
            hub.Clients.Client(ID).broadcastMessage(models);
        }
    }

    public class notifyHub : Hub
    {

        public static ProductObservable Observable = new ProductObservable();

        private static List<UserObserver> Observers = new List<UserObserver>();

        public void Notify(string name, string id)
        {
            UserObserver observer = new UserObserver()
            {
                ID = id,
                hub = new HubHelper(this)
            };

            Observable.AddObserver(observer);
            Observers.Add(observer);
        }

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            new HubHelper(this).Send(name, message);
        }

        public void sendToSpecific(string name, string message, string to)
        {

            new HubHelper(this).sendToSpecific(to, null);
        }

        public override Task OnDisconnected()
        {
            UserObserver observer = Observers.SingleOrDefault(x => x.ID == Context.ConnectionId.ToString());
            Observable.DeleteObserver(observer);
            Observers.Remove(observer);

            return null;

        }




    }

}