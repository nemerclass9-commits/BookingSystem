# Demo Account Credentials

The system automatically creates **2 demo accounts** when you first run it (if no users exist):

## 🔑 Demo Accounts

### Admin Account
- **Username**: `admin`
- **Password**: `admin123`
- **Role**: Admin
- **Access**: Full admin features + all customer features

### Customer Account
- **Username**: `alice`
- **Password**: `password1`
- **Role**: Customer
- **Access**: Search Events, My Bookings

---

## How to Use Demo Accounts

1. **Run the application**
2. On the Login Form, enter one of the demo credentials above
3. Click **"Sign In"**
4. You'll be logged in with the appropriate role

## Admin Features (when logged in as admin)

- ✅ **Admin Menu** visible with:
  - **Add Event** - Create new events
  - **Sales Report** - View sales statistics
- ✅ All Customer features:
  - Browse Events
  - My Bookings
  - Search Events

## Customer Features (when logged in as customer)

- ✅ Browse Events
- ✅ My Bookings
- ✅ Search Events
- ❌ Admin menu is **completely hidden**

---

**Note**: These demo accounts are automatically created only if the database is empty. If you delete the `booking_data.json` file and restart the app, the demo accounts will be recreated.

