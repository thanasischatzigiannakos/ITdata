using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
    /// Interaction logic for EmailsWindow.xaml
    /// </summary>
    public partial class EmailsWindow : Window
    {
        private int idValue = 0, unmatched_email_int = 0, userID = 0;

        public EmailsWindow()
        {
            InitializeComponent();
            fillList();
            user_list();
            emails_match_list();
        }

        private void email_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (email_listbox.SelectedItem != null)
            {
                DataRowView d1 = email_listbox.SelectedItem as DataRowView;
                email_tb.Text = d1["email"].ToString();
                passwd_tb.Text = d1["passwd"].ToString();
                idValue = (int)d1["id"];
            }
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

                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM mail", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    email_listbox.ItemsSource = dt.DefaultView;
                    email_listbox.DisplayMemberPath = "email"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    email_listbox.SelectedValuePath = "id";
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

        private void insert_new()
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))  //CONNECT TO THE DATABASE AND INSERT NEW VALUE IN THE TABLE
            {
                try
                {
                    CmdString = "INSERT INTO mail (email,passwd) VALUES ('" + email_tb.Text + "','" + passwd_tb.Text + "')";
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
        } //ADD NEW 

        private void update()
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "UPDATE mail SET email='" + email_tb.Text + "',passwd ='" + passwd_tb.Text + "' WHERE id ='" + idValue + "'"; //UPDATE VALUE OF TABLE PROPERTY WITH ID = idValue
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
        } //UPDATE

        private void delete()
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "DELETE FROM mail WHERE id = '" + idValue + "'";  //find the row where the id matches the idvalue from the listbox item
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

        private void add_new_btn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(email_tb.Text) || String.IsNullOrWhiteSpace(passwd_tb.Text))
            {
                MessageBox.Show("Please fill the required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                insert_new();
                fillList();
                user_list();
                emails_match_list();
                email_tb.Clear();
                passwd_tb.Clear();
                idValue = 0;
            }
        }

        private void update_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idValue == 0 || String.IsNullOrWhiteSpace(email_tb.Text) || String.IsNullOrWhiteSpace(passwd_tb.Text))
            {
                MessageBox.Show("Please fill the required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                update();
                fillList();
                user_list();
                emails_match_list();
                idValue = 0;
                email_tb.Clear();
                passwd_tb.Clear();
            }
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if (idValue == 0)
            {
                MessageBox.Show("Please select the item you wish to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                delete();
                fillList();
                user_list();
                emails_match_list();
                idValue = 0;
                email_tb.Clear();
                passwd_tb.Clear();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window2 win2 = new Window2();
            win2.Show();
            this.Close();
        }

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

                        MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM mail WHERE email LIKE '" + search.Text + "%'", con);  //-----PASS ALL THE DATA IN A DATASET
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        email_listbox.ItemsSource = dt.DefaultView;
                        email_listbox.DisplayMemberPath = "email"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                        email_listbox.SelectedValuePath = "id";
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

        private void emails_match_list()   //FILL THE LISTBOX WITH VALUES FROM THE DATABASE
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds = new DataSet();

                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM mail", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    unmatched_emails_listbox.ItemsSource = dt.DefaultView;
                    unmatched_emails_listbox.DisplayMemberPath = "email"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    unmatched_emails_listbox.SelectedValuePath = "id";
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

        private void unmatched_emails_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (unmatched_emails_listbox.SelectedItem != null)
            {
                DataRowView d3 = unmatched_emails_listbox.SelectedItem as DataRowView;
                email_selected_box.Text = d3["email"].ToString();
                unmatched_email_int = (int)d3["id"];
            }
        }

        private void user_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (user_listbox.SelectedItem != null)
            {
                DataRowView d2 = user_listbox.SelectedItem as DataRowView;
                user_selected_box.Text = d2["first_name"].ToString() + " " + d2["last_name"].ToString();
                userID = (int)d2["id"];
            }
        }

        private void match_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(email_selected_box.Text) || String.IsNullOrWhiteSpace(user_selected_box.Text))
            {
                MessageBox.Show("Please select an email AND a user", "Connection Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                String CmdString;
                String conString = Properties.dbSettings.Default.connectionString;

                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    try
                    {
                        CmdString = "INSERT INTO user_mail (user_id,mail_id) VALUES ('" + userID + "','" + unmatched_email_int + "')";
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
                        emails_match_list();
                        email_selected_box.Clear();
                        user_selected_box.Clear();
                        unmatched_emails_listbox.UnselectAll();
                        user_listbox.UnselectAll();
                        


                    }
                }
            }
        }

        private void filter_emails_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(filter_emails.Text))
            {
                emails_match_list();
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

                        MySqlDataAdapter adp4 = new MySqlDataAdapter("SELECT * FROM mail WHERE email LIKE '" + filter_emails.Text + "%'", con);  //-----PASS ALL THE DATA IN A DATASET
                        DataTable dt4 = new DataTable();
                        adp4.Fill(dt4);
                        unmatched_emails_listbox.ItemsSource = dt4.DefaultView;
                        unmatched_emails_listbox.DisplayMemberPath = "email"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                        unmatched_emails_listbox.SelectedValuePath = "id";
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

        private void clear_fields_Click(object sender, RoutedEventArgs e)
        {
            email_tb.Clear();
            passwd_tb.Clear();
            idValue = 0;
        }

        private void clear_filter_Click(object sender, RoutedEventArgs e)
        {
            search.Clear();
            fillList();

        }

        private void clear_filter_emails_Click(object sender, RoutedEventArgs e)
        {
            filter_emails.Clear();
            emails_match_list();

        }

        private void clear_filter_users_Click(object sender, RoutedEventArgs e)
        {
            filter_users.Clear();
            user_list();

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