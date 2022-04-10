using System.Data.SQLite;

namespace WakamProductReservationTool
{
    internal class UsefulFuncs
    {

        static void AddProductToDatabase(string name, int quantity)
        {
            Database databaseObject = new Database();

            string query = "INSERT INTO Products ('name', 'quantity') VALUES (@name, @quantity)";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            myCommand.Parameters.AddWithValue("@name", name);
            myCommand.Parameters.AddWithValue("@quantity", quantity);
            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            Console.WriteLine("Rows /added : {0}", result);

        }
    }
}
