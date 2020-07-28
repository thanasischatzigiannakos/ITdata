using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace ITdata
{
    /// <summary>
    /// Interaction logic for CompanyWindow.xaml
    /// </summary>
    public partial class CompanyWindow : Window
    {
        private int idValue = 0;
        

        public CompanyWindow()
        {
            InitializeComponent();
            fillList();
        }





        private void fillList()   //----------FILL THE LISTBOX WITH VALUES FROM THE DATABASE-----------------
        {
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    con.Open();
                    DataSet ds = new DataSet();

                    MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM companies", con);  //-----PASS ALL THE DATA IN A DATASET
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    companyListBox.ItemsSource = dt.DefaultView;
                    companyListBox.DisplayMemberPath = "company"; //DATABINDING -------SETTING VALUE MEMBER AND DISPLAY MEMBER--------------
                    companyListBox.SelectedValuePath = "id";
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
        private void insertNew(String companyname)
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))  //CONNECT TO THE DATABASE AND INSERT NEW VALUE IN THE TABLE
            {
                try
                {
                    CmdString = "INSERT INTO companies (company) VALUES ('" + companyname + "')";
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

        private void updateTable(String newname)    //-----------------------------FUNCTION TO UPDATE OUR TABLE----------------------------
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "UPDATE companies SET company = '" + newname + "' WHERE id ='" + idValue + "'"; //UPDATE VALUE OF TABLE PROPERTY WITH ID = idValue
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

        private void deleteCompany(String cmpName)   //------FUNCTION TO DELETE A COMPANY FROM THE TABLE ---------------
        {
            String CmdString;
            String conString = Properties.dbSettings.Default.connectionString;

            using (MySqlConnection con = new MySqlConnection(conString))
            {
                try
                {
                    CmdString = "DELETE FROM companies WHERE id = '" + idValue + "'";
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
                    con.Close();    //---------------CLOSE AND DISPOSE THE CONNECTION AND REFRESH THE LISTBOX----------------
                    con.Dispose();
                    fillList();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)  //----------------BACK BUTTON SENDS US TO THE MAINWINDOW-----------------
        {
            MainWindow mainwind = new MainWindow();
            mainwind.Show();
            this.Close();
        }

        private void add_new_button_Click(object sender, RoutedEventArgs e) //------------------WHEN THE ADD NEW BUTTON IS CLICKED-----------------
        {
            String input = inputbox.Text;
            if (String.IsNullOrWhiteSpace(input))
                MessageBox.Show("Please fill the name field", "Error", MessageBoxButton.OK, MessageBoxImage.Stop); //CHECK IF THE INPUT BOX IS EMPTY OR WHITESPACE
            else
            {
                insertNew(input);  //-----------------------IF IT IS CALL THE INSERT NEW FUNCTION--------------------------
                fillList();
                inputbox.Clear();
                inputbox.Focus();
            }
        }

        private void save_button_Click(object sender, RoutedEventArgs e)  //------WHEN THE SAVE BUTTON IS CLICKED CALL THE UPDATETABLE FUNCTION-------
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

        private void delete_button_Click(object sender, RoutedEventArgs e) // ----------WHEN THE DELETE BUTTON IS PRESSED CALL THE DELETECOMPANY FUNCTION-----------
        {
            String input = inputbox.Text;
            deleteCompany(input);
            fillList();
            inputbox.Clear();
            inputbox.Focus();
        }

        private void companyListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e) //WHEN A LISTBOX ITEM IS CLICKED SHOW ITS DISPLAY MEMBER IN THE TEXTBOX
        {
            if (companyListBox.SelectedItem != null)
            {
                DataRowView d1 = companyListBox.SelectedItem as DataRowView;
                inputbox.Text = d1["company"].ToString();
                idValue = (int)d1["id"];    //---------------AND ASIGN ITS VALUE TO THE idValue VARIABLE--------------------
            }
        }
    }
}