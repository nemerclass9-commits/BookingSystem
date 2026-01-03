using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BookingSystem
{
    // Data class to hold all collections for JSON serialization
    public class DatabaseData
    {
        public List<User> Users { get; set; }
        public List<Event> Events { get; set; }
        public List<Booking> Bookings { get; set; }

        public DatabaseData()
        {
            Users = new List<User>();
            Events = new List<Event>();
            Bookings = new List<Booking>();
        }
    }

    // Singleton Database Manager
    public class DatabaseManager
    {
        private static DatabaseManager _instance;
        private static readonly object _lock = new object();
        private const string DataFilePath = "booking_data.json";

        // Data collections
        public List<User> Users { get; private set; }
        public List<Event> Events { get; private set; }
        public List<Booking> Bookings { get; private set; }

        // Private constructor for Singleton
        private DatabaseManager()
        {
            Users = new List<User>();
            Events = new List<Event>();
            Bookings = new List<Booking>();
            LoadData();
        }

        // Singleton instance property
        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseManager();
                        }
                    }
                }
                return _instance;
            }
        }

        // Load data from JSON file
        public void LoadData()
        {
            try
            {
                if (File.Exists(DataFilePath))
                {
                    string jsonString = File.ReadAllText(DataFilePath);
                    var data = JsonSerializer.Deserialize<DatabaseData>(jsonString);
                    
                    if (data != null)
                    {
                        Users = data.Users ?? new List<User>();
                        Events = data.Events ?? new List<Event>();
                        Bookings = data.Bookings ?? new List<Booking>();
                    }
                    else
                    {
                        InitializeEmptyLists();
                    }
                }
                else
                {
                    InitializeEmptyLists();
                }

                // Initialize demo users if database is empty
                if (Users.Count == 0)
                {
                    InitializeDemoUsers();
                }
                else
                {
                    // Ensure demo admin user exists and has correct role
                    EnsureDemoAdminExists();
                }
            }
            catch (Exception ex)
            {
                // If loading fails, initialize empty lists
                InitializeEmptyLists();
                Console.WriteLine($"Error loading data: {ex.Message}");
                
                // Initialize demo users even if loading failed
                if (Users.Count == 0)
                {
                    InitializeDemoUsers();
                }
            }
        }

        // Save data to JSON file
        public void SaveData()
        {
            try
            {
                var data = new DatabaseData
                {
                    Users = Users,
                    Events = Events,
                    Bookings = Bookings
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(DataFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
                throw;
            }
        }

        // Initialize empty lists
        private void InitializeEmptyLists()
        {
            Users = new List<User>();
            Events = new List<Event>();
            Bookings = new List<Booking>();
        }

        // Helper Methods

        // Get Event by ID
        public Event GetEvent(string id)
        {
            return Events.FirstOrDefault(e => e.EventID == id);
        }

        // Get User by ID
        public User GetUser(string id)
        {
            return Users.FirstOrDefault(u => u.UserID == id);
        }

        // Get Booking by ID
        public Booking GetBooking(string id)
        {
            return Bookings.FirstOrDefault(b => b.BookingID == id);
        }

        // Get all Bookings for a specific Event (needed for Admin to see who is attending)
        public List<Booking> GetBookingsByEvent(string eventId)
        {
            return Bookings.Where(b => b.EventID == eventId).ToList();
        }

        // Authentication Methods

        // Find user by username and password
        public User FindUserByUsernameAndPassword(string username, string password)
        {
            return Users.FirstOrDefault(u => 
                u.Username != null && 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
                u.Password == password);
        }

        // Check if username already exists
        public bool CheckUsernameExists(string username)
        {
            return Users.Any(u => 
                u.Username != null && 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        // Register a new user
        public User RegisterUser(string username, string password, string name, string contact, string role = "Customer")
        {
            // Check if username already exists
            if (CheckUsernameExists(username))
            {
                return null; // Username already taken
            }

            // Create new user
            User newUser = new User
            {
                UserID = GenerateUserID(),
                Username = username,
                Password = password,
                Name = name,
                Contact = contact,
                Role = role
            };

            Users.Add(newUser);
            SaveData();

            return newUser;
        }

        // Helper method to generate unique User ID
        private string GenerateUserID()
        {
            string newID;
            do
            {
                newID = "USR" + DateTime.Now.Ticks.ToString().Substring(10) + new Random().Next(100, 999).ToString();
            } while (Users.Any(u => u.UserID == newID));

            return newID;
        }

        // Initialize demo users (1 Admin and 1 Customer)
        private void InitializeDemoUsers()
        {
            // Demo Admin User
            User adminUser = new User
            {
                UserID = "USR_ADMIN_DEMO",
                Username = "admin",
                Password = "admin123",
                Name = "Admin User",
                Contact = "admin@bookingsystem.com",
                Role = "Admin",
                BookedEvents = new List<string>()
            };

            // Demo Customer User
            User customerUser = new User
            {
                UserID = "USR_CUSTOMER_DEMO",
                Username = "alice",
                Password = "password1",
                Name = "Alice Johnson",
                Contact = "alice@bookingsystem.com",
                Role = "Customer",
                BookedEvents = new List<string>()
            };

            Users.Add(adminUser);
            Users.Add(customerUser);

            // Save demo users to file
            SaveData();

            Console.WriteLine("Demo users initialized:");
            Console.WriteLine("Admin - Username: admin, Password: admin123");
            Console.WriteLine("Customer - Username: alice, Password: password1");
        }

        // Ensure demo users exist and have correct roles
        private void EnsureDemoAdminExists()
        {
            // Check if admin user exists
            User adminUser = Users.FirstOrDefault(u => 
                u.Username != null && 
                u.Username.Equals("admin", StringComparison.OrdinalIgnoreCase));

            if (adminUser != null)
            {
                // Ensure admin user has Admin role
                if (!adminUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    adminUser.Role = "Admin";
                    SaveData();
                    Console.WriteLine("Fixed: Admin user role updated to 'Admin'");
                }
            }
            else
            {
                // Create admin user if it doesn't exist
                User newAdminUser = new User
                {
                    UserID = "USR_ADMIN_DEMO",
                    Username = "admin",
                    Password = "admin123",
                    Name = "System Administrator",
                    Contact = "admin@bookingsystem.com",
                    Role = "Admin",
                    BookedEvents = new List<string>()
                };
                Users.Add(newAdminUser);
                SaveData();
                Console.WriteLine("Created: Demo admin user (admin/admin123)");
            }

            // Check if alice user exists
            User aliceUser = Users.FirstOrDefault(u => 
                u.Username != null && 
                u.Username.Equals("alice", StringComparison.OrdinalIgnoreCase));

            if (aliceUser == null)
            {
                // Create alice user if it doesn't exist
                User newAliceUser = new User
                {
                    UserID = "USR_CUSTOMER_DEMO",
                    Username = "alice",
                    Password = "password1",
                    Name = "Alice Johnson",
                    Contact = "alice@bookingsystem.com",
                    Role = "Customer",
                    BookedEvents = new List<string>()
                };
                Users.Add(newAliceUser);
                SaveData();
                Console.WriteLine("Created: Demo customer user (alice/password1)");
            }
        }
    }
}

