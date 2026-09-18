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
                new() { Code = 0, Name = "None" },
                new() { Code = 1, Name = "Left Mouse Button" },
                new() { Code = 2, Name = "Right Mouse Button" },
                new() { Code = 4, Name = "Middle Mouse Button" },
                new() { Code = 5, Name = "Mouse XButton 1" },
                new() { Code = 6, Name = "Mouse XButton 2" },
                new() { Code = 8, Name = "Backspace" },
                new() { Code = 9, Name = "Tab" },
                new() { Code = 12, Name = "Clear" },
                new() { Code = 13, Name = "Enter" },
                new() { Code = 16, Name = "Shift" },
                new() { Code = 17, Name = "Ctrl" },
                new() { Code = 18, Name = "Alt" },
                new() { Code = 19, Name = "Pause" },
                new() { Code = 20, Name = "Caps Lock" },
                new() { Code = 27, Name = "Escape" },
                new() { Code = 32, Name = "Space" },
                new() { Code = 33, Name = "Page Up" },
                new() { Code = 34, Name = "Page Down" },
                new() { Code = 35, Name = "End" },
                new() { Code = 36, Name = "Home" },
                new() { Code = 37, Name = "Left Arrow" },
                new() { Code = 38, Name = "Up Arrow" },
                new() { Code = 39, Name = "Right Arrow" },
                new() { Code = 40, Name = "Down Arrow" },
                new() { Code = 44, Name = "Print Screen" },
                new() { Code = 45, Name = "Insert" },
                new() { Code = 46, Name = "Delete" },
            };

            // 0 - 9
            for (int i = 0; i <= 9; i++)
            {
                _allKeys.Add(new VirtualKeyInfo { Code = 48 + i, Name = $"Key {i}" });
            }

            // A - Z
            for (char c = 'A'; c <= 'Z'; c++)
            {
                _allKeys.Add(new VirtualKeyInfo { Code = c, Name = $"Key {c}" });
            }

            // Numpad 0 - 9
            for (int i = 0; i <= 9; i++)
            {
                _allKeys.Add(new VirtualKeyInfo { Code = 96 + i, Name = $"Numpad {i}" });
            }

            _allKeys.AddRange(new[]
            {
                new VirtualKeyInfo { Code = 106, Name = "Numpad *" },
                new VirtualKeyInfo { Code = 107, Name = "Numpad +" },
                new VirtualKeyInfo { Code = 109, Name = "Numpad -" },
                new VirtualKeyInfo { Code = 110, Name = "Numpad ." },
                new VirtualKeyInfo { Code = 111, Name = "Numpad /" },
            });

            // F1 - F24
            for (int i = 1; i <= 24; i++)
            {
                _allKeys.Add(new VirtualKeyInfo { Code = 111 + i, Name = $"F{i}" });
            }

            _allKeys.AddRange(new[]
            {
                new VirtualKeyInfo { Code = 144, Name = "Num Lock" },
                new VirtualKeyInfo { Code = 145, Name = "Scroll Lock" },
                new VirtualKeyInfo { Code = 186, Name = "Semicolon (;)" },
                new VirtualKeyInfo { Code = 187, Name = "Equals (=)" },
                new VirtualKeyInfo { Code = 188, Name = "Comma (,)" },
                new VirtualKeyInfo { Code = 189, Name = "Minus (-)" },
                new VirtualKeyInfo { Code = 190, Name = "Period (.)" },
                new VirtualKeyInfo { Code = 191, Name = "Slash (/)" },
                new VirtualKeyInfo { Code = 192, Name = "Tilde (`)" },
                new VirtualKeyInfo { Code = 219, Name = "Left Bracket ([)" },
                new VirtualKeyInfo { Code = 220, Name = "Backslash (\\)" },
                new VirtualKeyInfo { Code = 221, Name = "Right Bracket (])" },
                new VirtualKeyInfo { Code = 222, Name = "Quote (')" }
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
