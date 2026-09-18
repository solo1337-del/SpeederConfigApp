using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace SpeederConfigApp.Models
{
    public class MobColorFilterItem : INotifyPropertyChanged
    {
        private string _mobName = string.Empty;
        private int _colorValue = 120000;
        private bool _hideMob;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string MobName
        {
            get => _mobName;
            set { _mobName = value?.Trim() ?? string.Empty; OnPropertyChanged(); }
        }

        public int ColorValue
        {
            get => _colorValue;
            set
            {
                _colorValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HexColor));
                OnPropertyChanged(nameof(PreviewBrush));
            }
        }

        public string HexColor => ColorConverterHelper.IntToHex(ColorValue);

        public Brush PreviewBrush => ColorValue == 0 ? Brushes.Transparent : new SolidColorBrush(ColorConverterHelper.IntToColor(ColorValue));

        public bool HideMob
        {
            get => _hideMob || ColorValue == 0;
            set
            {
                _hideMob = value;
                if (value) ColorValue = 0;
                OnPropertyChanged();
            }
        }

        public string ToConfigString()
        {
            if (string.IsNullOrWhiteSpace(MobName)) return string.Empty;
            return $"{MobName},{ColorValue}";
        }

        public static MobColorFilterItem? FromConfigString(string segment)
        {
            if (string.IsNullOrWhiteSpace(segment)) return null;

            var parts = segment.Split(',');
            if (parts.Length < 2) return null;

            var item = new MobColorFilterItem
            {
                MobName = parts[0].Trim(),
                ColorValue = ColorConverterHelper.ParseColorString(parts[1].Trim())
            };
            item._hideMob = item.ColorValue == 0;
            return item;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
