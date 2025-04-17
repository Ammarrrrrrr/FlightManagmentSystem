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
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
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

                //textBox.TextChanged += (s, e) => ApplyFilter(grid);  // 🔥 This adds search behavior

                panel.Controls.Add(label);
                panel.Controls.Add(textBox);

                map[column.ColumnName] = textBox;
                y += 30;
            }

            return map;
        }
        private void ApplyFilter(DataGridView grid)
        {
            if (!gridTextBoxMaps.TryGetValue(grid, out var map) ||
                !bindingSources.TryGetValue(grid, out var source) ||
                source.DataSource is not DataTable dt)
                return;

            List<string> filters = new();

            foreach (var kvp in map)
            {
                string column = kvp.Key;
                string input = kvp.Value.Text.Trim().Replace("'", "''");

                if (!string.IsNullOrEmpty(input))
                {
                    if (dt.Columns[column].DataType == typeof(string))
                        filters.Add($"CONVERT([{column}], 'System.String') LIKE '%{input}%'");
                    else
                        filters.Add($"CONVERT([{column}], 'System.String') LIKE '{input}%'");
                }
            }

            source.Filter = string.Join(" AND ", filters);
        }



        private void AddCrudButtons(Panel panel, DataGridView grid)
        {
            int buttonWidth = 100;
            int buttonHeight = 30;
            int margin = 10;
            int y = panel.Controls.Count > 0 ? panel.Controls[panel.Controls.Count - 1].Bottom + margin * 2 : 10;
            int x = 10;

            void AddButton(string text, EventHandler action)
            {
                var button = new Button
                {
                    Text = text,
                    Location = new Point(x, y),
                    Size = new Size(buttonWidth, buttonHeight),
                    AutoSize = true,
                    UseVisualStyleBackColor = true
                };
                button.Click += action;
                panel.Controls.Add(button);
                x += buttonWidth + margin;
            }

            AddButton("Previous", (s, e) => MovePrevious(grid));
            AddButton("Next", (s, e) => MoveNext(grid));
            AddButton("Add", (s, e) => AddNewRow(grid));
            AddButton("Delete", (s, e) => DeleteSelectedRow(grid));
            AddButton("Save", (s, e) => SaveChanges(grid));
            AddButton("Search", (s, e) => ApplyFilter(grid)); // 🔍 SEARCH
            AddButton("Clear", (s, e) => ClearSearchAndSelection(grid)); // ❌ CLEAR
        }

        private void ClearSearchAndSelection(DataGridView grid)
        {
            if (gridTextBoxMaps.TryGetValue(grid, out var map))
            {
                foreach (var tb in map.Values)
                    tb.Text = string.Empty;
            }

            if (bindingSources.TryGetValue(grid, out var source))
                source.Filter = string.Empty;

            grid.ClearSelection();
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
            if (!gridTextBoxMaps.TryGetValue(grid, out var map) ||
                !tableAdapters.TryGetValue(grid, out var data) ||
                !bindingSources.TryGetValue(grid, out var source))
                return;

            DataTable dt = data.Item1;
            MySqlDataAdapter adapter = data.Item2;

            if (adapter.SelectCommand == null)
            {
                MessageBox.Show("Internal error: DataAdapter is not properly initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow row;
            bool isNewRow = false;

            if (grid.CurrentRow == null || grid.CurrentRow.IsNewRow || source.Position < 0)
            {
                row = dt.NewRow();
                dt.Rows.Add(row);
                isNewRow = true;
                source.MoveLast();
            }
            else
            {
                row = ((DataRowView)source.Current).Row;
            }

            foreach (var kvp in map)
            {
                string column = kvp.Key;
                TextBox textBox = kvp.Value;
                string value = textBox.Text.Trim();

                if (!dt.Columns.Contains(column)) continue;

                try
                {
                    var colType = dt.Columns[column].DataType;
                    object converted = string.IsNullOrWhiteSpace(value)
                        ? DBNull.Value
                        : Convert.ChangeType(value, Nullable.GetUnderlyingType(colType) ?? colType);

                    row[column] = converted;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Invalid input for column '{column}': {ex.Message}");
                    return;
                }
            }

            try
            {
                adapter.Update(dt);
                dt.AcceptChanges();
                MessageBox.Show(isNewRow ? "New record inserted." : "Changes saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
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
            if (!gridTextBoxMaps.TryGetValue(grid, out var map) ||
                !tableAdapters.TryGetValue(grid, out var data) ||
                !bindingSources.TryGetValue(grid, out var source))
            {
                MessageBox.Show("Grid binding is not ready.");
                return;
            }

            DataTable dt = data.Item1;
            MySqlDataAdapter adapter = data.Item2;

            if (adapter.SelectCommand == null)
            {
                MessageBox.Show("Internal error: DataAdapter is not properly initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow newRow = dt.NewRow();

            foreach (var kvp in map)
            {
                string columnName = kvp.Key;
                TextBox textBox = kvp.Value;

                if (!dt.Columns.Contains(columnName))
                    continue;

                string input = textBox.Text.Trim();
                var column = dt.Columns[columnName];
                object value;

                try
                {
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        value = DBNull.Value;
                    }
                    else
                    {
                        var colType = Nullable.GetUnderlyingType(column.DataType) ?? column.DataType;
                        value = Convert.ChangeType(input, colType);
                    }

                    newRow[columnName] = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error in field '{columnName}': {ex.Message}", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            dt.Rows.Add(newRow);
            source.MoveLast();
            ClearTextBoxes(map);
            MessageBox.Show("Row added. You can continue entering data or save.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearTextBoxes(Dictionary<string, TextBox> textBoxMap)
        {
            foreach (var tb in textBoxMap.Values)
            {
                tb.Text = string.Empty;
            }
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