using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using WMS.Client.Menus;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.ServiceContracts;

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
        public int Permissions { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.Content = "Logowanie...";
            LoginButton.IsEnabled = false;

            var authenticationChannelFactory = new ChannelFactory<IAuthenticationService>("BasicHttpBinding_IAuthenticationService");
            IAuthenticationService AuthService = authenticationChannelFactory.CreateChannel();

            UserDto user = new UserDto()
            {
                Username = LoginTextBox.Text,
                Password = PasswordTextBox.Password,
            };

            BaseMenu menu = new BaseMenu();
            menu.Execute(() => AuthService.Authenticate(new Request<UserDto>(user)), t =>
                {
                    Username = t.Data.Username;
                    Password = user.Password;
                    Permissions = t.Data.Permissions;

                    MainWindowContent.Children.Clear();
                    MainWindowContent.Children.Add(new WMS.Client.Menus.MainMenu(this));
                }, t =>
                {
                    LoginButton.Content = "Zaloguj";
                    LoginButton.IsEnabled = true;

                    MessageBox.Show(t.InnerException.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                });            
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
