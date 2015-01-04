using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEWebiste_OwenAttard
{
    public interface IObservable
    {
        void AddObserver(IObserver ob);
        void DeleteObserver(IObserver ob);
        void NotifyObservers();

    }
}
