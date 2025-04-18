using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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
                    if (grid != null)
                    {
                        grid.SelectionChanged += (sender, args) =>
                        {
                            if (gridTextBoxMaps.TryGetValue(grid, out var map))
                                BindRowToTextboxes(grid, map);
                        };
                    }
                }
            };
        }

        public void LoadData(string table, DataGridView grid, Panel panel)
        {
            var database = new Database();
            if (!database.connect_db())
            {
                MessageBox.Show("Database connection error");
                return;
            }

            string query = $"SELECT * FROM {table}";
            MySqlCommand cmd = new(query, database.mySqlConnection);
            MySqlDataAdapter adapter = new() { SelectCommand = cmd };
            DataTable dt = new();
            adapter.Fill(dt);
            new MySqlCommandBuilder(adapter);

            BindingSource source = new() { DataSource = dt };
            grid.DataSource = source;

            tableAdapters[grid] = (dt, adapter);
            bindingSources[grid] = source;

            var textBoxMap = GenerateTextBoxesFromTable(dt, panel, grid);
            gridTextBoxMaps[grid] = textBoxMap;

            BindRowToTextboxes(grid, textBoxMap);
            AddCrudButtons(panel, grid);

            database.close_db();
        }

        private Dictionary<string, TextBox> GenerateTextBoxesFromTable(DataTable table, Panel panel, DataGridView grid)
        {
            Dictionary<string, TextBox> map = new();
            panel.Controls.Clear();

            int y = 10;
            foreach (DataColumn column in table.Columns)
            {
                Label label = new()
                {
                    Text = column.ColumnName,
                    Location = new Point(10, y),
                    Width = 150
                };
                TextBox textBox = new()
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

        private void BindRowToTextboxes(DataGridView grid, Dictionary<string, TextBox> mapping)
        {
            if (grid.CurrentRow == null || grid.CurrentRow.Index < 0) return;

            foreach (var (columnName, textBox) in mapping)
            {
                textBox.Text = grid.Columns.Contains(columnName)
                    ? grid.CurrentRow.Cells[columnName].Value?.ToString() ?? ""
                    : "";
            }
        }

        private void ApplyFilter(DataGridView grid)
        {
            if (!gridTextBoxMaps.TryGetValue(grid, out var map) ||
                !bindingSources.TryGetValue(grid, out var source) ||
                source.DataSource is not DataTable dt)
                return;

            List<string> filters = new();

            foreach (var (column, textBox) in map)
            {
                string input = textBox.Text.Trim().Replace("'", "''");
                if (string.IsNullOrEmpty(input)) continue;

                string filter = $"CONVERT([{column}], 'System.String') LIKE ";
                filter += dt.Columns[column].DataType == typeof(string)
                    ? $"'%{input}%'"
                    : $"'{input}%'";
                filters.Add(filter);
            }

            source.Filter = string.Join(" AND ", filters);
        }

        private void AddCrudButtons(Panel panel, DataGridView grid)
        {
            int buttonWidth = 100, buttonHeight = 30, margin = 10;
            int y = panel.Controls.Count > 0 ? panel.Controls[^1].Bottom + margin * 2 : 10;
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
            AddButton("Search", (s, e) => ApplyFilter(grid));
            AddButton("Clear", (s, e) => ClearSearchAndSelection(grid));
        }

        private void ClearSearchAndSelection(DataGridView grid)
        {
            if (gridTextBoxMaps.TryGetValue(grid, out var map))
                foreach (var tb in map.Values) tb.Text = string.Empty;

            if (bindingSources.TryGetValue(grid, out var source))
                source.Filter = string.Empty;

            grid.ClearSelection();
        }

        private void SaveChanges(DataGridView grid)
        {
            if (!gridTextBoxMaps.TryGetValue(grid, out var map) ||
                !tableAdapters.TryGetValue(grid, out var data) ||
                !bindingSources.TryGetValue(grid, out var source))
                return;

            var (dt, adapter) = data;

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

            foreach (var (column, textBox) in map)
            {
                if (!dt.Columns.Contains(column)) continue;

                try
                {
                    var colType = dt.Columns[column].DataType;
                    object converted = string.IsNullOrWhiteSpace(textBox.Text)
                        ? DBNull.Value
                        : Convert.ChangeType(textBox.Text.Trim(), Nullable.GetUnderlyingType(colType) ?? colType);
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

        private void AddNewRow(DataGridView grid)
        {
            if (!gridTextBoxMaps.TryGetValue(grid, out var map) ||
                !tableAdapters.TryGetValue(grid, out var data) ||
                !bindingSources.TryGetValue(grid, out var source))
            {
                MessageBox.Show("Grid binding is not ready.");
                return;
            }

            var (dt, adapter) = data;

            if (adapter.SelectCommand == null)
            {
                MessageBox.Show("Internal error: DataAdapter is not properly initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow newRow = dt.NewRow();

            foreach (var (columnName, textBox) in map)
            {
                if (!dt.Columns.Contains(columnName)) continue;

                string input = textBox.Text.Trim();

                try
                {
                    object value = string.IsNullOrWhiteSpace(input)
                        ? DBNull.Value
                        : Convert.ChangeType(input, Nullable.GetUnderlyingType(dt.Columns[columnName].DataType) ?? dt.Columns[columnName].DataType);

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

        private void DeleteSelectedRow(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.SelectedRows)
            {
                if (!row.IsNewRow)
                    grid.Rows.RemoveAt(row.Index);
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

        private void ClearTextBoxes(Dictionary<string, TextBox> textBoxMap)
        {
            foreach (var tb in textBoxMap.Values)
                tb.Text = string.Empty;
        }

        private void tabPage1_Enter(object sender, EventArgs e) => LoadData("aircraft", dataGridView1, panel1);
        private void tabPage2_Enter(object sender, EventArgs e) => LoadData("airlines", dataGridView2, panel1);
        private void tabPage3_Enter(object sender, EventArgs e) => LoadData("airports", dataGridView3, panel1);
        private void tabPage4_Enter(object sender, EventArgs e) => LoadData("airport_crew", dataGridView4, panel1);
        private void tabPage5_Enter(object sender, EventArgs e) => LoadData("bookings", dataGridView5, panel1);
        private void tabPage6_Enter(object sender, EventArgs e) => LoadData("flights", dataGridView6, panel1);
        private void tabPage7_Enter(object sender, EventArgs e) => LoadData("passengers", dataGridView7, panel1);
    }
}
