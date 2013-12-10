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
using WMS.ServicesInterface;
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
        public PermissionLevel Permissions { get; set; }
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

            UserDto user = new UserDto()
            {
                Username = LoginTextBox.Text,
                Password = PasswordTextBox.Password,
            };

            var authenticationChannelFactory = new ChannelFactory<IAuthenticationService>("SecureBinding_IAuthenticationService");
            authenticationChannelFactory.Credentials.UserName.UserName = user.Username;
            authenticationChannelFactory.Credentials.UserName.Password = user.Password;
            IAuthenticationService AuthService = authenticationChannelFactory.CreateChannel();


            BaseMenu menu = new BaseMenu(this);
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

                    if(t.InnerException != null && t.InnerException.InnerException != null)
                        MessageBox.Show(t.InnerException.InnerException.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        MessageBox.Show("Nieznany błąd wewnętrzny serwera.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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
