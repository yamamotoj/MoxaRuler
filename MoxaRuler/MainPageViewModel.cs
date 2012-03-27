using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MoxaRuler
{
    class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            X = 0;
            Y = 0;
            Color = Color.FromArgb(0, 0, 255, 255);
            CloseCommand = new RelayCommand(onPressClose);

        }

        #region framework

        private void rise(string propertyName)
        {
            if (PropertyChanged == null) { return; }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool trySet<T>(string propertyName, ref T prop, T newValue)
        {
            if (prop != null && prop.Equals(newValue)) { return false; }
            if (prop == null && newValue == null) { return false; }
            prop = newValue;
            rise(propertyName);
            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private double _x;
        private double _y;
        private Color _color;
        private double _pixelWidth;
        private double _pixelHeight;
        private BitmapSource _scopeImage;
        private bool _isScopeEnabled;

        public bool IsScopeEnabled { get { return _isScopeEnabled; } set { trySet("IsScopeEnabled", ref _isScopeEnabled, value); } }
        public double X { get { return _x; } set { trySet("X", ref _x, value); } }
        public double Y { get { return _y; } set { trySet("Y", ref _y, value); } }
        public double PixelWidth { get { return _pixelWidth; } set { trySet("PixelWidth", ref _pixelWidth, value); } }
        public double PixelHeight { get { return _pixelHeight; } set { trySet("PixelHeight", ref _pixelHeight, value); } }
        public Color Color { get { return _color; } set { trySet("Color", ref _color, value); } }
        public RelayCommand CloseCommand { get; private set; }
        public BitmapSource ScopeImage { get { return _scopeImage; } set { trySet("ScopeImage", ref _scopeImage, value); } }

        private void onPressClose(Object parameter)
        {
            Application.Current.Shutdown();
        }

        public void Reset()
        {
            X = 0;
            Y = 0;
            PixelWidth = 0;
            PixelHeight = 0;
        }
    }
}


