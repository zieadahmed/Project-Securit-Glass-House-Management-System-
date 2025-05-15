//signIn
using DataBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarket
{
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void SignIn_Load(object sender, EventArgs e)
        {
            textBox4.PasswordChar = '●';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox4.Text.Trim();
            string email = textBox5.Text.Trim();
            string phone = textBox3.Text.Trim();

            // Username Validation
            if (string.IsNullOrEmpty(username) || username.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters long and contain only letters");
                return;
            }

            // Password Validation
            if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) ||
                !password.Any(char.IsDigit) || !password.Any(ch => "!@#$%^&*?".Contains(ch)))
            {
                MessageBox.Show("Password must be at least 8 characters long and include an uppercase letter, a lowercase letter, a number, and a special character.");
                return;
            }

            // Email Validation
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid email address.");
                return;
            }

            // Phone Number Validation
            if (phone.Length < 11)
            {
                MessageBox.Show("Phone number must be greater than or equal 11 digits.");
                return;
            }

            try
            {
                // Check if user already exists
                if (UserManager.UserExists(username, email))
                {
                    MessageBox.Show("User already exists. Please use a different username or email.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UserManager.Insert(username, "", email, password, phone, "Seller");
                // If all validations pass
                MessageBox.Show("Sign In Successful");
                Login login = new Login();
                login.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {

            Login login = new Login();
            login.Show();
            this.Hide();

        }
    }
}