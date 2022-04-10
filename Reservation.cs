
namespace WakamProductReservationTool
{
    public class Reservation
    {
        public string ReservationId { get; }
        public DateTime CreatedAt { get; }
        public List<OrderLine> OrdersLine { get; }
        public bool IsAvailable { get; }

        public Reservation(string _ReservationId, List<OrderLine> _OrdersLine, DateTime _CreatedAt, bool _IsAvailable)
        {
            ReservationId = _ReservationId;
            OrdersLine = _OrdersLine;
            CreatedAt = _CreatedAt;
            IsAvailable = _IsAvailable;
        }
    }
}
