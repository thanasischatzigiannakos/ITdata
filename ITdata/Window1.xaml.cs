using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ITdata
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            ShowStoredValues();
        }

        Stopwatch sw;
        // --------------------------SQL CONNECTION FUNCTION--------------------------------------------

        private void Test(object sender, RoutedEventArgs e)     //testinf the connection to the database
        {
            string username = Properties.dbSettings.Default.username; ;
            string connectionString = null;
            //SqlConnection cnn;
            MySqlConnection cnn;   //creating the connection string from saved settings
            connectionString = "datasource=" + Properties.dbSettings.Default.datasource + "; port=" + Properties.dbSettings.Default.port + "; database=" + Properties.dbSettings.Default.database + ";username=" + Properties.dbSettings.Default.username + "; password=" + Properties.dbSettings.Default.password + "; charset=utf8;";
            //Properties.Resources.connectionstring = connetionString;

            cnn = new MySqlConnection(connectionString);
            sw = Stopwatch.StartNew();
            try
            {
                cnn.Open();   //open the connection
                sw.Stop();
                
                MessageBox.Show(("Connection OK ! \nTime required to establish the conncetion:"+sw.Elapsed.TotalSeconds), "Connection Test", MessageBoxButton.OK, MessageBoxImage.Information);
                
              
            }
            catch (Exception ex)   //failed exception
            {
                MessageBox.Show(ex.Message, "Connection Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                sw.Stop();
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
                Properties.dbSettings.Default.connectionString = connectionString;
                Properties.dbSettings.Default.Save();


            }
        }

        //--------------------------------------ON SAVE CONNECTION DATA FUNCTION-------------------------------------
        private void Submit_Click(object sender, RoutedEventArgs e)  //function to save the data used for the connection
        {
            Boolean ipchecked = true;
            string srvAdd, dbname, user, dbpasswd, admin, dbport;
            srvAdd = ServerAddress.Text;
            dbname = DBName.Text;
            user = UserID.Text;
            dbpasswd = UserPasswd.Password.ToString();
            admin = AdminCredentials.Password.ToString();
            dbport = Port.Text;
            //CHECKING IF THE FIELDS ARE ALL FILLED
            if (String.IsNullOrWhiteSpace(srvAdd) == true || String.IsNullOrWhiteSpace(dbname) == true || String.IsNullOrWhiteSpace(user) == true || String.IsNullOrWhiteSpace(dbport) == true)
            {
                MessageBox.Show("Please fill all the required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else if (!srvAdd.Equals("localhost"))//IF THE SERVER ADDRESS IS NOT THE LOCALHOST -> CHECK IF IT IS A VALID IP
            {
                ipchecked = CheckIPValid(srvAdd);
                if (ipchecked == false)
                {
                    MessageBox.Show("Please enter a valid IP", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (admin.Equals(Properties.Resources.adminpasswd.ToString()) && ipchecked == true)//CHECKING ADMINS PASSWORD
                {
                    Properties.dbSettings.Default.datasource = srvAdd;     //save the checkobox data into the programs settings
                    Properties.dbSettings.Default.database = dbname;
                    Properties.dbSettings.Default.username = user;
                    Properties.dbSettings.Default.password = dbpasswd;
                    Properties.dbSettings.Default.port = dbport;
                    Properties.dbSettings.Default.Save();
                    MessageBox.Show("Changes saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowStoredValues();

                    ServerAddress.Clear();
                    DBName.Clear();
                    UserID.Clear();
                    UserPasswd.Clear();
                    AdminCredentials.Clear();
                    Port.Clear();
                }
                else
                    MessageBox.Show("You must enter admins password", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else if (srvAdd.Equals("localhost"))//IF THE SERVER ADDRESS IS THE LOCALHOST THEN CHECK FOR ADMINS PASSWORD
            {
                if (admin.Equals(Properties.Resources.adminpasswd.ToString()))
                {
                    Properties.dbSettings.Default.datasource = srvAdd;     //save the checkobox data into the programs settings
                    Properties.dbSettings.Default.database = dbname;
                    Properties.dbSettings.Default.username = user;
                    Properties.dbSettings.Default.password = dbpasswd;
                    Properties.dbSettings.Default.port = dbport;
                    Properties.dbSettings.Default.Save();
                    MessageBox.Show("Changes saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowStoredValues();
                    ServerAddress.Clear();
                    DBName.Clear();
                    UserID.Clear();
                    UserPasswd.Clear();
                    AdminCredentials.Clear();
                    Port.Clear();
                }
                else
                    MessageBox.Show("You must enter a valid admins password", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //------------------------------------------CREATING LABELS OF THE SAVED SETTINGS-------------------------------------
        public void ShowStoredValues()  //function to create labels of the stored settings
        {
            Label dynamicLabel = new Label();
            Label dynamicLabel2 = new Label();
            Label dynamicLabel3 = new Label();
            Label dynamicLabel4 = new Label();

            //create label for server address
            dynamicLabel.Name = "NewLabel";
            dynamicLabel.Content = "Server Address ->" + Properties.dbSettings.Default.datasource;
            dynamicLabel.Width = 240;
            dynamicLabel.Height = 30;
            dynamicLabel.Foreground = new SolidColorBrush(Colors.White);
            dynamicLabel.Background = new SolidColorBrush(Colors.Black);

            Grid.SetRow(dynamicLabel, 1);
            Grid.SetColumn(dynamicLabel, 3);
            MainGrid.Children.Add(dynamicLabel);

            //create label for port number

            dynamicLabel2.Name = "NewLabel2";
            dynamicLabel2.Content = "Port ->" + Properties.dbSettings.Default.port;
            dynamicLabel2.Width = 240;
            dynamicLabel2.Height = 30;
            dynamicLabel2.Foreground = new SolidColorBrush(Colors.White);
            dynamicLabel2.Background = new SolidColorBrush(Colors.Black);

            Grid.SetRow(dynamicLabel2, 2);
            Grid.SetColumn(dynamicLabel2, 3);
            MainGrid.Children.Add(dynamicLabel2);

            //create label for the database name

            dynamicLabel3.Name = "NewLabel3";
            dynamicLabel3.Content = "Database Name ->" + Properties.dbSettings.Default.database;
            dynamicLabel3.Width = 240;
            dynamicLabel3.Height = 30;
            dynamicLabel3.Foreground = new SolidColorBrush(Colors.White);
            dynamicLabel3.Background = new SolidColorBrush(Colors.Black);

            Grid.SetRow(dynamicLabel3, 3);
            Grid.SetColumn(dynamicLabel3, 3);
            MainGrid.Children.Add(dynamicLabel3);

            //create label for the User ID

            dynamicLabel4.Name = "NewLabel4";
            dynamicLabel4.Content = "User ID ->" + Properties.dbSettings.Default.username;
            dynamicLabel4.Width = 240;
            dynamicLabel4.Height = 30;
            dynamicLabel4.Foreground = new SolidColorBrush(Colors.White);
            dynamicLabel4.Background = new SolidColorBrush(Colors.Black);

            Grid.SetRow(dynamicLabel4, 4);
            Grid.SetColumn(dynamicLabel4, 3);
            MainGrid.Children.Add(dynamicLabel4);
        }

        //----------------------------------ON BACK CLICK OPEN THE MAIN WINDOW AND CLOSE THIS ONE------------------------------------
        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainwind = new MainWindow();
            mainwind.Show();
            this.Close();
        }

        private static readonly Regex _regex = new Regex("[^0-9.+/^l/^o/^c/^a/^h/^s/^t]"); //regex to allow only numbers and  dots

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

        //--------------------------------------------IP VALIDATION FUNCTION------------------------------------------------
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

                temp = int.Parse(strOctet);
                if (temp > MAXVALUE)
                {
                    return false;
                }
            }
            return true;
        }
    }
}