using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using SpeederConfigApp.Models;

namespace SpeederConfigApp.Controls
{
    public partial class KeyPickerControl : UserControl
    {
        public static readonly DependencyProperty KeyCodeProperty =
            DependencyProperty.Register(
                nameof(KeyCode),
                typeof(int),
                typeof(KeyPickerControl),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnKeyCodeChanged));

        public int KeyCode
        {
            get => (int)GetValue(KeyCodeProperty);
            set => SetValue(KeyCodeProperty, value);
        }

        private bool _isListening = false;
        private bool _isInternalUpdate = false;

        public KeyPickerControl()
        {
            InitializeComponent();
            KeyCombo.ItemsSource = VirtualKeyHelper.AllKeys;
            UpdateUi();
        }

        private static void OnKeyCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KeyPickerControl control)
            {
                control.UpdateUi();
            }
        }

        private void UpdateUi()
        {
            if (_isInternalUpdate) return;

            _isInternalUpdate = true;
            try
            {
                var match = VirtualKeyHelper.AllKeys.FirstOrDefault(k => k.Code == KeyCode);
                if (match != null)
                {
                    KeyCombo.SelectedItem = match;
                }
                else
                {
                    KeyCombo.SelectedItem = null;
                    KeyCombo.Text = $"Key ({KeyCode})";
                }
            }
            finally
            {
                _isInternalUpdate = false;
            }
        }

        private void KeyCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInternalUpdate) return;

            if (KeyCombo.SelectedItem is VirtualKeyInfo info)
            {
                _isInternalUpdate = true;
                try
                {
                    KeyCode = info.Code;
                }
                finally
                {
                    _isInternalUpdate = false;
                }
            }
        }

        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RecordBtn.IsChecked == true)
            {
                StartListening();
            }
            else
            {
                StopListening();
            }
        }

        private void StartListening()
        {
            _isListening = true;
            RecordBtn.IsChecked = true;
            RecordText.Text = "Press Key...";
            RecordDot.Fill = new SolidColorBrush(Color.FromRgb(245, 158, 11)); // Amber
            Focus();
            CaptureMouse();
        }

        private void StopListening()
        {
            _isListening = false;
            RecordBtn.IsChecked = false;
            RecordText.Text = "Detect Key";
            RecordDot.Fill = new SolidColorBrush(Color.FromRgb(239, 68, 68)); // Red
            ReleaseMouseCapture();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!_isListening) return;

            e.Handled = true;
            Key key = (e.Key == Key.System) ? e.SystemKey : e.Key;

            if (key == Key.Escape)
            {
                StopListening();
                return;
            }

            int vk = VirtualKeyHelper.FromWpfKey(key);
            if (vk > 0)
            {
                KeyCode = vk;
            }

            StopListening();
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_isListening)
            {
                StopListening();
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            KeyCode = 0;
            if (_isListening)
            {
                StopListening();
            }
        }
    }
}
