using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SpeederConfigApp.Models
{
    public class VirtualKeyInfo
    {
        public int Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public string Notes { get; set; } = string.Empty;
        public string DisplayText => Code == 0 ? "None (Disabled)" : $"{Name} ({Code})";

        public override string ToString() => DisplayText;
    }

    public static class VirtualKeyHelper
    {
        private static readonly List<VirtualKeyInfo> _allKeys;

        static VirtualKeyHelper()
        {
            _allKeys = new List<VirtualKeyInfo>
            {
                new() { Code = 0, Name = "None", Category = "General", Notes = "Disabled / unassigned" },
                new() { Code = 1, Name = "Left Mouse Button", Category = "Mouse", Notes = "Standard primary click" },
                new() { Code = 2, Name = "Right Mouse Button", Category = "Mouse", Notes = "Movement and interaction click" },
                new() { Code = 4, Name = "Middle Mouse Button", Category = "Mouse", Notes = "Wheel button" },
                new() { Code = 5, Name = "Mouse XButton 1", Category = "Mouse", Notes = "Side button 1" },
                new() { Code = 6, Name = "Mouse XButton 2", Category = "Mouse", Notes = "Side button 2" },
                new() { Code = 8, Name = "Backspace", Category = "Navigation", Notes = "Delete previous character" },
                new() { Code = 9, Name = "Tab", Category = "Navigation", Notes = "Focus navigation" },
                new() { Code = 12, Name = "Clear", Category = "Navigation", Notes = "Clear key" },
                new() { Code = 13, Name = "Enter", Category = "Navigation", Notes = "Return / Confirm" },
                new() { Code = 16, Name = "Shift", Category = "Modifier", Notes = "Shift key (generic)" },
                new() { Code = 17, Name = "Ctrl", Category = "Modifier", Notes = "Control key (generic)" },
                new() { Code = 18, Name = "Alt", Category = "Modifier", Notes = "Alt key (generic)" },
                new() { Code = 19, Name = "Pause", Category = "Navigation", Notes = "Pause / Break" },
                new() { Code = 20, Name = "Caps Lock", Category = "Modifier", Notes = "Capital lock toggle" },
                new() { Code = 27, Name = "Escape", Category = "Navigation", Notes = "Cancel / Menu / Unstick default" },
                new() { Code = 32, Name = "Space", Category = "Navigation", Notes = "Space bar" },
                new() { Code = 33, Name = "Page Up", Category = "Navigation", Notes = "Scroll page up" },
                new() { Code = 34, Name = "Page Down", Category = "Navigation", Notes = "Scroll page down" },
                new() { Code = 35, Name = "End", Category = "Navigation", Notes = "Jump to end" },
                new() { Code = 36, Name = "Home", Category = "Navigation", Notes = "Jump to home" },
                new() { Code = 37, Name = "Left Arrow", Category = "Navigation", Notes = "Directional left" },
                new() { Code = 38, Name = "Up Arrow", Category = "Navigation", Notes = "Directional up" },
                new() { Code = 39, Name = "Right Arrow", Category = "Navigation", Notes = "Directional right" },
                new() { Code = 40, Name = "Down Arrow", Category = "Navigation", Notes = "Directional down" },
                new() { Code = 44, Name = "Print Screen", Category = "Navigation", Notes = "Capture screen" },
                new() { Code = 45, Name = "Insert", Category = "Navigation", Notes = "Insert toggle" },
                new() { Code = 46, Name = "Delete", Category = "Navigation", Notes = "Delete next character" },
            };

            // 0 - 9
            for (int i = 0; i <= 9; i++)
            {
                _allKeys.Add(new VirtualKeyInfo { Code = 48 + i, Name = $"Key {i}", Category = "Alpha / Number", Notes = $"Number {i}" });
            }

            // A - Z
            for (char c = 'A'; c <= 'Z'; c++)
            {
                _allKeys.Add(new VirtualKeyInfo { Code = c, Name = $"Key {c}", Category = "Alpha / Number", Notes = $"Letter {c}" });
            }

            // Windows / Applications keys
            _allKeys.AddRange(new[]
            {
                new VirtualKeyInfo { Code = 91, Name = "Left Windows", Category = "System", Notes = "Left Win key" },
                new VirtualKeyInfo { Code = 92, Name = "Right Windows", Category = "System", Notes = "Right Win key" },
                new VirtualKeyInfo { Code = 93, Name = "Menu / Applications", Category = "System", Notes = "Context menu key" },
            });

            // Numpad 0 - 9
            for (int i = 0; i <= 9; i++)
            {
                _allKeys.Add(new VirtualKeyInfo { Code = 96 + i, Name = $"Numpad {i}", Category = "Numpad", Notes = $"Numpad numeric {i}" });
            }

            _allKeys.AddRange(new[]
            {
                new VirtualKeyInfo { Code = 106, Name = "Numpad *", Category = "Numpad", Notes = "Numpad multiply" },
                new VirtualKeyInfo { Code = 107, Name = "Numpad +", Category = "Numpad", Notes = "Numpad add" },
                new VirtualKeyInfo { Code = 109, Name = "Numpad -", Category = "Numpad", Notes = "Numpad subtract" },
                new VirtualKeyInfo { Code = 110, Name = "Numpad .", Category = "Numpad", Notes = "Numpad decimal" },
                new VirtualKeyInfo { Code = 111, Name = "Numpad /", Category = "Numpad", Notes = "Numpad divide" },
            });

            // F1 - F24
            for (int i = 1; i <= 24; i++)
            {
                _allKeys.Add(new VirtualKeyInfo { Code = 111 + i, Name = $"F{i}", Category = "Function", Notes = $"Function key F{i}" });
            }

            _allKeys.AddRange(new[]
            {
                new VirtualKeyInfo { Code = 144, Name = "Num Lock", Category = "Modifier", Notes = "Numpad lock toggle" },
                new VirtualKeyInfo { Code = 145, Name = "Scroll Lock", Category = "Modifier", Notes = "Scroll lock toggle" },
                new VirtualKeyInfo { Code = 160, Name = "Left Shift", Category = "Modifier", Notes = "Left Shift modifier" },
                new VirtualKeyInfo { Code = 161, Name = "Right Shift", Category = "Modifier", Notes = "Right Shift modifier" },
                new VirtualKeyInfo { Code = 162, Name = "Left Ctrl", Category = "Modifier", Notes = "Left Control modifier" },
                new VirtualKeyInfo { Code = 163, Name = "Right Ctrl", Category = "Modifier", Notes = "Right Control modifier" },
                new VirtualKeyInfo { Code = 164, Name = "Left Alt", Category = "Modifier", Notes = "Left Alt modifier" },
                new VirtualKeyInfo { Code = 165, Name = "Right Alt", Category = "Modifier", Notes = "Right Alt modifier" },
                new VirtualKeyInfo { Code = 186, Name = "Semicolon (;)", Category = "Symbol", Notes = "Semicolon and colon" },
                new VirtualKeyInfo { Code = 187, Name = "Equals (=)", Category = "Symbol", Notes = "Equals and plus" },
                new VirtualKeyInfo { Code = 188, Name = "Comma (,)", Category = "Symbol", Notes = "Comma and less-than" },
                new VirtualKeyInfo { Code = 189, Name = "Minus (-)", Category = "Symbol", Notes = "Minus and underscore" },
                new VirtualKeyInfo { Code = 190, Name = "Period (.)", Category = "Symbol", Notes = "Period and greater-than" },
                new VirtualKeyInfo { Code = 191, Name = "Slash (/)", Category = "Symbol", Notes = "Slash and question mark" },
                new VirtualKeyInfo { Code = 192, Name = "Tilde (`/ ~)", Category = "Symbol", Notes = "Backtick and tilde" },
                new VirtualKeyInfo { Code = 219, Name = "Left Bracket ([)", Category = "Symbol", Notes = "Left square and curly bracket" },
                new VirtualKeyInfo { Code = 220, Name = "Backslash (\\)", Category = "Symbol", Notes = "Backslash and pipe" },
                new VirtualKeyInfo { Code = 221, Name = "Right Bracket (])", Category = "Symbol", Notes = "Right square and curly bracket" },
                new VirtualKeyInfo { Code = 222, Name = "Quote (')", Category = "Symbol", Notes = "Single and double quote" },
                new VirtualKeyInfo { Code = 256, Name = "Scroll Up", Category = "Mouse", Notes = "Mouse wheel rolled up" },
                new VirtualKeyInfo { Code = 257, Name = "Scroll Down", Category = "Mouse", Notes = "Mouse wheel rolled down" }
            });
        }

        public static IReadOnlyList<VirtualKeyInfo> AllKeys => _allKeys;

        public static VirtualKeyInfo GetKeyByCode(int code)
        {
            var match = _allKeys.FirstOrDefault(k => k.Code == code);
            if (match != null) return match;
            return new VirtualKeyInfo { Code = code, Name = $"Key {code}" };
        }

        public static int FromWpfKey(Key key)
        {
            if (key == Key.System)
            {
                // Handle Alt or F10
                return 18; // VK_MENU
            }
            return KeyInterop.VirtualKeyFromKey(key);
        }
    }
}
