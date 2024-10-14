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
                                            Balance REAL DEFAULT 0
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

        // Kollar så att man inte har ett för enkelt lösenord
        if (p.Length < 8)
        {
            throw new Exception("Lösenordet måste vara minst 8 tecken långt.");
        }
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


    /* public User loginUser(string u, string p)
    {
        
    } */

    public List<User> getUsers()
    {
        return users;
    }
}