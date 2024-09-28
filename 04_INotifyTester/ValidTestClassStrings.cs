using System.ComponentModel;

namespace _04_INotifyTester
{
    public class ValidTestClassStrings : INotifyPropertyChanged
    {
        private string _validProperty = string.Empty;
        private string _invalidProperty = string.Empty;
        public string ValidProperty
        {
            get { return _validProperty; }

            set
            {
                _validProperty = value;
                OnPropertyChanged(nameof(ValidProperty));
            }
        }
        public string InvalidProperty
        {
            get { return _invalidProperty; }

            set
            {
                _invalidProperty = value;
                OnPropertyChanged(nameof(InvalidProperty));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}