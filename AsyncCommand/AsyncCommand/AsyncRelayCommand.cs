using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncCommand
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, bool> canExecute;
        private readonly Func<object, Task> executeAsync;
        private bool isExecuting;

        public AsyncRelayCommand(Func<object, Task> executeAsync, Func<object, bool> canExecute = null)
        {
            this.executeAsync = executeAsync;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            if (isExecuting)
            {
                return false;
            }
            else
            {
                return canExecute is null || canExecute(parameter);
            }
        }

        public void Execute(object? parameter)
        {
            if (!isExecuting)
            {
                isExecuting = true;
                executeAsync(parameter).ContinueWith(_ =>
                {
                    isExecuting = false;
                    CommandManager.InvalidateRequerySuggested();
                }, scheduler: TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}