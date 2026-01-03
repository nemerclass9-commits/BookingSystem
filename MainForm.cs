using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BookingSystem
{
    public partial class MainForm : Form
    {
        private User _currentUser;
        private DatabaseManager _dbManager;
        private BookingSystem _bookingSystem;
        private AdminSystem _adminSystem;

        // UI Components
        private Panel _sidebarPanel;
        private Panel _contentPanel;
        private Button _logoutButton;
        private Label _userLabel;
        private Label _roleLabel;

        // Navigation buttons
        private Button _searchEventsBtn;
        private Button _myBookingsBtn;
        private Button _addEventBtn;
        private Button _manageEventsBtn;
        private Button _salesReportBtn;
        private Button _userManagementBtn;
        private Button _dashboardBtn;

        // Panels for different views
        private Panel _dashboardPanel;
        private Panel _searchPanel;
        private Panel _bookingsPanel;
        private Panel _manageEventsPanel;
        private DataGridView _dgvBookings;
        private DataGridView _dgvEvents;

        public MainForm(User user)
        {
            _currentUser = user;
            _dbManager = DatabaseManager.Instance;
            _bookingSystem = new BookingSystem();
            _adminSystem = new AdminSystem();

            InitializeComponent();
            SetupSidebar();
            SetupContentArea();
            ShowDashboard();
        }

        private void InitializeComponent()
        {
            this.Text = $"Event Ticket Booking System - {_currentUser.Name}";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 600);
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Font = new Font("Microsoft Sans Serif", 9);

            // Create main layout
            _sidebarPanel = new Panel();
            _sidebarPanel.Size = new Size(250, this.ClientSize.Height);
            _sidebarPanel.Location = new Point(0, 0);
            _sidebarPanel.BackColor = Color.FromArgb(51, 51, 51);
            _sidebarPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            _contentPanel = new Panel();
            _contentPanel.Size = new Size(this.ClientSize.Width - 250, this.ClientSize.Height);
            _contentPanel.Location = new Point(250, 0);
            _contentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _contentPanel.BackColor = Color.White;
            _contentPanel.AutoScroll = true;

            this.Controls.AddRange(new Control[] { _sidebarPanel, _contentPanel });
            this.Resize += MainForm_Resize;
        }

        private void SetupSidebar()
        {
            int yOffset = 20;

            // User info section
            _userLabel = new Label();
            _userLabel.Text = _currentUser.Name;
            _userLabel.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            _userLabel.ForeColor = Color.White;
            _userLabel.Location = new Point(20, yOffset);
            _userLabel.AutoSize = true;

            yOffset += 30;

            _roleLabel = new Label();
            _roleLabel.Text = _currentUser.Role.ToUpper();
            _roleLabel.Font = new Font("Microsoft Sans Serif", 9);
            _roleLabel.ForeColor = _currentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) 
                ? Color.LightGreen 
                : Color.LightBlue;
            _roleLabel.Location = new Point(20, yOffset);
            _roleLabel.AutoSize = true;

            yOffset += 40;

            // Dashboard button (always visible)
            _dashboardBtn = CreateSidebarButton("Dashboard", yOffset);
            _dashboardBtn.Click += (s, e) => ShowDashboard();
            yOffset += 50;

            // Navigation buttons based on user role
            if (!_currentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase)) // Customer navigation
            {
                _searchEventsBtn = CreateSidebarButton("Search Events", yOffset);
                _searchEventsBtn.Click += (s, e) => ShowSearchForm();
                yOffset += 50;

                _myBookingsBtn = CreateSidebarButton("My Bookings", yOffset);
                _myBookingsBtn.Click += (s, e) => ShowMyBookings();
                yOffset += 50;
            }
            else // Admin navigation
            {
                yOffset += 20;

                var adminLabel = new Label();
                adminLabel.Text = "ADMINISTRATION";
                adminLabel.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                adminLabel.ForeColor = Color.LightGray;
                adminLabel.Location = new Point(20, yOffset);
                adminLabel.AutoSize = true;
                _sidebarPanel.Controls.Add(adminLabel);

                yOffset += 30;

                _addEventBtn = CreateSidebarButton("Add Event", yOffset);
                _addEventBtn.Click += (s, e) => ShowAddEventForm();
                yOffset += 50;

                _manageEventsBtn = CreateSidebarButton("Manage Events", yOffset);
                _manageEventsBtn.Click += (s, e) => ShowManageEvents();
                yOffset += 50;

                _salesReportBtn = CreateSidebarButton("Sales Report", yOffset);
                _salesReportBtn.Click += (s, e) => ShowSalesReport();
                yOffset += 50;

                _userManagementBtn = CreateSidebarButton("User Management", yOffset);
                _userManagementBtn.Click += (s, e) => ShowUserManagement();
                yOffset += 50;
            }

            // Logout button at bottom
            _logoutButton = CreateSidebarButton("Logout", this.ClientSize.Height - 60);
            _logoutButton.BackColor = Color.FromArgb(231, 76, 60);
            _logoutButton.Click += LogoutButton_Click;

            var sidebarControls = new List<Control> {
                _userLabel, _roleLabel, _dashboardBtn, _logoutButton
            };

            if (!_currentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                sidebarControls.AddRange(new Control[] {
                    _searchEventsBtn, _myBookingsBtn
                });
            }
            else
            {
                sidebarControls.AddRange(new Control[] {
                    _addEventBtn, _manageEventsBtn, _salesReportBtn, _userManagementBtn
                });
            }

            _sidebarPanel.Controls.AddRange(sidebarControls.ToArray());
        }

        private Button CreateSidebarButton(string text, int yPosition)
        {
            var button = new Button();
            button.Text = text;
            button.Size = new Size(210, 40);
            button.Location = new Point(20, yPosition);
            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = Color.FromArgb(64, 64, 64);
            button.ForeColor = Color.White;
            button.Font = new Font("Microsoft Sans Serif", 10);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(15, 0, 0, 0);
            button.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(89, 89, 89);
            button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(64, 64, 64);

            return button;
        }

        private void SetupContentArea()
        {
            // Initialize all panels
            InitializeDashboard();
            InitializeSearchView();
            InitializeBookingsView();
            InitializeManageEventsView();
        }

        private void InitializeDashboard()
        {
            _dashboardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.White
            };
            _contentPanel.Controls.Add(_dashboardPanel);
        }

        private void InitializeSearchView()
        {
            _searchPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.White,
                Padding = new Padding(30)
            };

            Label title = new Label
            {
                Text = "Search Events",
                Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold),
                Location = new Point(30, 30),
                AutoSize = true,
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            // TableLayoutPanel for form layout
            TableLayoutPanel grid = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 4,
                Location = new Point(30, 100),
                Size = new Size(600, 200),
                AutoSize = true
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));

            TextBox txtCat = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft Sans Serif", 11),
                Name = "txtCategory",
                Margin = new Padding(5)
            };
            TextBox txtPrice = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft Sans Serif", 11),
                Name = "txtMaxPrice",
                Margin = new Padding(5)
            };
            DateTimePicker dtp = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Microsoft Sans Serif", 11),
                Name = "dtpDate",
                Margin = new Padding(5)
            };

            grid.Controls.Add(new Label { Text = "Category:", Anchor = AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Microsoft Sans Serif", 10), TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            grid.Controls.Add(txtCat, 1, 0);
            grid.Controls.Add(new Label { Text = "Max Price ($):", Anchor = AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Microsoft Sans Serif", 10), TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
            grid.Controls.Add(txtPrice, 1, 1);
            grid.Controls.Add(new Label { Text = "Event Date:", Anchor = AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Microsoft Sans Serif", 10), TextAlign = ContentAlignment.MiddleLeft }, 0, 2);
            grid.Controls.Add(dtp, 1, 2);

            Button btnSearch = new Button
            {
                Text = "Search Events",
                Size = new Size(150, 40),
                Location = new Point(30, 320),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += (s, e) =>
            {
                DateTime? date = dtp.Checked ? dtp.Value.Date : (DateTime?)null;
                string category = string.IsNullOrWhiteSpace(txtCat.Text) ? null : txtCat.Text;
                decimal? maxPrice = decimal.TryParse(txtPrice.Text, out decimal p) ? p : (decimal?)null;
                var results = _bookingSystem.SearchEvents(date, category, maxPrice);
                ShowSearchResults(results);
            };

            _searchPanel.Controls.AddRange(new Control[] { title, grid, btnSearch });
            _contentPanel.Controls.Add(_searchPanel);
        }

        private void InitializeBookingsView()
        {
            _bookingsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.White
            };

            Label title = new Label
            {
                Text = "My Bookings",
                Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold),
                Location = new Point(30, 30),
                AutoSize = true,
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            _dgvBookings = new DataGridView
            {
                Name = "dgvMyBookings",
                Location = new Point(30, 80),
                Size = new Size(_contentPanel.Width - 60, _contentPanel.Height - 110),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            _dgvBookings.CellDoubleClick += DgvBookings_CellDoubleClick;

            _bookingsPanel.Controls.AddRange(new Control[] { title, _dgvBookings });
            _contentPanel.Controls.Add(_bookingsPanel);
        }

        private void InitializeManageEventsView()
        {
            _manageEventsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.White
            };

            Label title = new Label
            {
                Text = "Event Management",
                Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold),
                Location = new Point(30, 30),
                AutoSize = true,
                ForeColor = Color.FromArgb(51, 51, 51)
            };

            _dgvEvents = new DataGridView
            {
                Name = "dgvManageEvents",
                Location = new Point(30, 80),
                Size = new Size(_contentPanel.Width - 60, _contentPanel.Height - 110),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            _dgvEvents.CellDoubleClick += DgvEvents_CellDoubleClick;

            _manageEventsPanel.Controls.AddRange(new Control[] { title, _dgvEvents });
            _contentPanel.Controls.Add(_manageEventsPanel);
        }

        private void ShowDashboard()
        {
            HideAllPanels();
            _dashboardPanel.Visible = true;
            _dashboardPanel.Controls.Clear();

            var titleLabel = new Label();
            titleLabel.Text = "Dashboard";
            titleLabel.Font = new Font("Microsoft Sans Serif", 24, FontStyle.Bold);
            titleLabel.Location = new Point(30, 20);
            titleLabel.AutoSize = true;
            titleLabel.ForeColor = Color.FromArgb(51, 51, 51);

            var subtitleLabel = new Label();
            subtitleLabel.Text = _currentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ?
                $"Welcome back, {_currentUser.Name}! Manage your event system." :
                $"Welcome back, {_currentUser.Name}! Explore and book amazing events.";
            subtitleLabel.Font = new Font("Microsoft Sans Serif", 14);
            subtitleLabel.Location = new Point(30, 60);
            subtitleLabel.AutoSize = true;
            subtitleLabel.ForeColor = Color.FromArgb(102, 102, 102);

            var controls = new List<Control> { titleLabel, subtitleLabel };

            if (_currentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin sees system-wide statistics
                int cardY = 120;
                var eventsCard = CreateStatsCard("Active Events", GetActiveEventsCount().ToString(),
                                               Color.FromArgb(52, 152, 219), 30, cardY);
                var usersCard = CreateStatsCard("Registered Users", GetRegisteredUsersCount().ToString(),
                                              Color.FromArgb(155, 89, 182), 350, cardY);
                var bookingsCard = CreateStatsCard("Total Bookings", GetTotalBookingsCount().ToString(),
                                                 Color.FromArgb(46, 204, 113), 670, cardY);

                cardY += 120;
                var revenueCard = CreateStatsCard("Total Revenue", $"${GetTotalRevenue():F2}",
                                                Color.FromArgb(230, 126, 34), 30, cardY);
                controls.AddRange(new Control[] { eventsCard, usersCard, bookingsCard, revenueCard });
            }
            else
            {
                // Customers see only their personal statistics
                int cardY = 120;
                var availableEventsCard = CreateStatsCard("Available Events", GetAvailableEventsCount().ToString(),
                                                        Color.FromArgb(52, 152, 219), 30, cardY);
                var myBookingsCard = CreateStatsCard("My Bookings", GetMyBookingsCount().ToString(),
                                                   Color.FromArgb(46, 204, 113), 350, cardY);

                cardY += 120;
                var totalCostCard = CreateStatsCard("Your Total Cost", $"${GetMyTotalCost():F2}",
                                                  Color.FromArgb(230, 126, 34), 30, cardY);

                controls.AddRange(new Control[] { availableEventsCard, myBookingsCard, totalCostCard });
            }

            _dashboardPanel.Controls.AddRange(controls.ToArray());
        }

        private Panel CreateStatsCard(string title, string value, Color color, int x, int y)
        {
            var card = new Panel();
            card.Size = new Size(280, 100);
            card.Location = new Point(x, y);
            card.BackColor = color;
            card.BorderStyle = BorderStyle.None;

            // Add shadow effect
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(100, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            var titleLabel = new Label();
            titleLabel.Text = title;
            titleLabel.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Location = new Point(20, 15);
            titleLabel.AutoSize = true;

            var valueLabel = new Label();
            valueLabel.Text = value;
            valueLabel.Font = new Font("Microsoft Sans Serif", 28, FontStyle.Bold);
            valueLabel.ForeColor = Color.White;
            valueLabel.Location = new Point(20, 40);
            valueLabel.AutoSize = true;

            card.Controls.AddRange(new Control[] { titleLabel, valueLabel });
            return card;
        }

        private void ShowSearchForm()
        {
            HideAllPanels();
            _searchPanel.Visible = true;
        }

        private void ShowMyBookings()
        {
            HideAllPanels();
            _bookingsPanel.Visible = true;
            LoadMyBookings();
        }

        private void ShowManageEvents()
        {
            HideAllPanels();
            _manageEventsPanel.Visible = true;
            LoadEvents();
        }

        private void HideAllPanels()
        {
            if (_dashboardPanel != null) _dashboardPanel.Visible = false;
            if (_searchPanel != null) _searchPanel.Visible = false;
            if (_bookingsPanel != null) _bookingsPanel.Visible = false;
            if (_manageEventsPanel != null) _manageEventsPanel.Visible = false;
        }

        // Statistics Methods
        private int GetActiveEventsCount() => _dbManager.Events.Count;
        private int GetRegisteredUsersCount() => _dbManager.Users.Count;
        private int GetTotalBookingsCount() => _dbManager.Bookings.Count(b => b.Status == BookingStatus.Confirmed);
        private decimal GetTotalRevenue() => _dbManager.Bookings
            .Where(b => b.Status == BookingStatus.Confirmed)
            .Sum(b => _dbManager.GetEvent(b.EventID)?.Price ?? 0);

        private int GetAvailableEventsCount() => _dbManager.Events.Count(e => e.AvailableSeats > 0);
        private int GetMyBookingsCount() => _dbManager.Bookings.Count(b => b.UserID == _currentUser.UserID && b.Status == BookingStatus.Confirmed);
        private decimal GetMyTotalCost() => _dbManager.Bookings
            .Where(b => b.UserID == _currentUser.UserID && b.Status == BookingStatus.Confirmed)
            .Sum(b => _dbManager.GetEvent(b.EventID)?.Price ?? 0);

        private void LoadEvents()
        {
            _dgvEvents.DataSource = _dbManager.Events.Select(e => new
            {
                e.EventID,
                e.Name,
                e.Date,
                e.Category,
                Price = $"${e.Price:F2}",
                e.TotalSeats,
                e.AvailableSeats
            }).ToList();
        }

        private void LoadMyBookings()
        {
            var bookings = _dbManager.Bookings
                .Where(b => b.UserID == _currentUser.UserID && b.Status == BookingStatus.Confirmed)
                .ToList();

            _dgvBookings.DataSource = bookings.Select(b =>
            {
                var evt = _dbManager.GetEvent(b.EventID);
                return new
                {
                    b.BookingID,
                    EventName = evt?.Name ?? "Unknown",
                    EventDate = evt?.Date ?? DateTime.MinValue,
                    b.SeatNumber,
                    Price = $"${evt?.Price ?? 0:F2}"
                };
            }).ToList();
        }

        private void DgvEvents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;
            string eventID = dgv.Rows[e.RowIndex].Cells["EventID"].Value?.ToString();
            if (!string.IsNullOrEmpty(eventID))
            {
                ShowEventManagementDialog(eventID);
            }
        }

        private void DgvBookings_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;
            string bookingID = dgv.Rows[e.RowIndex].Cells["BookingID"].Value?.ToString();
            if (!string.IsNullOrEmpty(bookingID))
            {
                var result = MessageBox.Show("Do you want to cancel this booking?", "Cancel Booking", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (_bookingSystem.CancelBooking(bookingID))
                    {
                        MessageBox.Show("Booking cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMyBookings();
                        ShowDashboard();
                    }
                }
            }
        }

        private void ShowSearchResults(List<Event> events)
        {
            Form resultsForm = new Form
            {
                Text = "Search Results",
                Size = new Size(700, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };
            dgv.DataSource = events.Select(e => new { e.Name, e.Date, e.Category, Price = $"${e.Price:F2}", e.AvailableSeats }).ToList();
            dgv.CellDoubleClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0)
                {
                    var selectedEvent = events[ev.RowIndex];
                    ShowBookingDialog(selectedEvent.EventID);
                }
            };
            resultsForm.Controls.Add(dgv);
            resultsForm.ShowDialog();
        }

        private void ShowBookingDialog(string eventID)
        {
            Event eventObj = _dbManager.GetEvent(eventID);
            if (eventObj == null) return;

            Form bookingForm = new Form
            {
                Text = $"Book Seat - {eventObj.Name}",
                Size = new Size(400, 300),
                StartPosition = FormStartPosition.CenterParent
            };

            Label lblInfo = new Label
            {
                Text = $"Event: {eventObj.Name}\nDate: {eventObj.Date:MM/dd/yyyy}\nPrice: ${eventObj.Price:F2}\nAvailable Seats: {eventObj.AvailableSeats}",
                Location = new Point(20, 20),
                Size = new Size(350, 80)
            };
            bookingForm.Controls.Add(lblInfo);

            Label lblSeat = new Label { Text = "Select Seat Number:", Location = new Point(20, 110), Size = new Size(150, 23) };
            ComboBox cmbSeats = new ComboBox { Location = new Point(180, 107), Size = new Size(150, 23), DropDownStyle = ComboBoxStyle.DropDownList };
            var availableSeats = _bookingSystem.GetAvailableSeats(eventID);
            cmbSeats.DataSource = availableSeats;
            bookingForm.Controls.AddRange(new Control[] { lblSeat, cmbSeats });

            Button btnBook = new Button { Text = "Book Seat", Location = new Point(120, 150), Size = new Size(100, 30) };
            btnBook.Click += (s, args) =>
            {
                if (cmbSeats.SelectedItem != null)
                {
                    int seatNumber = (int)cmbSeats.SelectedItem;
                    if (_bookingSystem.BookSeat(_currentUser.UserID, eventID, seatNumber))
                    {
                        MessageBox.Show($"Seat {seatNumber} booked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bookingForm.Close();
                        ShowDashboard();
                    }
                    else
                    {
                        MessageBox.Show("Seat is no longer available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
            bookingForm.Controls.Add(btnBook);
            bookingForm.ShowDialog();
        }

        private void ShowEventManagementDialog(string eventID)
        {
            Event eventObj = _dbManager.GetEvent(eventID);
            if (eventObj == null) return;

            Form mgmtForm = new Form
            {
                Text = $"Manage Event - {eventObj.Name}",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            Label lblInfo = new Label
            {
                Text = $"Event: {eventObj.Name}\nDate: {eventObj.Date:MM/dd/yyyy}\nCategory: {eventObj.Category}\nPrice: ${eventObj.Price:F2}\nTotal Seats: {eventObj.TotalSeats}\nAvailable: {eventObj.AvailableSeats}",
                Location = new Point(20, 20),
                Size = new Size(450, 100)
            };
            mgmtForm.Controls.Add(lblInfo);

            Button btnRemove = new Button { Text = "Remove Event", Location = new Point(20, 140), Size = new Size(120, 30), BackColor = Color.Red, ForeColor = Color.White };
            btnRemove.Click += (s, e) =>
            {
                if (MessageBox.Show("Are you sure you want to remove this event?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (_adminSystem.RemoveEvent(eventID))
                    {
                        MessageBox.Show("Event removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        mgmtForm.Close();
                        LoadEvents();
                        ShowDashboard();
                    }
                }
            };
            mgmtForm.Controls.Add(btnRemove);

            Label lblNewCapacity = new Label { Text = "New Capacity:", Location = new Point(20, 190), Size = new Size(100, 23) };
            TextBox txtCapacity = new TextBox { Location = new Point(130, 187), Size = new Size(100, 23) };
            Button btnUpdate = new Button { Text = "Update Capacity", Location = new Point(240, 185), Size = new Size(120, 30) };
            btnUpdate.Click += (s, e) =>
            {
                if (int.TryParse(txtCapacity.Text, out int newCapacity))
                {
                    string result = _adminSystem.UpdateSeatCapacity(eventID, newCapacity);
                    MessageBox.Show(result, result.StartsWith("Success") ? "Success" : "Error", MessageBoxButtons.OK,
                        result.StartsWith("Success") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                    if (result.StartsWith("Success"))
                    {
                        mgmtForm.Close();
                        LoadEvents();
                        ShowDashboard();
                    }
                }
            };
            mgmtForm.Controls.AddRange(new Control[] { lblNewCapacity, txtCapacity, btnUpdate });

            mgmtForm.ShowDialog();
        }

        private void ShowAddEventForm()
        {
            Form addEventForm = new Form
            {
                Text = "Add New Event",
                Size = new Size(400, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            int yPos = 20;
            Font systemFont = new Font("Microsoft Sans Serif", 9);
            Label lblName = new Label { Text = "Event Name:", Location = new Point(20, yPos), Size = new Size(100, 23), Font = systemFont };
            TextBox txtName = new TextBox { Location = new Point(130, yPos - 3), Size = new Size(200, 23), Font = systemFont };
            yPos += 40;

            Label lblDate = new Label { Text = "Date:", Location = new Point(20, yPos), Size = new Size(100, 23), Font = systemFont };
            DateTimePicker dtpDate = new DateTimePicker { Location = new Point(130, yPos - 3), Size = new Size(200, 23), Font = systemFont };
            yPos += 40;

            Label lblCategory = new Label { Text = "Category:", Location = new Point(20, yPos), Size = new Size(100, 23), Font = systemFont };
            TextBox txtCategory = new TextBox { Location = new Point(130, yPos - 3), Size = new Size(200, 23), Font = systemFont };
            yPos += 40;

            Label lblPrice = new Label { Text = "Price:", Location = new Point(20, yPos), Size = new Size(100, 23), Font = systemFont };
            TextBox txtPrice = new TextBox { Location = new Point(130, yPos - 3), Size = new Size(200, 23), Font = systemFont };
            yPos += 40;

            Label lblSeats = new Label { Text = "Total Seats:", Location = new Point(20, yPos), Size = new Size(100, 23), Font = systemFont };
            TextBox txtSeats = new TextBox { Location = new Point(130, yPos - 3), Size = new Size(200, 23), Font = systemFont };
            yPos += 40;

            Button btnAdd = new Button
            {
                Text = "Add Event",
                Location = new Point(130, yPos),
                Size = new Size(100, 30)
            };
            btnAdd.Click += (s, args) =>
            {
                if (decimal.TryParse(txtPrice.Text, out decimal price) && int.TryParse(txtSeats.Text, out int seats))
                {
                    var newEvent = _adminSystem.AddEvent(txtName.Text, dtpDate.Value, txtCategory.Text, price, seats);
                    if (newEvent != null)
                    {
                        MessageBox.Show("Event added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        addEventForm.Close();
                        ShowDashboard();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter valid price and seat numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            addEventForm.Controls.AddRange(new Control[] { lblName, txtName, lblDate, dtpDate, lblCategory, txtCategory, lblPrice, txtPrice, lblSeats, txtSeats, btnAdd });
            addEventForm.ShowDialog();
        }

        private void ShowSalesReport()
        {
            string report = _adminSystem.GenerateSalesReport();
            MessageBox.Show(report, "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowUserManagement()
        {
            Form userMgmtForm = new Form
            {
                Text = "User Management",
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            DataGridView dgvUsers = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            dgvUsers.DataSource = _dbManager.Users.Select(u => new
            {
                u.Username,
                u.Name,
                u.Contact,
                u.Role,
                Bookings = _dbManager.Bookings.Count(b => b.UserID == u.UserID && b.Status == BookingStatus.Confirmed)
            }).ToList();

            userMgmtForm.Controls.Add(dgvUsers);
            userMgmtForm.ShowDialog();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (_sidebarPanel != null && _contentPanel != null)
            {
                _sidebarPanel.Height = this.ClientSize.Height;
                _contentPanel.Size = new Size(this.ClientSize.Width - 250, this.ClientSize.Height);
                _contentPanel.Location = new Point(250, 0);
            }
        }
    }
}
