# User Session Implementation Guide

## Overview
This project now has a simple authentication system implemented using the `UserSession` static class. This allows you to easily access logged-in user data throughout the entire application.

## Features Implemented

### 1. UserSession Class (`Helpers/UserSession.cs`)
A static class that stores the current logged-in user information and provides easy access methods:

```csharp
// Get current user info
UserSession.CurrentUser     // Returns AkunModel? of logged-in user
UserSession.Username        // Returns username string
UserSession.Role           // Returns role string  
UserSession.UserId         // Returns user ID integer
UserSession.IsLoggedIn     // Returns true if user is logged in

// Check user roles
UserSession.IsAdmin()      // Returns true if user is admin
UserSession.IsPetani()     // Returns true if user is petani

// Logout
UserSession.Logout()       // Clears the session
```

### 2. Login System
- Application now starts with `LoginForm` instead of `Container`
- Login validates username/password using `AkunController.Login()`
- Successful login stores user data in `UserSession.CurrentUser`
- After login, opens the main `Container` form

### 3. Container Form Updates
- Checks if user is logged in when opened
- Shows user info in window title
- Provides logout functionality
- Restricts access to admin-only menus

### 4. Access Control Examples
- `AkunIndex`: Only accessible by admin users
- `LahanIndex`: Shows current user info
- `LahanTambah`: Displays who is adding data
- `LahanController`: Logs user actions for audit trail

## How to Use UserSession in Your Files

### In Controllers (for audit logging):
```csharp
using Project_PBO.Helpers;

public static bool AddSomeData(SomeModel data)
{
    // Log who is performing the action
    Console.WriteLine($"User {UserSession.Username} (ID: {UserSession.UserId}) is adding new data");
    
    // Your business logic here
    return data.Insert(data);
}
```

### In Views (for access control):
```csharp
using Project_PBO.Helpers;

public partial class SomeView : UserControl
{
    public SomeView()
    {
        InitializeComponent();
        CheckUserAccess();
        ShowUserInfo();
    }
    
    private void CheckUserAccess()
    {
        if (!UserSession.IsLoggedIn)
        {
            MessageBox.Show("You must login first!", "Access Denied");
            return;
        }
          if (!UserSession.IsAdmin())
        {
            // Disable certain features for non-admin users
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
        }
        
        // Enable features for petani
        if (UserSession.IsPetani())
        {
            // Enable petani-specific features
            btnAddLahan.Enabled = true;
            btnViewReport.Enabled = true;
        }
    }
    
    private void ShowUserInfo()
    {
        lblCurrentUser.Text = $"Logged in as: {UserSession.Username} ({UserSession.Role})";
    }
}
```

### In Forms (for user tracking):
```csharp
using Project_PBO.Helpers;

public partial class AddDataForm : Form
{
    public AddDataForm()
    {
        InitializeComponent();
        SetupUserInfo();
    }
    
    private void SetupUserInfo()
    {
        this.Text = $"Add Data - User: {UserSession.Username}";
        lblCreatedBy.Text = $"Created by: {UserSession.Username}";
    }
    
    private void btnSave_Click(object sender, EventArgs e)
    {
        // You can also store the user ID with the data
        var newData = new SomeModel
        {
            Name = txtName.Text,
            CreatedBy = UserSession.UserId, // Store who created this record
            CreatedDate = DateTime.Now
        };
        
        // Save the data
        SomeController.AddData(newData);
    }
}
```

## Security Notes

This is a **simple** authentication system as requested. For production use, consider:

1. **Password Hashing**: Currently passwords are stored as plain text
2. **Session Security**: User data is stored in memory and lost when app closes
3. **Database Security**: Add proper SQL injection protection
4. **Role-based Permissions**: Create a more robust permission system

## Quick Examples

### Check if user is logged in:
```csharp
if (UserSession.IsLoggedIn)
{
    // User is logged in
    string username = UserSession.Username;
}
```

### Check user role:
```csharp
if (UserSession.IsAdmin())
{
    // Show admin features
    adminPanel.Visible = true;
}
else if (UserSession.IsPetani())
{
    // Show petani features
    petaniPanel.Visible = true;
}
else
{
    // Hide restricted features
    restrictedPanel.Visible = false;
}
```

### Get user info for display:
```csharp
lblWelcome.Text = $"Welcome, {UserSession.Username}!";
lblRole.Text = $"Role: {UserSession.Role}";
```

### Logout user:
```csharp
private void btnLogout_Click(object sender, EventArgs e)
{
    UserSession.Logout();
    // Redirect to login form
    LoginForm loginForm = new LoginForm();
    this.Hide();
    loginForm.ShowDialog();
    this.Close();
}
```

Now you can easily access user data in any file by adding `using Project_PBO.Helpers;` and using the `UserSession` class!
