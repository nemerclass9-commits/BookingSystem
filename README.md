# Event Ticket Booking System

A Windows Forms application for managing event bookings with role-based access control.

## Features

- **User Authentication**: Login and Sign-up system
- **Role-Based Access Control**: Admin and Customer roles
- **Event Management**: Browse, search, and book events
- **Seat Booking**: Select specific seats for events
- **Booking Management**: View and cancel bookings
- **Admin Features**: Add events, generate sales reports

## Requirements

- Visual Studio 2022 or later
- .NET 6.0 SDK or later
- Windows OS (for Windows Forms)

## How to Open in Visual Studio

### Method 1: Open Solution File
1. Open Visual Studio
2. Go to **File** → **Open** → **Project/Solution**
3. Navigate to the project folder
4. Select `BookingSystem.sln`
5. Click **Open**

### Method 2: Open Project Folder
1. Open Visual Studio
2. Go to **File** → **Open** → **Folder**
3. Navigate to and select the project folder
4. Visual Studio will detect the `.csproj` file

### Method 3: Double-Click Solution File
1. Navigate to the project folder in Windows Explorer
2. Double-click `BookingSystem.sln`
3. Visual Studio will open automatically

## Running the Application

1. Press **F5** or click the **Start** button in Visual Studio
2. The application will start with the Login Form
3. Sign up for a new account or sign in with existing credentials

## Default Accounts

- Create a new account through the Sign-up form (default role: Customer)
- To create an Admin account, you can manually edit the JSON file or modify the code

## Project Structure

- `Models.cs` - Data model classes (User, Event, Booking)
- `DatabaseManager.cs` - Singleton for JSON data management
- `BookingSystem.cs` - User booking logic
- `AdminSystem.cs` - Admin management logic
- `LoginForm.cs` - Login/Sign-up form
- `MainForm.cs` - Main application form with role-based UI
- `Program.cs` - Application entry point

## Data Storage

All data is stored in `booking_data.json` in the application directory.

