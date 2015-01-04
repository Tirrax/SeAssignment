using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEWebiste_OwenAttard.Models;

namespace SEWebiste_OwenAttard
{
    public interface IObserver
    {
        void Update(List<ProductModels> products);
    }
}
