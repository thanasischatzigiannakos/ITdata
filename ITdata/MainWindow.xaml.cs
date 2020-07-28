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
          
            s_company_list();
            s_locationlist();
            s_departmentlist();
            user_list();
            user_email();
         
        }

        private int cmpID = 0, locID = 0, depID = 0, s_cmpID = 0, s_locID = 0, s_depID = 0, s_statusInt = 0, userID = 0;

        //-------------------------ON CLICK OPEN WINDOW1 AND CLOSE THIS ONE-------------------------------------
        private void EditItem_Click_1(object sender, RoutedEventArgs e)
        {
            Window1 wind1 = new Window1();
            wind1.Show();
            this.Close();
        }

        //---------------------------ON CLICK EXIT THE PROGRAM-----------------------------------------
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
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            EmailsWindow emwind = new EmailsWindow();
            emwind.Show();
            this.Close();
        }

       
       

        

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(s_l_name.Text) || String.IsNullOrWhiteSpace(s_f_name.Text) || String.IsNullOrWhiteSpace(s_l_name_g.Text) || String.IsNullOrWhiteSpace(s_f_name_g.Text) || s_cmpID == 0 || s_locID == 0 || s_depID == 0 || String.IsNullOrWhiteSpace(s_username.Text) || String.IsNullOrWhiteSpace(s_passwd.Text))
            {
                MessageBox.Show("Please fill all the required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            else
            {
                MessageBox.Show("New User Created", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                insertNew();
                s_l_name.Clear();
                s_f_name.Clear();
                s_l_name_g.Clear();
                s_f_name_g.Clear();
                s_j_desc.Clear();
                s_h_name.Clear();
                s_username.Clear();
                s_passwd.Clear();
                s_admin_passwd.Clear();
                s_radmin_port.Clear();
                s_notes.Clear();
                user_list();
                s_company_combo.SelectedIndex = 0;
                s_location_combo.SelectedIndex = 0;
                s_department_combo.SelectedIndex = 0;
            }
        }

        public void insertNew()
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            if (s_status.IsChecked == true)
                s_statusInt = 1;
            else
                s_statusInt = 0;

            using (MySqlConnection con = new MySqlConnection(conString))  //CONNECT TO THE DATABASE AND INSERT NEW VALUE IN THE TABLE
            {
                try
                {
                    CmdString = "INSERT INTO users(first_name,last_name,gr_first_name,gr_last_name,company_id,location_id,department_id,job_desc,hostname,username,passwd,admin_passwd,status,radmin_port,notes) VALUES ('" + s_f_name.Text + "','" + s_l_name.Text + "','" + s_f_name_g.Text + "','" + s_l_name_g.Text + "','" + s_cmpID + "','" + s_locID + "','" + s_depID + "','" + s_j_desc.Text + "','" + s_h_name.Text + "','" + s_username.Text + "','" + s_passwd.Text + "','" + s_admin_passwd.Text + "','" + s_statusInt + "','" + s_radmin_port.Text + "','" + s_notes.Text
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
                    con.Dispose();
                    user_list();
                    user_email();
                    //---------------AFTER TASK IS FINISHED CLOSE AND DISPOSE THE CONNECTION----------------------
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

        //------------------------------------------------------------------------------------------------
        //******************************************SEARCH TAB ITEMS***********************************************************

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
                    user_listbox.SelectedIndex = 0;
                }
            }
            user_listbox.SelectedIndex = 0;
        }

        private void user_email()
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds2 = new DataSet();

                    MySqlDataAdapter adp2 = new MySqlDataAdapter("SELECT * FROM mail a JOIN user_mail b ON a.id=b.mail_id JOIN users c ON b.user_id=c.id WHERE c.id ='"+userID+"'", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt2 = new DataTable();
                    adp2.Fill(dt2);
                    user_mails_lb.ItemsSource = dt2.DefaultView;
                    //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    user_mails_lb.DisplayMemberPath = "email";
                    user_mails_lb.SelectedValuePath = "id";
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

        private void user_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (user_listbox.SelectedItem != null)
            {
                DataRowView d2 = user_listbox.SelectedItem as DataRowView;

                userID = (int)d2["id"];
                s_f_name.Text = d2["first_name"].ToString();
                s_l_name.Text = d2["last_name"].ToString();
                s_f_name_g.Text = d2["gr_first_name"].ToString();
                s_l_name_g.Text = d2["gr_last_name"].ToString();
                s_company_combo.SelectedValue = (int)d2["company_id"];
                s_cmpID = (int)d2["company_id"];
                s_location_combo.SelectedValue = (int)d2["location_id"];
                s_locID = (int)d2["location_id"];
                s_department_combo.SelectedValue = (int)d2["department_id"];
                s_depID = (int)d2["department_id"];
                s_j_desc.Text = d2["job_desc"].ToString();
                s_h_name.Text = d2["hostname"].ToString();
                s_username.Text = d2["username"].ToString();
                s_passwd.Text = d2["passwd"].ToString();
                s_admin_passwd.Text = d2["admin_passwd"].ToString();
                if ((int)d2["status"] == 1)
                {
                    s_status.IsChecked = true;
                }
                else
                    s_status.IsChecked = false;

                s_radmin_port.Text = d2["radmin_port"].ToString();
                s_notes.Text = d2["notes"].ToString();
                user_email();
            }
        }

        private void search_save_btn_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(s_l_name.Text) || String.IsNullOrWhiteSpace(s_f_name.Text) || String.IsNullOrWhiteSpace(s_l_name_g.Text) || String.IsNullOrWhiteSpace(s_f_name_g.Text) || s_cmpID == 0 || s_locID == 0 || s_depID == 0 || String.IsNullOrWhiteSpace(s_username.Text) || String.IsNullOrWhiteSpace(s_passwd.Text))
            {
                MessageBox.Show("Please fill all the required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            else
            {
                
                updateUser();
                MessageBox.Show("Users info updated", "Success", MessageBoxButton.OK, MessageBoxImage.Question);

            }



            }

        public void delete()
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "DELETE FROM users WHERE id = '" + userID + "'";  //find the row where the id matches the idvalue from the listbox item
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
                    user_list();
                    userID = 0;
                }
            }


        }

        private void search_delete_btn_Click(object sender, RoutedEventArgs e)
        {

            if (userID == 0)
            {
                MessageBox.Show("Please select the user you wish to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                delete();
                s_l_name.Clear();
                s_f_name.Clear();
                s_l_name_g.Clear();
                s_f_name_g.Clear();
                s_j_desc.Clear();
                s_h_name.Clear();
                s_username.Clear();
                s_passwd.Clear();
                s_admin_passwd.Clear();
                s_radmin_port.Clear();
                s_notes.Clear();
                user_list();
                
               
            }

        }

        

        private void filter_users_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(filter_user_TB.Text))
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

                        MySqlDataAdapter adp4 = new MySqlDataAdapter("SELECT * FROM users WHERE first_name LIKE '" + filter_user_TB.Text + "%' OR last_name LIKE '" + filter_user_TB.Text + "%'", con);  //-----PASS ALL THE DATA IN A DATASET
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
                        user_listbox.SelectedIndex = 0;
                    }
                }
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            s_l_name.Clear();
            s_f_name.Clear();
            s_l_name_g.Clear();
            s_f_name_g.Clear();
            s_j_desc.Clear();
            s_h_name.Clear();
            s_username.Clear();
            s_passwd.Clear();
            s_admin_passwd.Clear();
            s_radmin_port.Clear();
            s_notes.Clear();
            s_status.IsChecked = false;
            userID = 0;
            user_mails_lb.Items.Clear();
        }

        //*********************************UPDATE USER*******************************************************************
        private void updateUser()
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;
            if (s_status.IsChecked==true)
            {
                s_statusInt = 1;
            }

            using (MySqlConnection con = new MySqlConnection(conString))  //CONNECT TO THE DATABASE AND INSERT NEW VALUE IN THE TABLE
            {
                try
                {
                    CmdString = "UPDATE users SET first_name ='"+ s_f_name.Text.ToString()+"', last_name='"+s_l_name.Text.ToString() + "', gr_first_name='"+ s_f_name_g.Text.ToString() + "',gr_last_name='"+s_l_name_g.Text.ToString() + "',company_id='"+s_cmpID+"', location_id='"+s_locID+"', department_id='"+s_depID+"',job_desc='"+s_j_desc.Text.ToString() + "', hostname='"+ s_h_name.Text.ToString() + "', username='"+s_username.Text.ToString() + "', passwd='"+s_passwd.Text.ToString() + "', admin_passwd='"+s_admin_passwd.Text.ToString() + "', status='"+s_statusInt+"', radmin_port='"+s_radmin_port.Text.ToString() + "', notes='"+s_notes.Text.ToString() + "' WHERE id="+userID+";";
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
                    s_l_name.Clear();
                    s_f_name.Clear();
                    s_l_name_g.Clear();
                    s_f_name_g.Clear();
                    s_j_desc.Clear();
                    s_h_name.Clear();
                    s_username.Clear();
                    s_passwd.Clear();
                    s_admin_passwd.Clear();
                    s_radmin_port.Clear();
                    s_notes.Clear();
                    user_list();

                    //---------------AFTER TASK IS FINISHED CLOSE AND DISPOSE THE CONNECTION----------------------
                }
            }
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

                    MySqlDataAdapter adp1 = new MySqlDataAdapter("SELECT * FROM companies", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    s_company_combo.ItemsSource = dt1.DefaultView;
                    //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    s_company_combo.SelectedValuePath = "id";
                    s_company_combo.DisplayMemberPath = "company";
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

        private void s_locationlist()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
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
                    s_location_combo.ItemsSource = dt2.DefaultView;
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

        private void s_departmentlist()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
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
                    s_department_combo.ItemsSource = dt3.DefaultView;
                    //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    s_department_combo.SelectedValuePath = "id";
                    s_department_combo.DisplayMemberPath = "dept";
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

        private void s_company_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (s_company_combo.SelectedItem != null)
            {
                DataRowView d1 = s_company_combo.SelectedItem as DataRowView;
                s_cmpID = (int)d1["id"];
            }
        }

        private void s_location_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView d1 = s_location_combo.SelectedItem as DataRowView;
            s_locID = (int)d1["id"];
        }

        private void s_department_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView d1 = s_department_combo.SelectedItem as DataRowView;
            s_depID = (int)d1["id"];
        }
    }
}