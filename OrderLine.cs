
namespace WakamProductReservationTool
{
    public class OrderLine
    {
        public string OrderLineId { get; }
        public string ReservationId { get; set; }
        public string ProductId { get; }
        public int Quantity { get; }

        public OrderLine(string _OrderLineId, string _ReservationId, string _ProductId, int _Quantity)
        {
            OrderLineId = _OrderLineId;
            ReservationId = _ReservationId;
            ProductId = _ProductId;
            Quantity = _Quantity;
        }

    }
}
