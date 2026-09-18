using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SpeederConfigApp.Models
{
    public class ConfigSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        #region Line 1 - 10
        // Line 1: Master toggle key
        private int _masterToggleKey = 0;
        public int MasterToggleKey { get => _masterToggleKey; set { _masterToggleKey = value; OnPropertyChanged(); } }

        // Line 2: Movement speed multiplier (e.g. 1.05 = 5%, 1.1 = 10%)
        private double _movementSpeedMultiplier = 1.05;
        public double MovementSpeedMultiplier { get => _movementSpeedMultiplier; set { _movementSpeedMultiplier = value; OnPropertyChanged(); } }

        // Line 3: Movement speed toggle key
        private int _movementSpeedToggleKey = 0;
        public int MovementSpeedToggleKey { get => _movementSpeedToggleKey; set { _movementSpeedToggleKey = value; OnPropertyChanged(); } }

        // Line 4: Max zoom multiplier (e.g. 0.5 = 50%, 1.0 = double)
        private double _maxZoomMultiplier = 0.5;
        public double MaxZoomMultiplier { get => _maxZoomMultiplier; set { _maxZoomMultiplier = value; OnPropertyChanged(); } }

        // Line 5: Max zoom toggle key
        private int _maxZoomToggleKey = 0;
        public int MaxZoomToggleKey { get => _maxZoomToggleKey; set { _maxZoomToggleKey = value; OnPropertyChanged(); } }

        // Line 6: Rotate camera right key
        private int _cameraRotateRightKey = 0;
        public int CameraRotateRightKey { get => _cameraRotateRightKey; set { _cameraRotateRightKey = value; OnPropertyChanged(); } }

        // Line 7: Rotate camera left key
        private int _cameraRotateLeftKey = 0;
        public int CameraRotateLeftKey { get => _cameraRotateLeftKey; set { _cameraRotateLeftKey = value; OnPropertyChanged(); } }

        // Line 8: Camera rotation step horizontal
        private double _cameraRotateStepHorizontal = 5.0;
        public double CameraRotateStepHorizontal { get => _cameraRotateStepHorizontal; set { _cameraRotateStepHorizontal = value; OnPropertyChanged(); } }

        // Line 9: Rotate camera up key
        private int _cameraRotateUpKey = 0;
        public int CameraRotateUpKey { get => _cameraRotateUpKey; set { _cameraRotateUpKey = value; OnPropertyChanged(); } }

        // Line 10: Rotate camera down key
        private int _cameraRotateDownKey = 0;
        public int CameraRotateDownKey { get => _cameraRotateDownKey; set { _cameraRotateDownKey = value; OnPropertyChanged(); } }
        #endregion

        #region Line 11 - 20
        // Line 11: Camera rotation step vertical
        private double _cameraRotateStepVertical = 5.0;
        public double CameraRotateStepVertical { get => _cameraRotateStepVertical; set { _cameraRotateStepVertical = value; OnPropertyChanged(); } }

        // Line 12: Camera free-look hold key
        private int _cameraFreeLookHoldKey = 0;
        public int CameraFreeLookHoldKey { get => _cameraFreeLookHoldKey; set { _cameraFreeLookHoldKey = value; OnPropertyChanged(); } }

        // Line 13: Invert Y-axis for free look (1 = inverted, 0 = normal)
        private bool _cameraInvertY = false;
        public bool CameraInvertY { get => _cameraInvertY; set { _cameraInvertY = value; OnPropertyChanged(); } }

        // Line 14: Invert X-axis for free look (1 = inverted, 0 = normal)
        private bool _cameraInvertX = false;
        public bool CameraInvertX { get => _cameraInvertX; set { _cameraInvertX = value; OnPropertyChanged(); } }

        // Line 15: Deprecated
        private string _line15Deprecated = string.Empty;
        public string Line15Deprecated { get => _line15Deprecated; set { _line15Deprecated = value; OnPropertyChanged(); } }

        // Line 16: Camera swing speed (0.01 to 100+)
        private double _cameraSwingSpeed = 1.0;
        public double CameraSwingSpeed { get => _cameraSwingSpeed; set { _cameraSwingSpeed = value; OnPropertyChanged(); } }

        // Line 17: Lock X-axis camera movement (1 = locked, 0 = normal)
        private bool _cameraLockX = false;
        public bool CameraLockX { get => _cameraLockX; set { _cameraLockX = value; OnPropertyChanged(); } }

        // Line 18: Lock Y-axis camera movement (1 = locked, 0 = normal)
        private bool _cameraLockY = false;
        public bool CameraLockY { get => _cameraLockY; set { _cameraLockY = value; OnPropertyChanged(); } }

        // Line 19: Screen clamp boundary min_x|max_x|min_y|max_y
        private int _clampMinX = 100;
        public int ClampMinX { get => _clampMinX; set { _clampMinX = value; OnPropertyChanged(); } }

        private int _clampMaxX = 1820;
        public int ClampMaxX { get => _clampMaxX; set { _clampMaxX = value; OnPropertyChanged(); } }

        private int _clampMinY = 100;
        public int ClampMinY { get => _clampMinY; set { _clampMinY = value; OnPropertyChanged(); } }

        private int _clampMaxY = 980;
        public int ClampMaxY { get => _clampMaxY; set { _clampMaxY = value; OnPropertyChanged(); } }

        public bool ClampEnabled { get; set; } = true;

        // Line 20: Driver name (must match Driver.ini)
        private string _driverName = string.Empty;
        public string DriverName { get => _driverName; set { _driverName = value; OnPropertyChanged(); } }
        #endregion

        #region Line 21 - 30
        // Line 21: Field of View (e.g. 1.0, 0.5, or 0.5|0.75)
        private string _fieldOfView = "1";
        public string FieldOfView { get => _fieldOfView; set { _fieldOfView = value; OnPropertyChanged(); } }

        // Line 22: Waymark pause/resume key
        private int _waymarkPauseResumeKey = 0;
        public int WaymarkPauseResumeKey { get => _waymarkPauseResumeKey; set { _waymarkPauseResumeKey = value; OnPropertyChanged(); } }

        // Line 23: Waymark arrival tolerance distance
        private double _waymarkArrivalTolerance = 3.0;
        public double WaymarkArrivalTolerance { get => _waymarkArrivalTolerance; set { _waymarkArrivalTolerance = value; OnPropertyChanged(); } }

        // Line 24: Waymark sleep jitter range (0 to 1)
        private double _waymarkSleepJitter = 0.05;
        public double WaymarkSleepJitter { get => _waymarkSleepJitter; set { _waymarkSleepJitter = value; OnPropertyChanged(); } }

        // Line 25: Waymark failsafe timeout ms
        private int _waymarkTimeoutMs = 5000;
        public int WaymarkTimeoutMs { get => _waymarkTimeoutMs; set { _waymarkTimeoutMs = value; OnPropertyChanged(); } }

        // Line 26: Waymark cursor offset pixels
        private int _waymarkCursorOffsetPx = 100;
        public int WaymarkCursorOffsetPx { get => _waymarkCursorOffsetPx; set { _waymarkCursorOffsetPx = value; OnPropertyChanged(); } }

        // Line 27: Record coordinates key (-cl command)
        private int _recordCoordinatesKey = 0;
        public int RecordCoordinatesKey { get => _recordCoordinatesKey; set { _recordCoordinatesKey = value; OnPropertyChanged(); } }

        // Line 28: Camera hacks master toggle key
        private int _cameraHacksToggleKey = 0;
        public int CameraHacksToggleKey { get => _cameraHacksToggleKey; set { _cameraHacksToggleKey = value; OnPropertyChanged(); } }

        // Line 29: Macro file name
        private string _macroFileName = string.Empty;
        public string MacroFileName { get => _macroFileName; set { _macroFileName = value; OnPropertyChanged(); } }

        // Line 30: Injected keys allowed
        private bool _injectedKeysAllowed = false;
        public bool InjectedKeysAllowed { get => _injectedKeysAllowed; set { _injectedKeysAllowed = value; OnPropertyChanged(); } }
        #endregion

        #region Line 31 - 40
        // Line 31: Mouse smoothing value (0 = disabled, e.g. 200)
        private int _mouseSmoothing = 200;
        public int MouseSmoothing { get => _mouseSmoothing; set { _mouseSmoothing = value; OnPropertyChanged(); } }

        // Line 32: Window center Y adjustment
        private double _windowCenterYAdjustment = 0.0;
        public double WindowCenterYAdjustment { get => _windowCenterYAdjustment; set { _windowCenterYAdjustment = value; OnPropertyChanged(); } }

        // Line 33: DPI/Scaling multiplier for -gpc
        private double _gpcMultiplier = 1.0;
        public double GpcMultiplier { get => _gpcMultiplier; set { _gpcMultiplier = value; OnPropertyChanged(); } }

        // Line 34: Polling interval for nonessential addresses (ms)
        private int _pollingIntervalMs = 1000;
        public int PollingIntervalMs { get => _pollingIntervalMs; set { _pollingIntervalMs = value; OnPropertyChanged(); } }

        // Line 35: Memory read/write debug display
        private bool _memoryDebugEnabled = false;
        public bool MemoryDebugEnabled { get => _memoryDebugEnabled; set { _memoryDebugEnabled = value; OnPropertyChanged(); } }

        // Line 36: Toggle Line 19 mouse clamp key
        private int _clampToggleKey = 0;
        public int ClampToggleKey { get => _clampToggleKey; set { _clampToggleKey = value; OnPropertyChanged(); } }

        // Line 37: Player detection syntax: MinPlayers|MaxDist|TimerMs|Command
        private int _detectionMinPlayers = 1;
        public int DetectionMinPlayers { get => _detectionMinPlayers; set { _detectionMinPlayers = value; OnPropertyChanged(); } }

        private double _detectionMaxDistance = 0.0; // 0 = infinite
        public double DetectionMaxDistance { get => _detectionMaxDistance; set { _detectionMaxDistance = value; OnPropertyChanged(); } }

        private int _detectionTimerMs = 2000;
        public int DetectionTimerMs { get => _detectionTimerMs; set { _detectionTimerMs = value; OnPropertyChanged(); } }

        private string _detectionSystemCommand = string.Empty;
        public string DetectionSystemCommand { get => _detectionSystemCommand; set { _detectionSystemCommand = value; OnPropertyChanged(); } }

        // Line 38: Deprecated
        private string _line38Deprecated = string.Empty;
        public string Line38Deprecated { get => _line38Deprecated; set { _line38Deprecated = value; OnPropertyChanged(); } }

        // Line 39: Output new players appearing in memory to console
        private bool _outputNewPlayersToConsole = false;
        public bool OutputNewPlayersToConsole { get => _outputNewPlayersToConsole; set { _outputNewPlayersToConsole = value; OnPropertyChanged(); } }

        // Line 40: Deprecated
        private string _line40Deprecated = string.Empty;
        public string Line40Deprecated { get => _line40Deprecated; set { _line40Deprecated = value; OnPropertyChanged(); } }
        #endregion

        #region Line 41 - 50
        // Line 41: Deprecated
        private string _line41Deprecated = string.Empty;
        public string Line41Deprecated { get => _line41Deprecated; set { _line41Deprecated = value; OnPropertyChanged(); } }

        // Line 42: Kernel input
        private string _kernelInputSetting = "1";
        public string KernelInputSetting { get => _kernelInputSetting; set { _kernelInputSetting = value; OnPropertyChanged(); } }

        // Line 43: Radar settings
        private bool _radarEnabled = true;
        public bool RadarEnabled { get => _radarEnabled; set { _radarEnabled = value; OnPropertyChanged(); } }

        private int _radarPosX = 0;
        public int RadarPosX { get => _radarPosX; set { _radarPosX = value; OnPropertyChanged(); } }

        private int _radarPosY = 0;
        public int RadarPosY { get => _radarPosY; set { _radarPosY = value; OnPropertyChanged(); } }

        private int _radarWidth = 400;
        public int RadarWidth { get => _radarWidth; set { _radarWidth = value; OnPropertyChanged(); } }

        private int _radarHeight = 400;
        public int RadarHeight { get => _radarHeight; set { _radarHeight = value; OnPropertyChanged(); } }

        private int _radarId = 1;
        public int RadarId { get => _radarId; set { _radarId = value; OnPropertyChanged(); } }

        private int _radarTotalEntities = 1000;
        public int RadarTotalEntities { get => _radarTotalEntities; set { _radarTotalEntities = value; OnPropertyChanged(); } }

        private int _radarFontSize = 14;
        public int RadarFontSize { get => _radarFontSize; set { _radarFontSize = value; OnPropertyChanged(); } }

        private int _radarBgColor = 3618615; // Dark gray
        public int RadarBgColor { get => _radarBgColor; set { _radarBgColor = value; OnPropertyChanged(); } }

        private int _radarMobColor = 255; // Blue
        public int RadarMobColor { get => _radarMobColor; set { _radarMobColor = value; OnPropertyChanged(); } }

        private int _radarPlayerColorOutside = 120000;
        public int RadarPlayerColorOutside { get => _radarPlayerColorOutside; set { _radarPlayerColorOutside = value; OnPropertyChanged(); } }

        private int _radarPlayerColorInside = 160000;
        public int RadarPlayerColorInside { get => _radarPlayerColorInside; set { _radarPlayerColorInside = value; OnPropertyChanged(); } }

        private int _radarGatheringColor = 16711680; // Red
        public int RadarGatheringColor { get => _radarGatheringColor; set { _radarGatheringColor = value; OnPropertyChanged(); } }

        private string _radarCenterIndicator = "*";
        public string RadarCenterIndicator { get => _radarCenterIndicator; set { _radarCenterIndicator = value; OnPropertyChanged(); } }

        private bool _radarShowMobs = true;
        public bool RadarShowMobs { get => _radarShowMobs; set { _radarShowMobs = value; OnPropertyChanged(); } }

        private bool _radarShowPlayers = true;
        public bool RadarShowPlayers { get => _radarShowPlayers; set { _radarShowPlayers = value; OnPropertyChanged(); } }

        private bool _radarShowGathering = true;
        public bool RadarShowGathering { get => _radarShowGathering; set { _radarShowGathering = value; OnPropertyChanged(); } }

        private bool _radarTransparent = false;
        public bool RadarTransparent { get => _radarTransparent; set { _radarTransparent = value; OnPropertyChanged(); } }

        private bool _radarAlwaysOnTop = false;
        public bool RadarAlwaysOnTop { get => _radarAlwaysOnTop; set { _radarAlwaysOnTop = value; OnPropertyChanged(); } }

        private double _radarDistanceMultiplier = 10.0;
        public double RadarDistanceMultiplier { get => _radarDistanceMultiplier; set { _radarDistanceMultiplier = value; OnPropertyChanged(); } }

        private int _radarRefreshRate = 200;
        public int RadarRefreshRate { get => _radarRefreshRate; set { _radarRefreshRate = value; OnPropertyChanged(); } }

        // Line 44: Console window settings
        private bool _consoleEnabled = false;
        public bool ConsoleEnabled { get => _consoleEnabled; set { _consoleEnabled = value; OnPropertyChanged(); } }

        private int _consolePosX = 0;
        public int ConsolePosX { get => _consolePosX; set { _consolePosX = value; OnPropertyChanged(); } }

        private int _consolePosY = 0;
        public int ConsolePosY { get => _consolePosY; set { _consolePosY = value; OnPropertyChanged(); } }

        private int _consoleWidth = 520;
        public int ConsoleWidth { get => _consoleWidth; set { _consoleWidth = value; OnPropertyChanged(); } }

        private int _consoleHeight = 380;
        public int ConsoleHeight { get => _consoleHeight; set { _consoleHeight = value; OnPropertyChanged(); } }

        private int _consoleFontSize = 13;
        public int ConsoleFontSize { get => _consoleFontSize; set { _consoleFontSize = value; OnPropertyChanged(); } }

        private int _consoleBgColor = 3618615; // Dark gray
        public int ConsoleBgColor { get => _consoleBgColor; set { _consoleBgColor = value; OnPropertyChanged(); } }

        private int _consoleTextColor = 16777215; // White
        public int ConsoleTextColor { get => _consoleTextColor; set { _consoleTextColor = value; OnPropertyChanged(); } }

        private bool _consoleTransparent = false;
        public bool ConsoleTransparent { get => _consoleTransparent; set { _consoleTransparent = value; OnPropertyChanged(); } }

        private bool _consoleAlwaysOnTop = true;
        public bool ConsoleAlwaysOnTop { get => _consoleAlwaysOnTop; set { _consoleAlwaysOnTop = value; OnPropertyChanged(); } }

        private string _consoleFont = "Consolas";
        public string ConsoleFont { get => _consoleFont; set { _consoleFont = value; OnPropertyChanged(); } }

        private string _consoleCustomOverride = string.Empty;
        public string ConsoleCustomOverride { get => _consoleCustomOverride; set { _consoleCustomOverride = value; OnPropertyChanged(); } }

        // Line 45: Toggle player detection key (Line 37)
        private int _playerDetectionToggleKey = 0;
        public int PlayerDetectionToggleKey { get => _playerDetectionToggleKey; set { _playerDetectionToggleKey = value; OnPropertyChanged(); } }

        // Line 46: ESP Settings
        private bool _espEnabled = true;
        public bool EspEnabled { get => _espEnabled; set { _espEnabled = value; OnPropertyChanged(); } }

        private int _espTotalEntities = 200;
        public int EspTotalEntities { get => _espTotalEntities; set { _espTotalEntities = value; OnPropertyChanged(); } }

        private int _espFontSize = 18;
        public int EspFontSize { get => _espFontSize; set { _espFontSize = value; OnPropertyChanged(); } }

        private int _espMobColor = 255;
        public int EspMobColor { get => _espMobColor; set { _espMobColor = value; OnPropertyChanged(); } }

        private int _espPlayerColorOutside = 120000;
        public int EspPlayerColorOutside { get => _espPlayerColorOutside; set { _espPlayerColorOutside = value; OnPropertyChanged(); } }

        private int _espPlayerColorInside = 160000;
        public int EspPlayerColorInside { get => _espPlayerColorInside; set { _espPlayerColorInside = value; OnPropertyChanged(); } }

        private int _espGatheringColor = 16711680;
        public int EspGatheringColor { get => _espGatheringColor; set { _espGatheringColor = value; OnPropertyChanged(); } }

        private bool _espShowMobs = true;
        public bool EspShowMobs { get => _espShowMobs; set { _espShowMobs = value; OnPropertyChanged(); } }

        private bool _espShowPlayers = true;
        public bool EspShowPlayers { get => _espShowPlayers; set { _espShowPlayers = value; OnPropertyChanged(); } }

        private bool _espShowGathering = true;
        public bool EspShowGathering { get => _espShowGathering; set { _espShowGathering = value; OnPropertyChanged(); } }

        private int _espRefreshRate = 50;
        public int EspRefreshRate { get => _espRefreshRate; set { _espRefreshRate = value; OnPropertyChanged(); } }

        private string _espFont = string.Empty;
        public string EspFont { get => _espFont; set { _espFont = value; OnPropertyChanged(); } }

        // Line 47: Additional ESP / Targeting Settings (FOV|Yaw|Pitch)
        private double _espTargetingFov = 46.5;
        public double EspTargetingFov { get => _espTargetingFov; set { _espTargetingFov = value; OnPropertyChanged(); } }

        private double _espCameraYaw = 7.0;
        public double EspCameraYaw { get => _espCameraYaw; set { _espCameraYaw = value; OnPropertyChanged(); } }

        private double _espCameraPitch = 0.02;
        public double EspCameraPitch { get => _espCameraPitch; set { _espCameraPitch = value; OnPropertyChanged(); } }

        // Line 48: ESP and Radar Scaling factor (default 1)
        private double _espRadarScaling = 1.0;
        public double EspRadarScaling { get => _espRadarScaling; set { _espRadarScaling = value; OnPropertyChanged(); } }

        // Line 49: Gathering filters
        public ObservableCollection<GatheringFilterItem> GatheringFilters { get; } = new();

        // Line 50: Mounted players color (activate|color|delay)
        private bool _mountedPlayerHighlightActive = true;
        public bool MountedPlayerHighlightActive { get => _mountedPlayerHighlightActive; set { _mountedPlayerHighlightActive = value; OnPropertyChanged(); } }

        private int _mountedPlayerColor = 60000;
        public int MountedPlayerColor { get => _mountedPlayerColor; set { _mountedPlayerColor = value; OnPropertyChanged(); } }

        private int _mountedPlayerDelayMs = 5000;
        public int MountedPlayerDelayMs { get => _mountedPlayerDelayMs; set { _mountedPlayerDelayMs = value; OnPropertyChanged(); } }
        #endregion

        #region Line 51 - 55
        // Line 51: Mobs color filters
        public ObservableCollection<MobColorFilterItem> MobColorFilters { get; } = new();

        // Line 52: Player display text (ESP|Radar)
        private string _playerEspText = "* %hp%";
        public string PlayerEspText { get => _playerEspText; set { _playerEspText = value; OnPropertyChanged(); } }

        private string _playerRadarText = "* %dist%";
        public string PlayerRadarText { get => _playerRadarText; set { _playerRadarText = value; OnPropertyChanged(); } }

        // Line 53: Gathering display text (ESP|Radar)
        private string _gatheringEspText = "* %name% (%num%)";
        public string GatheringEspText { get => _gatheringEspText; set { _gatheringEspText = value; OnPropertyChanged(); } }

        private string _gatheringRadarText = "* %name% (%num%)";
        public string GatheringRadarText { get => _gatheringRadarText; set { _gatheringRadarText = value; OnPropertyChanged(); } }

        // Line 54: Mob display text (ESP|Radar)
        private string _mobEspText = "* %hp%";
        public string MobEspText { get => _mobEspText; set { _mobEspText = value; OnPropertyChanged(); } }

        private string _mobRadarText = "* %name% (%dist%)";
        public string MobRadarText { get => _mobRadarText; set { _mobRadarText = value; OnPropertyChanged(); } }

        // Line 55: Minimum Mob HP threshold for ESP & Radar
        private int _mobMinHpThreshold = 500;
        public int MobMinHpThreshold { get => _mobMinHpThreshold; set { _mobMinHpThreshold = value; OnPropertyChanged(); } }
        #endregion

        public ConfigSettings()
        {
            // Seed default gathering filters as shown in description
            GatheringFilters.Add(new GatheringFilterItem { ResourceName = "WOOD", ColorValue = 160000, MinTier = 2, MinRarity = 1 });
            GatheringFilters.Add(new GatheringFilterItem { ResourceName = "WOOD", ColorValue = 0, MinTier = null, MinRarity = null });
            GatheringFilters.Add(new GatheringFilterItem { ResourceName = "FIBER", ColorValue = 120000, MinTier = 3, MinRarity = 2 });
            GatheringFilters.Add(new GatheringFilterItem { ResourceName = "FIBER", ColorValue = 0, MinTier = null, MinRarity = null });
            GatheringFilters.Add(new GatheringFilterItem { ResourceName = "ROCK", ColorValue = 0, MinTier = null, MinRarity = null });

            // Seed default mob filters
            MobColorFilters.Add(new MobColorFilterItem { MobName = "T2_MOB_HIDE_SNAKE", ColorValue = 120000 });
            MobColorFilters.Add(new MobColorFilterItem { MobName = "T1_MOB_HIDE_SWAMP_FROG", ColorValue = 160000 });
        }

        public string[] GenerateLines()
        {
            var lines = new string[55];
            var ci = CultureInfo.InvariantCulture;

            // Line 1: Master toggle key
            lines[0] = MasterToggleKey > 0 ? MasterToggleKey.ToString() : "";
            // Line 2: Movement speed multiplier
            lines[1] = MovementSpeedMultiplier.ToString("0.##", ci);
            // Line 3: Movement speed toggle key
            lines[2] = MovementSpeedToggleKey > 0 ? MovementSpeedToggleKey.ToString() : "";
            // Line 4: Max zoom multiplier
            lines[3] = MaxZoomMultiplier.ToString("0.##", ci);
            // Line 5: Max zoom toggle key
            lines[4] = MaxZoomToggleKey > 0 ? MaxZoomToggleKey.ToString() : "";
            // Line 6: Rotate camera right key
            lines[5] = CameraRotateRightKey > 0 ? CameraRotateRightKey.ToString() : "";
            // Line 7: Rotate camera left key
            lines[6] = CameraRotateLeftKey > 0 ? CameraRotateLeftKey.ToString() : "";
            // Line 8: Camera rotate step horizontal
            lines[7] = CameraRotateStepHorizontal.ToString("0.##", ci);
            // Line 9: Rotate camera up key
            lines[8] = CameraRotateUpKey > 0 ? CameraRotateUpKey.ToString() : "";
            // Line 10: Rotate camera down key
            lines[9] = CameraRotateDownKey > 0 ? CameraRotateDownKey.ToString() : "";

            // Line 11: Camera rotate step vertical
            lines[10] = CameraRotateStepVertical.ToString("0.##", ci);
            // Line 12: Camera free-look hold key
            lines[11] = CameraFreeLookHoldKey > 0 ? CameraFreeLookHoldKey.ToString() : "";
            // Line 13: Invert Y-axis
            lines[12] = CameraInvertY ? "1" : "0";
            // Line 14: Invert X-axis
            lines[13] = CameraInvertX ? "1" : "0";
            // Line 15: Deprecated
            lines[14] = Line15Deprecated;
            // Line 16: Swing camera speed
            lines[15] = CameraSwingSpeed.ToString("0.##", ci);
            // Line 17: Lock X-axis
            lines[16] = CameraLockX ? "1" : "0";
            // Line 18: Lock Y-axis
            lines[17] = CameraLockY ? "1" : "0";
            // Line 19: Screen clamp boundary
            lines[18] = ClampEnabled ? $"{ClampMinX}|{ClampMaxX}|{ClampMinY}|{ClampMaxY}" : "";
            // Line 20: Driver name
            lines[19] = DriverName;

            // Line 21: Field of View
            lines[20] = FieldOfView;
            // Line 22: Waymark pause/resume key
            lines[21] = WaymarkPauseResumeKey > 0 ? WaymarkPauseResumeKey.ToString() : "";
            // Line 23: Waymark proximity tolerance
            lines[22] = WaymarkArrivalTolerance.ToString("0.##", ci);
            // Line 24: Waymark sleep jitter
            lines[23] = WaymarkSleepJitter.ToString("0.###", ci);
            // Line 25: Waymark timeout ms
            lines[24] = WaymarkTimeoutMs.ToString();
            // Line 26: Waymark cursor offset px
            lines[25] = WaymarkCursorOffsetPx.ToString();
            // Line 27: Record coordinates key
            lines[26] = RecordCoordinatesKey > 0 ? RecordCoordinatesKey.ToString() : "";
            // Line 28: Camera hacks master toggle key
            lines[27] = CameraHacksToggleKey > 0 ? CameraHacksToggleKey.ToString() : "";
            // Line 29: Macro file name
            lines[28] = MacroFileName;
            // Line 30: Injected keys allowed
            lines[29] = InjectedKeysAllowed ? "1" : "0";

            // Line 31: Mouse smoothing
            lines[30] = MouseSmoothing.ToString();
            // Line 32: Window center Y adjustment
            lines[31] = WindowCenterYAdjustment.ToString("0.##", ci);
            // Line 33: DPI/scaling multiplier
            lines[32] = GpcMultiplier.ToString("0.##", ci);
            // Line 34: Polling interval ms
            lines[33] = PollingIntervalMs.ToString();
            // Line 35: Memory read/write debug
            lines[34] = MemoryDebugEnabled ? "1" : "0";
            // Line 36: Toggle clamp key
            lines[35] = ClampToggleKey > 0 ? ClampToggleKey.ToString() : "";
            // Line 37: Player detection
            lines[36] = $"{DetectionMinPlayers}|{DetectionMaxDistance.ToString("0.##", ci)}|{DetectionTimerMs}|{DetectionSystemCommand}";
            // Line 38: Deprecated
            lines[37] = Line38Deprecated;
            // Line 39: Output new players to console
            lines[38] = OutputNewPlayersToConsole ? "1" : "0";
            // Line 40: Deprecated
            lines[39] = Line40Deprecated;

            // Line 41: Deprecated
            lines[40] = Line41Deprecated;
            // Line 42: Kernel input
            lines[41] = KernelInputSetting;
            // Line 43: Radar settings
            if (RadarEnabled)
            {
                string playerColorRadar = RadarPlayerColorOutside == RadarPlayerColorInside
                    ? RadarPlayerColorOutside.ToString()
                    : $"{RadarPlayerColorOutside},{RadarPlayerColorInside}";

                lines[42] = $"{RadarPosX}|{RadarPosY}|{RadarWidth}|{RadarHeight}|{RadarId}|{RadarTotalEntities}|{RadarFontSize}|{RadarBgColor}|{RadarMobColor}|{playerColorRadar}|{RadarGatheringColor}|{RadarCenterIndicator}|{(RadarShowMobs ? 1 : 0)}|{(RadarShowPlayers ? 1 : 0)}|{(RadarShowGathering ? 1 : 0)}|{(RadarTransparent ? 1 : 0)}|{(RadarAlwaysOnTop ? 1 : 0)}|{RadarDistanceMultiplier.ToString("0.##", ci)}|{RadarRefreshRate}";
            }
            else
            {
                lines[42] = "";
            }

            // Line 44: Console window settings
            if (!string.IsNullOrWhiteSpace(ConsoleCustomOverride))
            {
                lines[43] = ConsoleCustomOverride;
            }
            else if (ConsoleEnabled)
            {
                lines[43] = $"{ConsolePosX}|{ConsolePosY}|{ConsoleWidth}|{ConsoleHeight}|{ConsoleFontSize}|{ConsoleBgColor}|{ConsoleTextColor}|{(ConsoleTransparent ? 1 : 0)}|{(ConsoleAlwaysOnTop ? 1 : 0)}" +
                    (string.IsNullOrWhiteSpace(ConsoleFont) ? "" : $"|{ConsoleFont}");
            }
            else
            {
                lines[43] = "";
            }
            // Line 45: Toggle player detection key
            lines[44] = PlayerDetectionToggleKey > 0 ? PlayerDetectionToggleKey.ToString() : "";

            // Line 46: ESP Settings
            if (EspEnabled)
            {
                string playerColorEsp = EspPlayerColorOutside == EspPlayerColorInside
                    ? EspPlayerColorOutside.ToString()
                    : $"{EspPlayerColorOutside},{EspPlayerColorInside}";

                lines[45] = $"{EspTotalEntities}|{EspFontSize}|{EspMobColor}|{playerColorEsp}|{EspGatheringColor}|{(EspShowMobs ? 1 : 0)}|{(EspShowPlayers ? 1 : 0)}|{(EspShowGathering ? 1 : 0)}|{EspRefreshRate}" +
                    (string.IsNullOrWhiteSpace(EspFont) ? "" : $"|{EspFont}");
            }
            else
            {
                lines[45] = "";
            }

            // Line 47: Additional ESP & targeting
            lines[46] = $"{EspTargetingFov.ToString("0.##", ci)}|{EspCameraYaw.ToString("0.##", ci)}|{EspCameraPitch.ToString("0.###", ci)}";

            // Line 48: ESP & Radar scaling
            lines[47] = EspRadarScaling.ToString("0.##", ci);

            // Line 49: Gathering filters
            var gFilters = GatheringFilters
                .Select(f => f.ToConfigString())
                .Where(s => !string.IsNullOrWhiteSpace(s));
            lines[48] = string.Join("|", gFilters);

            // Line 50: Mounted players color
            lines[49] = $"{(MountedPlayerHighlightActive ? 1 : 0)}|{MountedPlayerColor}|{MountedPlayerDelayMs}";

            // Line 51: Mob name color filters
            var mFilters = MobColorFilters
                .Select(f => f.ToConfigString())
                .Where(s => !string.IsNullOrWhiteSpace(s));
            lines[50] = string.Join("|", mFilters);

            // Line 52: Player display text
            lines[51] = $"{PlayerEspText}|{PlayerRadarText}";

            // Line 53: Gathering display text
            lines[52] = $"{GatheringEspText}|{GatheringRadarText}";

            // Line 54: Mob display text
            lines[53] = $"{MobEspText}|{MobRadarText}";

            // Line 55: Minimum Mob HP threshold
            lines[54] = MobMinHpThreshold.ToString();

            return lines;
        }

        public void LoadFromLines(string[] rawLines)
        {
            if (rawLines == null || rawLines.Length == 0) return;

            var ci = CultureInfo.InvariantCulture;

            string GetLine(int lineIndex1Based)
            {
                int idx = lineIndex1Based - 1;
                return (idx >= 0 && idx < rawLines.Length) ? rawLines[idx].Trim() : string.Empty;
            }

            int ParseInt(string s, int defVal = 0) => int.TryParse(s, NumberStyles.Integer, ci, out int v) ? v : defVal;
            double ParseDouble(string s, double defVal = 0.0) => double.TryParse(s, NumberStyles.Float, ci, out double v) ? v : defVal;
            bool ParseBool(string s) => s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase);

            // Line 1 - 10
            MasterToggleKey = ParseInt(GetLine(1));
            MovementSpeedMultiplier = ParseDouble(GetLine(2), 1.05);
            MovementSpeedToggleKey = ParseInt(GetLine(3));
            MaxZoomMultiplier = ParseDouble(GetLine(4), 0.5);
            MaxZoomToggleKey = ParseInt(GetLine(5));
            CameraRotateRightKey = ParseInt(GetLine(6));
            CameraRotateLeftKey = ParseInt(GetLine(7));
            CameraRotateStepHorizontal = ParseDouble(GetLine(8), 5.0);
            CameraRotateUpKey = ParseInt(GetLine(9));
            CameraRotateDownKey = ParseInt(GetLine(10));

            // Line 11 - 20
            CameraRotateStepVertical = ParseDouble(GetLine(11), 5.0);
            CameraFreeLookHoldKey = ParseInt(GetLine(12));
            CameraInvertY = ParseBool(GetLine(13));
            CameraInvertX = ParseBool(GetLine(14));
            Line15Deprecated = GetLine(15);
            CameraSwingSpeed = ParseDouble(GetLine(16), 1.0);
            CameraLockX = ParseBool(GetLine(17));
            CameraLockY = ParseBool(GetLine(18));

            // Line 19: Screen clamp
            string clampLine = GetLine(19);
            if (!string.IsNullOrWhiteSpace(clampLine))
            {
                var clampParts = clampLine.Split('|');
                if (clampParts.Length >= 4)
                {
                    ClampMinX = ParseInt(clampParts[0], 100);
                    ClampMaxX = ParseInt(clampParts[1], 1820);
                    ClampMinY = ParseInt(clampParts[2], 100);
                    ClampMaxY = ParseInt(clampParts[3], 980);
                    ClampEnabled = true;
                }
            }
            else
            {
                ClampEnabled = false;
            }

            // Line 20: Driver
            DriverName = GetLine(20);

            // Line 21 - 30
            FieldOfView = string.IsNullOrWhiteSpace(GetLine(21)) ? "1" : GetLine(21);
            WaymarkPauseResumeKey = ParseInt(GetLine(22));
            WaymarkArrivalTolerance = ParseDouble(GetLine(23), 3.0);
            WaymarkSleepJitter = ParseDouble(GetLine(24), 0.05);
            WaymarkTimeoutMs = ParseInt(GetLine(25), 5000);
            WaymarkCursorOffsetPx = ParseInt(GetLine(26), 100);
            RecordCoordinatesKey = ParseInt(GetLine(27));
            CameraHacksToggleKey = ParseInt(GetLine(28));
            MacroFileName = GetLine(29);
            InjectedKeysAllowed = ParseBool(GetLine(30));

            // Line 31 - 40
            MouseSmoothing = ParseInt(GetLine(31), 200);
            WindowCenterYAdjustment = ParseDouble(GetLine(32), 0.0);
            GpcMultiplier = ParseDouble(GetLine(33), 1.0);
            PollingIntervalMs = ParseInt(GetLine(34), 1000);
            MemoryDebugEnabled = ParseBool(GetLine(35));
            ClampToggleKey = ParseInt(GetLine(36));

            // Line 37: Detection
            string detLine = GetLine(37);
            if (!string.IsNullOrWhiteSpace(detLine))
            {
                var detParts = detLine.Split(new[] { '|' }, 4);
                if (detParts.Length >= 1) DetectionMinPlayers = ParseInt(detParts[0], 1);
                if (detParts.Length >= 2) DetectionMaxDistance = ParseDouble(detParts[1], 0.0);
                if (detParts.Length >= 3) DetectionTimerMs = ParseInt(detParts[2], 2000);
                if (detParts.Length >= 4) DetectionSystemCommand = detParts[3];
            }

            Line38Deprecated = GetLine(38);
            OutputNewPlayersToConsole = ParseBool(GetLine(39));
            Line40Deprecated = GetLine(40);

            // Line 41 - 45
            Line41Deprecated = GetLine(41);
            KernelInputSetting = string.IsNullOrWhiteSpace(GetLine(42)) ? "1" : GetLine(42);

            // Line 43: Radar
            string radarLine = GetLine(43);
            if (!string.IsNullOrWhiteSpace(radarLine))
            {
                RadarEnabled = true;
                var rParts = radarLine.Split('|');
                if (rParts.Length >= 1) RadarPosX = ParseInt(rParts[0]);
                if (rParts.Length >= 2) RadarPosY = ParseInt(rParts[1]);
                if (rParts.Length >= 3) RadarWidth = ParseInt(rParts[2], 400);
                if (rParts.Length >= 4) RadarHeight = ParseInt(rParts[3], 400);
                if (rParts.Length >= 5) RadarId = ParseInt(rParts[4], 1);
                if (rParts.Length >= 6) RadarTotalEntities = ParseInt(rParts[5], 1000);
                if (rParts.Length >= 7) RadarFontSize = ParseInt(rParts[6], 14);
                if (rParts.Length >= 8) RadarBgColor = ParseInt(rParts[7], 3618615);
                if (rParts.Length >= 9) RadarMobColor = ParseInt(rParts[8], 255);
                if (rParts.Length >= 10)
                {
                    var pcParts = rParts[9].Split(',');
                    RadarPlayerColorOutside = ParseInt(pcParts[0], 120000);
                    RadarPlayerColorInside = pcParts.Length > 1 ? ParseInt(pcParts[1], RadarPlayerColorOutside) : RadarPlayerColorOutside;
                }
                if (rParts.Length >= 11) RadarGatheringColor = ParseInt(rParts[10], 16711680);
                if (rParts.Length >= 12) RadarCenterIndicator = rParts[11];
                if (rParts.Length >= 13) RadarShowMobs = ParseBool(rParts[12]);
                if (rParts.Length >= 14) RadarShowPlayers = ParseBool(rParts[13]);
                if (rParts.Length >= 15) RadarShowGathering = ParseBool(rParts[14]);
                if (rParts.Length >= 16) RadarTransparent = ParseBool(rParts[15]);
                if (rParts.Length >= 17) RadarAlwaysOnTop = ParseBool(rParts[16]);
                if (rParts.Length >= 18) RadarDistanceMultiplier = ParseDouble(rParts[17], 10.0);
                if (rParts.Length >= 19) RadarRefreshRate = ParseInt(rParts[18], 200);
            }
            else
            {
                RadarEnabled = false;
            }

            // Line 44: Console window settings
            string cLine = GetLine(44);
            if (!string.IsNullOrWhiteSpace(cLine))
            {
                ConsoleEnabled = true;
                var cParts = cLine.Split('|');
                if (cParts.Length >= 1) ConsolePosX = ParseInt(cParts[0], 0);
                if (cParts.Length >= 2) ConsolePosY = ParseInt(cParts[1], 0);
                if (cParts.Length >= 3) ConsoleWidth = ParseInt(cParts[2], 520);
                if (cParts.Length >= 4) ConsoleHeight = ParseInt(cParts[3], 380);
                if (cParts.Length >= 5) ConsoleFontSize = ParseInt(cParts[4], 13);
                if (cParts.Length >= 6) ConsoleBgColor = ParseInt(cParts[5], 3618615);
                if (cParts.Length >= 7) ConsoleTextColor = ParseInt(cParts[6], 16777215);
                if (cParts.Length >= 8) ConsoleTransparent = ParseBool(cParts[7]);
                if (cParts.Length >= 9) ConsoleAlwaysOnTop = ParseBool(cParts[8]);
                ConsoleFont = cParts.Length >= 10 ? cParts[9] : string.Empty;

                string reconstructed = $"{ConsolePosX}|{ConsolePosY}|{ConsoleWidth}|{ConsoleHeight}|{ConsoleFontSize}|{ConsoleBgColor}|{ConsoleTextColor}|{(ConsoleTransparent ? 1 : 0)}|{(ConsoleAlwaysOnTop ? 1 : 0)}" +
                    (string.IsNullOrWhiteSpace(ConsoleFont) ? "" : $"|{ConsoleFont}");

                if (!cLine.Equals(reconstructed, StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleCustomOverride = cLine;
                }
                else
                {
                    ConsoleCustomOverride = string.Empty;
                }
            }
            else
            {
                ConsoleEnabled = false;
                ConsoleCustomOverride = string.Empty;
            }
            PlayerDetectionToggleKey = ParseInt(GetLine(45));

            // Line 46: ESP
            string espLine = GetLine(46);
            if (!string.IsNullOrWhiteSpace(espLine))
            {
                EspEnabled = true;
                var eParts = espLine.Split('|');
                if (eParts.Length >= 1) EspTotalEntities = ParseInt(eParts[0], 200);
                if (eParts.Length >= 2) EspFontSize = ParseInt(eParts[1], 18);
                if (eParts.Length >= 3) EspMobColor = ParseInt(eParts[2], 255);
                if (eParts.Length >= 4)
                {
                    var pcParts = eParts[3].Split(',');
                    EspPlayerColorOutside = ParseInt(pcParts[0], 120000);
                    EspPlayerColorInside = pcParts.Length > 1 ? ParseInt(pcParts[1], EspPlayerColorOutside) : EspPlayerColorOutside;
                }
                if (eParts.Length >= 5) EspGatheringColor = ParseInt(eParts[4], 16711680);
                if (eParts.Length >= 6) EspShowMobs = ParseBool(eParts[5]);
                if (eParts.Length >= 7) EspShowPlayers = ParseBool(eParts[6]);
                if (eParts.Length >= 8) EspShowGathering = ParseBool(eParts[7]);
                if (eParts.Length >= 9) EspRefreshRate = ParseInt(eParts[8], 50);
                EspFont = eParts.Length >= 10 ? eParts[9] : string.Empty;
            }
            else
            {
                EspEnabled = false;
            }

            // Line 47: ESP Targeting
            string espTargLine = GetLine(47);
            if (!string.IsNullOrWhiteSpace(espTargLine))
            {
                var etParts = espTargLine.Split('|');
                if (etParts.Length >= 1) EspTargetingFov = ParseDouble(etParts[0], 46.5);
                if (etParts.Length >= 2) EspCameraYaw = ParseDouble(etParts[1], 7.0);
                if (etParts.Length >= 3) EspCameraPitch = ParseDouble(etParts[2], 0.02);
            }

            // Line 48: Scaling
            EspRadarScaling = ParseDouble(GetLine(48), 1.0);

            // Line 49: Gathering Filters
            string gFilterLine = GetLine(49);
            GatheringFilters.Clear();
            if (!string.IsNullOrWhiteSpace(gFilterLine))
            {
                foreach (var seg in gFilterLine.Split('|'))
                {
                    var item = GatheringFilterItem.FromConfigString(seg);
                    if (item != null) GatheringFilters.Add(item);
                }
            }

            // Line 50: Mounted Players
            string mountedLine = GetLine(50);
            if (!string.IsNullOrWhiteSpace(mountedLine))
            {
                var mParts = mountedLine.Split('|');
                if (mParts.Length >= 1) MountedPlayerHighlightActive = ParseBool(mParts[0]);
                if (mParts.Length >= 2) MountedPlayerColor = ParseInt(mParts[1], 60000);
                if (mParts.Length >= 3) MountedPlayerDelayMs = ParseInt(mParts[2], 5000);
            }

            // Line 51: Mob Filters
            string mobFilterLine = GetLine(51);
            MobColorFilters.Clear();
            if (!string.IsNullOrWhiteSpace(mobFilterLine))
            {
                foreach (var seg in mobFilterLine.Split('|'))
                {
                    var item = MobColorFilterItem.FromConfigString(seg);
                    if (item != null) MobColorFilters.Add(item);
                }
            }

            // Line 52: Player display text
            string pText = GetLine(52);
            if (!string.IsNullOrWhiteSpace(pText))
            {
                var pParts = pText.Split(new[] { '|' }, 2);
                PlayerEspText = pParts[0];
                if (pParts.Length > 1) PlayerRadarText = pParts[1];
            }

            // Line 53: Gathering display text
            string gText = GetLine(53);
            if (!string.IsNullOrWhiteSpace(gText))
            {
                var gParts = gText.Split(new[] { '|' }, 2);
                GatheringEspText = gParts[0];
                if (gParts.Length > 1) GatheringRadarText = gParts[1];
            }

            // Line 54: Mob display text
            string mText = GetLine(54);
            if (!string.IsNullOrWhiteSpace(mText))
            {
                var mParts = mText.Split(new[] { '|' }, 2);
                MobEspText = mParts[0];
                if (mParts.Length > 1) MobRadarText = mParts[1];
            }

            // Line 55: Mob min HP
            MobMinHpThreshold = ParseInt(GetLine(55), 500);

            // Notify all
            OnPropertyChanged(string.Empty);
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
