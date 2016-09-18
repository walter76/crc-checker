using System;
using System.Windows.Input;

namespace FileIoPerformanceComparer
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _Execute;
        private readonly Predicate<object> _CanExecute;

        private EventHandler InternalCanExecuteChanged;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        /// <see cref="ICommand.CanExecuteChanged"/>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                InternalCanExecuteChanged += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                InternalCanExecuteChanged -= value;
            }
        }

        /// <see cref="ICommand.Execute(object)"/>
        public void Execute(object parameter)
        {
            _Execute(parameter);
        }

        /// <see cref="ICommand.CanExecute(object)"/>
        public bool CanExecute(object parameter) => _CanExecute == null ? true : _CanExecute(parameter);

        public void RaiseCanExecuteChanged()
        {
            InternalCanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
