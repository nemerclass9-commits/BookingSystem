using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingSystem
{
    // Logic Layer: Admin/Manager Actions
    public class AdminSystem
    {
        private DatabaseManager _dbManager;

        public AdminSystem()
        {
            _dbManager = DatabaseManager.Instance;
        }

        // Add a new event
        // Creates event with OccupiedSeats starting as an empty list
        public Event AddEvent(string name, DateTime date, string category, decimal price, int totalSeats)
        {
            Event newEvent = new Event
            {
                EventID = GenerateEventID(),
                Name = name,
                Date = date,
                Category = category,
                Price = price,
                TotalSeats = totalSeats,
                AvailableSeats = totalSeats,  // Initially all seats are available
                OccupiedSeats = new List<int>()  // Starts as empty list
            };

            _dbManager.Events.Add(newEvent);
            _dbManager.SaveData();

            return newEvent;
        }

        // Remove an event
        // Finds all Bookings where EventID == eventID, sets them to Cancelled,
        // Removes the Event from the database and calls SaveData()
        public bool RemoveEvent(string eventID)
        {
            Event eventObj = _dbManager.GetEvent(eventID);
            if (eventObj == null)
            {
                return false;
            }

            // Find all Bookings where EventID == eventID
            List<Booking> eventBookings = _dbManager.GetBookingsByEvent(eventID);

            // Set them to Cancelled
            foreach (Booking booking in eventBookings)
            {
                if (booking.Status == BookingStatus.Confirmed)
                {
                    booking.Status = BookingStatus.Cancelled;

                    // Update user's BookedEvents list
                    User user = _dbManager.GetUser(booking.UserID);
                    if (user != null && user.BookedEvents.Contains(eventID))
                    {
                        user.BookedEvents.Remove(eventID);
                    }
                }
            }

            // Remove the Event from the database
            _dbManager.Events.Remove(eventObj);

            // Call SaveData()
            _dbManager.SaveData();

            return true;
        }

        // Update seat capacity for an event
        // Includes safeguards for seat management
        public string UpdateSeatCapacity(string eventID, int newCapacity)
        {
            Event eventObj = _dbManager.GetEvent(eventID);
            if (eventObj == null)
            {
                return "Error: Event not found.";
            }

            // Critical Check: Find the highest number currently in the OccupiedSeats list
            int highestBookedSeat = 0;
            if (eventObj.OccupiedSeats != null && eventObj.OccupiedSeats.Count > 0)
            {
                highestBookedSeat = eventObj.OccupiedSeats.Max();
            }

            // If newCapacity < Highest Booked Seat, return Error
            if (newCapacity < highestBookedSeat)
            {
                return $"Error: You cannot shrink the venue if someone is already sitting in the back row. Highest booked seat is {highestBookedSeat}, but new capacity is {newCapacity}.";
            }

            // Otherwise, update TotalSeats
            int oldTotalSeats = eventObj.TotalSeats;
            eventObj.TotalSeats = newCapacity;

            // Recalculate AvailableSeats
            // AvailableSeats = TotalSeats - Number of occupied seats
            eventObj.AvailableSeats = newCapacity - eventObj.OccupiedSeats.Count;

            _dbManager.SaveData();

            return $"Success: Seat capacity updated from {oldTotalSeats} to {newCapacity}. Available seats: {eventObj.AvailableSeats}.";
        }

        // Generate sales report
        // Loops through all Events, sums the price of all Confirmed bookings for that event
        // Returns: A formatted string showing: Event Name | Seats Sold | Total Revenue
        public string GenerateSalesReport()
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("=== SALES REPORT ===");
            report.AppendLine();
            report.AppendLine("Event Name | Seats Sold | Total Revenue");
            report.AppendLine(new string('-', 60));

            decimal grandTotal = 0;

            // Loop through all Events
            foreach (Event eventObj in _dbManager.Events)
            {
                // Sum the price of all Confirmed bookings for that event
                var confirmedBookings = _dbManager.Bookings
                    .Where(b => b.EventID == eventObj.EventID && b.Status == BookingStatus.Confirmed)
                    .ToList();

                int seatsSold = confirmedBookings.Count;
                decimal totalRevenue = confirmedBookings.Sum(b => eventObj.Price);

                grandTotal += totalRevenue;

                // Format: Event Name | Seats Sold | Total Revenue
                report.AppendLine($"{eventObj.Name,-30} | {seatsSold,10} | ${totalRevenue,10:F2}");
            }

            report.AppendLine(new string('-', 60));
            report.AppendLine($"{"GRAND TOTAL",-30} | {"",10} | ${grandTotal,10:F2}");

            return report.ToString();
        }

        // Helper method to generate unique Event ID
        private string GenerateEventID()
        {
            string newID;
            do
            {
                newID = "EVT" + DateTime.Now.Ticks.ToString().Substring(10) + new Random().Next(100, 999).ToString();
            } while (_dbManager.Events.Any(e => e.EventID == newID));

            return newID;
        }
    }
}

