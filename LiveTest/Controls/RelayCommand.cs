namespace AutomatePdm.LiveTest
{
    using System;
    using System.Windows.Input;

    internal class RelayCommand(
        Action<object?> onExecute,
        Func<object?, bool>? canExecute = null) : ICommand
    {
        private readonly Action<object?> onExecute = onExecute;
        private readonly Func<object?, bool> canExecute = canExecute ?? CanAlwaysExecute;

        static bool CanAlwaysExecute(object? obj) => true;

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object? parameter) => this.canExecute(parameter);

        public void Execute(object? parameter)
        {
            if (this.CanExecute(parameter))
            {
                this.onExecute(parameter);
            }
        }
    }
}
