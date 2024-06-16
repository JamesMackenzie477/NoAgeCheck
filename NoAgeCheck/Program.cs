using System;
using System.Data.SQLite;
using System.IO;

namespace NoAgeCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            // gets the appdata local path
            string AppDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            // validates the path exists
            if (!string.IsNullOrEmpty(AppDataLocal))
            {
                // constructs the cookies database path
                string CookiesPath = Path.Combine(AppDataLocal, "Steam\\htmlcache\\Cookies");
                // validates the database exists
                if (File.Exists(CookiesPath))
                {
                    // connects to the cookies data base
                    SQLiteConnection Connection = new SQLiteConnection("Data Source=" + CookiesPath);
                    // opens the connection
                    Connection.Open();
                    // constructs the add cookie query
                    string AddCookie = "INSERT INTO cookies (host_key, name, value, path, expires_utc, secure, httponly, last_access_utc) SELECT 'store.steampowered.com', 'birthtime', 0, '/', 0, 0, 0, 0 WHERE NOT EXISTS (SELECT 1 FROM cookies WHERE name = 'birthtime');";
                    // constructs a command with the query
                    SQLiteCommand command = new SQLiteCommand(AddCookie, Connection);
                    // executes the command
                    if (command.ExecuteNonQuery() == 1)
                    {
                        // notifies the user
                        Console.WriteLine("Successfully disabled Steam age check.");
                    }
                    else
                    {
                        // notifies the user
                        Console.WriteLine("Could not add the spoofed cookie to the database.");
                    }
                    // closes the database
                    Connection.Close();
                }
                else
                {
                    // notifies the user
                    Console.WriteLine("Could not find the cookies database.");
                }
            }
            else
            {
                // notifies the user
                Console.WriteLine("Could not find the AppData Local folder.");
            }
        }
    }
}
