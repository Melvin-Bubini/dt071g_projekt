using Microsoft.Data.Sqlite;
using BCrypt.Net;
using System.Reflection.Metadata;

namespace dt071g_projekt;

public class UserManagement
{
    private List<User> users = new List<User>();

    // Sqlite databas fil sökväg
    string connectionString = "Data Source=C:/Users/melvi/OneDrive/Skrivbord/dt071g_projekt/user.db";

    public UserManagement()
    {
        // Initiera db anslutningen

        // skapar ett anslutnings objekt
        using (var connection = new SqliteConnection(connectionString))
        {
            // Öppnar anslutningen
            connection.Open();

            // Skapar tabellen om den inte existerar
            string createTableQuery = @"CREATE TABLE IF NOT EXISTS users (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            username TEXT NOT NULL UNIQUE,
                                            password TEXT NOT NULL,
                                            balance REAL DEFAULT 1000
                                        );";

            using (var command = new SqliteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            // Läs in alla användare 
            string selectQuery = @"SELECT * FROM users;";
            using (var command = new SqliteCommand(selectQuery, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    User obj = new User();
                    obj.Username = reader["Username"].ToString();
                    obj.Password = reader["Password"].ToString();
                    users.Add(obj);
                }
            }
        }
    }

    public User addUser(string u, string p)
    {
        User obj = new User();
        obj.Username = u;

    
        // Krypterar lösenordet innan det sparas
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(p);
        obj.Password = hashedPassword;

        users.Add(obj);

        using (var connection = new SqliteConnection(connectionString))
        {
            // Öppnar anslutningen
            connection.Open();

            string insertQuery = @"INSERT INTO users ( username, password )
            VALUES ( @username, @password );";

            using (var command = new SqliteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@username", u);
                command.Parameters.AddWithValue("@password", hashedPassword); // Sparar det hashade lösenordet
                command.ExecuteNonQuery();
            }
        }
        return obj;
    }


    public bool loginUser(string u, string p)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectQuery = @" SELECT password FROM users WHERE username = @username;";
            using(var command = new SqliteCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@username", u);
                var storedPasswordHash = command.ExecuteScalar()?.ToString();

                if (storedPasswordHash != null)
                {
                    // Verifierar lösenordet
                    return BCrypt.Net.BCrypt.Verify(p, storedPasswordHash);
                }
            }
        }
        return false;
    } 

    public List<User> getUsers()
    {
        return users;
    }
}