using System;
using System.Drawing;
using System.Windows.Forms;

namespace BookingSystem
{
    public partial class LoginForm : Form
    {
        private DatabaseManager _dbManager;
        private bool _isSignUpMode = false;

        public User LoggedInUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            _dbManager = DatabaseManager.Instance;
            SetSignInMode();
        }

        private void InitializeComponent()
        {
            this.Text = "Event Ticket Booking System - Login";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255); // Light blue background

            // Welcome Title
            Label lblWelcome = new Label
            {
                Text = "Welcome to Event Ticket Booking",
                Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(80, 40),
                ForeColor = Color.FromArgb(50, 50, 50)
            };
            this.Controls.Add(lblWelcome);

            // User ID Label
            Label lblUserID = new Label
            {
                Text = "User ID:",
                Location = new Point(100, 120),
                Size = new Size(100, 23),
                Font = new Font("Microsoft Sans Serif", 10)
            };
            this.Controls.Add(lblUserID);

            // User ID TextBox
            TextBox txtUserID = new TextBox
            {
                Location = new Point(200, 117),
                Size = new Size(200, 25),
                Name = "txtUserID",
                Font = new Font("Microsoft Sans Serif", 10),
                UseSystemPasswordChar = false
            };
            this.Controls.Add(txtUserID);

