using DataBusinessLayer;
using SuperMarket.Models;
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
    public partial class Order : Form
    {
        private string role;
        public Order()
        {
            InitializeComponent();
        }
        public Order(string role)
        {
            InitializeComponent();
            this.role = role;
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
            Users userform = new Users();
            this.Hide();
            userform.ShowDialog();
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Supplier supplierForm = new Supplier(role);
            this.Hide();
            supplierForm.ShowDialog();
            this.Close();
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            // Handle selection change if needed
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            dataGridView2.DataSource = ProductManager.GetByName(name);
            dataGridView2.Columns["CategoryID"].Visible = false;
            dataGridView2.Columns["Image"].Visible = false;
            dataGridView2.Columns["SupplierID"].Visible = false;
        }

        private void InitializeComboBox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("");
            comboBox.Items.Add("Cash");
            comboBox.Items.Add("Visa");
            comboBox.SelectedIndex = 0;
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
        private void Order_Load(object sender, EventArgs e)
        {
            if (this.role != "Admin")
            {
                ControlVisiblity(sender, e);
            }
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;

            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Rows[0].Selected = true;
            }

            ConfigureListView();
            InitializeComboBox(comboBox1);
        }

        private void ConfigureListView()
        {
            listView2.View = View.Details;
            listView2.Columns.Add("ID", 50);
            listView2.Columns.Add("Product Name", 150);
            listView2.Columns.Add("Price", 100);
            listView2.Columns.Add("Quantity", 100);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.CurrentRow != null)
                {
                    var selectedRow = dataGridView2.CurrentRow;
                    int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    int quantity = (int)numericUpDown2.Value;
                    if (quantity <= 0)
                    {
                        MessageBox.Show("Please enter a valid quantity.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DataBusinessLayer.Product product = ProductManager.GetById(id);

                    if (product != null)
                    {
                        bool exists = listView2.Items.Cast<ListViewItem>().Any(item => item.Text == id.ToString());

                        if (exists)
                        {
                            MessageBox.Show("Product is already added to the order.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (product.NumOfStock >= quantity)
                        {
                            ListViewItem item = new ListViewItem(product.ID.ToString());
                            item.SubItems.Add(product.Name);
                            item.SubItems.Add(product.Price.ToString());
                            item.SubItems.Add(quantity.ToString());
                            listView2.Items.Add(item);
                            PriceLabel.Text = (Convert.ToDouble(PriceLabel.Text) + (product.Price * quantity)).ToString();

                            MessageBox.Show("Product added to order successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Insufficient stock! Available quantity: " + product.NumOfStock, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Product not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a product from the list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the product to the order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView2.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in listView2.SelectedItems)
                    {
                        listView2.Items.Remove(item);
                        PriceLabel.Text = (Convert.ToDouble(PriceLabel.Text) - (Convert.ToDouble(item.SubItems[2].Text) * Convert.ToDouble(item.SubItems[3].Text))).ToString();
                    }
                    MessageBox.Show("Product removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please select a product from the order list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while removing the product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView2.Items.Count == 0)
                {
                    MessageBox.Show("No products in the order!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string CustomerName = textBox2.Text;
                string CustomerPhone = textBox3.Text;
                string paymentmethod = comboBox1.Text;
                int price;
                bool isValidPrice = int.TryParse(PriceLabel.Text, out price);

                if (!isValidPrice)
                {
                    MessageBox.Show("Invalid price format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrEmpty(CustomerName) || string.IsNullOrEmpty(CustomerPhone) || string.IsNullOrEmpty(paymentmethod))
                {
                    MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int orderID = OrderManager.insert(CustomerName, CustomerPhone, paymentmethod, price, DateTime.Now);

                foreach (ListViewItem item in listView2.Items)
                {
                    int productId = Convert.ToInt32(item.Text);
                    int quantity = Convert.ToInt32(item.SubItems[3].Text);
                    ProductManager.UpdateByProductIDAndQuantity(productId, quantity);
                    ProductOrderManager.Insert(productId, orderID, quantity);
                }
                MessageBox.Show($"Order confirmed successfully {orderID}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listView2.Items.Clear();
                PriceLabel.Text = "0.00";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while confirming the order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            listView2.Items.Clear();
            PriceLabel.Text = "0.00";
            comboBox1.SelectedIndex = 0;
        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            Users d = new Users(role);
            this.Hide();
            d.ShowDialog();
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