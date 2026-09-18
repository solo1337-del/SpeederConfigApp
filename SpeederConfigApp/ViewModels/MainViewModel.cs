using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using SpeederConfigApp.Models;

namespace SpeederConfigApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ConfigSettings _settings;
        private string _rawConfigPreview = string.Empty;
        private string _lineNumberedPreview = string.Empty;
        private string _currentFilePath = string.Empty;
        private string _statusMessage = "Ready. Configure settings or open an existing config.txt";
        private bool _statusIsError = false;

        // Bounding box calculator properties
        private int _calcResWidth = 1920;
        private int _calcResHeight = 1080;
        private int _calcMargin = 100;
        private string _selectedResPreset = "1920x1080 (1080p)";

        // Filter selection
        private GatheringFilterItem? _selectedGatheringFilter;
        private MobColorFilterItem? _selectedMobFilter;

        public ConfigSettings Settings
        {
            get => _settings;
            set
            {
                if (_settings != null)
                {
                    _settings.PropertyChanged -= OnSettingsPropertyChanged;
                    _settings.GatheringFilters.CollectionChanged -= OnCollectionChanged;
                    _settings.MobColorFilters.CollectionChanged -= OnCollectionChanged;
                }
                _settings = value;
                if (_settings != null)
                {
                    _settings.PropertyChanged += OnSettingsPropertyChanged;
                    _settings.GatheringFilters.CollectionChanged += OnCollectionChanged;
                    _settings.MobColorFilters.CollectionChanged += OnCollectionChanged;
                }
                OnPropertyChanged();
                UpdatePreview();
            }
        }

        public string RawConfigPreview
        {
            get => _rawConfigPreview;
            set => SetProperty(ref _rawConfigPreview, value);
        }

        public string LineNumberedPreview
        {
            get => _lineNumberedPreview;
            set => SetProperty(ref _lineNumberedPreview, value);
        }

        public string CurrentFilePath
        {
            get => _currentFilePath;
            set => SetProperty(ref _currentFilePath, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool StatusIsError
        {
            get => _statusIsError;
            set => SetProperty(ref _statusIsError, value);
        }

        public int CalcResWidth
        {
            get => _calcResWidth;
            set => SetProperty(ref _calcResWidth, value);
        }

        public int CalcResHeight
        {
            get => _calcResHeight;
            set => SetProperty(ref _calcResHeight, value);
        }

        public int CalcMargin
        {
            get => _calcMargin;
            set => SetProperty(ref _calcMargin, value);
        }

        public string SelectedResPreset
        {
            get => _selectedResPreset;
            set
            {
                if (SetProperty(ref _selectedResPreset, value))
                {
                    ApplyResolutionPreset(value);
                }
            }
        }

        public GatheringFilterItem? SelectedGatheringFilter
        {
            get => _selectedGatheringFilter;
            set => SetProperty(ref _selectedGatheringFilter, value);
        }

        public MobColorFilterItem? SelectedMobFilter
        {
            get => _selectedMobFilter;
            set => SetProperty(ref _selectedMobFilter, value);
        }

        #region Commands
        public RelayCommand NewConfigCommand { get; }
        public RelayCommand OpenConfigCommand { get; }
        public RelayCommand SaveConfigCommand { get; }
        public RelayCommand SaveAsConfigCommand { get; }
        public RelayCommand CopyToClipboardCommand { get; }
        public RelayCommand CalculateBoundsCommand { get; }
        public RelayCommand AddGatheringFilterCommand { get; }
        public RelayCommand RemoveGatheringFilterCommand { get; }
        public RelayCommand AddMobFilterCommand { get; }
        public RelayCommand RemoveMobFilterCommand { get; }
        public RelayCommand ApplyVlcPresetCommand { get; }
        public RelayCommand ApplyPowershellSoundPresetCommand { get; }
        public RelayCommand SetDefaultFieldOfViewCommand { get; }
        public RelayCommand ApplyConsolePresetTopLeftCommand { get; }
        public RelayCommand ApplyConsolePresetBottomRightCommand { get; }
        public RelayCommand ApplyConsolePresetOverlayCommand { get; }
        #endregion

        public MainViewModel()
        {
            _settings = new ConfigSettings();
            _settings.PropertyChanged += OnSettingsPropertyChanged;
            _settings.GatheringFilters.CollectionChanged += OnCollectionChanged;
            _settings.MobColorFilters.CollectionChanged += OnCollectionChanged;

            NewConfigCommand = new RelayCommand(ExecuteNewConfig);
            OpenConfigCommand = new RelayCommand(ExecuteOpenConfig);
            SaveConfigCommand = new RelayCommand(ExecuteSaveConfig);
            SaveAsConfigCommand = new RelayCommand(ExecuteSaveAsConfig);
            CopyToClipboardCommand = new RelayCommand(ExecuteCopyToClipboard);
            CalculateBoundsCommand = new RelayCommand(ExecuteCalculateBounds);
            AddGatheringFilterCommand = new RelayCommand(ExecuteAddGatheringFilter);
            RemoveGatheringFilterCommand = new RelayCommand(ExecuteRemoveGatheringFilter);
            AddMobFilterCommand = new RelayCommand(ExecuteAddMobFilter);
            RemoveMobFilterCommand = new RelayCommand(ExecuteRemoveMobFilter);
            ApplyVlcPresetCommand = new RelayCommand(ExecuteApplyVlcPreset);
            ApplyPowershellSoundPresetCommand = new RelayCommand(ExecuteApplyPowershellSoundPreset);
            SetDefaultFieldOfViewCommand = new RelayCommand(p => Settings.FieldOfView = p?.ToString() ?? "1");

            ApplyConsolePresetTopLeftCommand = new RelayCommand(() =>
            {
                Settings.ConsoleEnabled = true;
                Settings.ConsolePosX = 20;
                Settings.ConsolePosY = 20;
                Settings.ConsoleWidth = 520;
                Settings.ConsoleHeight = 380;
                Settings.ConsoleFontSize = 13;
                Settings.ConsoleBgColor = 3618615; // Dark gray
                Settings.ConsoleTextColor = 16777215; // White
                Settings.ConsoleTransparent = false;
                Settings.ConsoleAlwaysOnTop = true;
                Settings.ConsoleFont = "Consolas";
                Settings.ConsoleCustomOverride = string.Empty;
                StatusMessage = "Applied Top-Left Console preset.";
                StatusIsError = false;
                UpdatePreview();
            });

            ApplyConsolePresetBottomRightCommand = new RelayCommand(() =>
            {
                Settings.ConsoleEnabled = true;
                Settings.ConsolePosX = 1380;
                Settings.ConsolePosY = 660;
                Settings.ConsoleWidth = 520;
                Settings.ConsoleHeight = 380;
                Settings.ConsoleFontSize = 13;
                Settings.ConsoleBgColor = 3618615;
                Settings.ConsoleTextColor = 16777215;
                Settings.ConsoleTransparent = false;
                Settings.ConsoleAlwaysOnTop = true;
                Settings.ConsoleFont = "Consolas";
                Settings.ConsoleCustomOverride = string.Empty;
                StatusMessage = "Applied Bottom-Right Console preset.";
                StatusIsError = false;
                UpdatePreview();
            });

            ApplyConsolePresetOverlayCommand = new RelayCommand(() =>
            {
                Settings.ConsoleEnabled = true;
                Settings.ConsolePosX = 10;
                Settings.ConsolePosY = 10;
                Settings.ConsoleWidth = 550;
                Settings.ConsoleHeight = 350;
                Settings.ConsoleFontSize = 13;
                Settings.ConsoleBgColor = 0; // Black
                Settings.ConsoleTextColor = 65280; // Terminal green
                Settings.ConsoleTransparent = true;
                Settings.ConsoleAlwaysOnTop = true;
                Settings.ConsoleFont = "Consolas";
                Settings.ConsoleCustomOverride = string.Empty;
                StatusMessage = "Applied Transparent Green Terminal Console preset.";
                StatusIsError = false;
                UpdatePreview();
            });

            UpdatePreview();
        }

        private void OnSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdatePreview();
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += (s, ev) => UpdatePreview();
                }
            }
            UpdatePreview();
        }

        public void UpdatePreview()
        {
            var lines = Settings.GenerateLines();
            RawConfigPreview = string.Join(Environment.NewLine, lines);

            var sb = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                string lineNum = (i + 1).ToString().PadLeft(2, '0');
                string content = lines[i];
                string desc = GetLineSummary(i + 1);
                sb.AppendLine($"Line {lineNum} [{desc}]: {content}");
            }
            LineNumberedPreview = sb.ToString();
        }

        private string GetLineSummary(int line)
        {
            return line switch
            {
                1 => "Master Toggle Key",
                2 => "Movement Speed Multiplier",
                3 => "Movement Speed Toggle Key",
                4 => "Max Zoom Multiplier",
                5 => "Max Zoom Toggle Key",
                6 => "Rotate Right Key",
                7 => "Rotate Left Key",
                8 => "Rotate Step Horiz",
                9 => "Rotate Up Key",
                10 => "Rotate Down Key",
                11 => "Rotate Step Vert",
                12 => "Free Look Hold Key",
                13 => "Invert Y Axis",
                14 => "Invert X Axis",
                15 => "Deprecated",
                16 => "Swing Speed",
                17 => "Lock X Axis",
                18 => "Lock Y Axis",
                19 => "Screen Bounds Clamp",
                20 => "Driver Name",
                21 => "Field Of View (FOV)",
                22 => "Waymark Pause/Resume Key",
                23 => "Waymark Tolerance",
                24 => "Waymark Sleep Jitter",
                25 => "Waymark Timeout ms",
                26 => "Cursor Offset px",
                27 => "Record Coordinates Key",
                28 => "Camera Hacks Master Toggle",
                29 => "Macro File Name",
                30 => "Injected Keys Allowed",
                31 => "Mouse Smoothing",
                32 => "Window Center Y Adjustment",
                33 => "DPI Scaling Multiplier",
                34 => "Polling Interval ms",
                35 => "Memory Debug Display",
                36 => "Screen Clamp Toggle Key",
                37 => "Nearby Player Detection",
                38 => "Deprecated",
                39 => "Console Player Alerts",
                40 => "Deprecated",
                41 => "Deprecated",
                42 => "Kernel Input",
                43 => "Radar Settings",
                44 => "Console Window Settings",
                45 => "Player Detection Toggle Key",
                46 => "ESP Settings",
                47 => "ESP/Targeting FOV & Pitch",
                48 => "ESP & Radar Scaling",
                49 => "Gathering Filters",
                50 => "Mounted Player Highlight",
                51 => "Mob Color Filters",
                52 => "Player Display Text",
                53 => "Gathering Display Text",
                54 => "Mob Display Text",
                55 => "Mob Min HP Threshold",
                _ => $"Line {line}"
            };
        }

        private void ExecuteNewConfig()
        {
            var msgResult = MessageBox.Show("Reset all settings to safe defaults?", "New Config", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgResult == MessageBoxResult.Yes)
            {
                Settings = new ConfigSettings();
                CurrentFilePath = string.Empty;
                StatusMessage = "Config reset to default settings.";
                StatusIsError = false;
            }
        }

        private void ExecuteOpenConfig()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Config Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Select config.txt to open"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var lines = File.ReadAllLines(dlg.FileName);
                    Settings.LoadFromLines(lines);
                    CurrentFilePath = dlg.FileName;
                    StatusMessage = $"Loaded {lines.Length} lines from {Path.GetFileName(dlg.FileName)}";
                    StatusIsError = false;
                    UpdatePreview();
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error opening file: {ex.Message}";
                    StatusIsError = true;
                }
            }
        }

        private void ExecuteSaveConfig()
        {
            if (string.IsNullOrWhiteSpace(CurrentFilePath))
            {
                ExecuteSaveAsConfig();
                return;
            }

            try
            {
                var lines = Settings.GenerateLines();
                File.WriteAllLines(CurrentFilePath, lines);
                StatusMessage = $"Successfully saved 55 lines to {CurrentFilePath}";
                StatusIsError = false;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to save: {ex.Message}";
                StatusIsError = true;
            }
        }

        private void ExecuteSaveAsConfig()
        {
            var dlg = new SaveFileDialog
            {
                FileName = "config.txt",
                DefaultExt = ".txt",
                Filter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Save config.txt"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var lines = Settings.GenerateLines();
                    File.WriteAllLines(dlg.FileName, lines);
                    CurrentFilePath = dlg.FileName;
                    StatusMessage = $"Saved config.txt with 55 lines to {dlg.FileName}";
                    StatusIsError = false;
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Failed to save: {ex.Message}";
                    StatusIsError = true;
                }
            }
        }

        private void ExecuteCopyToClipboard()
        {
            try
            {
                Clipboard.SetText(RawConfigPreview);
                StatusMessage = "Configuration copied to clipboard (55 lines).";
                StatusIsError = false;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Clipboard error: {ex.Message}";
                StatusIsError = true;
            }
        }

        private void ApplyResolutionPreset(string preset)
        {
            if (preset.Contains("1920x1080"))
            {
                CalcResWidth = 1920;
                CalcResHeight = 1080;
            }
            else if (preset.Contains("2560x1440"))
            {
                CalcResWidth = 2560;
                CalcResHeight = 1440;
            }
            else if (preset.Contains("3840x2160"))
            {
                CalcResWidth = 3840;
                CalcResHeight = 2160;
            }
        }

        private void ExecuteCalculateBounds()
        {
            if (CalcResWidth <= 0 || CalcResHeight <= 0)
            {
                StatusMessage = "Resolution width and height must be positive.";
                StatusIsError = true;
                return;
            }

            int margin = Math.Max(0, CalcMargin);
            Settings.ClampMinX = margin;
            Settings.ClampMaxX = Math.Max(margin, CalcResWidth - margin);
            Settings.ClampMinY = margin;
            Settings.ClampMaxY = Math.Max(margin, CalcResHeight - margin);
            Settings.ClampEnabled = true;

            StatusMessage = $"Calculated clamp: {Settings.ClampMinX}|{Settings.ClampMaxX}|{Settings.ClampMinY}|{Settings.ClampMaxY} for {CalcResWidth}x{CalcResHeight}";
            StatusIsError = false;
        }

        private void ExecuteAddGatheringFilter()
        {
            var newItem = new GatheringFilterItem
            {
                ResourceName = "WOOD",
                ColorValue = 160000,
                MinTier = 2,
                MinRarity = 1
            };
            Settings.GatheringFilters.Add(newItem);
            SelectedGatheringFilter = newItem;
        }

        private void ExecuteRemoveGatheringFilter()
        {
            if (SelectedGatheringFilter != null)
            {
                Settings.GatheringFilters.Remove(SelectedGatheringFilter);
                SelectedGatheringFilter = Settings.GatheringFilters.FirstOrDefault();
            }
        }

        private void ExecuteAddMobFilter()
        {
            var newItem = new MobColorFilterItem
            {
                MobName = "NEW_MOB_NAME",
                ColorValue = 120000
            };
            Settings.MobColorFilters.Add(newItem);
            SelectedMobFilter = newItem;
        }

        private void ExecuteRemoveMobFilter()
        {
            if (SelectedMobFilter != null)
            {
                Settings.MobColorFilters.Remove(SelectedMobFilter);
                SelectedMobFilter = Settings.MobColorFilters.FirstOrDefault();
            }
        }

        private void ExecuteApplyVlcPreset()
        {
            Settings.DetectionSystemCommand = "\"C:\\Program Files\\VideoLAN\\VLC\\vlc\" --qt-start-minimized --play-and-exit \"C:\\soundfile.mp3\"";
            StatusMessage = "Applied VLC sound trigger command preset.";
            StatusIsError = false;
        }

        private void ExecuteApplyPowershellSoundPreset()
        {
            Settings.DetectionSystemCommand = "powershell -c [System.Media.SystemSounds]::Exclamation.Play()";
            StatusMessage = "Applied Windows System Sound command preset.";
            StatusIsError = false;
        }
    }
}
