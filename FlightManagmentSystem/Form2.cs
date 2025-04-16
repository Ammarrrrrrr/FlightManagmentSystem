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
using BCrypt.Net; // BCrypt.Net-Next


namespace FlightManagmentSystem
{
    public partial class Form2 : Form
    {
        private Form1 _mainboard;
        public Form2(Form1 mainboard)
        {
            InitializeComponent();
            _mainboard = mainboard;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            var db = new Database(); // assuming you have your DB connection class
            if (db.connect_db())
            {
                string query = "SELECT password_hash FROM airport_crew WHERE username = @username";
                MySqlCommand cmd = new MySqlCommand(query, db.mySqlConnection);
                cmd.Parameters.AddWithValue("@username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string hash = reader.GetString("password_hash");

                        if (BCrypt.Net.BCrypt.Verify(password, hash))
                        {
                            MessageBox.Show("Login successful!");
                            
                            DataEditor Adminboard = new DataEditor();
                            Adminboard.Show();
                            _mainboard.Hide(); // hides the original Form1
                            this.Hide();

                        }
                        else
                        {
                            MessageBox.Show("Invalid password.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username not found.");
                    }
                }

                db.close_db();
            }
            else
            {
                MessageBox.Show("Database connection failed.");
            }
        }
    

        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            _mainboard.Show();
            this.Hide();
        }

        
    }
}
