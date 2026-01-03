# Admin Features Documentation

## Admin Role Access Control

The system implements **Role-Based Access Control (RBAC)** to ensure that only users with the "Admin" role can access administrative features.

## Admin-Only Features

When logged in as **Admin**, the following features are available:

### 1. **Add Event** ✅
- **Location**: Admin Menu → Add Event
- **Functionality**: Create new events with:
  - Event Name
  - Date
  - Category
  - Price
  - Total Seats
- **Access**: Admin Only

### 2. **Remove Event** ✅
- **Location**: Admin Menu → Remove Event
- **Functionality**: 
  - View all events in a list
  - Select an event to remove
  - Automatically cancels all bookings for that event
  - Removes the event from the database
- **Access**: Admin Only

### 3. **Update Seat Capacity** ✅
- **Location**: Admin Menu → Update Seat Capacity
- **Functionality**:
  - View all events with current capacity
  - Select an event and enter new capacity
  - **Safety Check**: Prevents reducing capacity below highest booked seat
  - Automatically recalculates available seats
- **Access**: Admin Only

### 4. **Sales Report** ✅
- **Location**: Admin Menu → Sales Report
- **Functionality**:
  - View comprehensive sales statistics
  - Shows: Event Name | Seats Sold | Total Revenue
  - Displays grand total across all events
- **Access**: Admin Only

## Customer Features (Non-Admin)

Regular customers with "Customer" role can only access:
- ✅ Browse Events
- ✅ My Bookings
- ✅ Search Events
- ❌ **Admin menu is completely hidden**

## Security Implementation

### Role-Based Visibility
- The `SetupRoleBasedVisibility()` method checks the user's role
- If role is "Admin" → Admin menu is visible
- If role is "Customer" → Admin menu is completely hidden
- Case-insensitive role checking for reliability

### Code Location
- **MainForm.cs**: `SetupRoleBasedVisibility()` method (line ~195)
- **Menu Visibility**: Controlled by `menuAdmin.Visible` property

## Testing Admin Features

### Demo Admin Account
- **Username**: `admin`
- **Password**: `admin123`
- **Role**: Admin

### Steps to Test:
1. Login with admin credentials
2. Verify "Admin" menu appears in the menu bar
3. Test each admin feature:
   - Add a new event
   - Remove an event
   - Update seat capacity
   - View sales report

### Steps to Verify Customer Access:
1. Login with customer credentials (`customer` / `customer123`)
2. Verify "Admin" menu is **NOT visible**
3. Only "Browse Events", "My Bookings", and "Search Events" are available

## Admin Menu Structure

```
Admin Menu
├── Add Event
├── Remove Event
├── Update Seat Capacity
├── ───────────── (separator)
└── Sales Report
```

All menu items are only visible when `_currentUser.Role == "Admin"`.

