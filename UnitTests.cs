
namespace WakamProductReservationTool
{
    class UnitTests
    {
        public void RunUnitTests()
        {
            Database database = new Database();
            database.ClearDatabase();

            CanMakeReservation_1Product_ReturnTrue();
            CanMakeReservation_25Product_ReturnTrue();
            CannotAllowDuplicateProducts_10Products5Duplicates_Return5();
            CannotOrderUnknownProduct_2Products1Unknown_Return1();
            GetReservations_Cursor1Limit3_Return3AtIndex1();
            CannotCreateEmptyReservation_Product0_returnFalse();
            ReservationAvailability_ProductQuantity0_returnFalse();
            GetProducts_Cursor1Limit3_Return3AtIndex1();
            GetProductsInvalidCurspr_Cursor1000Limit5_ReturnNull();
        }

        void PassOrFail(bool res)
        {
            if (res == true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("PASSED\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("FAILED\n");
            }
            Console.ResetColor();
        }

        static void PrintReservation(Reservation reservation)
        {
            Console.WriteLine("ReservationID: " + reservation.ReservationId + " - CreatedAt: " + reservation.CreatedAt + " - Available: " + reservation.IsAvailable);
            foreach (var order in reservation.OrdersLine)
                Console.WriteLine("\tOrderLineID: " + order.OrderLineId + " - ReservationId: " + order.ReservationId + " - ProductId: " + order.ProductId + " - Quantity: " + order.Quantity);
        }

        static void PrintProductList(List<Product> products)
        {
            Console.WriteLine("Product List:");
            foreach (var product in products)
                Console.WriteLine("\tID: " + product.ProductId + " - Product Name: " + product.ProductName + " - Quantity: " + product.Quantity);
        }

        void CanMakeReservation_1Product_ReturnTrue()
        {
            Console.WriteLine("TEST: CanMakeReservation_1Product_Resturn1\n");

            //Arrange
            BackEndService backEndService = new BackEndService();

            //Act
            backEndService.SetProduct("1", 1);
            Reservation testReservation = backEndService.CreateReservation(backEndService.MainOrder);

            //Assert
            PrintReservation(testReservation);
            if (testReservation.OrdersLine.Count() == 1)
                PassOrFail(true);
            else
                PassOrFail(false);
        }

        void CanMakeReservation_25Product_ReturnTrue()
        {
            Console.WriteLine("TEST: CanMakeReservation_25Product_Return25\n");

            //Arrange
            BackEndService backEndService = new BackEndService();

            //Act
            for (int i = 1; i <= 25; i++)
                backEndService.SetProduct(i.ToString(), 1);
            Reservation testReservation = backEndService.CreateReservation(backEndService.MainOrder);

            //Assert
            PrintReservation(testReservation);
            if (testReservation.OrdersLine.Count() == 25)
                PassOrFail(true);
            else
                PassOrFail(false);
        }

        void CannotAllowDuplicateProducts_10Products5Duplicates_Return5()
        {
            Console.WriteLine("TEST: CannotAllowDuplicateProducts_10Products5Duplicates_Return5\n");

            //Arrange
            BackEndService backEndService = new BackEndService();

            //Act
            backEndService.SetProduct("1", 10);
            backEndService.SetProduct("2", 11);
            backEndService.SetProduct("3", 12);
            backEndService.SetProduct("4", 13);
            backEndService.SetProduct("5", 14);

            backEndService.SetProduct("1", 1);
            backEndService.SetProduct("2", 1);
            backEndService.SetProduct("3", 1);
            backEndService.SetProduct("4", 1);
            backEndService.SetProduct("5", 1);

            Reservation testReservation = backEndService.CreateReservation(backEndService.MainOrder);

            //Assert
            PrintReservation(testReservation);

            if (testReservation.OrdersLine.Count() == 5)
                PassOrFail(true);
            else
                PassOrFail(false);
        }

        void CannotOrderUnknownProduct_2Products1Unknown_Return1()
        {
            Console.WriteLine("TEST: CannotOrderUnknownProduct_2Products1Unknown_Return1\n");

            //Arrange
            BackEndService backEndService = new BackEndService();

            //Act
            backEndService.SetProduct("1", 1);
            backEndService.SetProduct("9000", 1);  //Unknown Product ID

            Reservation testReservation = backEndService.CreateReservation(backEndService.MainOrder);

            //Assert
            PrintReservation(testReservation);
            if (testReservation.OrdersLine.Count() == 1)
                PassOrFail(true);
            else
                PassOrFail(false);
        }

        void GetReservations_Cursor1Limit3_Return3AtIndex1()
        {
            Console.WriteLine("TEST: GetReservations_Cursor1Limit3_Return3AtIndex1\n");

            //Arrange
            Database database = new Database();
            database.ClearDatabase();
            BackEndService backEndService = new BackEndService();
            List<Reservation> testReservations = new List<Reservation>();
            List<Reservation> getReservations = new List<Reservation>();


            //Act
            backEndService.SetProduct("1", 1);
            testReservations.Add(backEndService.CreateReservation(backEndService.MainOrder));
            backEndService.SetProduct("2", 1);
            testReservations.Add(backEndService.CreateReservation(backEndService.MainOrder));
            backEndService.SetProduct("3", 1);
            testReservations.Add(backEndService.CreateReservation(backEndService.MainOrder));
            backEndService.SetProduct("4", 1);
            testReservations.Add(backEndService.CreateReservation(backEndService.MainOrder));

            //Assert
            getReservations = backEndService.GetReservations(1, 3); //cursor 1, limit 3

            foreach (Reservation reservation in getReservations)
                PrintReservation(reservation);

            if (testReservations[1].ReservationId == getReservations[0].ReservationId)
                PassOrFail(true);
            else
                PassOrFail(false);
        }

        void CannotCreateEmptyReservation_Product0_returnFalse()
        {
            Console.WriteLine("TEST: CannotCreateEmptyReservation_Product0_returnFalse\n");

            //Arrange
            BackEndService backEndService = new BackEndService();

            //Act
            Reservation testReservation = backEndService.CreateReservation(backEndService.MainOrder);

            //Assert
            if (testReservation == null)
                PassOrFail(true);
            else
                PassOrFail(false);
        }

        void ReservationAvailability_ProductQuantity0_returnFalse()
        {
            Console.WriteLine("TEST: ReservationAvailability_ProductQuantity0_returnFalse\n");

            //Arrange
            BackEndService backEndService = new BackEndService();

            //Act
            backEndService.SetProduct("1", 1);      //Unavailable Product 
            Reservation testReservation = backEndService.CreateReservation(backEndService.MainOrder);

            //Assert
            PrintReservation(testReservation);
            if (testReservation.IsAvailable == false)
                PassOrFail(true);
            else
                PassOrFail(false);
        }

        void GetProducts_Cursor1Limit3_Return3AtIndex1()
        {
            Console.WriteLine("TEST: GetProducts_Cursor1Limit3_Return3AtIndex1\n");

            //Arrange
            BackEndService backEndService = new BackEndService();


            //Act
            List<Product> testProducts = backEndService.GetProducts(1, 3);

            //Assert
            PrintProductList(testProducts);
            if (testProducts.Count == 3)
                PassOrFail(true);
            else
                PassOrFail(false);
        }

        void GetProductsInvalidCurspr_Cursor1000Limit5_ReturnNull()
        {
            Console.WriteLine("TEST: GetProductsInvalidCurspr_Cursor1000Limit5_ReturnNull\n");

            //Arrange
            BackEndService backEndService = new BackEndService();

            //Act
            List<Product> testProducts = backEndService.GetProducts(1000, 5);

            //Assert
            if (testProducts == null)
                PassOrFail(true);
            else
                PassOrFail(false);
        }
    }
}
