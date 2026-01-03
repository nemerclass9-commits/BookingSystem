using System;
using System.Collections.Generic;

namespace BookingSystem
{
    // Enum for Booking Status
    public enum BookingStatus
    {
        Confirmed,
        Cancelled
    }

    // User Model
    public class User
    {
        public string UserID { get; set; }  // Primary Key
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Username { get; set; }  // Username for login
        public string Password { get; set; }  // Password for authentication
        public string Role { get; set; }  // "Admin" or "Customer"
        public List<string> BookedEvents { get; set; }  // IDs of events this user has tickets for

        public User()
        {
            BookedEvents = new List<string>();
        }
    }

    // Event Model
    public class Event
    {
        public string EventID { get; set; }  // Primary Key
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public List<int> OccupiedSeats { get; set; }  // Stores specific seat numbers like 12, 45, 88 that are already taken

        public Event()
        {
            OccupiedSeats = new List<int>();
        }
    }

    // Booking Model
    public class Booking
    {
        public string BookingID { get; set; }  // Primary Key
        public string UserID { get; set; }
        public string EventID { get; set; }
        public int SeatNumber { get; set; }  // The specific seat chosen by the user
        public BookingStatus Status { get; set; }  // Enum: Confirmed or Cancelled
    }
}

