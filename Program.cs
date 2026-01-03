using System;
using System.Windows.Forms;

namespace BookingSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Application starts with LoginForm
            while (true)
            {
                using (LoginForm loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK && loginForm.LoggedInUser != null)
                    {
                        // If login successful, show MainForm
                        using (MainForm mainForm = new MainForm(loginForm.LoggedInUser))
                        {
                            if (mainForm.ShowDialog() == DialogResult.OK)
                            {
                                // User logged out, return to login form
                                continue;
                            }
                            else
                            {
                                // MainForm closed, exit application
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Login cancelled or failed, exit application
                        break;
                    }
                }
            }
        }
    }
}

