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
using System.ServiceModel;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.DataContracts;
using WMS.Client.Dialogs;

namespace WMS.Client.Menus
{
    /// <summary>
    /// Interaction logic for AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : BaseMenu
    {
        private MainWindow mainWindow;
        bool isLoaded = false;
        private List<UserDto> usersToCompare = new List<UserDto>();
        private List<UserDto> usersList = new List<UserDto>();

        public AdminMenu(MainWindow mainWindow)
            : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.ReloadWindow = LoadData;
            mainWindow.Title = "Panel Administratora";
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            Execute(() => AuthenticationService.GetUsers(new Request()), t =>
            {
                usersList = t.Data;
                isLoaded = true;
                InitializeData();
            });
        }

        private void InitializeData()
        {
            if (!isLoaded)
                return;

            users.Items.Clear();

            foreach (UserDto u in usersList)
                users.Items.Add(u);

            foreach (UserDto u in usersList)
                usersToCompare.Add(u);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Na pewno chcesz usunąć tego użytkownika?", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No, MessageBoxOptions.None) == MessageBoxResult.Yes)
            {
                int id = (int)((sender as Button).Tag);
                Execute(() => AuthenticationService.Delete(new Request<int>(id)), t =>
                    {
                        mainWindow.ReloadWindow();
                    },
                    t =>
                    {
                        if (t.InnerException != null && t.InnerException.GetType() == typeof(FaultException<ServiceException>))
                            MessageBox.Show((t.InnerException as FaultException<ServiceException>).Detail.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                            MessageBox.Show("Nieznany błąd wewnętrzny serwera.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ReloadWindow();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < usersList.Count; i++)
            {
                if (!usersList[i].Equals(usersToCompare[i]))
                {
                    usersList[i].Permissions = (WMS.ServicesInterface.PermissionLevel)usersList[i].PermissionsVal;
                    Execute(() => AuthenticationService.Edit(new Request<UserDto>(usersList[i])), t =>
                        {
                            mainWindow.ReloadWindow();
                        },
                        t =>
                        {
                            if (t.InnerException != null && t.InnerException.GetType() == typeof(FaultException<ServiceException>))
                                MessageBox.Show((t.InnerException as FaultException<ServiceException>).Detail.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                            else
                                MessageBox.Show("Nieznany błąd wewnętrzny serwera.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        });
                }
            }
        }

        private void AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            UserDialog userDialog = new UserDialog(mainWindow);
            userDialog.Show();
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new MainMenu(mainWindow));
        }

        /// <summary>
        /// Ładowanie menu
        /// </summary>
        /// <param name="menu"></param>
        private void LoadNewMenu(UserControl menu)
        {
            Grid content = Parent as Grid;

            content.Children.Remove(this);
            content.Children.Add(menu);
        }

        private void EditPassword_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
