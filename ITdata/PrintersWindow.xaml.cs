using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ITdata
{
    /// <summary>
    /// Interaction logic for PrintersWindow.xaml
    /// </summary>
    public partial class PrintersWindow : Window
    {
        int s_locID=0,idValue=0;
        public PrintersWindow()
        {
            InitializeComponent();
            s_company_list();
            fill_printer_list();
        }

        private void s_company_list()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds1 = new DataSet();

                    MySqlDataAdapter adp1 = new MySqlDataAdapter("SELECT * FROM location", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    s_location_combo.ItemsSource = dt1.DefaultView;
                    //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    s_location_combo.SelectedValuePath = "id";
                    s_location_combo.DisplayMemberPath = "location";
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

        private void s_location_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView d1 = s_location_combo.SelectedItem as DataRowView;
            s_locID = (int)d1["id"];
        }

        private void fill_printer_list()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds1 = new DataSet();

                    MySqlDataAdapter adp1 = new MySqlDataAdapter("SELECT * FROM printers", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    printer_lb.ItemsSource = dt1.DefaultView;
                    printer_lb.DisplayMemberPath = "network_id"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    printer_lb.SelectedValuePath = "id";
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

        private void add_new_printer() {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))  //CONNECT TO THE DATABASE AND INSERT NEW VALUE IN THE TABLE
            {
                try
                {
                    CmdString = "INSERT INTO printers (network_id,ip_address,model,location,admin_name,admin_passwd) VALUES ('" + net_id_tb.Text + "','" + ip_tb.Text + "','" + model_tb.Text + "','" + s_locID + "','" + admin_name_tb.Text + "','" + admin_passwd_tb.Text + "')";
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

        private void update_printer()
        {

            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "UPDATE printers SET network_id='" + net_id_tb.Text + "',ip_address ='" + ip_tb.Text + "' ,model = '" + model_tb.Text + "' ,location ='" + s_locID + "' ,admin_name ='" + admin_name_tb.Text + "',admin_passwd='" + admin_passwd_tb.Text + "' WHERE id ='" + idValue + "'"; //UPDATE VALUE OF TABLE PROPERTY WITH ID = idValue
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
                    idValue = 0;
                }
            }
        }

        private void delete_printer()
        {

            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "DELETE FROM printers WHERE id = '" + idValue + "'";  //find the row where the id matches the idvalue from the listbox item
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
                    con.Close();    //---------------CLOSE AND DISPOSE THE CONNECTION AND REFRESH THE LISTBOX----------------
                    con.Dispose();
                    fill_printer_list();
                    idValue = 0;
                }
            }
        }

        private void add_new_btn_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(net_id_tb.Text)|| String.IsNullOrEmpty(ip_tb.Text)|| String.IsNullOrEmpty(model_tb.Text)||s_locID==0|| String.IsNullOrEmpty(admin_name_tb.Text)|| String.IsNullOrEmpty(admin_passwd_tb.Text))
            {
                MessageBox.Show("Make sure all the fields are filled", "Failed", MessageBoxButton.OK, MessageBoxImage.Question);

            }else if (CheckIPValid(ip_tb.Text) != true)
            {    
                    MessageBox.Show("Invalid ip at IP Address field", "Failed", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            else
            {
                add_new_printer();
                fill_printer_list();
                clearFields();
            }
        }

        private void update_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idValue == 0)
            {
                MessageBox.Show("You must first select the printer you wish to update", "Failed", MessageBoxButton.OK, MessageBoxImage.Question);
            }else if (String.IsNullOrEmpty(net_id_tb.Text) || String.IsNullOrEmpty(ip_tb.Text) || String.IsNullOrEmpty(model_tb.Text) || s_locID == 0 || String.IsNullOrEmpty(admin_name_tb.Text) || String.IsNullOrEmpty(admin_passwd_tb.Text))
            {
                MessageBox.Show("Make sure all the fields are filled", "Failed", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            else if (CheckIPValid(ip_tb.Text) != true)
            {
                MessageBox.Show("Invalid ip at IP Address field", "Failed", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            else
            {
                update_printer();
                fill_printer_list();
                clearFields();
            }

        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idValue == 0)
            {
                MessageBox.Show("Please select the item you wish to delete", "Failed", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            else
            {
                delete_printer();
                fill_printer_list();
                clearFields();
            }
        }

        private void clearFields()
        {
            net_id_tb.Clear();
            ip_tb.Clear();
            model_tb.Clear();
            s_location_combo.SelectedIndex = 0;
            s_locID = 0;
            admin_name_tb.Clear();
            admin_passwd_tb.Clear();
            idValue = 0;
        }

        private void clear_btn_Click(object sender, RoutedEventArgs e)
        {
            clearFields();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mnwin = new MainWindow();
            mnwin.Show();
            this.Close();
        }

        private void filter_tb_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(filter_tb.Text))
            {
                fill_printer_list();
            }
            else
            {
                String conString = Properties.dbSettings.Default.connectionString;

                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    try
                    {
                        con.Open();
                        DataSet ds = new DataSet();

                        MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM printers WHERE network_id LIKE '" + filter_tb.Text + "%'", con);  //-----PASS ALL THE DATA IN A DATASET
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        printer_lb.ItemsSource = dt.DefaultView;
                        printer_lb.DisplayMemberPath = "network_id"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                        printer_lb.SelectedValuePath = "id";
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

        }

        private void printer_lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (printer_lb.SelectedItem != null)
            {
                DataRowView d1 = printer_lb.SelectedItem as DataRowView;
                net_id_tb.Text = d1["network_id"].ToString();
                ip_tb.Text = d1["ip_address"].ToString();
                model_tb.Text = d1["model"].ToString();
                s_location_combo.SelectedValue = (int)d1["location"];
                s_locID = (int)d1["location"];
                admin_name_tb.Text = d1["admin_name"].ToString();
                admin_passwd_tb.Text = d1["admin_passwd"].ToString();
                idValue = (int)d1["id"];
            }

        }





        // _____________________________________________________________________________________________________________
        public Boolean CheckIPValid(String strIP)
        {
            //  Split string by ".", check that array length is 3
            char chrFullStop = '.';
            string[] arrOctets = strIP.Split(chrFullStop);
            if (arrOctets.Length != 4)
            {
                return false;
            }
            //  Check each substring checking that the int value is less than 255 and that is char[] length is !> 2
            Int16 MAXVALUE = 255;
            Int32 temp; // Parse returns Int32
            foreach (String strOctet in arrOctets)
            {
                if (strOctet.Length > 3)
                {
                    return false;
                }
                temp = 32;
                try
                {
                    temp = int.Parse(strOctet);
                }
                catch (Exception e)
                {
                    MessageBox.Show("There was an unexpected error with the following message " + e.Message + "Please make sure all the fields were filled correctly", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return false;
                }
                if (temp > MAXVALUE)
                {
                    return false;
                }
            }
            return true;
        }


        //REGEXES AND HANDLERS FOR INPUT IN TEXTBOXS
        private static readonly Regex _regex = new Regex("[^0-9.+/]"); //regex to allow only numbers and  dots

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void ServerAddress_PreviewTextInput(object sender, TextCompositionEventArgs e) //event handler to only allow input of IP format(numbers and dots)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }



    }
}
