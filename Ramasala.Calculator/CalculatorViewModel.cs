using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ramasala.Calculator
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        public CalculatorViewModel()
        {
            History = new ObservableCollection<string>();
        }

        private string _displayText;
        private ObservableCollection<string> _history;

        public ObservableCollection<string> History
        {
            get { return _history; }
            set
            {
                _history = value;
                OnPropertyChanged();
            }
        }

        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                _displayText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}