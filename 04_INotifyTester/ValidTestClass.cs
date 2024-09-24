using System.ComponentModel;

namespace _04_INotifyTester
{
    public class ValidTestClass : INotifyPropertyChanged
    {
        private int _validPropertyOne;
        private int _validPropertyTwo;

        public int ValidPropertyOne
        {
            get { return _validPropertyOne; }

            set
            {
                if (value != _validPropertyOne)
                {
                    _validPropertyOne = value;
                    OnPropertyChanged(nameof(ValidPropertyOne));
                }
            }
        }

        public int ValidPropertyTwo
        {
            get { return _validPropertyTwo; }

            set
            {
                if (value != _validPropertyTwo)
                {
                    _validPropertyTwo = value;
                    OnPropertyChanged(nameof(ValidPropertyTwo));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
