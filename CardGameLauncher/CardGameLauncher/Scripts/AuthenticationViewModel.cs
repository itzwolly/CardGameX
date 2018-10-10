using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace CardGameLauncher.Scripts {
    public class AuthenticationViewModel : ObservableObject, IViewModel {
        private readonly IAuthenticationService _authenticationService;

        private readonly RelayCommand _loginRelayCommand;
        private readonly RelayCommand _logoutRelayCommand;

        private string _username;
        private string _status;

        public AuthenticationViewModel(IAuthenticationService authenticationService) {
            _authenticationService = authenticationService;

            _loginRelayCommand = new RelayCommand(o => {
                Login(o);
                ShowView(o);
            }, o => CanLogin(o));

            _logoutRelayCommand = new RelayCommand(o => {
                Logout(o);
                ShowView(o);
            }, o => CanLogout(o));
        }

        #region Properties
        public string Username {
            get { return _username; }
            set { _username = value; NotifyPropertyChanged("Username"); }
        }
        public string AuthenticatedUser {
            get {
                if (IsAuthenticated) {
                    return string.Format("Signed in as {0}. {1}",
                            Thread.CurrentPrincipal.Identity.Name,
                            Thread.CurrentPrincipal.IsInRole("Administrator") ? "You are an administrator!"
                                : "You are NOT a member of the administrators group.");
                }
                return "Not authenticated!";
            }
        }
        public string Status {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged("Status"); }
        }
        public string Name {
            get { return "AuthenticationViewModel"; }
        }
        public bool IsAuthenticated {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }
        #endregion

        #region Commands
        //public DelegateCommand LoginCommand { get { return _loginCommand; } }
        //public DelegateCommand LogoutCommand { get { return _logoutCommand; } }
        //public DelegateCommand ShowViewCommand { get { return _showViewCommand; } }
        public RelayCommand LoginRelayCommand { get { return _loginRelayCommand; } }
        public RelayCommand LogoutRelayCommand { get { return _logoutRelayCommand; } }
        #endregion

        private void Login(object parameter) {
            PasswordBox passwordBox = parameter as PasswordBox;
            string clearTextPassword = passwordBox.Password;
            try {
                //Validate credentials through the authentication service
                User user = _authenticationService.AuthenticateUser(Username, clearTextPassword);

                //Get the current principal object
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null) {
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");
                }

                //Authenticate the user
                customPrincipal.Identity = new CustomIdentity(user.Username, user.Email, user.Roles);

                //Update UI
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginRelayCommand.RaiseCanExecuteChanged();
                _logoutRelayCommand.RaiseCanExecuteChanged();

                Username = string.Empty; //reset
                passwordBox.Password = string.Empty; //reset
                Status = string.Empty;
            } catch (UnauthorizedAccessException e) {
                Status = e.Message;
            } catch (Exception ex) {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        private void Logout(object parameter) {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null) {
                customPrincipal.Identity = new AnonymousIdentity();
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginRelayCommand.RaiseCanExecuteChanged();
                _logoutRelayCommand.RaiseCanExecuteChanged();

                Status = string.Empty;
            }
        }

        private void ShowView(object parameter) {
            try {
                Status = string.Empty;

                Window parentWindow = Window.GetWindow(parameter as DependencyObject);

                IView view;
                if (parentWindow.Name == "winAuthorized") { // occurs when you're logging out..
                    view = new LoginWindow();
                    view.ViewModel = this;
                } else {
                    view = new AuthorizedWindow();
                    view.ViewModel = this;
                }
                view.Show();

                parentWindow.Close();
            } catch (SecurityException) {
                Status = "You are not authorized!";
            }
        }

        private void Play() {
            // check if you have the files at X location
            // If you don't -> download the files from the server
            // If you do -> make sure they're up-to-date and valid
            // If they aren't -> download the files from the server
            // if they are -> open the game.


        }

        private bool CanLogin(object parameter) {
            return !IsAuthenticated;
        }

        private bool CanLogout(object parameter) {
            return IsAuthenticated;
        }
    }
}
