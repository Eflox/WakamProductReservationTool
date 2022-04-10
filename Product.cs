
namespace WakamProductReservationTool
{
    public class Product
    {
        public string ProductId { get; }
        public string ProductName { get; }
        public int Quantity { get; set; }

        public Product(string _ProductId, string _ProductName, int _Quantity)
        {
            ProductId = _ProductId;
            ProductName = _ProductName;
            Quantity = _Quantity;
        }
    }
}
