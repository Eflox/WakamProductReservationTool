using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakamProductReservationTool
{
    interface IInventoryRepositiry
    {
        IQueryable<Product> GetProducts();
        IQueryable<Reservation> GetReservations();
    }
}
