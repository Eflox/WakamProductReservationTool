using System.Data.SQLite;

namespace WakamProductReservationTool
{
    public class Database
    {
        public SQLiteConnection myConnection;
        
        public Database()
        {
            myConnection = new SQLiteConnection("Data Source=database.sqlite3");

            if (!File.Exists("./database.sqlite3"))
            {
                SQLiteConnection.CreateFile("database.sqlite3");
                Console.WriteLine("Database file created");
            }
        }

        public void OpenConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Open)
                myConnection.Open();
        }

        public void CloseConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Closed)
                myConnection.Close();
        }

        public void ClearDatabase()
        {
            SQLiteCommand ClearOrderLineCommand = new SQLiteCommand("DELETE FROM OrderLine", myConnection);
            SQLiteCommand ClearReservationsCommand = new SQLiteCommand("DELETE FROM Reservations", myConnection);

            OpenConnection();
            ClearOrderLineCommand.ExecuteReader();
            ClearReservationsCommand.ExecuteReader();
            CloseConnection();
        }
    }
}
