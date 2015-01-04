using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL;
using Common;
using SEWebiste_OwenAttard.Models;

namespace SEWebiste_OwenAttard.Observer
{
    public class ProductObservable : IObservable
    {
        public  List<IObserver> observers = new List<IObserver>();

        List<ProductModels> LatestProducts = new List<ProductModels>();
        public  DateTime LastSyncTime = DateTime.Now;


        public void AddObserver(IObserver ob)
        {
            observers.Add(ob);
        }

        public void DeleteObserver(IObserver ob)
        {
            observers.Remove(ob);
        }

        public void NotifyObservers()
        {
            foreach (IObserver o in observers)
            {
                o.Update(LatestProducts);
            }
        }

        public void NewProductAdded()
        {
            var diffInSeconds = (DateTime.Now - LastSyncTime).TotalSeconds;

            if (diffInSeconds < 60)
                return;


            LatestProducts.Clear();
            LatestProducts = new ProductBL().GetLatestProducts().Select(cur => new ProductModels()
            {
                Desc = cur.Features,
                Qty = cur.Qty,
                ImgPath = cur.Image,
                Name = cur.Name,
                price = cur.Price,
            }).ToList();

            NotifyObservers();
        }

    }
}