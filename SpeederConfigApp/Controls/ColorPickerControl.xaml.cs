using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SpeederConfigApp.Models;

namespace SpeederConfigApp.Controls
{
    public partial class ColorPickerControl : UserControl
    {
        public static readonly DependencyProperty ColorValueProperty =
            DependencyProperty.Register(
                nameof(ColorValue),
                typeof(int),
                typeof(ColorPickerControl),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorValueChanged));

        public int ColorValue
        {
            get => (int)GetValue(ColorValueProperty);
            set => SetValue(ColorValueProperty, value);
        }

        private bool _isUpdating = false;

        public ColorPickerControl()
        {
            InitializeComponent();
            UpdateUiFromValue();
        }

        private static void OnColorValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPickerControl ctrl)
            {
                ctrl.UpdateUiFromValue();
            }
        }

        private void UpdateUiFromValue()
        {
            if (_isUpdating) return;
            _isUpdating = true;
            try
            {
                int val = ColorValue;
                DecInput.Text = val.ToString();
                HexDisplay.Text = ColorConverterHelper.IntToHex(val);

                if (val <= 0)
                {
                    ColorBadge.Background = new SolidColorBrush(Colors.Black);
                    ColorBadge.Opacity = 0.4;
                    SliderR.Value = 0;
                    SliderG.Value = 0;
                    SliderB.Value = 0;
                }
                else
                {
                    ColorBadge.Opacity = 1.0;
                    var col = ColorConverterHelper.IntToColor(val);
                    ColorBadge.Background = new SolidColorBrush(col);
                    SliderR.Value = col.R;
                    SliderG.Value = col.G;
                    SliderB.Value = col.B;
                }

                TextR.Text = ((int)SliderR.Value).ToString();
                TextG.Text = ((int)SliderG.Value).ToString();
                TextB.Text = ((int)SliderB.Value).ToString();
            }
            finally
            {
                _isUpdating = false;
            }
        }

        private void DecInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating) return;
            int parsed = ColorConverterHelper.ParseColorString(DecInput.Text, -1);
            if (parsed >= 0 && parsed != ColorValue)
            {
                _isUpdating = true;
                try
                {
                    ColorValue = parsed;
                    HexDisplay.Text = ColorConverterHelper.IntToHex(parsed);
                    var col = ColorConverterHelper.IntToColor(parsed);
                    ColorBadge.Background = new SolidColorBrush(col);
                    ColorBadge.Opacity = parsed == 0 ? 0.4 : 1.0;
                    SliderR.Value = col.R;
                    SliderG.Value = col.G;
                    SliderB.Value = col.B;
                    TextR.Text = col.R.ToString();
                    TextG.Text = col.G.ToString();
                    TextB.Text = col.B.ToString();
                }
                finally
                {
                    _isUpdating = false;
                }
            }
        }

        private void Preset_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tagStr && int.TryParse(tagStr, out int val))
            {
                ColorValue = val;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isUpdating) return;

            byte r = (byte)SliderR.Value;
            byte g = (byte)SliderG.Value;
            byte b = (byte)SliderB.Value;

            TextR.Text = r.ToString();
            TextG.Text = g.ToString();
            TextB.Text = b.ToString();

            int calculated = (r << 16) | (g << 8) | b;
            if (calculated != ColorValue)
            {
                _isUpdating = true;
                try
                {
                    ColorValue = calculated;
                    DecInput.Text = calculated.ToString();
                    HexDisplay.Text = ColorConverterHelper.IntToHex(calculated);
                    ColorBadge.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
                    ColorBadge.Opacity = calculated == 0 ? 0.4 : 1.0;
                }
                finally
                {
                    _isUpdating = false;
                }
            }
        }

        private void ToggleExpandBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SlidersPanel.Visibility == Visibility.Visible)
            {
                SlidersPanel.Visibility = Visibility.Collapsed;
                ToggleExpandBtn.Content = "Custom RGB ▼";
            }
            else
            {
                SlidersPanel.Visibility = Visibility.Visible;
                ToggleExpandBtn.Content = "Custom RGB ▲";
            }
        }
    }
}
