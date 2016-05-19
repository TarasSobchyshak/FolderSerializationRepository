using System;
using System.Windows.Input;

namespace FolderSerialization.Client.Models
{
    public class AutoCanExecuteCommandWrapper : ICommand
    {
        public ICommand WrappedCommand { get; private set; }

        public AutoCanExecuteCommandWrapper(ICommand wrappedCommand)
        {
            if (wrappedCommand == null)
            {
                throw new ArgumentNullException("Wrapped command");
            }

            WrappedCommand = wrappedCommand;
        }

        public void Execute(object parameter)
        {
            WrappedCommand.Execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return WrappedCommand.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
