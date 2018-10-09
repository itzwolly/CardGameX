﻿using CardGameLauncher.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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

namespace CardGameLauncher {
    /// <summary>
    /// Interaction logic for SecretWindow.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand)]
    public partial class AuthorizedWindow : Window, IView, IClosable {
        public AuthorizedWindow() {
            InitializeComponent();
        }

        #region IView Members
        public IViewModel ViewModel {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
        #endregion
    }
}
