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
using System.Windows.Shapes;

namespace ojm.Views {
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window {
        public LoginView() {
            InitializeComponent();
        }

        private void UsernamePlaceholder_GotFocus(object sender, RoutedEventArgs e) {
            UsernamePlaceholder.Visibility = Visibility.Hidden;
            UsernameBox.Focus();
        }

        private void UsernameBox_LostFocus(object sender, RoutedEventArgs e) {
            if (UsernameBox.Text == "") {
                UsernamePlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void UsernameBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Tab) {
                PasswordPlaceholder.Visibility = Visibility.Hidden;
                PasswordBox.Focus();
            }
        }

        private void PasswordPlaceholder_GotFocus(object sender, RoutedEventArgs e) {
            PasswordPlaceholder.Visibility = Visibility.Hidden;
            PasswordBox.Focus();
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e) {
            if (PasswordBox.Password == "") {
                PasswordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {

            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e) {

        }

        private void LoginButton_MouseEnter(object sender, MouseEventArgs e) {
            LoginButton.Background = Brushes.Green;
        }

        private void LoginButton_MouseLeave(object sender, MouseEventArgs e) {
            LoginButton.Background = Brushes.Green;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            MainView MainView = new MainView();
            MainView.Show();
            this.Close();
        }
    }
}
