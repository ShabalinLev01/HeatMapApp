using System;
using System.Windows.Input;

namespace HeatMapApp.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public DelegateCommand(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
