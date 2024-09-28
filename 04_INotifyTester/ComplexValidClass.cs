using System.ComponentModel;

namespace _04_INotifyTester
{
    public class ComplexValidClass : INotifyPropertyChanged
    {
        private int _intProperty;
        private string _stringProperty = string.Empty;
        private bool _boolProperty = false;
        private char _charProperty = '\0';
        private ValidTestClassInts? _validTestClassInts;
        private ushort _ushortProperty;
        private int[]? _intArrayProperty;
        private char[]? _charArrayProperty;

        public int IntProperty
        {

            get { return _intProperty; }

            set
            {
                _intProperty = value;
                OnPropertyChanged(nameof(IntProperty));
            }
        }

        public string StringProperty
        {

            get { return _stringProperty; }

            set
            {
                _stringProperty = value;
                OnPropertyChanged(nameof(StringProperty));
            }
        }

        public bool BoolProperty
        {

            get { return _boolProperty; }

            set
            {
                _boolProperty = value;
                OnPropertyChanged(nameof(BoolProperty));
            }
        }

        public char CharProperty
        {

            get { return _charProperty; }

            set
            {
                _charProperty = value;
                OnPropertyChanged(nameof(CharProperty));
            }
        }

        public ValidTestClassInts? ValidTestClassInts
        {

            get { return _validTestClassInts; }

            set
            {
                _validTestClassInts = value;
                OnPropertyChanged(nameof(ValidTestClassInts));
            }
        }

        public ushort UshortProperty
        {

            get { return _ushortProperty; }

            set
            {
                _ushortProperty = value;
                OnPropertyChanged(nameof(UshortProperty));
            }
        }

        public int[]? IntArrayProperty
        {

            get { return _intArrayProperty; }

            set
            {
                _intArrayProperty = value;
                OnPropertyChanged(nameof(IntArrayProperty));
            }
        }

        public char[]? CharArrayProperty
        {

            get { return _charArrayProperty; }

            set
            {
                _charArrayProperty = value;
                OnPropertyChanged(nameof(CharArrayProperty));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}