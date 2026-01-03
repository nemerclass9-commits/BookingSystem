using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingSystem
{
    // Logic Layer: User Actions
    public class BookingSystem
    {
        private DatabaseManager _dbManager;

        public BookingSystem()
        {
            _dbManager = DatabaseManager.Instance;
        }

        // Get available seats for an event
        // Returns a list of integers from 1 to TotalSeats that are not in the event's OccupiedSeats list
        public List<int> GetAvailableSeats(string eventID)
        {
            Event eventObj = _dbManager.GetEvent(eventID);
            if (eventObj == null)
            {
                return new List<int>();
            }

            // Generate list of all seats (1 to TotalSeats)
            List<int> allSeats = Enumerable.Range(1, eventObj.TotalSeats).ToList();

            // Return seats that are not in OccupiedSeats
            return allSeats.Where(seat => !eventObj.OccupiedSeats.Contains(seat)).ToList();
        }

        // Book a specific seat for a user
        // Returns true if booking successful, false if seat unavailable
        public bool BookSeat(string userID, string eventID, int requestedSeat)
        {
            // Get event and user
            Event eventObj = _dbManager.GetEvent(eventID);
            User user = _dbManager.GetUser(userID);

            if (eventObj == null || user == null)
            {
                return false;
            }

            // Validation: Check if requestedSeat > TotalSeats OR requestedSeat is in OccupiedSeats
            if (requestedSeat > eventObj.TotalSeats || eventObj.OccupiedSeats.Contains(requestedSeat))
            {
                return false; // Seat Unavailable
            }

            // Assign: Add requestedSeat to the Event's OccupiedSeats list
            eventObj.OccupiedSeats.Add(requestedSeat);

            // Update: Subtract 1 from AvailableSeats
            eventObj.AvailableSeats--;

            // Record: Create Booking with requestedSeat
            Booking newBooking = new Booking
            {
                BookingID = GenerateBookingID(),
                UserID = userID,
                EventID = eventID,
                SeatNumber = requestedSeat,
                Status = BookingStatus.Confirmed
            };

            _dbManager.Bookings.Add(newBooking);

            // Update user's BookedEvents list
            if (!user.BookedEvents.Contains(eventID))
            {
                user.BookedEvents.Add(eventID);
            }

            // Save: DatabaseManager.Instance.SaveData()
            _dbManager.SaveData();

            return true;
        }

        // Cancel a booking
        public bool CancelBooking(string bookingID)
        {
            // Find booking
            Booking booking = _dbManager.GetBooking(bookingID);
            if (booking == null || booking.Status == BookingStatus.Cancelled)
            {
                return false;
            }

            // Get the event
            Event eventObj = _dbManager.GetEvent(booking.EventID);
            if (eventObj == null)
            {
                return false;
            }

            // Update Event: Remove the SeatNumber from the Event's OccupiedSeats list
            if (eventObj.OccupiedSeats.Contains(booking.SeatNumber))
            {
                eventObj.OccupiedSeats.Remove(booking.SeatNumber);
            }

            // Update Counts: Add 1 back to AvailableSeats
            eventObj.AvailableSeats++;

            // Set status to Cancelled
            booking.Status = BookingStatus.Cancelled;

            // Update user's BookedEvents list (remove if no other confirmed bookings for this event)
            User user = _dbManager.GetUser(booking.UserID);
            if (user != null)
            {
                // Check if user has any other confirmed bookings for this event
                bool hasOtherConfirmedBookings = _dbManager.Bookings
                    .Any(b => b.UserID == booking.UserID && 
                              b.EventID == booking.EventID && 
                              b.BookingID != bookingID && 
                              b.Status == BookingStatus.Confirmed);

                if (!hasOtherConfirmedBookings && user.BookedEvents.Contains(booking.EventID))
                {
                    user.BookedEvents.Remove(booking.EventID);
                }
            }

            // Save data
            _dbManager.SaveData();

            return true;
        }

        // Search events with filters using LINQ
        public List<Event> SearchEvents(DateTime? date = null, string category = null, decimal? maxPrice = null)
        {
            var query = _dbManager.Events.AsQueryable();

            // Filter by Date (if provided)
            if (date.HasValue)
            {
                query = query.Where(e => e.Date.Date == date.Value.Date);
            }

            // Filter by Category (if provided)
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by Price (if provided - finds events with price <= maxPrice)
            if (maxPrice.HasValue)
            {
                query = query.Where(e => e.Price <= maxPrice.Value);
            }

            return query.ToList();
        }

        // Helper method to generate unique Booking ID
        private string GenerateBookingID()
        {
            string newID;
            do
            {
                newID = "BK" + DateTime.Now.Ticks.ToString().Substring(10) + new Random().Next(100, 999).ToString();
            } while (_dbManager.Bookings.Any(b => b.BookingID == newID));

            return newID;
        }
    }
}

