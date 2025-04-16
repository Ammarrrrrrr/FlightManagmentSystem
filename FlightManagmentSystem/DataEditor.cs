using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlightManagmentSystem
{
    public partial class DataEditor : Form
    {
        private Dictionary<DataGridView, (DataTable, MySqlDataAdapter)> tableAdapters = new();
        private Dictionary<DataGridView, BindingSource> bindingSources = new();

        public DataEditor()
        {
            InitializeComponent();
        }

        private void DataEditor_Load(object sender, EventArgs e)
        {

        }

        public void LoadData(string table, DataGridView grid)
        {
            var database = new Database();
            if (database.connect_db())
            {
                string query = $"SELECT * FROM {table}";
                MySqlCommand cmd = new MySqlCommand(query, database.mySqlConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);

                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = dt;
                grid.DataSource = bindingSource;

                tableAdapters[grid] = (dt, adapter); // already part of CRUD setup
                bindingSources[grid] = bindingSource; // store this too

                database.close_db();
            }
            else
            {
                MessageBox.Show("Database connection error");
            }
        }

        private void SaveChanges(DataGridView grid)
        {
            if (tableAdapters.TryGetValue(grid, out var data))
            {
                try
                {
                    data.Item2.Update(data.Item1);
                    MessageBox.Show("Changes saved.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving: " + ex.Message);
                }
            }
        }
        private void DeleteSelectedRow(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    grid.Rows.RemoveAt(row.Index);
                }
            }
        }
        private void AddNewRow(DataGridView grid)
        {
            if (bindingSources.TryGetValue(grid, out var source))
            {
                if (source.DataSource is DataTable dt)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }
        }

        private void MovePrevious(DataGridView grid)
        {
            if (bindingSources.TryGetValue(grid, out var source))
            {
                if (source.Position > 0)
                    source.MovePrevious();
            }
        }

        private void MoveNext(DataGridView grid)
        {
            if (bindingSources.TryGetValue(grid, out var source))
            {
                if (source.Position < source.Count - 1)
                    source.MoveNext();
            }
        }



        private void tabPage1_Enter(object sender, EventArgs e)
        {
            LoadData("aircraft", dataGridView1);
        }
        private void tabPage2_Enter(object sender, EventArgs e)
        {
            LoadData("airlines", dataGridView2);
        }
        private void tabPage3_Enter(object sender, EventArgs e)
        {
            LoadData("airports", dataGridView3);
        }
        private void tabPage4_Enter(object sender, EventArgs e)
        {
            LoadData("airport_crew", dataGridView4);
        }
        private void tabPage5_Enter(object sender, EventArgs e)
        {
            LoadData("bookings", dataGridView5);
        }
        private void tabPage6_Enter(object sender, EventArgs e)
        {
            LoadData("flights", dataGridView6);
        }
        private void tabPage7_Enter(object sender, EventArgs e)
        {
            LoadData("passengers", dataGridView7);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MovePrevious(dataGridView1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MoveNext(dataGridView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddNewRow(dataGridView1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveChanges(dataGridView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DeleteSelectedRow(dataGridView1);
        }

        
    }
}
