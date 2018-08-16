using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFCrudApp
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_cansel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txt_first.Text = txt_last.Text = txt_city.Text = txt_adress.Text = "";
            btn_save.Text = "Save";
            btn_delete.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            model.FirstName = txt_first.Text.Trim();
            model.LastName = txt_last.Text.Trim();
            model.City = txt_city.Text.Trim();
            model.Adress = txt_adress.Text.Trim();
            using (DBEntities db = new DBEntities())
            {
                if (model.CustomerID == 0)
                    db.Customers.Add(model);
                else
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Submitted Successfully");
        }

        void PopulateDataGridView()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using(DBEntities db = new DBEntities())
            {
                dgvCustomer.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["CustomerID"].Value);
                using(DBEntities db = new DBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txt_first.Text = model.FirstName;
                    txt_last.Text = model.LastName;
                    txt_city.Text = model.City;
                    txt_adress.Text = model.Adress;
                }
                btn_save.Text = "Update";
                btn_delete.Enabled = true;
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to delate this record?" , "EF CRUD operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using(DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Customers.Attach(model);
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Deleted Successfully");
                }
            }
        }
    }
}
