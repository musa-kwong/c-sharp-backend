// using System;
// using System.Collections.Generic;
using Npgsql;

namespace Local.DB
{
    public class Account
    {
        public int Id { get; set; } // its so cool that you can have auto gets and sets :D
        public required string EmailAddress { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public int OwnerAccount { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime DatePosted { get; set; }
    }

    public class DataDB
    {
        // Currently: Host=localhost;Port=5432;Username=postgres;Password=<secret :3>;Database=postgres
        private static readonly string _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        private static readonly NpgsqlConnection _connection = new NpgsqlConnection(_connectionString);

        public static void Init()
        {
            // Initialize the database_connection
            _connection.Open();

            // Executes the SQL command file from ../sql/service.sql to create the tables
            string sqlScript = File.ReadAllText("../sql/service.sql"); // Read the SQL script
            using var command = new NpgsqlCommand(sqlScript,_connection); // Creates the command
            command.ExecuteNonQuery(); // Execute the SQL script

            Console.WriteLine("Database initialized successfully.");
        }

        // Fetch all accounts, probably shouldn't with plaintext passwords
        public static List<Account> GetAllAccounts()
        {
            var accounts = new List<Account>();

            using var command = new NpgsqlCommand("SELECT id, emailaddress, name, password FROM Account",_connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                accounts.Add(new Account
                {
                    Id = reader.GetInt32(0),
                    EmailAddress = reader.GetString(1),
                    Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Password = reader.GetString(3)
                });
            }

            return accounts;
        }

        // Fetch all items
        public static List<Item> GetAllItems()
        {
            var items = new List<Item>();         
            using var command = new NpgsqlCommand("SELECT id, owneraccount, name, description, dateposted FROM Item",_connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new Item
                {
                    Id = reader.GetInt32(0),
                    OwnerAccount = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                    DatePosted = reader.GetDateTime(4)
                });
            }

            return items;
        }

        public static Item? GetItem(int id)
        {
            // Fetches item through parameterized query to prevent SQL injection
            // @id is placeholder, set afterwards with command.Parameters.AddWithValue
            using var command = new NpgsqlCommand("SELECT id, owneraccount, name, description, dateposted FROM Item WHERE id = @id", _connection);
            command.Parameters.AddWithValue("id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read()) {
                return new Item
                {
                    Id = reader.GetInt32(0),
                    OwnerAccount = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                    DatePosted = reader.GetDateTime(4)
                };
            }
            return null; //nothing found
        }

        // Insert new account
        public static Account CreateAccount(Account account)
        {
            using var command = new NpgsqlCommand(
                "INSERT INTO Account (emailaddress, name, password) VALUES (@EmailAddress, @Name, @Password) RETURNING id",_connection);

            command.Parameters.AddWithValue("EmailAddress", account.EmailAddress);
            command.Parameters.AddWithValue("Name", (object)account.Name ?? DBNull.Value);
            command.Parameters.AddWithValue("Password", account.Password);

            account.Id = (int)command.ExecuteScalar();
            return account;
        }

        // Insert new item
        public static Item CreateItem(Item item)
        {
            using var command = new NpgsqlCommand(
                "INSERT INTO Item (owneraccount, name, description) VALUES (@OwnerAccount, @Name, @Description) RETURNING id",_connection);

            command.Parameters.AddWithValue("OwnerAccount", item.OwnerAccount);
            command.Parameters.AddWithValue("Name", item.Name);
            command.Parameters.AddWithValue("Description", (object)item.Description ?? DBNull.Value);

            item.Id = (int)command.ExecuteScalar();
            return item;
        }

        // Delete account by ID
        public static void RemoveAccount(int id)
        {
            using var command = new NpgsqlCommand("DELETE FROM Account WHERE id = @id",_connection);
            command.Parameters.AddWithValue("id", id);
            command.ExecuteNonQuery();
        }

        // Delete item by ID
        public static void RemoveItem(int id)
        {
            using var command = new NpgsqlCommand("DELETE FROM Item WHERE id = @id",_connection);
            command.Parameters.AddWithValue("id", id);
            command.ExecuteNonQuery();
        }
    }
}
