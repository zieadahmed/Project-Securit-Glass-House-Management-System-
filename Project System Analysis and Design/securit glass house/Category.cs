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
    public partial class Category : Form
    {
        private string role;
        public Category()
        {
            InitializeComponent();
        }
        public Category(string role)
        {
            InitializeComponent();
            this.role = role;
        }
        private void Category_Load(object sender, EventArgs e)
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
            dataGridView1.DataSource = CategoryManager.GetAll();
            dataGridView1.Columns["Image"].Visible = false;
        }
        private void label2_Click(object sender, EventArgs e)
        {
            products prodForm = new products(role);
            this.Hide();
            prodForm.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string name = TxtBox1.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a valid category name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int result = CategoryManager.Insert(name);
            if (result > 0)
            {
                MessageBox.Show("Category added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGrid();
            }
            else
            {
                MessageBox.Show("Failed to add category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TxtBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    var selectedRow = dataGridView1.CurrentRow;
                    int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    string name = TxtBox1.Text;
                    if (name != null)
                    {
                        int count = CategoryManager.Update(id, name);
                        if (count > 0)
                        {
                            MessageBox.Show("Category updated successfully.");
                            LoadDataGrid();
                        }
                        else
                        {
                            MessageBox.Show("Category update failed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var selectedRow = dataGridView1.CurrentRow;
                TxtBox1.Text = selectedRow.Cells["Name"].Value?.ToString() ?? "";
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
                    int count = CategoryManager.Delete(id);
                    if (count > 0)
                    {
                        MessageBox.Show("Category deleted successfully.");
                        LoadDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("Category deletion failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            LoadDataGrid();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string name = textBox4.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a valid category name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dataGridView1.DataSource = CategoryManager.SearchByName(name);
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

        private void label1_Click(object sender, EventArgs e)
        {

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
