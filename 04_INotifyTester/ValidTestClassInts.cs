using System.ComponentModel;

namespace _04_INotifyTester
{
    public class ValidTestClassInts : INotifyPropertyChanged
    {
        private int _validPropertyOne;
        private int _validPropertyTwo;

        public int ValidPropertyOne
        {
            get { return _validPropertyOne; }

            set
            {
                _validPropertyOne = value;
                OnPropertyChanged(nameof(ValidPropertyOne));
            }
        }

        public int ValidPropertyTwo
        {
            get { return _validPropertyTwo; }

            set
            {
                _validPropertyTwo = value;
                OnPropertyChanged(nameof(ValidPropertyTwo));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}