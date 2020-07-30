using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ITdata
{
    /// <summary>
    /// Interaction logic for PhoneWindow.xaml
    /// </summary>
    public partial class PhoneWindow : Window
    {
        private int idValue = 0;
        private int userID=0;
        private int unmatchedphoneInt=0;

        public PhoneWindow()
        {
            InitializeComponent();
            fillList();
            user_list();
            match_phone_list();
        }


        //WHEN A LISTBOX ITEM IS SELECTED PASS ITS VALUES IN THE TEXTBOXS 
        private void phone_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (phone_listbox.SelectedItem != null)
            {
                DataRowView d1 = phone_listbox.SelectedItem as DataRowView;
                web_usr_txt.Text = d1["web_user"].ToString();
                model_num_txt.Text = d1["model"].ToString();
                serial_numtxt.Text = d1["serial_num"].ToString();
                ip_txt.Text = d1["ip"].ToString();
                mac_txt.Text = d1["mac_address"].ToString();
                internal_num_txt.Text = d1["internal_num"].ToString();
                ext_num_txt.Text = d1["external_num"].ToString();
                web_psw_txt.Text = d1["web_passwd"].ToString();
                port_tb.Text = d1["port"].ToString();
                idValue = (int)d1["id"];
            }
        }

        //BACK WINDOW (WHEN CLICKED CLOSES THIS WINDOW AND OPENS THE MAIN WINDOW
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mnwin = new MainWindow();
            mnwin.Show();
            this.Close();
        }

        private void fillList()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds = new DataSet();

                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM phones ORDER BY internal_num", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    phone_listbox.ItemsSource = dt.DefaultView;
                    phone_listbox.DisplayMemberPath = "internal_num"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    phone_listbox.SelectedValuePath = "id";
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

        private void match_phone_list()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds1 = new DataSet();

                    MySqlDataAdapter adp1 = new MySqlDataAdapter("SELECT * FROM phones WHERE user_id IS NULL ORDER BY internal_num", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    unmatched_phones_listbox.ItemsSource = dt1.DefaultView;
                    unmatched_phones_listbox.DisplayMemberPath = "internal_num"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    unmatched_phones_listbox.SelectedValuePath = "id";
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


        private void user_list()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds2 = new DataSet();

                    MySqlDataAdapter adp2 = new MySqlDataAdapter("SELECT * FROM users", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt2 = new DataTable();
                    adp2.Fill(dt2);
                    user_listbox.ItemsSource = dt2.DefaultView;
                   //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    user_listbox.SelectedValuePath = "id";
                    
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





        //FUNCTION TO ENTER A NEW ROW IN THE DATABASE TABLE
        private void insert_new(String wb_user, String wb_passwd, String model, String serial, String ip, String mac, String int_num, String ext_num)
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))  //CONNECT TO THE DATABASE AND INSERT NEW VALUE IN THE TABLE
            {
                try
                {
                    CmdString = "INSERT INTO phones (web_user,web_passwd,model,serial_num,ip,mac_address,internal_num,external_num,port) VALUES ('" + wb_user + "','" + wb_passwd + "','" + model + "','" + serial + "','" + ip + "','" + mac + "','" + int_num + "','" + ext_num + "','"+port_tb.Text+"')";
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

        //FUNCTION TO UPDATE A ROW IN THE DATABASE TABLE
        private void update(String wb_user, String wb_passwd, String model, String serial, String ip, String mac, String int_num, String ext_num)
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "UPDATE phones SET web_user='" + wb_user + "',web_passwd ='" + wb_passwd + "' ,model = '" + model + "' ,serial_num ='" + serial + "' ,ip ='" + ip + "',mac_address='" + mac + "',internal_num ='" + int_num + "',external_num ='" + ext_num +"',port='"+port_tb.Text+"' WHERE id ='" + idValue + "'"; //UPDATE VALUE OF TABLE PROPERTY WITH ID = idValue
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


        
        private void delete()
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "DELETE FROM phones WHERE id = '" + idValue + "'";  //find the row where the id matches the idvalue from the listbox item
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
                    fillList();
                    idValue = 0;
                }
            }
        }//FUNCTION TO DELETE A ROW


        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(web_psw_txt.Text) || String.IsNullOrWhiteSpace(web_usr_txt.Text) || String.IsNullOrWhiteSpace(model_num_txt.Text) || String.IsNullOrWhiteSpace(serial_numtxt.Text) || String.IsNullOrWhiteSpace(ip_txt.Text) || String.IsNullOrWhiteSpace(mac_txt.Text) || String.IsNullOrWhiteSpace(internal_num_txt.Text) || String.IsNullOrWhiteSpace(ext_num_txt.Text)|| String.IsNullOrWhiteSpace(port_tb.Text))
                MessageBox.Show("Please fill all the required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Question); //CHECK IF THE INPUT BOX IS EMPTY OR WHITESPACE
            else
            {
                Boolean macchecked = CheckIPValid(mac_txt.Text);
                Boolean ipchecked = CheckIPValid(ip_txt.Text);
                if (ipchecked == false)
                {
                    MessageBox.Show("Please enter a valid IP", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (macchecked == false)
                {
                    MessageBox.Show("Please enter a valid MAC Address", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    insert_new(web_usr_txt.Text, web_psw_txt.Text, model_num_txt.Text, serial_numtxt.Text, ip_txt.Text, mac_txt.Text, internal_num_txt.Text, ext_num_txt.Text);  //-----------------------IF IT IS CALL THE INSERT NEW FUNCTION--------------------------
                    fillList();
                    
                    web_usr_txt.Clear();
                    web_psw_txt.Clear();
                    model_num_txt.Clear();
                    serial_numtxt.Clear();
                    ip_txt.Clear();
                    mac_txt.Clear();
                    internal_num_txt.Clear();
                    ext_num_txt.Clear();
                    port_tb.Clear();
                    match_phone_list();
                }
            }
        }  //FUNCTION TO INSERT NEW ROW IN OUR TABLE

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(web_psw_txt.Text) || String.IsNullOrWhiteSpace(web_usr_txt.Text) || String.IsNullOrWhiteSpace(model_num_txt.Text) || String.IsNullOrWhiteSpace(serial_numtxt.Text) || String.IsNullOrWhiteSpace(ip_txt.Text) || String.IsNullOrWhiteSpace(mac_txt.Text) || String.IsNullOrWhiteSpace(internal_num_txt.Text) || String.IsNullOrWhiteSpace(ext_num_txt.Text)|| String.IsNullOrWhiteSpace(port_tb.Text))
                MessageBox.Show("Please fill all the necessary fields", "Error", MessageBoxButton.OK, MessageBoxImage.Stop); //CHECK IF THE INPUT BOX IS EMPTY OR WHITESPACE
            else
            {
                Boolean ipchecked = CheckIPValid(ip_txt.Text);
                Boolean macchecked = CheckIPValid(mac_txt.Text);
                if (ipchecked == false)
                {
                    MessageBox.Show("Please enter a valid IP", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (macchecked == false)
                {
                    MessageBox.Show("Please enter a valid MAC Address", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    update(web_usr_txt.Text, web_psw_txt.Text, model_num_txt.Text, serial_numtxt.Text, ip_txt.Text, mac_txt.Text, internal_num_txt.Text, ext_num_txt.Text);  //-----------------------IF IT IS CALL THE INSERT NEW FUNCTION--------------------------
                    fillList();
                    match_phone_list();
                    web_usr_txt.Clear();
                    web_psw_txt.Clear();
                    model_num_txt.Clear();
                    serial_numtxt.Clear();
                    ip_txt.Clear();
                    mac_txt.Clear();
                    internal_num_txt.Clear();
                    ext_num_txt.Clear();
                    port_tb.Clear();
                    idValue = 0;
                }
            }
        }  //FUNCTION TO UPDATE A ROW IN THE TABLE


        //WHEN DELETE BUTTON CLICKED FUNCTION
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (idValue == 0)
            {
                MessageBox.Show("Please select the item you wish to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                delete();
                fillList();
                match_phone_list();
                web_usr_txt.Clear();
                web_psw_txt.Clear();
                model_num_txt.Clear();
                serial_numtxt.Clear();
                ip_txt.Clear();
                mac_txt.Clear();
                internal_num_txt.Clear();
                ext_num_txt.Clear();
            }
        }

        //IP VALIDATION FUNCTION
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

        private static readonly Regex _regex2 = new Regex("[^0-9]+");//regex to allow only numbers

        private static bool OnlyNumbersAllowed(string text)//event handler for port textbox that only allows numbers as input
        {
            return !_regex2.IsMatch(text);
        }

        private void Port_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !OnlyNumbersAllowed(e.Text);
        }

        //REGEXES AND HANDLERS FOR INPUT IN TEXTBOXS


        //WHEN A KEY IS PRESSED IN THE SEARCH TEXTBOX AUTOMATICALLY REFRESH THE LISTBOX WITH ITEMS MATCHING THE SEARCH TEXTBOXS INPUT
        private void search_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(search.Text))
            {
                fillList();
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

                        MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM phones WHERE internal_num LIKE '" + search.Text + "%' ORDER BY internal_num", con);  //-----PASS ALL THE DATA IN A DATASET
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        phone_listbox.ItemsSource = dt.DefaultView;
                        phone_listbox.DisplayMemberPath = "internal_num"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                        phone_listbox.SelectedValuePath = "id";
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

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

            web_usr_txt.Clear();
            web_psw_txt.Clear();
            model_num_txt.Clear();
            serial_numtxt.Clear();
            ip_txt.Clear();
            mac_txt.Clear();
            internal_num_txt.Clear();
            ext_num_txt.Clear();
            port_tb.Clear();
            idValue = 0;

        }

        private void unmatched_phones_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (unmatched_phones_listbox.SelectedItem != null)
            {
                DataRowView d3 = unmatched_phones_listbox.SelectedItem as DataRowView;
                phone_selected_box.Text = d3["internal_num"].ToString();
                unmatchedphoneInt = (int)d3["id"];
            }

        }

        private void user_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (user_listbox.SelectedItem != null)
            {
                DataRowView d2 = user_listbox.SelectedItem as DataRowView;
                user_selected_box.Text = d2["first_name"].ToString() +" "+d2["last_name"].ToString();
                userID = (int)d2["id"];
            }

        }

        private void match_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(phone_selected_box.Text)|| String.IsNullOrWhiteSpace(user_selected_box.Text)) {

                MessageBox.Show("Please select a phone AND a user", "Connection Failed", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {

                String CmdString;
                String conString = Properties.dbSettings.Default.connectionString;

                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    try
                    {
                        CmdString = "UPDATE phones SET user_id='" + userID + "' WHERE internal_num = '" + phone_selected_box.Text + "'"; //UPDATE VALUE OF TABLE PROPERTY WITH ID = idValue
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
                        userID = 0;
                        match_phone_list();
                        phone_selected_box.Clear();
                        user_selected_box.Clear();
                        filter_phones.Clear();
                        filter_users.Clear();
                        
                    }
                }



            }

        }

        private void filter_phones_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(filter_phones.Text))
            {
                match_phone_list();
            }
            else
            {
                String conString = Properties.dbSettings.Default.connectionString;

                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    try
                    {
                        con.Open();
                        DataSet ds4 = new DataSet();

                        MySqlDataAdapter adp4 = new MySqlDataAdapter("SELECT * FROM phones WHERE internal_num LIKE '" + filter_phones.Text + "%' AND user_id IS NULL ORDER BY internal_num", con);  //-----PASS ALL THE DATA IN A DATASET
                        DataTable dt4 = new DataTable();
                        adp4.Fill(dt4);
                        unmatched_phones_listbox.ItemsSource = dt4.DefaultView;
                        unmatched_phones_listbox.DisplayMemberPath = "internal_num"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                        unmatched_phones_listbox.SelectedValuePath = "id";
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

        private void filter_users_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(filter_users.Text))
            {
                user_list();
            }
            else
            {
                String conString = Properties.dbSettings.Default.connectionString;

                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    try
                    {
                        con.Open();
                        DataSet ds4 = new DataSet();

                        MySqlDataAdapter adp4 = new MySqlDataAdapter("SELECT * FROM users WHERE first_name LIKE '" + filter_users.Text + "%' OR last_name LIKE '" + filter_users.Text + "%'", con);  //-----PASS ALL THE DATA IN A DATASET
                        DataTable dt4 = new DataTable();
                        adp4.Fill(dt4);
                        user_listbox.ItemsSource = dt4.DefaultView;
                        //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                        user_listbox.SelectedValuePath = "id";
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
    }
}