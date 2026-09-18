using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace SpeederConfigApp.Models
{
    public class GatheringFilterItem : INotifyPropertyChanged
    {
        private string _resourceName = "WOOD";
        private int _colorValue = 160000;
        private int? _minTier = 2;
        private int? _minRarity = 1;
        private bool _hideResource;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string ResourceName
        {
            get => _resourceName;
            set { _resourceName = value?.Trim().ToUpperInvariant() ?? string.Empty; OnPropertyChanged(); }
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

        public int? MinTier
        {
            get => _minTier;
            set { _minTier = value; OnPropertyChanged(); }
        }

        public int? MinRarity
        {
            get => _minRarity;
            set { _minRarity = value; OnPropertyChanged(); }
        }

        public bool HideResource
        {
            get => _hideResource || ColorValue == 0;
            set
            {
                _hideResource = value;
                if (value) ColorValue = 0;
                OnPropertyChanged();
            }
        }

        public string ToConfigString()
        {
            if (string.IsNullOrWhiteSpace(ResourceName)) return string.Empty;

            if (ColorValue == 0 || (MinTier == null && MinRarity == null))
            {
                return $"{ResourceName},{ColorValue}";
            }

            return $"{ResourceName},{ColorValue},{MinTier ?? 1},{MinRarity ?? 0}";
        }

        public static GatheringFilterItem? FromConfigString(string segment)
        {
            if (string.IsNullOrWhiteSpace(segment)) return null;

            var parts = segment.Split(',');
            if (parts.Length < 2) return null;

            var item = new GatheringFilterItem
            {
                ResourceName = parts[0].Trim(),
                ColorValue = ColorConverterHelper.ParseColorString(parts[1].Trim())
            };

            if (parts.Length >= 3 && int.TryParse(parts[2].Trim(), out int tier))
            {
                item.MinTier = tier;
            }
            else
            {
                item.MinTier = null;
            }

            if (parts.Length >= 4 && int.TryParse(parts[3].Trim(), out int rarity))
            {
                item.MinRarity = rarity;
            }
            else
            {
                item.MinRarity = null;
            }

            item._hideResource = item.ColorValue == 0;
            return item;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
