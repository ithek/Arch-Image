using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Commun
{
    public class RelaiCommande : ICommand
    {
        Action execute;
        public RelaiCommande(Action execute)
        {
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            execute();
        }
    }

    public class RelaiCommande<T> : ICommand
    {
        private Action<T> execute;
        public RelaiCommande(Action<T> execute)
        {
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null || !(parameter  is T))
            {
                return false;
            }
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(T parameter)
        {
            execute(parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
        
    }
}

