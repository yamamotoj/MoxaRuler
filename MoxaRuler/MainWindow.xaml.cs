using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoxaRuler
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainPageViewModel _viewModel { get { return DataContext as MainPageViewModel; } }
        private Point _pointOffset { get { return new Point(150-1, 70-1); } }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel();
            MouseMove += new MouseEventHandler(onMouseMove);
            MouseLeftButtonDown += new MouseButtonEventHandler(onMouseLeftButtonDown);
            LocationChanged += new EventHandler(onLocationChanged);
            Activated += (s, a) =>
            {
                updateColor();
                updateScopeImage();
            };
        }

        void onLocationChanged(object sender, EventArgs e)
        {
            updateColor();
            updateScopeImage();
        }

        void onMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        void onMouseMove(object sender, MouseEventArgs e)
        {
        }

        private void updateColor()
        {
            var pos = PointToScreen(_pointOffset);
            Console.WriteLine("pos:" + pos.ToString());
            _viewModel.Color = DeskTopCapture.GetColor(pos.X, pos.Y);
        }

        private void updateScopeImage()
        {
            if(!_viewModel.IsScopeEnabled)return;
            var w = 26;
            var pos = PointToScreen(new Point(_pointOffset.X + _viewModel.X, _pointOffset.Y + _viewModel.Y));
            var si = DeskTopCapture.CaptureScreen(pos.X -w/2 + 2, pos.Y -w/2 +2, w, w);
            _viewModel.ScopeImage = si;
        }


        private void DragStartButton_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
        }

        private void DragStartButton_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
        }

        private void DragStartButton_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var x = _viewModel.X + e.HorizontalChange;
            var y = _viewModel.Y + e.VerticalChange;
            _viewModel.X = x < 0 ? 0 : x;
            _viewModel.Y = y < 0 ? 0 : y;
            Console.WriteLine("change({0}, {1}), x:{2} y:{3}", e.HorizontalChange, e.VerticalChange, x, y);
            var zero = PointToScreen(new Point(0, 0));
            var screen = PointToScreen(new Point(_viewModel.X, _viewModel.Y));
            _viewModel.PixelWidth = screen.X - zero.X;
            _viewModel.PixelHeight = screen.Y - zero.Y;
            updateScopeImage();
        }

        private void CapturedCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.Reset();
            updateScopeImage();

        }

        private void DragStartButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.IsScopeEnabled = !_viewModel.IsScopeEnabled;
            if (_viewModel.IsScopeEnabled)
            {
                updateScopeImage();
            }

        }

        private void DragEndButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Reset();
            updateScopeImage();
        }

        private void ScreenShot_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.PixelWidth < 2 || _viewModel.PixelHeight < 2) { return; }
            var p1 = CapturedCanvas.PointToScreen(new Point(1, 1));
            var bmp = DeskTopCapture.CaptureScreen(p1.X, p1.Y, _viewModel.PixelWidth -2, _viewModel.PixelHeight-2);
            Clipboard.SetImage(bmp);
        }
    }
}

