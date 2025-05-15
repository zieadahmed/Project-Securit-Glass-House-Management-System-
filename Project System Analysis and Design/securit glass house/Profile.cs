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
    public partial class Profile : Form
    {
        private string role;
        public Profile()
        {
            InitializeComponent();
        }
        public Profile(string role)
        {
            this.role = role;
            InitializeComponent();
        }
        private void ControlVisiblity(object sender, EventArgs e)
        {
            //category
            pictureBox6.Visible = false;
            label1.Visible = false;
            //products
            panel2.Visible = false;
            label2.Visible = false;
            //users
            pictureBox2.Visible = false;
            label3.Visible = false;
            //suppliers
            pictureBox5.Visible = false;
            label5.Visible = false;
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            if (this.role != "Admin")
            {
                ControlVisiblity(sender, e);
            }

            User u = new User();
            u = UserManager.GetById(Login.UserID);
            label10.Text = u.FName;
            label8.Text = u.Email;
            label12.Text = u.Number;
            TxtBox1.PasswordChar = '●';
            TxtBox2.PasswordChar = '●';
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Order d = new Order(role);
            this.Hide();
            d.ShowDialog();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Category c = new Category(role);
            this.Hide();
            c.ShowDialog();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            products products = new products(role);
            this.Hide();
            products.ShowDialog();
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Users d = new Users(role);
            this.Hide();
            d.ShowDialog();
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Supplier s = new Supplier(role);
            this.Hide();
            s.ShowDialog();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string passOld = TxtBox1.Text;
            string passNew = TxtBox2.Text;
            User s = UserManager.GetById(Login.UserID);
            if (s.Password == passOld)
            {
                if (passNew.Length >= 8)
                {
                    UserManager.Update(Login.UserID, s.FName, s.LName, passNew, s.Email, s.Number);
                    MessageBox.Show("Changed Successfully.");
                }
                else
                {
                    MessageBox.Show("Invalid new Password");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid Old Password.");
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
