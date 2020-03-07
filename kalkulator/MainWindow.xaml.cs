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

namespace kalkulator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string>CalculatorIn;
        public MainWindow()
        {
            InitializeComponent();
            CalculatorIn = new List<string>();

            
        }

        private void AC_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
        }
        private void C_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
        }
        private void PM_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
        }
        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
        }
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
        }
        private void Result_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button p = (Button)sender;
            string g = p.Name.ToString();
            CalculatorIn.Add(p.Name);
        }
        private void Bracket_Click(object sender, RoutedEventArgs e)
        {

        }

        private void View_lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Result_lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
