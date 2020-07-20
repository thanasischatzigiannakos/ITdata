using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ITdata
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            company_list();
            locationlist();
            departmentlist();
        }

        private int cmpID = 0, locID = 0, depID = 0, statusInt = 0;

        //-------------------------ON CLICK OPEN WINDOW1 AND CLOSE THIS ONE-------------------------------------
        private void EditItem_Click_1(object sender, RoutedEventArgs e)
        {
            Window1 wind1 = new Window1();
            wind1.Show();
            this.Close();
        }

        //---------------------------ON CLIKC EXIT THE PROGRAM-----------------------------------------
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Company_Click(object sender, RoutedEventArgs e)
        {
            CompanyWindow cmpwin = new CompanyWindow();
            cmpwin.Show();
            this.Close();
        }

        private void Location_Click(object sender, RoutedEventArgs e)
        {
            LocationWindow lctwin = new LocationWindow();
            lctwin.Show();
            this.Close();
        }

        private void Dept_Click(object sender, RoutedEventArgs e)
        {
            DepartmentWindow dptwin = new DepartmentWindow();
            dptwin.Show();
            this.Close();
        }

        private void Phone_Click(object sender, RoutedEventArgs e)
        {
            PhoneWindow pnwin = new PhoneWindow();
            pnwin.Show();
            this.Close();
        }

        private void company_list()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds1 = new DataSet();

                    MySqlDataAdapter adp1 = new MySqlDataAdapter("SELECT * FROM companies", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    company_combo.ItemsSource = dt1.DefaultView;
                    //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    company_combo.SelectedValuePath = "id";
                    company_combo.DisplayMemberPath = "company";
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

        private void locationlist()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds2 = new DataSet();

                    MySqlDataAdapter adp2 = new MySqlDataAdapter("SELECT * FROM location", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt2 = new DataTable();
                    adp2.Fill(dt2);
                    location_combo.ItemsSource = dt2.DefaultView;
                    //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    location_combo.SelectedValuePath = "id";
                    location_combo.DisplayMemberPath = "location";
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

        private void departmentlist()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds3 = new DataSet();

                    MySqlDataAdapter adp3 = new MySqlDataAdapter("SELECT * FROM department", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt3 = new DataTable();
                    adp3.Fill(dt3);
                    department_combo.ItemsSource = dt3.DefaultView;
                    //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    department_combo.SelectedValuePath = "id";
                    department_combo.DisplayMemberPath = "dept";
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

        private void company_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (company_combo.SelectedItem != null)
            {
                DataRowView d1 = company_combo.SelectedItem as DataRowView;
                cmpID = (int)d1["id"];
            }
        }

        private void location_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView d1 = location_combo.SelectedItem as DataRowView;
            locID = (int)d1["id"];
        }

        private void department_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView d1 = department_combo.SelectedItem as DataRowView;
            depID = (int)d1["id"];
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(l_name.Text) || String.IsNullOrWhiteSpace(f_name.Text) || String.IsNullOrWhiteSpace(l_name_g.Text) || String.IsNullOrWhiteSpace(f_name_g.Text) || cmpID == 0 || locID == 0 || depID == 0 || String.IsNullOrWhiteSpace(username.Text) || String.IsNullOrWhiteSpace(passwd.Text))
            {
                MessageBox.Show("Please fill all the required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            else
            {
                MessageBox.Show("New User Created", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                insertNew();
                l_name.Clear();
                f_name.Clear();
                l_name_g.Clear();
                f_name_g.Clear();
                j_desc.Clear();
                h_name.Clear();
                username.Clear();
                passwd.Clear();
                admin_passwd.Clear();
                radmin_port.Clear();
                notes.Clear();
            }
        }

        public void insertNew()
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            if (status.IsChecked == true)
                statusInt = 1;
            else
                statusInt = 0;

            using (MySqlConnection con = new MySqlConnection(conString))  //CONNECT TO THE DATABASE AND INSERT NEW VALUE IN THE TABLE
            {
                try
                {
                    CmdString = "INSERT INTO users(first_name,last_name,gr_first_name,gr_last_name,company_id,location_id,department_id,job_desc,hostname,username,passwd,admin_passwd,status,radmin_port,notes) VALUES ('" + f_name.Text + "','" + l_name.Text + "','" + f_name_g.Text + "','" + l_name_g.Text + "','" + cmpID + "','" + locID + "','" + depID + "','" + j_desc.Text + "','" + h_name.Text + "','" + username.Text + "','" + passwd.Text + "','" + admin_passwd.Text + "','" + statusInt + "','" + radmin_port.Text + "','" + notes.Text
                        + "')";
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
                    con.Dispose();  //---------------AFTER TASK IS FINISHED CLOSE AND DISPOSE THE CONNECTION----------------------
                }
            }
        }

        private static readonly Regex _regex2 = new Regex("[^0-9]+");//regex to allow only numbers

        private static bool OnlyNumbersAllowed(string text)//event handler for a textbox that only allows numbers as input
        {
            return !_regex2.IsMatch(text);
        }

        private void Port_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !OnlyNumbersAllowed(e.Text);
        }
    }
}