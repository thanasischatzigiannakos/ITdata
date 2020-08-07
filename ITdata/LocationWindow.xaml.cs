using MySqlConnector;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;


namespace ITdata
{
    /// <summary>
    /// Interaction logic for LocationWindow.xaml
    /// </summary>
    public partial class LocationWindow : Window
    {
        private int idValue;

        public LocationWindow()
        {
            InitializeComponent();
            fillList();
        }

        private void fillList() //--------FILL LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds = new DataSet();

                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM location ORDER BY location", con);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    locationListBox.ItemsSource = dt.DefaultView;
                    locationListBox.DisplayMemberPath = "location";
                    locationListBox.SelectedValuePath = "id";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Connection Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        private void insertNew(String locationname)   //INSERT NEW VALUE IN THE TABLE
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "INSERT INTO location (location) VALUES ('" + locationname + "')";
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(CmdString, con);
                    cmd.ExecuteNonQuery();
                    //fillDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Connection Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        private void updateTable(String newname)  // UPDATE THE TABLE
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "UPDATE location SET location = '" + newname + "' WHERE id ='" + idValue + "'";
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(CmdString, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Connection Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        private void deleteLocation(String lctName) //DELETE A VALUE FROM THE TABLE
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "DELETE FROM location WHERE id = '" + idValue + "'";
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(CmdString, con);
                    cmd.ExecuteNonQuery();
                    //fillDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Connection Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                    fillList();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window2 win2 = new Window2();
            win2.Show();
            this.Close();
        }

        private void add_new_button_Click(object sender, RoutedEventArgs e)
        {
            String input = inputbox.Text;
            if (String.IsNullOrWhiteSpace(input))
                MessageBox.Show("Please fill the name field", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            else
            {
                insertNew(input);
                fillList();
                inputbox.Clear();
                inputbox.Focus();
            }
        }

        private void save_button_Click(object sender, RoutedEventArgs e)  // GO BACK TO THE MAIN WINDOW
        {
            if (String.IsNullOrWhiteSpace(inputbox.Text) || idValue == 0)
                MessageBox.Show("Please select the item you wish to update and change name field", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            else
            {
                updateTable(inputbox.Text);
                fillList();
                inputbox.Clear();
                inputbox.Focus();
                idValue = 0;
            }
        }

        private void delete_button_Click(object sender, RoutedEventArgs e) //ON CLICK CALL THE DELETE FUNCTION FOR THE SELECTED LISTBOX ITEM
        {
            String input = inputbox.Text;
            deleteLocation(input);
            fillList();
            inputbox.Clear();
            inputbox.Focus();
        }

        private void locationListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e) //WHEN A LISTBOX ITEM IS CLICKED GET ITS DISPLAY AND VALUE MEMBERS AND SHOW THE DISPLAY MEMBER IN THE TEXTBOX
        {
            if (locationListBox.SelectedItem != null)
            {
                DataRowView d1 = locationListBox.SelectedItem as DataRowView;
                inputbox.Text = d1["location"].ToString();
                idValue = (int)d1["id"];
            }
        }
    }
}