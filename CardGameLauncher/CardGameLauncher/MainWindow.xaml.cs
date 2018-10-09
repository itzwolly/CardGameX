﻿using System;
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
using CardGameLauncher.Scripts;
using System.Diagnostics;

namespace CardGameLauncher {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void btnLogin_Click(object pSender, RoutedEventArgs pEvent) {
            // Do something
            AuthenticationService service = new AuthenticationService();
            //User user = service.AuthenticateUser("user_1", "kappa");
            string result;
            User user = service.AuthenticateUser(txtUsername.Text, pbPassword.Password, out result);

            lblResult.Content = result;
        }
    }
}
