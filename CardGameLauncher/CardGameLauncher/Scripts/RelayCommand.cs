using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CardGameLauncher.Scripts {

    public class RelayCommand : ICommand {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter) {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged() {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class RelayCommand<T> : ICommand {
        private Action<T> _targetExecuteMethod;
        private Func<T, bool> _targetCanExecuteMethod;

        public RelayCommand(Action<T> executeMethod) {
            _targetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) {
            _targetExecuteMethod = executeMethod;
            _targetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged() {
            CanExecuteChanged(this, EventArgs.Empty);
        }
        #region ICommand Members

        bool ICommand.CanExecute(object parameter) {
            if (_targetCanExecuteMethod != null) {
                T tparm = (T) parameter;
                return _targetCanExecuteMethod(tparm);
            }
            if (_targetExecuteMethod != null) {
                return true;
            }
            return false;
        }

        // Beware - should use weak references if command instance lifetime is longer than lifetime of UI objects that get hooked up to command
        // Prism commands solve this in their implementation
        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter) {
            if (_targetExecuteMethod != null) {
                _targetExecuteMethod((T) parameter);
            }
        }
        #endregion
    }
}
