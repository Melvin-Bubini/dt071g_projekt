using Microsoft.Data.Sqlite;

namespace dt071g_projekt;

public class Balance
{
    // Sqlite databas fil sökväg
    string connectionString = "Data Source=C:/Users/melvi/OneDrive/Skrivbord/dt071g_projekt/user.db";

    public decimal GetBalance(string username)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectQuery = @"SELECT balance FROM users WHERE username = @username;";
            using (var command = new SqliteCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("username", username);
                var balance = command.ExecuteScalar();

                if (balance != null)
                {
                    return Convert.ToDecimal(balance);
                }
            }
        }
        return 0; // Ifall användaren inte hittas så retuneras 0
    }

    public void updateBalance(string username, decimal newBalance)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string updateQuery = @"UPDATE users SET balance = @balance WHERE username = @username;";
            using (var command = new SqliteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@balance", newBalance);
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
            }
        }
    }
}
