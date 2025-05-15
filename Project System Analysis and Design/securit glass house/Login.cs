//login.cs
using DataBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SuperMarket
{
    public partial class Login : Form
    {
        public static int UserID;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '●';
        }

        private void label4_Click(object sender, EventArgs e)
        {
            SignIn signIn = new SignIn();
            signIn.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string password = textBox2.Text;
            // check if email and password exist return id
            int id  = UserManager.UserLogin(email, password);

            if (id != 0)
            {
                string role = UserManager.GetRole(id);
                UserID = id;
                MessageBox.Show("Login Successfully.");
                Order d = new Order(role);
                this.Hide();
                d.ShowDialog();

            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }
    }
}
