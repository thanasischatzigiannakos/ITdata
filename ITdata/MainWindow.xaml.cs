using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ITdata
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

       
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
    }
}
