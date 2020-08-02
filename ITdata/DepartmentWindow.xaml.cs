using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace ITdata
{
    /// <summary>
    /// Interaction logic for DepartmentWindow.xaml
    /// </summary>
    public partial class DepartmentWindow : Window
    {
        public DepartmentWindow()
        {
            InitializeComponent();
            fillList();
        }

        private int idValue;

        private void fillList()  //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds = new DataSet();

                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM department", con);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    departmentListBox.ItemsSource = dt.DefaultView;
                    departmentListBox.DisplayMemberPath = "dept";
                    departmentListBox.SelectedValuePath = "id";
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

        //insert data in companies table
        private void insertNew(String deptname)  //INSERT A NEW VALUE IN THE TABLE
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "INSERT INTO department (dept) VALUES ('" + deptname + "')";
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

        private void updateTable(String newname)   //UPDATE A VALUE
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "UPDATE department SET dept = '" + newname + "' WHERE id ='" + idValue + "'";
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

        private void deleteDepartment(String dptName)  //DELETE A VALUE FROM THE DATABASE
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "DELETE FROM department WHERE id = '" + idValue + "'";
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

        private void Button_Click(object sender, RoutedEventArgs e)  //ON BUTTON CLICK CLOSE THIS WINDOW AND HEAD BACK O THE MAIN WINDOW
        {
            MainWindow mainwind = new MainWindow();
            mainwind.Show();
            this.Close();
        }

        private void add_new_button_Click(object sender, RoutedEventArgs e)   //WHEN TH4E ADD NEW BUTTON IS CLICKED CALL THE ADD NEW FUNCTION
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

        private void save_button_Click(object sender, RoutedEventArgs e) //WHEN THE SAVE BUTTON IS CLICKED CALL THE UPDATE FUNCTION
        {
            if (String.IsNullOrWhiteSpace(inputbox.Text) || idValue == 0)
                MessageBox.Show("Please select the item you wish to update and change name field", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            else
            {
                updateTable(inputbox.Text);
                fillList();
                inputbox.Clear();
                inputbox.Focus();
            }
        }

        private void delete_button_Click(object sender, RoutedEventArgs e) //WHEN THE DELETE BUTTON IS CLICKED CALL THE DELETE FUNCTION
        {
            String input = inputbox.Text;
            deleteDepartment(input);
            fillList();
            inputbox.Clear();
            inputbox.Focus();
        }

        private void departmentListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e) //WHEN A LISTBOX ITEM IS SELECTED GET ITS VALUES AND SHOW THE DISPLAY MEMBER IN THE TEXTBOX
        {
            if (departmentListBox.SelectedItem != null)
            {
                DataRowView d1 = departmentListBox.SelectedItem as DataRowView;
                inputbox.Text = d1["dept"].ToString();
                idValue = (int)d1["id"];
            }
        }
    }
}