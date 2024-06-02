using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Oil.Models;

namespace Oil.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<OilRigViewModel> oilRigs;
        private Dispatcher dispatcher;
        private int numberOilRig = 0;

        public ObservableCollection<OilRigViewModel> OilRigs
        {
            get { return oilRigs; }
            set
            {
                oilRigs = value;
                OnPropertyChanged(nameof(OilRigs));
            }
        }

        public ICommand AddOilRigCommand { get; }

        public MainViewModel(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            OilRigs = new ObservableCollection<OilRigViewModel>();
            AddOilRigCommand = new RelayCommand(async () => await InitializeOilRigsAsync());
        }

        public async Task InitializeOilRigsAsync()
        {
            await Task.Delay(100);
            OilRigs.Add(new OilRigViewModel(numberOilRig, dispatcher));
            numberOilRig++;
            OnPropertyChanged(nameof(numberOilRig));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
