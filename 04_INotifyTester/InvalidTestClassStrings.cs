using System.ComponentModel;

namespace _04_INotifyTester
{
    public class InvalidTestClassStrings : INotifyPropertyChanged
    {
        private string _validProperty = string.Empty;
        public string ValidProperty
        {
            get { return _validProperty; }

            set
            {
                _validProperty = value;
                OnPropertyChanged(nameof(ValidProperty));
            }
        }
        public string InvalidProperty { get; set; } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}