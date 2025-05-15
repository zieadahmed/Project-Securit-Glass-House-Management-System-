//Users
using DataBusinessLayer;
using SuperMarket.Models;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SuperMarket
{
    public partial class Users : Form
    {
        private string role;
        public Users()
        {
            InitializeComponent();
        }
        public Users(string role)
        {
            InitializeComponent();
            this.role = role;
        }
        private void label2_Click(object sender, EventArgs e)
        {
            products prodForm = new products(role);
            this.Hide();
            prodForm.ShowDialog();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Category catForm = new Category(role);
            this.Hide();
            catForm.ShowDialog();
            this.Close();
        }
        private void Users_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        private void LoadDataGrid()
        {
            dataGridView1.DataSource = UserManager.GetAll();
            InitializeComboBox(comboBox4);
            InitializeComboBox(comboBox3);
        }

        private void InitializeComboBox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("Choose a role...");
            comboBox.Items.Add("Seller");
            comboBox.Items.Add("User");
            comboBox.Items.Add("Admin");
            comboBox.SelectedIndex = 0;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var selectedRow = dataGridView1.CurrentRow;
                TxtBox1.Text = selectedRow.Cells["Fname"].Value?.ToString() ?? "";
                TxtBox2.Text = selectedRow.Cells["Lname"].Value?.ToString() ?? "";
                textBox1.Text = selectedRow.Cells["Email"].Value?.ToString() ?? "";
                TxtBox3.Text = selectedRow.Cells["Password"].Value?.ToString() ?? "";
                textBox2.Text = selectedRow.Cells["Number"].Value?.ToString() ?? "";
                comboBox4.Text = selectedRow.Cells["Role"].Value?.ToString() ?? "";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            TxtBox1.Text = "";
            TxtBox2.Text = "";
            textBox1.Text = "";
            TxtBox3.Text = "";
            textBox2.Text = "";
            comboBox4.SelectedIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ValidateForm(out string fname, out string lname, out string email, out string password, out string number, out string role))
            {
                int result = UserManager.Insert(fname, lname, email, password, number, role);
                if (result > 0)
                {
                    MessageBox.Show("User added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGrid();
                }
                else
                {
                    MessageBox.Show("User could not be added.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    var selectedRow = dataGridView1.CurrentRow;
                    int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    if (ValidateForm(out string fname, out string lname, out string email, out string password, out string number, out string role))
                    {
                        int count = UserManager.Update(id, fname, lname, password, email, number);
                        if (count > 0)
                        {
                            MessageBox.Show("User updated successfully.");
                            LoadDataGrid();
                        }
                        else
                        {
                            MessageBox.Show("User update failed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm(out string fname, out string lname, out string email, out string password, out string number, out string role)
        {
            fname = TxtBox1.Text;
            lname = TxtBox2.Text;
            email = textBox1.Text;
            password = TxtBox3.Text;
            number = textBox2.Text;
            role = comboBox4.Text;

            if (string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(lname) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(number) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please enter all the fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidPhoneNumber(number))
            {
                MessageBox.Show("Please enter a valid phone number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string number)
        {
            return Regex.IsMatch(number, @"^\d{11}$");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            textBox3.Text = "";
            comboBox3.SelectedIndex = 0;
            LoadDataGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    var selectedRow = dataGridView1.CurrentRow;
                    int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    int count = UserManager.Delete(id);
                    if (count > 0)
                    {
                        MessageBox.Show("User deleted successfully.");
                        LoadDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("User deletion failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string email = textBox4.Text;
            string number = textBox3.Text;
            string role = comboBox3.SelectedIndex == 0 ? "" : comboBox3.Text;
            dataGridView1.DataSource = UserManager.GetByAll(email, number, role);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Order d = new Order(role);
            this.Hide();
            d.ShowDialog();
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Users s = new Users();
            this.Hide();
            s.ShowDialog();
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Supplier d = new Supplier(role);
            this.Hide();
            d.ShowDialog();
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Profile profile = new Profile(role);
            this.Hide();
            profile.ShowDialog();
            this.Close();
        }
    }
}