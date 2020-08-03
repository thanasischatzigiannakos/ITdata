using System;
using System.Collections.Generic;
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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {

        public Window2()
        {
            InitializeComponent();

        }

        
       

        private void user_btn_MouseEnter_1(object sender, MouseEventArgs e)
        {
            user_lb.Visibility = Visibility.Visible;
        }

        private void email_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            email_lb.Visibility = Visibility.Visible;

        }

        private void email_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            email_lb.Visibility = Visibility.Collapsed;

        }

        private void phone_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            phone_lb.Visibility = Visibility.Visible;

        }

        private void phone_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            phone_lb.Visibility = Visibility.Collapsed;

        }

        private void printer_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            printer_lb.Visibility = Visibility.Visible;

        }

        private void printer_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            printer_lb.Visibility = Visibility.Collapsed;
        }

        private void db_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            database_lb.Visibility = Visibility.Visible;
        }

        private void db_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            database_lb.Visibility = Visibility.Collapsed;

        }

        private void location_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            location_lb.Visibility = Visibility.Visible;

        }

        private void location_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            location_lb.Visibility = Visibility.Collapsed;

        }

        private void company_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            company_lb.Visibility = Visibility.Visible;
        }

        private void department_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            department_lb.Visibility = Visibility.Collapsed;

        }

        private void company_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            company_lb.Visibility = Visibility.Collapsed;

        }

        private void department_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            department_lb.Visibility = Visibility.Visible;

        }

        private void user_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            user_lb.Visibility = Visibility.Collapsed;

        }

        private void user_btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwin = new MainWindow();
            mainwin.Show();
            this.Close();

        }

        private void email_btn_Click(object sender, RoutedEventArgs e)
        {
            EmailsWindow mailwin =new EmailsWindow();
            mailwin.Show();
            this.Close();

        }

        private void phone_btn_Click(object sender, RoutedEventArgs e)
        {
            PhoneWindow phonewin = new PhoneWindow();
            phonewin.Show();
            this.Close();


        }

        private void printer_btn_Click(object sender, RoutedEventArgs e)
        {
            PrintersWindow printwin = new PrintersWindow();
            printwin.Show();
            this.Close();

        }

        private void db_btn_Click(object sender, RoutedEventArgs e)
        {
            Window1 win1 = new Window1();
            win1.Show();
            this.Close();
        }

        private void location_btn_Click(object sender, RoutedEventArgs e)
        {
            LocationWindow locwin = new LocationWindow();
            locwin.Show();
            this.Close();
        }

        private void company_btn_Click(object sender, RoutedEventArgs e)
        {
            CompanyWindow cmpwin = new CompanyWindow();
            cmpwin.Show();
            this.Close();
        }

        private void department_btn_Click(object sender, RoutedEventArgs e)
        {
            DepartmentWindow deptwin = new DepartmentWindow();
            deptwin.Show();
            this.Close();
        }

        private void exit_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
