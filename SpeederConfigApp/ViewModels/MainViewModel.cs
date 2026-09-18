using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // Active Tab Index (0 = Config 55 Lines, 1 = Macro Studio, 2 = Waymark Studio, 3 = Reference Cheatsheet)
        private int _activeTabIndex = 0;

        // Bounding box calculator properties
        private int _calcResWidth = 1920;
        private int _calcResHeight = 1080;
        private int _calcMargin = 100;
        private string _selectedResPreset = "1920x1080 (1080p)";

        // Filter selection
        private GatheringFilterItem? _selectedGatheringFilter;
        private MobColorFilterItem? _selectedMobFilter;

        // Macro Studio properties
        private MacroFileModel _macroFile = null!;
        private MacroItem? _selectedMacro;

        // Waymark Studio properties
        private WaymarkFileModel _waymarkFile = null!;
        private WaymarkPoint? _selectedWaymarkPoint;
        private WaymarkVariable? _selectedWaymarkVariable;

        // Reference Tab properties & filtering
        private string _searchReferenceText = string.Empty;
        private readonly List<ConsoleCommandItem> _allConsoleCommands;
        private readonly List<MacroSyntaxItem> _allMacroCommands;
        private readonly List<VirtualKeyInfo> _allVirtualKeys;

        public int ActiveTabIndex
        {
            get => _activeTabIndex;
            set => SetProperty(ref _activeTabIndex, value);
        }

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

        // Macro Studio Models
        public MacroFileModel MacroFile
        {
            get => _macroFile;
            set
            {
                if (SetProperty(ref _macroFile, value))
                {
                    SelectedMacro = _macroFile?.Macros.FirstOrDefault();
                }
            }
        }

        public MacroItem? SelectedMacro
        {
            get => _selectedMacro;
            set => SetProperty(ref _selectedMacro, value);
        }

        // Waymark Studio Models
        public WaymarkFileModel WaymarkFile
        {
            get => _waymarkFile;
            set
            {
                if (SetProperty(ref _waymarkFile, value))
                {
                    SelectedWaymarkPoint = _waymarkFile?.Points.FirstOrDefault();
                    SelectedWaymarkVariable = _waymarkFile?.Variables.FirstOrDefault();
                }
            }
        }

        public WaymarkPoint? SelectedWaymarkPoint
        {
            get => _selectedWaymarkPoint;
            set => SetProperty(ref _selectedWaymarkPoint, value);
        }

        public WaymarkVariable? SelectedWaymarkVariable
        {
            get => _selectedWaymarkVariable;
            set => SetProperty(ref _selectedWaymarkVariable, value);
        }

        // Reference Tab Collections
        public string SearchReferenceText
        {
            get => _searchReferenceText;
            set
            {
                if (SetProperty(ref _searchReferenceText, value))
                {
                    ApplyReferenceFilter();
                }
            }
        }

        public string MacroIniPreview => _macroFile?.GenerateIni() ?? string.Empty;
        public string WaymarkIniPreview => _waymarkFile?.GenerateIni() ?? string.Empty;

        public void UpdateMacroPreview() => OnPropertyChanged(nameof(MacroIniPreview));
        public void UpdateWaymarkPreview() => OnPropertyChanged(nameof(WaymarkIniPreview));

        public ObservableCollection<ConsoleCommandItem> FilteredConsoleCommands { get; } = new ObservableCollection<ConsoleCommandItem>();
        public ObservableCollection<MacroSyntaxItem> FilteredMacroCommands { get; } = new ObservableCollection<MacroSyntaxItem>();
        public ObservableCollection<VirtualKeyInfo> FilteredVirtualKeys { get; } = new ObservableCollection<VirtualKeyInfo>();

        #region Commands
        // Navigation & General Commands
        public RelayCommand SwitchTabCommand { get; }
        public RelayCommand ClearReferenceSearchCommand { get; }
        public RelayCommand CopyMacroIniCommand { get; }
        public RelayCommand CopyWaymarkIniCommand { get; }
        // Config Tab Commands
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

        // Macro Studio Commands
        public RelayCommand NewMacroFileCommand { get; }
        public RelayCommand OpenMacroFileCommand { get; }
        public RelayCommand SaveMacroFileCommand { get; }
        public RelayCommand SaveAsMacroFileCommand { get; }
        public RelayCommand AddMacroCommand { get; }
        public RelayCommand DeleteSelectedMacroCommand { get; }
        public RelayCommand CloneSelectedMacroCommand { get; }
        public RelayCommand AddKeysLineCommand { get; }
        public RelayCommand RemoveKeysLineCommand { get; }
        public RelayCommand ApplyMacroTemplateCommand { get; }
        public RelayCommand LinkMacroToConfigCommand { get; }

        // Waymark Studio Commands
        public RelayCommand NewWaymarkFileCommand { get; }
        public RelayCommand OpenWaymarkFileCommand { get; }
        public RelayCommand SaveWaymarkFileCommand { get; }
        public RelayCommand SaveAsWaymarkFileCommand { get; }
        public RelayCommand AddWaymarkPointCommand { get; }
        public RelayCommand DeleteSelectedWaymarkPointCommand { get; }
        public RelayCommand MoveWaymarkUpCommand { get; }
        public RelayCommand MoveWaymarkDownCommand { get; }
        public RelayCommand ToggleStopMovementCommand { get; }
        public RelayCommand ToggleSeamlessMovementCommand { get; }
        public RelayCommand AddVariableCommand { get; }
        public RelayCommand RemoveVariableCommand { get; }

        // Reference Tab Commands
        public RelayCommand CopyReferenceSnippetCommand { get; }
        #endregion

        public MainViewModel()
        {
            _settings = new ConfigSettings();
            _settings.PropertyChanged += OnSettingsPropertyChanged;
            _settings.GatheringFilters.CollectionChanged += OnCollectionChanged;
            _settings.MobColorFilters.CollectionChanged += OnCollectionChanged;

            // Initialize Config Tab Commands
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

            // Initialize Macro Studio
            _macroFile = MacroFileModel.CreateDefaultTemplates();
            _selectedMacro = _macroFile.Macros.FirstOrDefault();

            NewMacroFileCommand = new RelayCommand(ExecuteNewMacroFile);
            OpenMacroFileCommand = new RelayCommand(ExecuteOpenMacroFile);
            SaveMacroFileCommand = new RelayCommand(ExecuteSaveMacroFile);
            SaveAsMacroFileCommand = new RelayCommand(ExecuteSaveAsMacroFile);
            AddMacroCommand = new RelayCommand(ExecuteAddMacro);
            DeleteSelectedMacroCommand = new RelayCommand(ExecuteDeleteSelectedMacro);
            CloneSelectedMacroCommand = new RelayCommand(ExecuteCloneSelectedMacro);
            AddKeysLineCommand = new RelayCommand(ExecuteAddKeysLine);
            RemoveKeysLineCommand = new RelayCommand(ExecuteRemoveKeysLine);
            ApplyMacroTemplateCommand = new RelayCommand(ExecuteApplyMacroTemplate);
            LinkMacroToConfigCommand = new RelayCommand(ExecuteLinkMacroToConfig);

            // Initialize Waymark Studio
            _waymarkFile = WaymarkFileModel.CreateSampleRoute();
            _selectedWaymarkPoint = _waymarkFile.Points.FirstOrDefault();
            _selectedWaymarkVariable = _waymarkFile.Variables.FirstOrDefault();

            NewWaymarkFileCommand = new RelayCommand(ExecuteNewWaymarkFile);
            OpenWaymarkFileCommand = new RelayCommand(ExecuteOpenWaymarkFile);
            SaveWaymarkFileCommand = new RelayCommand(ExecuteSaveWaymarkFile);
            SaveAsWaymarkFileCommand = new RelayCommand(ExecuteSaveAsWaymarkFile);
            AddWaymarkPointCommand = new RelayCommand(ExecuteAddWaymarkPoint);
            DeleteSelectedWaymarkPointCommand = new RelayCommand(ExecuteDeleteSelectedWaymarkPoint);
            MoveWaymarkUpCommand = new RelayCommand(ExecuteMoveWaymarkUp);
            MoveWaymarkDownCommand = new RelayCommand(ExecuteMoveWaymarkDown);
            ToggleStopMovementCommand = new RelayCommand(ExecuteToggleStopMovement);
            ToggleSeamlessMovementCommand = new RelayCommand(ExecuteToggleSeamlessMovement);
            AddVariableCommand = new RelayCommand(ExecuteAddVariable);
            RemoveVariableCommand = new RelayCommand(ExecuteRemoveVariable);

            // Initialize Reference Tab
            _allConsoleCommands = ReferenceRepository.GetAllConsoleCommands().ToList();
            _allMacroCommands = ReferenceRepository.GetAllMacroSyntax().ToList();
            _allVirtualKeys = VirtualKeyHelper.AllKeys.ToList();

            CopyReferenceSnippetCommand = new RelayCommand(ExecuteCopyReferenceSnippet);
            ApplyReferenceFilter();

            SwitchTabCommand = new RelayCommand(p =>
            {
                if (int.TryParse(p?.ToString(), out int idx))
                {
                    ActiveTabIndex = idx;
                }
            });
            ClearReferenceSearchCommand = new RelayCommand(() => SearchReferenceText = string.Empty);
            CopyMacroIniCommand = new RelayCommand(() =>
            {
                try
                {
                    Clipboard.SetText(MacroIniPreview);
                    StatusMessage = $"Copied {MacroFile.Macros.Count} macros to clipboard.";
                    StatusIsError = false;
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Clipboard error: {ex.Message}";
                    StatusIsError = true;
                }
            });
            CopyWaymarkIniCommand = new RelayCommand(() =>
            {
                try
                {
                    Clipboard.SetText(WaymarkIniPreview);
                    StatusMessage = $"Copied {WaymarkFile.Points.Count} waypoints to clipboard.";
                    StatusIsError = false;
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Clipboard error: {ex.Message}";
                    StatusIsError = true;
                }
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

        #region Config Execution Methods
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
        #endregion

        #region Macro Studio Execution Methods
        private void ExecuteNewMacroFile()
        {
            MacroFile = MacroFileModel.CreateDefaultTemplates();
            StatusMessage = "Created new Macro file template with 8 default macros.";
            StatusIsError = false;
        }

        private void ExecuteOpenMacroFile()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Macro Files (*.ini)|*.ini|All Files (*.*)|*.*",
                Title = "Open Macro INI File"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    MacroFile = MacroFileModel.Load(dlg.FileName);
                    StatusMessage = $"Loaded {MacroFile.Macros.Count} macros from {MacroFile.FileName}";
                    StatusIsError = false;
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error opening macro file: {ex.Message}";
                    StatusIsError = true;
                }
            }
        }

        private void ExecuteSaveMacroFile()
        {
            if (string.IsNullOrWhiteSpace(MacroFile.FilePath))
            {
                ExecuteSaveAsMacroFile();
                return;
            }
            try
            {
                MacroFile.Save();
                StatusMessage = $"Successfully saved macro file to {MacroFile.FilePath}";
                StatusIsError = false;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to save macro file: {ex.Message}";
                StatusIsError = true;
            }
        }

        private void ExecuteSaveAsMacroFile()
        {
            var dlg = new SaveFileDialog
            {
                FileName = string.IsNullOrWhiteSpace(MacroFile.FileName) ? "macros.ini" : MacroFile.FileName,
                DefaultExt = ".ini",
                Filter = "Macro Files (*.ini)|*.ini|All Files (*.*)|*.*",
                Title = "Save Macro File As"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    MacroFile.Save(dlg.FileName);
                    StatusMessage = $"Saved macro file to {dlg.FileName}";
                    StatusIsError = false;
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Failed to save macro file: {ex.Message}";
                    StatusIsError = true;
                }
            }
        }

        private void ExecuteAddMacro()
        {
            var newMacro = new MacroItem
            {
                TriggerKey = 113, // F2 default
                Repeat = 0,
                Description = "New Custom Macro"
            };
            newMacro.KeysLines.Add("nop");
            MacroFile.Macros.Add(newMacro);
            SelectedMacro = newMacro;
            StatusMessage = "Added new macro.";
            StatusIsError = false;
            UpdateMacroPreview();
        }

        private void ExecuteDeleteSelectedMacro()
        {
            if (SelectedMacro != null)
            {
                int idx = MacroFile.Macros.IndexOf(SelectedMacro);
                MacroFile.Macros.Remove(SelectedMacro);
                if (idx >= MacroFile.Macros.Count) idx = MacroFile.Macros.Count - 1;
                SelectedMacro = idx >= 0 ? MacroFile.Macros[idx] : null;
                StatusMessage = "Deleted selected macro.";
                StatusIsError = false;
                UpdateMacroPreview();
            }
        }

        private void ExecuteCloneSelectedMacro()
        {
            if (SelectedMacro != null)
            {
                var clone = SelectedMacro.Clone();
                clone.Description = string.IsNullOrWhiteSpace(clone.Description) ? "Copy" : $"{clone.Description} (Copy)";
                int idx = MacroFile.Macros.IndexOf(SelectedMacro);
                if (idx >= 0 && idx < MacroFile.Macros.Count - 1)
                    MacroFile.Macros.Insert(idx + 1, clone);
                else
                    MacroFile.Macros.Add(clone);
                SelectedMacro = clone;
                StatusMessage = $"Cloned macro [{clone.DisplayName}].";
                StatusIsError = false;
                UpdateMacroPreview();
            }
        }

        private void ExecuteAddKeysLine(object? parameter)
        {
            if (SelectedMacro != null)
            {
                string line = parameter?.ToString() ?? "nop";
                SelectedMacro.KeysLines.Add(line);
                StatusMessage = "Added keys line to macro.";
                StatusIsError = false;
                UpdateMacroPreview();
            }
        }

        private void ExecuteRemoveKeysLine(object? parameter)
        {
            if (SelectedMacro != null)
            {
                if (parameter is string line && SelectedMacro.KeysLines.Contains(line))
                {
                    SelectedMacro.KeysLines.Remove(line);
                }
                else if (parameter is int idx && idx >= 0 && idx < SelectedMacro.KeysLines.Count)
                {
                    SelectedMacro.KeysLines.RemoveAt(idx);
                }
                else if (SelectedMacro.KeysLines.Count > 0)
                {
                    SelectedMacro.KeysLines.RemoveAt(SelectedMacro.KeysLines.Count - 1);
                }
                StatusMessage = "Removed keys line from macro.";
                StatusIsError = false;
                UpdateMacroPreview();
            }
        }

        private void ExecuteApplyMacroTemplate(object? parameter)
        {
            if (parameter is MacroItem item)
            {
                var clone = item.Clone();
                MacroFile.Macros.Add(clone);
                SelectedMacro = clone;
                StatusMessage = $"Applied macro template: {clone.DisplayName}";
                StatusIsError = false;
            }
            else if (parameter is string name && !string.IsNullOrWhiteSpace(name))
            {
                var templates = MacroFileModel.CreateDefaultTemplates();
                var match = templates.Macros.FirstOrDefault(m =>
                    m.DisplayName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                    m.Description.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                    m.SectionHeader.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    var clone = match.Clone();
                    MacroFile.Macros.Add(clone);
                    SelectedMacro = clone;
                    StatusMessage = $"Applied template: {clone.DisplayName}";
                    StatusIsError = false;
                }
                else
                {
                    StatusMessage = $"Template '{name}' not found.";
                    StatusIsError = true;
                }
            }
            else
            {
                var templates = MacroFileModel.CreateDefaultTemplates();
                foreach (var m in templates.Macros)
                {
                    MacroFile.Macros.Add(m.Clone());
                }
                SelectedMacro = MacroFile.Macros.LastOrDefault();
                StatusMessage = "Appended default macro templates.";
                StatusIsError = false;
            }
            UpdateMacroPreview();
        }

        private void ExecuteLinkMacroToConfig()
        {
            Settings.MacroFileName = MacroFile.FileName;
            UpdatePreview();
            StatusMessage = $"Linked macro file '{MacroFile.FileName}' to Config Line 29.";
            StatusIsError = false;
        }
        #endregion

        #region Waymark Studio Execution Methods
        private void ExecuteNewWaymarkFile()
        {
            WaymarkFile = WaymarkFileModel.CreateSampleRoute();
            StatusMessage = "Created new sample Waymark route with 4 waypoints.";
            StatusIsError = false;
        }

        private void ExecuteOpenWaymarkFile()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Waymark Files (*.ini)|*.ini|All Files (*.*)|*.*",
                Title = "Open Waymark INI File"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    WaymarkFile = WaymarkFileModel.Load(dlg.FileName);
                    StatusMessage = $"Loaded {WaymarkFile.Points.Count} waypoints from {WaymarkFile.FileName}";
                    StatusIsError = false;
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error opening waymark file: {ex.Message}";
                    StatusIsError = true;
                }
            }
        }

        private void ExecuteSaveWaymarkFile()
        {
            if (string.IsNullOrWhiteSpace(WaymarkFile.FilePath))
            {
                ExecuteSaveAsWaymarkFile();
                return;
            }
            try
            {
                WaymarkFile.Save();
                StatusMessage = $"Successfully saved waymark file to {WaymarkFile.FilePath}";
                StatusIsError = false;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to save waymark file: {ex.Message}";
                StatusIsError = true;
            }
        }

        private void ExecuteSaveAsWaymarkFile()
        {
            var dlg = new SaveFileDialog
            {
                FileName = string.IsNullOrWhiteSpace(WaymarkFile.FileName) ? "Recorded Waymarks.ini" : WaymarkFile.FileName,
                DefaultExt = ".ini",
                Filter = "Waymark Files (*.ini)|*.ini|All Files (*.*)|*.*",
                Title = "Save Waymark File As"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    WaymarkFile.Save(dlg.FileName);
                    StatusMessage = $"Saved waymark file to {dlg.FileName}";
                    StatusIsError = false;
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Failed to save waymark file: {ex.Message}";
                    StatusIsError = true;
                }
            }
        }

        private void ExecuteAddWaymarkPoint()
        {
            var last = WaymarkFile.Points.LastOrDefault();
            double newX = last != null ? last.X + 5.0 : 0.0;
            double newY = last != null ? last.Y : 0.0;
            double newZ = last != null ? last.Z + 5.0 : 0.0;
            var pt = WaymarkFile.AddPoint(newX, newY, newZ, 30);
            SelectedWaymarkPoint = pt;
            StatusMessage = $"Added Waypoint [{pt.Index}].";
            StatusIsError = false;
            UpdateWaymarkPreview();
        }

        private void ExecuteDeleteSelectedWaymarkPoint()
        {
            if (SelectedWaymarkPoint != null)
            {
                int idx = WaymarkFile.Points.IndexOf(SelectedWaymarkPoint);
                WaymarkFile.Points.Remove(SelectedWaymarkPoint);
                WaymarkFile.ReindexPoints();
                if (idx >= WaymarkFile.Points.Count) idx = WaymarkFile.Points.Count - 1;
                SelectedWaymarkPoint = idx >= 0 ? WaymarkFile.Points[idx] : null;
                StatusMessage = "Deleted waypoint and re-indexed route.";
                StatusIsError = false;
                UpdateWaymarkPreview();
            }
        }

        private void ExecuteMoveWaymarkUp()
        {
            if (SelectedWaymarkPoint != null)
            {
                int idx = WaymarkFile.Points.IndexOf(SelectedWaymarkPoint);
                if (idx > 0)
                {
                    WaymarkFile.Points.Move(idx, idx - 1);
                    WaymarkFile.ReindexPoints();
                    StatusMessage = $"Moved Waypoint to index [{SelectedWaymarkPoint.Index}].";
                    StatusIsError = false;
                    UpdateWaymarkPreview();
                }
            }
        }

        private void ExecuteMoveWaymarkDown()
        {
            if (SelectedWaymarkPoint != null)
            {
                int idx = WaymarkFile.Points.IndexOf(SelectedWaymarkPoint);
                if (idx >= 0 && idx < WaymarkFile.Points.Count - 1)
                {
                    WaymarkFile.Points.Move(idx, idx + 1);
                    WaymarkFile.ReindexPoints();
                    StatusMessage = $"Moved Waypoint to index [{SelectedWaymarkPoint.Index}].";
                    StatusIsError = false;
                    UpdateWaymarkPreview();
                }
            }
        }

        private void ExecuteToggleStopMovement()
        {
            if (SelectedWaymarkPoint != null)
            {
                SelectedWaymarkPoint.IsStopMovement = !SelectedWaymarkPoint.IsStopMovement;
                StatusMessage = SelectedWaymarkPoint.IsStopMovement
                    ? $"Waypoint [{SelectedWaymarkPoint.Index}] set to Stop Movement (x=0.1)."
                    : $"Waypoint [{SelectedWaymarkPoint.Index}] restored to normal movement.";
                StatusIsError = false;
                UpdateWaymarkPreview();
            }
        }

        private void ExecuteToggleSeamlessMovement()
        {
            if (SelectedWaymarkPoint != null)
            {
                SelectedWaymarkPoint.IsSeamlessMovement = !SelectedWaymarkPoint.IsSeamlessMovement;
                StatusMessage = SelectedWaymarkPoint.IsSeamlessMovement
                    ? $"Waypoint [{SelectedWaymarkPoint.Index}] set to Seamless Movement (wait time=1)."
                    : $"Waypoint [{SelectedWaymarkPoint.Index}] restored to standard wait time (30ms).";
                StatusIsError = false;
                UpdateWaymarkPreview();
            }
        }

        private void ExecuteAddVariable()
        {
            var newVar = new WaymarkVariable("newVar", "0");
            WaymarkFile.Variables.Add(newVar);
            SelectedWaymarkVariable = newVar;
            StatusMessage = "Added route variable.";
            StatusIsError = false;
            UpdateWaymarkPreview();
        }

        private void ExecuteRemoveVariable(object? parameter)
        {
            var target = parameter as WaymarkVariable ?? SelectedWaymarkVariable;
            if (target != null)
            {
                WaymarkFile.Variables.Remove(target);
                SelectedWaymarkVariable = WaymarkFile.Variables.FirstOrDefault();
                StatusMessage = "Removed route variable.";
                StatusIsError = false;
                UpdateWaymarkPreview();
            }
        }
        #endregion

        #region Reference Cheatsheet Methods
        private void ApplyReferenceFilter()
        {
            string term = SearchReferenceText?.Trim() ?? string.Empty;

            // 1. Console Commands
            FilteredConsoleCommands.Clear();
            var matchedConsole = string.IsNullOrEmpty(term)
                ? _allConsoleCommands
                : _allConsoleCommands.Where(c =>
                    c.Command.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    c.Description.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    c.Category.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    c.Arguments.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    c.Example.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    c.Syntax.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    c.RelatedConfigLine.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    c.Notes.Contains(term, StringComparison.OrdinalIgnoreCase));
            foreach (var item in matchedConsole)
            {
                FilteredConsoleCommands.Add(item);
            }

            // 2. Macro Syntax
            FilteredMacroCommands.Clear();
            var matchedMacro = string.IsNullOrEmpty(term)
                ? _allMacroCommands
                : _allMacroCommands.Where(m =>
                    m.Command.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    m.Description.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    m.Category.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    m.Syntax.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    m.Example.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    m.Notes.Contains(term, StringComparison.OrdinalIgnoreCase));
            foreach (var item in matchedMacro)
            {
                FilteredMacroCommands.Add(item);
            }

            // 3. Virtual Keys
            FilteredVirtualKeys.Clear();
            var matchedKeys = string.IsNullOrEmpty(term)
                ? _allVirtualKeys
                : _allVirtualKeys.Where(k =>
                    k.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    k.Code.ToString().Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    k.Category.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    k.Notes.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    k.DisplayText.Contains(term, StringComparison.OrdinalIgnoreCase));
            foreach (var item in matchedKeys)
            {
                FilteredVirtualKeys.Add(item);
            }
        }

        private void ExecuteCopyReferenceSnippet(object? parameter)
        {
            string? textToCopy = null;
            if (parameter is ConsoleCommandItem cci)
            {
                textToCopy = string.IsNullOrWhiteSpace(cci.Example) ? cci.Syntax : cci.Example;
            }
            else if (parameter is MacroSyntaxItem msi)
            {
                textToCopy = string.IsNullOrWhiteSpace(msi.Example) ? msi.Syntax : msi.Example;
            }
            else if (parameter is VirtualKeyInfo vki)
            {
                textToCopy = vki.Code.ToString();
            }
            else if (parameter is string str)
            {
                textToCopy = str;
            }

            if (!string.IsNullOrEmpty(textToCopy))
            {
                try
                {
                    Clipboard.SetText(textToCopy);
                    StatusMessage = $"Copied snippet to clipboard: {textToCopy}";
                    StatusIsError = false;
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Clipboard error: {ex.Message}";
                    StatusIsError = true;
                }
            }
        }
        #endregion
    }
}
