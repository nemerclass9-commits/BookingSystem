# Fix Admin Access Issue

If you login as `admin` / `admin123` but don't see the Admin menu, follow these steps:

## Quick Fix Method 1: Delete and Recreate Database

1. **Close the application completely**
2. **Delete the file**: `booking_data.json` (in the same folder as the .exe or in bin/Debug/net8.0-windows/)
3. **Restart the application**
4. Demo users will be automatically created with correct roles
5. **Login again** with `admin` / `admin123`

## Quick Fix Method 2: Manually Edit JSON File

1. **Close the application**
2. **Open** `booking_data.json` in a text editor
3. **Find** the user with username "admin"
4. **Change** the Role field to exactly: `"Role": "Admin"`
5. **Save** the file
6. **Restart** the application and login again

Example:
```json
{
  "Users": [
    {
      "UserID": "USR_ADMIN_DEMO",
      "Username": "admin",
      "Password": "admin123",
      "Name": "Admin User",
      "Contact": "admin@bookingsystem.com",
      "Role": "Admin",  // <-- Make sure this says "Admin" (not "Customer")
      "BookedEvents": []
    }
  ]
}
```

## Quick Fix Method 3: Use the Auto-Fix Feature

The application now automatically fixes the admin role on startup. Just:
1. **Restart the application**
2. The system will check and fix the admin user's role automatically
3. **Login** with `admin` / `admin123`

## Verify Admin Access

After fixing, when you login as admin, you should see:
- ✅ **Admin Menu** in the menu bar with:
  - Add Event
  - Remove Event
  - Update Seat Capacity
  - Sales Report

If you still don't see it, check:
1. The username is exactly "admin" (case-sensitive for login, but role check is case-insensitive)
2. The Role in JSON is exactly "Admin" (the code checks case-insensitively)
3. You restarted the application after making changes

