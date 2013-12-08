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

namespace WMS.Client
{
    /// <summary>
    /// Główne okno programu, na które wczytywane są okna w postaci kontrolek.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Przeładowywanie kontrolki i jej zawartości w oknie głównym
        /// </summary>
        public Action ReloadWindow { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindowContent.Children.Clear();
            MainWindowContent.Children.Add(new WMS.Client.Menus.MainMenu(this));
        }

        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            LoginTextBox.Text = "";
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Password = "";
        }
    }
}
