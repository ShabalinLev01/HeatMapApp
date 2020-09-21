using System;
using System.Windows.Input;

namespace HeatMapApp.ViewModels
{
    /// <summary>
    /// Class for command execution
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        /// <summary>
        /// Constructor of DelegateCommand
        /// </summary>
        /// <param name="action">Action delegate to void execution method</param>
        public DelegateCommand(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// Executes command logic
        /// </summary>
        public void Execute(object parameter)
        {
            _action();
        }

        /// <summary>
        /// Determines if a command can be executed
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Called when conditions change, indicating whether the command can be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
