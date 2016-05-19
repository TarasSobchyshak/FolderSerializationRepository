using System;
using System.Windows.Input;

namespace FolderSerialization.Client.Models
{
    public class DelegateCommand : ICommand
    {
        Action<object> _execute;
        Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action<object> execute)
        {
            _execute = execute;
            _canExecute = AlwaysCanExecute;
        }

        public void Execute(object param)
        {
            _execute(param);
        }

        public bool CanExecute(object param)
        {
            return _canExecute(param);
        }
        private bool AlwaysCanExecute(object param) => true;
    }
}