            // Password Label
            Label lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(100, 170),
                Size = new Size(100, 23),
                Font = new Font("Microsoft Sans Serif", 10)
            };
            this.Controls.Add(lblPassword);

            // Password TextBox
            TextBox txtPassword = new TextBox
            {
                Location = new Point(200, 167),
                Size = new Size(200, 25),
                PasswordChar = '*',
                Name = "txtPassword",
                Font = new Font("Microsoft Sans Serif", 10),
                UseSystemPasswordChar = false
            };
            this.Controls.Add(txtPassword);

            // Login Button
            Button btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(100, 230),
                Size = new Size(90, 35),
                Name = "btnLogin",
                BackColor = Color.FromArgb(144, 238, 144), // Light green
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };
            btnLogin.FlatAppearance.BorderColor = Color.Blue;
            btnLogin.FlatAppearance.BorderSize = 2;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // Sign Up Button
            Button btnSignUp = new Button
            {
                Text = "Sign Up",
                Location = new Point(205, 230),
                Size = new Size(90, 35),
                Name = "btnSignUp",
                BackColor = Color.FromArgb(173, 216, 230), // Light blue
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };
            btnSignUp.FlatAppearance.BorderColor = Color.Blue;
            btnSignUp.FlatAppearance.BorderSize = 2;
            btnSignUp.Click += BtnSignUp_Click;
            this.Controls.Add(btnSignUp);

            // Exit Button
            Button btnExit = new Button
            {
                Text = "Exit",
                Location = new Point(310, 230),
                Size = new Size(90, 35),
                Name = "btnExit",
                BackColor = Color.FromArgb(255, 182, 193), // Light red
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };
            btnExit.FlatAppearance.BorderColor = Color.Blue;
            btnExit.FlatAppearance.BorderSize = 2;
            btnExit.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnExit);

            // Demo Accounts Label
            Label lblDemoAccounts = new Label
            {
                Text = "Demo Accounts:",
                Location = new Point(100, 300),
                Size = new Size(150, 20),
                Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50)
            };
            this.Controls.Add(lblDemoAccounts);

            Label lblDemoCustomer = new Label
            {
                Text = "• Customer: alice / password1",
                Location = new Point(100, 325),
                Size = new Size(300, 20),
                Font = new Font("Microsoft Sans Serif", 9),
                ForeColor = Color.FromArgb(50, 50, 50)
            };
            this.Controls.Add(lblDemoCustomer);

            Label lblDemoAdmin = new Label
            {
                Text = "• Admin: admin / admin123",
                Location = new Point(100, 350),
                Size = new Size(300, 20),
                Font = new Font("Microsoft Sans Serif", 9),
                ForeColor = Color.FromArgb(50, 50, 50)
            };
            this.Controls.Add(lblDemoAdmin);

            // Status Label (hidden initially)
            Label lblStatus = new Label
            {
                Text = "",
                Location = new Point(100, 280),
                Size = new Size(300, 20),
                ForeColor = Color.Red,
                Name = "lblStatus",
                Font = new Font("Arial", 9)
            };
            this.Controls.Add(lblStatus);
        }

        private void SetSignInMode()
        {
            _isSignUpMode = false;
            Control btnLogin = this.Controls["btnLogin"];
            Control btnSignUp = this.Controls["btnSignUp"];
            if (btnLogin != null) btnLogin.Visible = true;
            if (btnSignUp != null) btnSignUp.Visible = true;
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            // Open sign up form
            Form signUpForm = new Form
            {
                Text = "Sign Up",
                Size = new Size(400, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            int yPos = 30;
            Font systemFont = new Font("Microsoft Sans Serif", 9);
            Label lblUsername = new Label { Text = "Username:", Location = new Point(30, yPos), Size = new Size(100, 23), Font = systemFont };
            TextBox txtUsername = new TextBox { Location = new Point(140, yPos - 3), Size = new Size(200, 23), Font = systemFont };
            yPos += 40;

            Label lblPassword = new Label { Text = "Password:", Location = new Point(30, yPos), Size = new Size(100, 23), Font = systemFont };
            TextBox txtPassword = new TextBox { Location = new Point(140, yPos - 3), Size = new Size(200, 23), PasswordChar = '*', Font = systemFont };
            yPos += 40;

            Label lblName = new Label { Text = "Name:", Location = new Point(30, yPos), Size = new Size(100, 23), Font = systemFont };
            TextBox txtName = new TextBox { Location = new Point(140, yPos - 3), Size = new Size(200, 23), Font = systemFont };
            yPos += 40;

            Label lblContact = new Label { Text = "Contact:", Location = new Point(30, yPos), Size = new Size(100, 23), Font = systemFont };
            TextBox txtContact = new TextBox { Location = new Point(140, yPos - 3), Size = new Size(200, 23), Font = systemFont };
            yPos += 40;

            Label lblAdminCode = new Label { Text = "Admin Code (optional):", Location = new Point(30, yPos), Size = new Size(120, 23), Font = systemFont };
            TextBox txtAdminCode = new TextBox { Location = new Point(140, yPos - 3), Size = new Size(200, 23), PasswordChar = '*', Font = systemFont };
            yPos += 50;

            Button btnCreate = new Button
            {
                Text = "Create Account",
                Location = new Point(140, yPos),
                Size = new Size(120, 30)
            };
            btnCreate.Click += (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtContact.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_dbManager.CheckUsernameExists(txtUsername.Text))
                {
                    MessageBox.Show("Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string role = txtAdminCode.Text == "ADMIN2024" ? "Admin" : "Customer";
                User newUser = _dbManager.RegisterUser(txtUsername.Text, txtPassword.Text, txtName.Text, txtContact.Text, role);

                if (newUser != null)
                {
                    MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    signUpForm.Close();
                }
            };

            signUpForm.Controls.AddRange(new Control[] { lblUsername, txtUsername, lblPassword, txtPassword, lblName, txtName, lblContact, txtContact, lblAdminCode, txtAdminCode, btnCreate });
            signUpForm.ShowDialog();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            TextBox txtUserID = this.Controls["txtUserID"] as TextBox;
            TextBox txtPassword = this.Controls["txtPassword"] as TextBox;
            Label lblStatus = this.Controls["lblStatus"] as Label;

            if (txtUserID == null || txtPassword == null) return;

            string userID = txtUserID.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(password))
            {
                if (lblStatus != null)
                {
                    lblStatus.Text = "Please enter User ID and Password.";
                    lblStatus.ForeColor = Color.Red;
                }
                return;
            }

            User user = _dbManager.FindUserByUsernameAndPassword(userID, password);

            if (user != null)
            {
                LoggedInUser = user;
                if (lblStatus != null)
                {
                    lblStatus.Text = "Login successful!";
                    lblStatus.ForeColor = Color.Green;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                if (lblStatus != null)
                {
                    lblStatus.Text = "Invalid User ID or Password.";
                    lblStatus.ForeColor = Color.Red;
                }
            }
        }
    }
}
