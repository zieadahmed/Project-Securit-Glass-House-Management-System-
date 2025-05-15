using DataBusinessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SuperMarket
{
    public partial class products : Form
    {
        private string role;
        public products()
        {
            InitializeComponent();
        }
        public products(string role)
        {
            InitializeComponent();
            this.role = role;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
            InitializeComboBoxes();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }
        }

        private void LoadDataGrid()
        {
            dataGridView1.DataSource = ProductManager.GetAll();
            dataGridView1.Columns["CategoryID"].Visible = false;
            dataGridView1.Columns["Image"].Visible = false;
        }

        private void InitializeComboBoxes()
        {
            InitializeComboBox(comboBox2, CategoryManager.GetAllNames(), "Choose");
            InitializeComboBox(comboBox1, CategoryManager.GetAllNames(), "Choose");
            InitializeComboBox(comboBox3, SupplierManager.GetAllNames(), "Choose");
            InitializeComboBox(comboBox4, SupplierManager.GetAllNames(), "Choose");
        }

        private void InitializeComboBox(ComboBox comboBox, List<KeyValuePair<int, string>> dataSource, string defaultText)
        {
            comboBox.DataSource = new BindingSource(dataSource, null);
            comboBox.DisplayMember = "Value";
            comboBox.ValueMember = "Key";
            comboBox.SelectedIndex = -1;
            comboBox.Text = defaultText;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var selectedRow = dataGridView1.CurrentRow;
                TxtBox1.Text = selectedRow.Cells["Name"].Value?.ToString() ?? "";
                TxtBox3.Text = selectedRow.Cells["Price"].Value?.ToString() ?? "";
                TxtBox2.Text = selectedRow.Cells["NumOfStock"].Value?.ToString() ?? "";
                comboBox2.SelectedValue = selectedRow.Cells["CategoryID"].Value ?? DBNull.Value;
                comboBox4.SelectedValue = selectedRow.Cells["ID"].Value ?? DBNull.Value;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearForm();
            InitializeComboBoxes();
        }

        private void ClearForm()
        {
            TxtBox1.Text = "";
            TxtBox2.Text = "";
            TxtBox3.Text = "";
        }

        private Product GetProductFromForm()
        {
            string name = TxtBox1.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a valid product name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            if (!int.TryParse(TxtBox3.Text, out int price))
            {
                MessageBox.Show("Please enter a valid price.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            if (!int.TryParse(TxtBox2.Text, out int quantity))
            {
                MessageBox.Show("Please enter a valid quantity.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            int categoryId = (comboBox2.SelectedItem != null && comboBox2.SelectedValue != null)
                             ? Convert.ToInt32(comboBox2.SelectedValue)
                             : -1;

            int supplierId = (comboBox4.SelectedItem != null && comboBox4.SelectedValue != null)
                             ? Convert.ToInt32(comboBox4.SelectedValue)
                             : -1;

            if (categoryId == -1)
            {
                MessageBox.Show("Please select a valid category.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            if (supplierId == -1)
            {
                MessageBox.Show("Please select a valid supplier.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return new Product
            {
                Name = name,
                Price = price,
                Image = null,
                NumOfStock = quantity,
                CategoryID = categoryId,
                SupplierID = supplierId,
                SupplierName = SupplierManager.GetById(supplierId),
                CategoryName = CategoryManager.GetById(categoryId)
            };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Product product = GetProductFromForm();
                if (product != null)
                {
                    ProductManager.Insert(product);
                    MessageBox.Show("Product saved successfully.");
                    LoadDataGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    Product product = GetProductFromForm();
                    if (product != null)
                    {
                        product.ID = id;
                        int count = ProductManager.Update(product);
                        if (count > 0)
                        {
                            MessageBox.Show("Product updated successfully.");
                            LoadDataGrid();
                        }
                        else
                        {
                            MessageBox.Show("Product update failed.");
                        }
                    }
                }
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
                    int count = ProductManager.Delete(id);
                    if (count > 0)
                    {
                        MessageBox.Show("Product deleted successfully.");
                        LoadDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("Product deletion failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string name = textBox4.Text;
            int CategoryID = (comboBox1.SelectedItem != null && comboBox1.SelectedValue != null)
                             ? Convert.ToInt32(comboBox1.SelectedValue)
                             : -1;
            int SupplierID = (comboBox3.SelectedItem != null && comboBox3.SelectedValue != null)
                             ? Convert.ToInt32(comboBox3.SelectedValue)
                             : -1;

            dataGridView1.DataSource = ProductManager.GetByAll(name, CategoryID, SupplierID);

        }
        private void label1_Click(object sender, EventArgs e)
        {
            Category catForm = new Category(role);
            this.Hide();
            catForm.ShowDialog();
            this.Close();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            LoadDataGrid();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Order d = new Order(role);
            this.Hide();
            d.ShowDialog();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            products products = new products();
            this.Hide();
            products.ShowDialog();
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            this.Hide();
            users.ShowDialog();
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Supplier s = new Supplier();
            this.Hide();
            s.ShowDialog();
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