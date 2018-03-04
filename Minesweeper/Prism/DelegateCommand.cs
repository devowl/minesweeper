using System;
using System.Windows.Input;

namespace Minesweeper.Prism
{
    /// <summary>
    /// Prism delegate command.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;

        private readonly Action<object> _execute;

        /// <summary>
        /// <see cref="DelegateCommand"/> constructor.
        /// </summary>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Can execute command.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        /// <returns>Execute possibility.</returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Raise can execute.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}