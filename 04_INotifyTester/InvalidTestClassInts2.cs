﻿using System.ComponentModel;

namespace _04_INotifyTester
{
    public class InvalidTestClassInts2 : INotifyPropertyChanged
    {
        private int _validProperty;
        public int ValidProperty
        {
            get { return _validProperty; }

            set
            {
                _validProperty = value;
                OnPropertyChanged(nameof(ValidProperty));
            }
        }
        public int InvalidProperty { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}