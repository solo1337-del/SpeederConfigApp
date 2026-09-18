using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpeederConfigApp.Models
{
    /// <summary>
    /// Represents a dynamic variable in the [variables] section of a waymark file.
    /// </summary>
    public class WaymarkVariable : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _value = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public WaymarkVariable()
        {
        }

        public WaymarkVariable(string name, string value)
        {
            _name = name ?? string.Empty;
            _value = value ?? string.Empty;
        }

        public string Name
        {
            get => _name;
            set
            {
                var val = value ?? string.Empty;
                if (_name != val)
                {
                    _name = val;
                    OnPropertyChanged();
                }
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                var val = value ?? string.Empty;
                if (_value != val)
                {
                    _value = val;
                    OnPropertyChanged();
                }
            }
        }

        public WaymarkVariable Clone() => new WaymarkVariable(Name, Value);

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Configuration for Speeder's unstick recovery mechanics in the [unstick] section.
    /// </summary>
    public class UnstickConfig : INotifyPropertyChanged
    {
        private bool _enabled = true;
        private double _distance = 5.0;
        private string _keys = "27";
        private int _timer = 5000;
        private int _timer2 = 2000;
        private int _giveUp = 5;
        private string _script = "probablydead.ini";

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Whether the [unstick] section is output. Default true if any value set.
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Distance threshold to test if character moved (default e.g. 5.0).
        /// </summary>
        public double Distance
        {
            get => _distance;
            set
            {
                if (Math.Abs(_distance - value) > 0.0001)
                {
                    _distance = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Key sequence executed when stuck (e.g. "27" for ESC, or other keys).
        /// </summary>
        public string Keys
        {
            get => _keys;
            set
            {
                var val = value ?? string.Empty;
                if (_keys != val)
                {
                    _keys = val;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Milliseconds before assuming stuck (default 5000).
        /// </summary>
        public int Timer
        {
            get => _timer;
            set
            {
                if (_timer != value)
                {
                    _timer = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Milliseconds before activating unstick keys (default 2000; 0 = immediate).
        /// </summary>
        public int Timer2
        {
            get => _timer2;
            set
            {
                if (_timer2 != value)
                {
                    _timer2 = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Max attempts before giving up (default 5).
        /// </summary>
        public int GiveUp
        {
            get => _giveUp;
            set
            {
                if (_giveUp != value)
                {
                    _giveUp = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Script / INI to load after giving up (e.g. "probablydead.ini").
        /// </summary>
        public string Script
        {
            get => _script;
            set
            {
                var val = value ?? string.Empty;
                if (_script != val)
                {
                    _script = val;
                    OnPropertyChanged();
                }
            }
        }

        public UnstickConfig Clone()
        {
            return new UnstickConfig
            {
                Enabled = this.Enabled,
                Distance = this.Distance,
                Keys = this.Keys,
                Timer = this.Timer,
                Timer2 = this.Timer2,
                GiveUp = this.GiveUp,
                Script = this.Script
            };
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Represents a single waypoint entry in a Speeder route ([0], [1], [2], ...).
    /// </summary>
    public class WaymarkPoint : INotifyPropertyChanged
    {
        private int _index;
        private double _x;
        private double _y;
        private double _z;
        private int _waitTime = 30;
        private string _keys = string.Empty;
        private string _script = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Sequential index starting at 0 ([0], [1], [2], ...).
        /// </summary>
        public int Index
        {
            get => _index;
            set
            {
                if (_index != value)
                {
                    _index = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// World coordinate X.
        /// </summary>
        public double X
        {
            get => _x;
            set
            {
                if (Math.Abs(_x - value) > 0.00001)
                {
                    _x = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsStopMovement));
                    OnPropertyChanged(nameof(IsMovementPrevented));
                }
            }
        }

        /// <summary>
        /// Height coordinate Y (ignorable in Albion, default 0 or parsed value).
        /// </summary>
        public double Y
        {
            get => _y;
            set
            {
                if (Math.Abs(_y - value) > 0.00001)
                {
                    _y = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// World coordinate Z (North/South).
        /// </summary>
        public double Z
        {
            get => _z;
            set
            {
                if (Math.Abs(_z - value) > 0.00001)
                {
                    _z = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Milliseconds to wait (default 30).
        /// </summary>
        public int WaitTime
        {
            get => _waitTime;
            set
            {
                if (_waitTime != value)
                {
                    _waitTime = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsSeamlessMovement));
                }
            }
        }

        /// <summary>
        /// Computed helper property: returns Math.Abs(X - 0.1) &lt; 0.001. When set to true, sets X = 0.1.
        /// </summary>
        public bool IsStopMovement
        {
            get => Math.Abs(_x - 0.1) < 0.001;
            set
            {
                if (value)
                {
                    X = 0.1;
                }
                else if (IsStopMovement)
                {
                    X = 0.0;
                }
            }
        }

        /// <summary>
        /// Alias for IsStopMovement.
        /// </summary>
        public bool IsMovementPrevented => IsStopMovement;

        /// <summary>
        /// Computed helper property: returns WaitTime == 1. When set to true, sets WaitTime = 1 (instructs Speeder not to release movement keys).
        /// </summary>
        public bool IsSeamlessMovement
        {
            get => _waitTime == 1;
            set
            {
                if (value)
                {
                    WaitTime = 1;
                }
                else if (_waitTime == 1)
                {
                    WaitTime = 30;
                }
            }
        }

        /// <summary>
        /// Commands/keys separated by |. Default string.Empty.
        /// </summary>
        public string Keys
        {
            get => _keys;
            set
            {
                var val = value ?? string.Empty;
                if (_keys != val)
                {
                    _keys = val;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Target script or chaining instruction (e.g. nextinifile.ini|5 or executable). Default string.Empty.
        /// </summary>
        public string Script
        {
            get => _script;
            set
            {
                var val = value ?? string.Empty;
                if (_script != val)
                {
                    _script = val;
                    OnPropertyChanged();
                }
            }
        }

        public WaymarkPoint Clone()
        {
            return new WaymarkPoint
            {
                Index = this.Index,
                X = this.X,
                Y = this.Y,
                Z = this.Z,
                WaitTime = this.WaitTime,
                Keys = this.Keys,
                Script = this.Script
            };
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Model representing a complete Speeder waymark route file with variables, unstick failsafe, and waypoints.
    /// </summary>
    public class WaymarkFileModel : INotifyPropertyChanged
    {
        private string _fileName = "Recorded Waymarks.ini";
        private string _filePath = string.Empty;
        private UnstickConfig _unstick = new UnstickConfig();

        public event PropertyChangedEventHandler? PropertyChanged;

        public WaymarkFileModel()
        {
            Variables.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Variables));
            };
            Points.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Points));
            };
        }

        public ObservableCollection<WaymarkVariable> Variables { get; } = new ObservableCollection<WaymarkVariable>();

        public UnstickConfig Unstick
        {
            get => _unstick;
            set
            {
                if (_unstick != value)
                {
                    _unstick = value ?? new UnstickConfig();
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<WaymarkPoint> Points { get; } = new ObservableCollection<WaymarkPoint>();

        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Generates a valid Speeder waymark INI string.
        /// </summary>
        public string GenerateIni()
        {
            var sb = new StringBuilder();
            bool hasPreviousSection = false;

            // 1. [variables] section
            if (Variables.Count > 0)
            {
                sb.AppendLine("[variables]");
                foreach (var variable in Variables)
                {
                    sb.AppendLine($"{variable.Name}={variable.Value}");
                }
                hasPreviousSection = true;
            }

            // 2. [unstick] section
            if (Unstick != null && Unstick.Enabled)
            {
                if (hasPreviousSection)
                {
                    sb.AppendLine();
                }

                sb.AppendLine("[unstick]");
                sb.AppendLine($"distance={Unstick.Distance.ToString("0.##", CultureInfo.InvariantCulture)}");
                sb.AppendLine($"keys={Unstick.Keys}");
                sb.AppendLine($"timer={Unstick.Timer}");
                sb.AppendLine($"timer2={Unstick.Timer2}");
                sb.AppendLine($"giveup={Unstick.GiveUp}");
                if (!string.IsNullOrWhiteSpace(Unstick.Script))
                {
                    sb.AppendLine($"script={Unstick.Script}");
                }
                hasPreviousSection = true;
            }

            // 3. Points: [0], [1], [2], ...
            for (int i = 0; i < Points.Count; i++)
            {
                if (hasPreviousSection || i > 0)
                {
                    sb.AppendLine();
                }

                var point = Points[i];
                int index = point.Index >= 0 ? point.Index : i;
                sb.AppendLine($"[{index}]");
                sb.AppendLine($"x={point.X.ToString("0.##", CultureInfo.InvariantCulture)}");
                sb.AppendLine($"y={point.Y.ToString("0.##", CultureInfo.InvariantCulture)}");
                sb.AppendLine($"z={point.Z.ToString("0.##", CultureInfo.InvariantCulture)}");
                sb.AppendLine($"wait time={point.WaitTime}");

                if (!string.IsNullOrWhiteSpace(point.Keys))
                {
                    sb.AppendLine($"keys={point.Keys}");
                }

                if (!string.IsNullOrWhiteSpace(point.Script))
                {
                    sb.AppendLine($"script={point.Script}");
                }

                hasPreviousSection = true;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Parses an INI string into a WaymarkFileModel instance.
        /// </summary>
        public static WaymarkFileModel ParseIni(string iniText)
        {
            var model = new WaymarkFileModel();
            model.Unstick.Enabled = false; // default to disabled unless [unstick] is explicitly present

            if (string.IsNullOrWhiteSpace(iniText))
                return model;

            using var reader = new StringReader(iniText);
            string? line;
            string currentSection = string.Empty;
            WaymarkPoint? currentPoint = null;
            var parsedPoints = new List<WaymarkPoint>();

            void FinalizeCurrentPoint()
            {
                if (currentPoint != null)
                {
                    parsedPoints.Add(currentPoint);
                    currentPoint = null;
                }
            }

            while ((line = reader.ReadLine()) != null)
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed))
                    continue;

                // Section header
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    FinalizeCurrentPoint();
                    currentSection = trimmed.Substring(1, trimmed.Length - 2).Trim();

                    if (int.TryParse(currentSection, out int pIdx))
                    {
                        currentPoint = new WaymarkPoint { Index = pIdx };
                    }
                    else
                    {
                        if (currentSection.Equals("unstick", StringComparison.OrdinalIgnoreCase))
                        {
                            model.Unstick.Enabled = true;
                        }
                    }
                    continue;
                }

                // Comment line: line without '=' or starting with ';' or '#'
                if (trimmed.StartsWith(";") || trimmed.StartsWith("#") || !trimmed.Contains("="))
                {
                    continue;
                }

                int eqIndex = trimmed.IndexOf('=');
                if (eqIndex <= 0)
                    continue;

                string key = trimmed.Substring(0, eqIndex).Trim();
                string val = trimmed.Substring(eqIndex + 1).Trim();

                // 1. [variables]
                if (currentSection.Equals("variables", StringComparison.OrdinalIgnoreCase))
                {
                    model.Variables.Add(new WaymarkVariable(key, val));
                }
                // 2. [unstick]
                else if (currentSection.Equals("unstick", StringComparison.OrdinalIgnoreCase))
                {
                    model.Unstick.Enabled = true;
                    if (key.Equals("distance", StringComparison.OrdinalIgnoreCase))
                    {
                        if (double.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out double d))
                            model.Unstick.Distance = d;
                    }
                    else if (key.Equals("keys", StringComparison.OrdinalIgnoreCase))
                    {
                        model.Unstick.Keys = val;
                    }
                    else if (key.Equals("timer", StringComparison.OrdinalIgnoreCase))
                    {
                        if (int.TryParse(val, out int t))
                            model.Unstick.Timer = t;
                    }
                    else if (key.Equals("timer2", StringComparison.OrdinalIgnoreCase))
                    {
                        if (int.TryParse(val, out int t2))
                            model.Unstick.Timer2 = t2;
                    }
                    else if (key.Equals("giveup", StringComparison.OrdinalIgnoreCase))
                    {
                        if (int.TryParse(val, out int g))
                            model.Unstick.GiveUp = g;
                    }
                    else if (key.Equals("script", StringComparison.OrdinalIgnoreCase))
                    {
                        model.Unstick.Script = val;
                    }
                    else if (key.Equals("enabled", StringComparison.OrdinalIgnoreCase))
                    {
                        if (bool.TryParse(val, out bool en))
                            model.Unstick.Enabled = en;
                    }
                }
                // 3. Waypoints [0], [1], ...
                else if (currentPoint != null)
                {
                    if (key.Equals("x", StringComparison.OrdinalIgnoreCase))
                    {
                        if (double.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out double px))
                            currentPoint.X = px;
                    }
                    else if (key.Equals("y", StringComparison.OrdinalIgnoreCase))
                    {
                        if (double.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out double py))
                            currentPoint.Y = py;
                    }
                    else if (key.Equals("z", StringComparison.OrdinalIgnoreCase))
                    {
                        if (double.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out double pz))
                            currentPoint.Z = pz;
                    }
                    else if (key.Equals("wait time", StringComparison.OrdinalIgnoreCase) ||
                             key.Equals("waittime", StringComparison.OrdinalIgnoreCase) ||
                             key.Equals("wait", StringComparison.OrdinalIgnoreCase))
                    {
                        if (int.TryParse(val, out int wt))
                            currentPoint.WaitTime = wt;
                    }
                    else if (key.Equals("keys", StringComparison.OrdinalIgnoreCase))
                    {
                        currentPoint.Keys = val;
                    }
                    else if (key.Equals("script", StringComparison.OrdinalIgnoreCase))
                    {
                        currentPoint.Script = val;
                    }
                }
            }

            FinalizeCurrentPoint();

            foreach (var pt in parsedPoints.OrderBy(p => p.Index))
            {
                model.Points.Add(pt);
            }

            return model;
        }

        /// <summary>
        /// Creates a sample route as defined in the reference documentation.
        /// </summary>
        public static WaymarkFileModel CreateSampleRoute()
        {
            var model = new WaymarkFileModel
            {
                FileName = "Recorded Waymarks.ini"
            };

            // Variable variablename=true
            model.Variables.Add(new WaymarkVariable("variablename", "true"));

            // Unstick: distance=5, keys=27, timer=5000, timer2=2000, giveup=5, script=probablydead.ini
            model.Unstick.Enabled = true;
            model.Unstick.Distance = 5;
            model.Unstick.Keys = "27";
            model.Unstick.Timer = 5000;
            model.Unstick.Timer2 = 2000;
            model.Unstick.GiveUp = 5;
            model.Unstick.Script = "probablydead.ini";

            // Point [0]: x=84.66, y=-78, z=832, wait time=30, keys=2d|s3000|2u|eq % variablename,true|dbg % starting waymarks!|store % variablename,false
            model.AddPoint(
                x: 84.66,
                y: -78,
                z: 832,
                waitTime: 30,
                keys: "2d|s3000|2u|eq % variablename,true|dbg % starting waymarks!|store % variablename,false"
            );

            // Point [1]: x=90.34, y=-60, z=783.21, wait time=30, script=nextinifile.ini|5
            model.AddPoint(
                x: 90.34,
                y: -60,
                z: 783.21,
                waitTime: 30,
                script: "nextinifile.ini|5"
            );

            // Plus 2 additional sample points to show a full loop.
            model.AddPoint(
                x: 95.12,
                y: -65,
                z: 795.50,
                waitTime: 30
            );

            model.AddPoint(
                x: 88.00,
                y: -72,
                z: 820.10,
                waitTime: 1 // Seamless movement demonstration
            );

            return model;
        }

        /// <summary>
        /// Ensures points have sequential index 0, 1, 2, ...
        /// </summary>
        public void ReindexPoints()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i].Index = i;
            }
        }

        /// <summary>
        /// Appends a new point with correct index and returns it.
        /// </summary>
        public WaymarkPoint AddPoint(double x, double y, double z, int waitTime = 30, string keys = "", string script = "")
        {
            var point = new WaymarkPoint
            {
                Index = Points.Count,
                X = x,
                Y = y,
                Z = z,
                WaitTime = waitTime,
                Keys = keys ?? string.Empty,
                Script = script ?? string.Empty
            };
            Points.Add(point);
            return point;
        }

        public WaymarkFileModel Clone()
        {
            var clone = new WaymarkFileModel
            {
                FileName = this.FileName,
                FilePath = this.FilePath,
                Unstick = this.Unstick.Clone()
            };
            foreach (var v in this.Variables)
            {
                clone.Variables.Add(v.Clone());
            }
            foreach (var p in this.Points)
            {
                clone.Points.Add(p.Clone());
            }
            return clone;
        }

        public void Save(string? path = null)
        {
            var targetPath = path ?? FilePath;
            if (string.IsNullOrWhiteSpace(targetPath))
                throw new InvalidOperationException("FilePath must be specified to save.");
            File.WriteAllText(targetPath, GenerateIni());
            FilePath = targetPath;
            FileName = Path.GetFileName(targetPath);
        }

        public static WaymarkFileModel Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Waymark file not found: {filePath}", filePath);
            var content = File.ReadAllText(filePath);
            var model = ParseIni(content);
            model.FilePath = filePath;
            model.FileName = Path.GetFileName(filePath);
            return model;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
