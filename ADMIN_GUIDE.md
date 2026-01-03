# How to Access Admin Mode

There are **3 ways** to create an Admin account and access admin features:

## Method 1: Using Admin Code (Recommended - Easiest)

1. **Run the application** and click **"Don't have an account? Sign up"**
2. Fill in the sign-up form:
   - **Username**: Choose any username (e.g., "admin")
   - **Password**: Choose any password (e.g., "admin123")
   - **Name**: Your name
   - **Contact**: Your contact info
   - **Admin Code**: Enter `ADMIN2024` (this is the secret admin code)
3. Click **"Sign Up"**
4. You will be logged in as an **Admin** with full access to:
   - Add Event
   - Sales Report
   - All customer features

## Method 2: Manually Edit JSON File

1. **Create a regular account** first (sign up as Customer)
2. **Close the application**
3. Open `booking_data.json` in a text editor
4. Find your user object in the `Users` array
5. Change the `"Role"` field from `"Customer"` to `"Admin"`
6. Save the file
7. **Restart the application** and sign in with your account
8. You will now have Admin access

Example JSON:
```json
{
  "Users": [
    {
      "UserID": "USR123456789",
      "Name": "Admin User",
      "Contact": "admin@example.com",
      "Username": "admin",
      "Password": "admin123",
      "Role": "Admin",  // <-- Change this from "Customer" to "Admin"
      "BookedEvents": []
    }
  ]
}
```

## Method 3: Modify Code Temporarily

1. Open `LoginForm.cs`
2. Find the line that says: `string role = "Customer";`
3. Temporarily change it to: `string role = "Admin";`
4. Create your account
5. Change it back to `"Customer"` after creating the admin account

## Admin Features

Once logged in as Admin, you will see:
- ✅ **Admin Menu** with:
  - **Add Event** - Create new events
  - **Sales Report** - View sales statistics
- ✅ All Customer features:
  - Browse Events
  - My Bookings
  - Search Events

## Customer Features (Non-Admin)

Regular customers will only see:
- Browse Events
- My Bookings
- Search Events
- ❌ Admin menu is **completely hidden**

---

**Note**: The Admin Code `ADMIN2024` can be changed in the code if you want a different secret code for security.

