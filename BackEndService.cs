using System.Data;
using System.Data.SQLite;

namespace WakamProductReservationTool
{
    public class BackEndService : IBackEndService, IInventoryRepositiry
    {
        
        public List<OrderLine> MainOrder = new List<OrderLine>();

        public Reservation CreateReservation(List<OrderLine> order)
        {
            string reservationId = Guid.NewGuid().ToString();
            order = order.DistinctBy(x => x.ProductId).ToList();    //removes any duplicate products
            if (order.Count == 0)
            {
                Console.WriteLine("Error: No products have been selected for the reservation");
                return null;
            }

            foreach (OrderLine item in order)
                item.ReservationId = reservationId;

            Reservation reservation = new Reservation(reservationId, order, DateTime.Now, ReservationAvailable(order));
            AppendReservationToDb(reservation);
            AppendOrderLineToDb(order, reservation.ReservationId);
            MainOrder.Clear();
            
            return reservation;
        }

        public List<Reservation> GetReservations(int cursor, int limit)
        {

            IQueryable<Reservation> data = GetReservations();

            if (limit + cursor > data.Count() || limit == 0)
                limit = data.Count() - cursor;

            if (cursor < 0 || cursor > data.Count())
            {
                Console.WriteLine("Error: Cursor and limit cannot index the data");
                return null;
            }

            List<Reservation> reservations = new List<Reservation>();
            for (int i = cursor; i < cursor + limit; i++)
            {
                reservations.Add(data.ElementAt(i));
            }
            return (reservations);
        }

        public void SetProduct(string productId, int quantity)
        {
            if (GetProducts(0, 0).Where(p => p.ProductId == productId).Count() == 0)    //check if product ID exists
            {
                Console.WriteLine("Error: Unknown product ID");
                return;
            }
            MainOrder.Add(new OrderLine(Guid.NewGuid().ToString(), "", productId, quantity));
        }

        public  List<Product> GetProducts(int cursor, int limit)
        {
            IQueryable<Product> data = GetProducts();

            if (limit + cursor > data.Count() || limit == 0)
                limit = data.Count() - cursor;

            if (cursor < 0 || cursor > data.Count())
            {
                Console.WriteLine("Error: Cursor cannot index the data");
                return null;
            }
            List<Product> products = new List<Product>();
            for (int i = cursor; i < cursor + limit; i++)
            {
                products.Add(data.ElementAt(i));
            }
            return(products);
        }
        
        public IQueryable<Product> GetProducts()
        {

            Database databaseObject = new Database();

            string query = "SELECT * FROM Products";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            List<Product> products = new List<Product>();

            while (result.Read())
            {
                products.Add(new Product(result["id"].ToString(), result["name"].ToString(), int.Parse(result["quantity"].ToString())));
            }

            databaseObject.CloseConnection();

            return products.AsQueryable();
        }

        public List<OrderLine> GetOrderLine(string reservationId)
        {
            IQueryable<OrderLine> data = GetOrderLine();

            List<OrderLine> order = GetOrderLine().ToList();
            List<OrderLine> newOrder = new List<OrderLine>();


            foreach (OrderLine orderLine in order)
            {
                if (orderLine.ReservationId == reservationId)
                    newOrder.Add(orderLine);
            }
            return newOrder;
        }
        public IQueryable<Reservation> GetReservations()
        {
            Database databaseObject = new Database();

            string query = "SELECT * FROM Reservations";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            List<Reservation> reservations = new List<Reservation>();
            while (result.Read())
            {
                reservations.Add(new Reservation(result["id"].ToString(), GetOrderLine(result["id"].ToString()), /*DateTime.ParseExact(result["CreatedAt"].ToString()*/new DateTime(), returnBool(int.Parse(result["IsAvailable"].ToString()))));
            }
            databaseObject.CloseConnection();
            return reservations.AsQueryable();
        }

        public IQueryable<OrderLine> GetOrderLine()
        {
            Database databaseObject = new Database();

            List<OrderLine> orderLine = new List<OrderLine>();
            string query = "SELECT * FROM OrderLine";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();

            while (result.Read())
            {
                orderLine.Add(new OrderLine(result["id"].ToString(), result["ReservationId"].ToString(), result["ProductId"].ToString(), int.Parse(result["Quantity"].ToString())));
            }
            databaseObject.CloseConnection();
            
            return orderLine.AsQueryable();
        }

        public void AppendOrderLineToDb(List<OrderLine> order, string reservationId)
        {
            Database database = new Database();

            string query = "INSERT INTO OrderLine ('id', 'ReservationId', 'ProductId', 'Quantity') VALUES (@id, @ReservationId, @ProductId, @Quantity)";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            
            database.OpenConnection();
            foreach (OrderLine orderLine in order)
            {
                myCommand.Parameters.AddWithValue("@id", orderLine.OrderLineId);
                myCommand.Parameters.AddWithValue("@ReservationId", reservationId);
                myCommand.Parameters.AddWithValue("@ProductId", orderLine.ProductId);
                myCommand.Parameters.AddWithValue("@Quantity", orderLine.Quantity);
                myCommand.ExecuteNonQuery();
            }
            database.CloseConnection();
        }

        public void AppendReservationToDb(Reservation reservation)
        {
            Database database = new Database();

            string query = "INSERT INTO Reservations ('id', 'CreatedAt', 'IsAvailable') VALUES (@id, @CreatedAt, @IsAvailable)";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);

            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@id", reservation.ReservationId);
            myCommand.Parameters.AddWithValue("@CreatedAt", reservation.CreatedAt);
            myCommand.Parameters.AddWithValue("@IsAvailable", reservation.IsAvailable);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();
        }

        bool ReservationAvailable(List<OrderLine> order)
        {
            List<Product> productsOutOfStock = new List<Product>();

            foreach (Product product in GetProducts(0,0))
                if (product.Quantity < 0)
                    productsOutOfStock.Add(product);

            foreach (OrderLine item in order)
                foreach (Product product in productsOutOfStock)
                    if (item.ProductId == product.ProductId)
                        if (product.Quantity <= 0)
                            return false;

            return true;
        }
        bool returnBool(int i)
        {
            if (i == 0)
                return false;
            return true;
        }
    }
}