using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpeederConfigApp.Models
{
    public class MacroItem : INotifyPropertyChanged
    {
        private int _triggerKey;
        private string _functionName = string.Empty;
        private bool _isFunction;
        private string _endKeys = string.Empty;
        private int _repeat;
        private int? _interrupt;
        private string _description = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MacroItem()
        {
            KeysLines.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(KeysLines));
            };
        }

        public int TriggerKey
        {
            get => _triggerKey;
            set
            {
                if (_triggerKey != value)
                {
                    _triggerKey = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SectionHeader));
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public string FunctionName
        {
            get => _functionName;
            set
            {
                var val = value ?? string.Empty;
                if (_functionName != val)
                {
                    _functionName = val;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SectionHeader));
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public bool IsFunction
        {
            get => _isFunction;
            set
            {
                if (_isFunction != value)
                {
                    _isFunction = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SectionHeader));
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public string SectionHeader
        {
            get
            {
                if (IsFunction)
                {
                    if (string.IsNullOrWhiteSpace(FunctionName)) return "f";
                    return FunctionName.StartsWith("f", StringComparison.OrdinalIgnoreCase)
                        ? FunctionName
                        : "f" + FunctionName;
                }
                return TriggerKey.ToString();
            }
        }

        public string DisplayName
        {
            get
            {
                if (IsFunction)
                {
                    var func = SectionHeader;
                    var desc = GetCleanDescriptionSummary(Description);
                    return string.IsNullOrWhiteSpace(desc) ? $"[{func}] Function" : $"[{func}] {desc}";
                }
                else
                {
                    var key = VirtualKeyHelper.GetKeyByCode(TriggerKey);
                    var keyLabel = key != null ? $"{key.Name} ({TriggerKey})" : TriggerKey.ToString();
                    var desc = GetCleanDescriptionSummary(Description);
                    return string.IsNullOrWhiteSpace(desc) ? $"[{keyLabel}] Macro" : $"[{keyLabel}] {desc}";
                }
            }
        }

        private static string? GetCleanDescriptionSummary(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            var first = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .FirstOrDefault()?.Trim().TrimStart(';', '#', '%').Trim();
            return string.IsNullOrWhiteSpace(first) ? null : first;
        }

        public ObservableCollection<string> KeysLines { get; } = new ObservableCollection<string>();

        public string EndKeys
        {
            get => _endKeys;
            set
            {
                var val = value ?? string.Empty;
                if (_endKeys != val)
                {
                    _endKeys = val;
                    OnPropertyChanged();
                }
            }
        }

        public int Repeat
        {
            get => _repeat;
            set
            {
                if (_repeat != value)
                {
                    _repeat = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? Interrupt
        {
            get => _interrupt;
            set
            {
                if (_interrupt != value)
                {
                    _interrupt = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                var val = value ?? string.Empty;
                if (_description != val)
                {
                    _description = val;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public MacroItem Clone()
        {
            var clone = new MacroItem
            {
                TriggerKey = this.TriggerKey,
                FunctionName = this.FunctionName,
                IsFunction = this.IsFunction,
                EndKeys = this.EndKeys,
                Repeat = this.Repeat,
                Interrupt = this.Interrupt,
                Description = this.Description
            };
            foreach (var line in this.KeysLines)
            {
                clone.KeysLines.Add(line);
            }
            return clone;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MacroFileModel : INotifyPropertyChanged
    {
        private string _fileName = "macros.ini";
        private string _filePath = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<MacroItem> Macros { get; } = new ObservableCollection<MacroItem>();

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

        public string GenerateIni()
        {
            var sb = new StringBuilder();
            for (int m = 0; m < Macros.Count; m++)
            {
                var macro = Macros[m];

                // Description / comments above the section
                if (!string.IsNullOrWhiteSpace(macro.Description))
                {
                    using var reader = new StringReader(macro.Description);
                    string? dLine;
                    while ((dLine = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dLine))
                        {
                            sb.AppendLine(dLine);
                        }
                    }
                }

                // Section header
                sb.AppendLine($"[{macro.SectionHeader}]");

                // Keys lines
                for (int i = 0; i < macro.KeysLines.Count; i++)
                {
                    var keyName = i == 0 ? "keys" : $"keys{i + 1}";
                    sb.AppendLine($"{keyName}={macro.KeysLines[i]}");
                }

                // EndKeys
                if (!string.IsNullOrWhiteSpace(macro.EndKeys))
                {
                    sb.AppendLine($"endkeys={macro.EndKeys}");
                }

                // Repeat
                sb.AppendLine($"repeat={macro.Repeat}");

                // Interrupt
                if (macro.Interrupt.HasValue)
                {
                    sb.AppendLine($"interrupt={macro.Interrupt.Value}");
                }

                // Blank line between sections
                if (m < Macros.Count - 1)
                {
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        public static MacroFileModel ParseIni(string iniText)
        {
            var model = new MacroFileModel();
            if (string.IsNullOrWhiteSpace(iniText))
                return model;

            using var reader = new StringReader(iniText);
            string? line;
            MacroItem? currentMacro = null;
            var currentKeys = new List<(int Order, string Line)>();
            var pendingComments = new List<string>();
            bool hasPropertiesStarted = false;

            void FinalizeCurrentMacro()
            {
                if (currentMacro != null)
                {
                    foreach (var k in currentKeys.OrderBy(x => x.Order))
                    {
                        currentMacro.KeysLines.Add(k.Line);
                    }
                    model.Macros.Add(currentMacro);
                    currentMacro = null;
                    currentKeys.Clear();
                    hasPropertiesStarted = false;
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
                    FinalizeCurrentMacro();

                    var header = trimmed.Substring(1, trimmed.Length - 2).Trim();
                    currentMacro = new MacroItem();

                    if (header.StartsWith("f", StringComparison.OrdinalIgnoreCase))
                    {
                        currentMacro.IsFunction = true;
                        currentMacro.FunctionName = header;
                    }
                    else if (int.TryParse(header, out int vk))
                    {
                        currentMacro.IsFunction = false;
                        currentMacro.TriggerKey = vk;
                    }
                    else
                    {
                        currentMacro.IsFunction = true;
                        currentMacro.FunctionName = header;
                    }

                    if (pendingComments.Count > 0)
                    {
                        currentMacro.Description = string.Join(Environment.NewLine, pendingComments);
                        pendingComments.Clear();
                    }

                    continue;
                }

                // Comment lines (natural comments without '=' and '[', or prefixed with ';' or '#')
                bool isComment = trimmed.StartsWith(";") || trimmed.StartsWith("#") || (!trimmed.Contains("[") && !trimmed.Contains("="));
                if (isComment)
                {
                    if (currentMacro == null || hasPropertiesStarted)
                    {
                        pendingComments.Add(trimmed);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(currentMacro.Description))
                            currentMacro.Description = trimmed;
                        else
                            currentMacro.Description += Environment.NewLine + trimmed;
                    }
                    continue;
                }

                // Key-Value pair
                int eqIndex = trimmed.IndexOf('=');
                if (eqIndex <= 0)
                    continue;

                string key = trimmed.Substring(0, eqIndex).Trim().ToLowerInvariant();
                string val = trimmed.Substring(eqIndex + 1).Trim();

                if (currentMacro == null)
                    continue;

                hasPropertiesStarted = true;

                if (key.Equals("keys", StringComparison.OrdinalIgnoreCase))
                {
                    int order = 1;
                    if (currentKeys.Any(k => k.Order == 1))
                    {
                        order = currentKeys.Max(k => k.Order) + 1;
                    }
                    currentKeys.Add((order, val));
                }
                else if (key.StartsWith("keys", StringComparison.OrdinalIgnoreCase) && int.TryParse(key.Substring(4), out int kOrder))
                {
                    currentKeys.Add((kOrder, val));
                }
                else if (key.Equals("endkeys", StringComparison.OrdinalIgnoreCase))
                {
                    currentMacro.EndKeys = val;
                }
                else if (key.Equals("repeat", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(val, out int rep))
                    {
                        currentMacro.Repeat = rep;
                    }
                }
                else if (key.Equals("interrupt", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(val, out int intr))
                    {
                        currentMacro.Interrupt = intr;
                    }
                }
            }

            FinalizeCurrentMacro();
            return model;
        }

        public static MacroFileModel CreateDefaultTemplates()
        {
            var model = new MacroFileModel { FileName = "macros.ini" };

            // 1. F2 - Combat Cooldown Rotation
            var f2 = new MacroItem
            {
                TriggerKey = 113,
                Repeat = 2,
                Description = "Combat Cooldown Rotation"
            };
            f2.KeysLines.Add("!cm|gt8");
            f2.KeysLines.Add("cc1|81|dbg % Q key|s300");
            f2.KeysLines.Add("cc2|87|dbg % W key|s300");
            f2.KeysLines.Add("cc3|69|dbg % E key|s300");
            f2.KeysLines.Add("cc4|82|dbg % R key|s300");
            f2.KeysLines.Add("cc5|70|dbg % F key|s300");
            f2.KeysLines.Add("cc6|71|dbg % G key|s300");
            f2.KeysLines.Add("s10");
            model.Macros.Add(f2);

            // 2. F3 - Player Target Lock
            var f3 = new MacroItem
            {
                TriggerKey = 114,
                Repeat = 0,
                EndKeys = "lt-",
                Description = "Player Target Lock"
            };
            f3.KeysLines.Add("tcp*50,0,1|dbg % locked target on closest player within 50 distance");
            model.Macros.Add(f3);

            // 3. F4 - Player Target Lock & Combat Rotation
            var f4 = new MacroItem
            {
                TriggerKey = 115,
                Repeat = 2,
                Description = "Player Target Lock & Combat Rotation"
            };
            f4.KeysLines.Add("tcp*50,0,1|gt3");
            f4.KeysLines.Add("gt9");
            f4.KeysLines.Add("cc1|81|dbg % Q key|s300");
            f4.KeysLines.Add("cc2|87|dbg % W key|s300");
            f4.KeysLines.Add("cc3|69|dbg % E key|s300");
            f4.KeysLines.Add("cc4|82|dbg % R key|s300");
            f4.KeysLines.Add("cc5|70|dbg % F key|s300");
            f4.KeysLines.Add("cc6|71|dbg % G key|s300");
            f4.KeysLines.Add("s10");
            model.Macros.Add(f4);

            // 4. F5 - Quick Target & Return Cursor
            var f5 = new MacroItem
            {
                TriggerKey = 116,
                Repeat = 0,
                EndKeys = "m(VAR % CX),(VAR % CY)",
                Description = "Quick Target & Return Cursor"
            };
            f5.KeysLines.Add("get % cursor");
            f5.KeysLines.Add("tcp*50");
            model.Macros.Add(f5);

            // 5. F6 - Continuous Rock Gatherer
            var f6 = new MacroItem
            {
                TriggerKey = 117,
                Repeat = 2,
                Description = "Continuous Rock Gatherer"
            };
            f6.KeysLines.Add("cng % ROCK,1,20|tcg % ROCK,20|dbg % targeting ROCK|1d|rs150,200|1u");
            f6.KeysLines.Add("s10");
            model.Macros.Add(f6);

            // 6. NUMPAD6 - Increase Movement Speed (+1%)
            var num6 = new MacroItem
            {
                TriggerKey = 102,
                Repeat = 0,
                Description = "Increase Movement Speed (+1%)"
            };
            num6.KeysLines.Add("cmp(VAR % movementSpeed),1.0|store % movementSpeed,1.01|gt3");
            num6.KeysLines.Add("add % movementSpeed,0.01");
            num6.KeysLines.Add("ms(VAR % movementSpeed)|dbg % movement speed: (VAR % movementSpeed)");
            model.Macros.Add(num6);

            // 7. NUMPAD4 - Decrease Movement Speed (-1% / Disable)
            var num4 = new MacroItem
            {
                TriggerKey = 100,
                Repeat = 0,
                Description = "Decrease Movement Speed (-1% / Disable)"
            };
            num4.KeysLines.Add("!cmp(VAR % movementSpeed),1.005|sub % movementSpeed,0.01|gt3");
            num4.KeysLines.Add("store % movementSpeed,0");
            num4.KeysLines.Add("ms(VAR % movementSpeed)|dbg % movement speed: (VAR % movementSpeed)");
            model.Macros.Add(num4);

            // 8. fTest - Reusable Helper Function
            var fTest = new MacroItem
            {
                IsFunction = true,
                FunctionName = "fTest",
                Repeat = 0,
                Description = "Reusable Helper Function"
            };
            fTest.KeysLines.Add("dbg % test line 1");
            fTest.KeysLines.Add("dbg % test line 2");
            fTest.KeysLines.Add("dbg % test line 3");
            model.Macros.Add(fTest);

            return model;
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

        public static MacroFileModel Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Macro file not found: {filePath}", filePath);
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
