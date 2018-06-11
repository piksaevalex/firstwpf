using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace firstWpf.Models
{
    public class ViewModelBase : NotifyPropertyChangedBase
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            protected set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;
                OnPropertyChanged();
            }
        }
    }

    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }

    public class Chapter : NotifyPropertyChangedBase
    {
        public Chapter(string name)
        {
            Name = name;
            Positions = new ObservableCollection<Position>();
        }

        public string Name { get; }

        public ObservableCollection<Position> Positions { get; }
    }

    public class Position : NotifyPropertyChangedBase
    {
        public Position(int number, string code, string name, string units, string quantity)
        {
            Number = number;
            Code = code;
            Name = name;
            Units = units;
            Quantity = quantity;
            TzmMchs = new ObservableCollection<TzmMch>();
        }

        public int Number { get; }
        public string Code { get; }
        public string Name { get; }
        public string Units { get; }
        public string Quantity { get; }

        public ObservableCollection<TzmMch> TzmMchs { get; }
    }

    public class TzmMch : NotifyPropertyChangedBase
    {
        public TzmMch(string code, string name, string units, string quantity)
        {
            Code = code;
            Name = name;
            Units = units;
            Quantity = quantity;
        }

        public string Code { get; }
        public string Name { get; }
        public string Units { get; }
        public string Quantity { get; }
    }

}
