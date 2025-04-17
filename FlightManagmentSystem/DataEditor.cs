using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FlightManagmentSystem
{
    public partial class DataEditor : Form
    {
        private Dictionary<DataGridView, (DataTable, MySqlDataAdapter)> tableAdapters = new();
        private Dictionary<DataGridView, BindingSource> bindingSources = new();
        private Dictionary<DataGridView, Dictionary<string, TextBox>> gridTextBoxMaps = new();

        public DataEditor()
        {
            InitializeComponent();

            // Hook up dynamic textbox binding on form load
            this.Load += (s, e) =>
            {
                foreach (TabPage tab in tabControl1.TabPages)
                {
                    var grid = tab.Controls.OfType<DataGridView>().FirstOrDefault();
                    var panel = tab.Controls.OfType<Panel>().FirstOrDefault();

                    if (grid != null)
                        grid.SelectionChanged += (sender, args) => {
                            if (gridTextBoxMaps.TryGetValue(grid, out var map))
                                BindRowToTextboxes(grid, map);
                        };
                }
            };
        }

        public void LoadData(string table, DataGridView grid, Panel panel)
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

                BindingSource source = new BindingSource();
                source.DataSource = dt;
                grid.DataSource = source;

                tableAdapters[grid] = (dt, adapter);
                bindingSources[grid] = source;

                var textBoxMap = GenerateTextBoxesFromTable(dt, panel, grid);
                gridTextBoxMaps[grid] = textBoxMap;

                BindRowToTextboxes(grid, textBoxMap);
                AddCrudButtons(panel, grid);

                database.close_db();
            }
            else
            {
                MessageBox.Show("Database connection error");
            }
        }

        private Dictionary<string, TextBox> GenerateTextBoxesFromTable(DataTable table, Panel panel, DataGridView grid)
        {
            Dictionary<string, TextBox> map = new();
            panel.Controls.Clear();

            int y = 10;
            foreach (DataColumn column in table.Columns)
            {
                Label label = new Label
                {
                    Text = column.ColumnName,
                    Location = new Point(10, y),
                    Width = 150
                };
                TextBox textBox = new TextBox
                {
                    Name = $"txt{column.ColumnName}_{grid.Name}",
                    Location = new Point(160, y),
                    Width = 240
                };

                panel.Controls.Add(label);
                panel.Controls.Add(textBox);

                map[column.ColumnName] = textBox;
                y += 30;
            }

            return map;
        }

        private void AddCrudButtons(Panel panel, DataGridView grid)
        {
            int y = panel.Controls.Count > 0 ? panel.Controls[panel.Controls.Count - 1].Bottom + 20 : 10;
            int x = 10;

            void AddButton(string text, EventHandler action)
            {
                var button = new Button
                {
                    Text = text,
                    Location = new Point(x, y),
                    Width = 80
                };
                button.Click += action;
                panel.Controls.Add(button);
                x += 90;
            }

            AddButton("Previous", (s, e) => MovePrevious(grid));
            AddButton("Next", (s, e) => MoveNext(grid));
            AddButton("Add", (s, e) => AddNewRow(grid));
            AddButton("Delete", (s, e) => DeleteSelectedRow(grid));
            AddButton("Save", (s, e) => SaveChanges(grid));
        }

        private void BindRowToTextboxes(DataGridView grid, Dictionary<string, TextBox> mapping)
        {
            if (grid.CurrentRow == null || grid.CurrentRow.Index < 0) return;

            foreach (var kv in mapping)
            {
                string columnName = kv.Key;
                TextBox textBox = kv.Value;

                if (grid.Columns.Contains(columnName))
                    textBox.Text = grid.CurrentRow.Cells[columnName].Value?.ToString() ?? "";
                else
                    textBox.Text = "";
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
                    grid.Rows.RemoveAt(row.Index);
            }
        }

        private void AddNewRow(DataGridView grid)
        {
            if (bindingSources.TryGetValue(grid, out var source) && source.DataSource is DataTable dt)
                dt.Rows.Add(dt.NewRow());
        }

        private void MovePrevious(DataGridView grid)
        {
            if (bindingSources.TryGetValue(grid, out var source) && source.Position > 0)
                source.MovePrevious();
        }

        private void MoveNext(DataGridView grid)
        {
            if (bindingSources.TryGetValue(grid, out var source) && source.Position < source.Count - 1)
                source.MoveNext();
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            LoadData("aircraft", dataGridView1, panel1);
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            LoadData("airlines", dataGridView2, panel1);
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            LoadData("airports", dataGridView3, panel1);
        }

        private void tabPage4_Enter(object sender, EventArgs e)
        {
            LoadData("airport_crew", dataGridView4, panel1);
        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {
            LoadData("bookings", dataGridView5, panel1);
        }

        private void tabPage6_Enter(object sender, EventArgs e)
        {
            LoadData("flights", dataGridView6, panel1);
        }

        private void tabPage7_Enter(object sender, EventArgs e)
        {
            LoadData("passengers", dataGridView7, panel1);
        }
    }
}
