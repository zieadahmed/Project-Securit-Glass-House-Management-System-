//supplier
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

namespace SuperMarket
{
    public partial class Supplier : Form
    {
        private string role;
        public Supplier()
        {
            InitializeComponent();
        }
        public Supplier(string role)
        {
            InitializeComponent();
            this.role = role;
        }
        private void Supplier_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
            ConfigureDataGridView();
        }

        private void LoadDataGrid()
        {
            dataGridView1.DataSource = SupplierManager.GetAll();
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var selectedRow = dataGridView1.CurrentRow;
                TxtBox1.Text = selectedRow.Cells["Name"].Value?.ToString() ?? "";
                TxtBox2.Text = selectedRow.Cells["Phone"].Value?.ToString() ?? "";
                TxtBox3.Text = selectedRow.Cells["Address"].Value?.ToString() ?? "";
            }
        }

        private void ClearForm()
        {
            TxtBox1.Text = "";
            TxtBox2.Text = "";
            TxtBox3.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            LoadDataGrid();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string number = textBox4.Text;
                dataGridView1.DataSource = SupplierManager.GetByPhone(number);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    var selectedRow = dataGridView1.CurrentRow;
                    int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    int count = SupplierManager.Delete(id);
                    if (count > 0)
                    {
                        MessageBox.Show("Supplier deleted successfully.");
                        LoadDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("Supplier deletion failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ValidateForm(out string name, out string phone, out string address))
            {
                int result = SupplierManager.insert(name, phone, address);
                if (result > 0)
                {
                    MessageBox.Show("Supplier added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGrid();
                }
                else
                {
                    MessageBox.Show("Supplier could not be added.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var selectedRow = dataGridView1.CurrentRow;
                int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                if (ValidateForm(out string name, out string phone, out string address))
                {
                    int result = SupplierManager.Update(id, name, phone, address);
                    if (result > 0)
                    {
                        MessageBox.Show("Supplier updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("Supplier could not be updated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool ValidateForm(out string name, out string phone, out string address)
        {
            name = TxtBox1.Text;
            phone = TxtBox2.Text;
            address = TxtBox3.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please enter all the fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Category catForm = new Category(role);
            this.Hide();
            catForm.ShowDialog();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            products prodForm = new products(role);
            this.Hide();
            prodForm.ShowDialog();
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Users usersForm = new Users(role);
            this.Hide();
            usersForm.ShowDialog();
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Order order = new Order(role);
            this.Hide();
            order.ShowDialog();
            this.Close();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
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