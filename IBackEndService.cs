using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakamProductReservationTool
{
    interface IBackEndService
    {
        Reservation CreateReservation(List<OrderLine> order);
        List<Reservation> GetReservations(int cursor, int limit);
        void SetProduct(string productId, int quantity);
        List<Product> GetProducts(int cursor, int limit);
    }
}
