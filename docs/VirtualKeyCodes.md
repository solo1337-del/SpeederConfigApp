# Speeder Virtual-Key Codes Reference Guide

To have Speeder trigger macros, simulate clicks, or press keys in waymarks, you must specify the **Virtual-Key (VK) Code** as a decimal number.

---

## Important Rules & Notes

### Mouse Codes
- **1**: Left Mouse Button (`VK_LBUTTON`)
- **2**: Right Mouse Button (`VK_RBUTTON`)
- **4**: Middle Mouse Button / Scroll Wheel Click (`VK_MBUTTON`)
- **5**: X1 Mouse Button (Back side button) (`VK_XBUTTON1`)
- **6**: X2 Mouse Button (Forward side button) (`VK_XBUTTON2`)
- **256**: Wheel Scroll Up *(Speeder custom code)*
- **257**: Wheel Scroll Down *(Speeder custom code)*
- *Note on MMO mice*: MMO gaming mouse buttons (e.g. 12-key thumb grids) are mapped to keyboard keys via manufacturer software (Razer Synapse, Corsair iCUE, Logitech G HUB). Use the virtual key code of the bound keyboard key.

### Modifier Keys Warning
When automating modifiers (Shift, Control, Alt), **do not use the generic codes (16, 17, 18)** because Windows dispatching behavior can cause sticky keys. **Always use the explicit left/right specific virtual key codes:**
- **160** (`VK_LSHIFT`): Left Shift
- **161** (`VK_RSHIFT`): Right Shift
- **162** (`VK_LCONTROL`): Left Control
- **163** (`VK_RCONTROL`): Right Control
- **164** (`VK_LMENU`): Left Alt
- **165** (`VK_RMENU`): Right Alt

---

## Commonly Used Quick Reference

| Key | Decimal | Hex | Category | Recommended Use |
|---|---|---|---|---|
| Left Click | **1** | `0x01` | Mouse | Direct interact / attack |
| Right Click | **2** | `0x02` | Mouse | Movement / pathing |
| Middle Click | **4** | `0x04` | Mouse | Camera / utility |
| Scroll Up | **256** | `0x100` | Mouse (Speeder) | Quick zoom / macro trigger |
| Scroll Down | **257** | `0x101` | Mouse (Speeder) | Quick zoom / macro trigger |
| Backspace | **8** | `0x08` | Control | Delete / back |
| Tab | **9** | `0x09` | Control | Target cycle |
| Enter / Return | **13** | `0x0D` | Control | Confirm / chat |
| Escape | **27** | `0x1B` | Control | Cancel / clear target / unstick |
| Spacebar | **32** | `0x20` | Action | Jump / mount |
| Q | **81** | `0x51` | Abilities | Primary combat skill |
| W | **87** | `0x57` | Abilities | Secondary combat skill |
| E | **69** | `0x45` | Abilities | Third combat skill |
| R | **82** | `0x52` | Abilities | Chest / Ultimate skill |
| F | **70** | `0x46` | Abilities | Boots / Sprint skill |
| D | **68** | `0x44` | Abilities | Helmet ability |
| F1 to F12 | **112–123** | `0x70–0x7B` | Function | Safe macro hotkeys |
| Left Shift | **160** | `0xA0` | Modifiers | Always prefer over 16 |
| Left Ctrl | **162** | `0xA2` | Modifiers | Always prefer over 17 |
| Left Alt | **164** | `0xA4` | Modifiers | Always prefer over 18 |

---

## Complete Virtual-Key Codes Table

| Symbolic Constant | Decimal Code | Hex Code | Equivalent Key / Description |
|---|---|---|---|
| `VK_LBUTTON` | **1** | `0x01` | Left mouse button |
| `VK_RBUTTON` | **2** | `0x02` | Right mouse button |
| `VK_CANCEL` | **3** | `0x03` | Control-break processing |
| `VK_MBUTTON` | **4** | `0x04` | Middle mouse button (wheel click) |
| `VK_XBUTTON1` | **5** | `0x05` | X1 mouse button (side back button) |
| `VK_XBUTTON2` | **6** | `0x06` | X2 mouse button (side forward button) |
| — | 7 | `0x07` | Undefined |
| `VK_BACK` | **8** | `0x08` | BACKSPACE key |
| `VK_TAB` | **9** | `0x09` | TAB key |
| — | 10–11 | `0x0A–0x0B` | Reserved |
| `VK_CLEAR` | **12** | `0x0C` | CLEAR key |
| `VK_RETURN` | **13** | `0x0D` | ENTER / RETURN key |
| — | 14–15 | `0x0E–0x0F` | Undefined |
| `VK_SHIFT` | **16** | `0x10` | SHIFT key *(Reserved — use 160 or 161 instead)* |
| `VK_CONTROL` | **17** | `0x11` | CONTROL key *(Reserved — use 162 or 163 instead)* |
| `VK_MENU` | **18** | `0x12` | ALT / MENU key *(Reserved — use 164 or 165 instead)* |
| `VK_PAUSE` | **19** | `0x13` | PAUSE key |
| `VK_CAPITAL` | **20** | `0x14` | CAPS LOCK key |
| `VK_KANA` / `VK_HANGUL` | **21** | `0x15` | IME Kana / Hangul mode |
| — | 22 | `0x16` | Undefined |
| `VK_JUNJA` | **23** | `0x17` | IME Junja mode |
| `VK_FINAL` | **24** | `0x18` | IME final mode |
| `VK_HANJA` / `VK_KANJI` | **25** | `0x19` | IME Hanja / Kanji mode |
| — | 26 | `0x1A` | Undefined |
| `VK_ESCAPE` | **27** | `0x1B` | ESC (Escape) key |
| `VK_CONVERT` | **28** | `0x1C` | IME convert |
| `VK_NONCONVERT` | **29** | `0x1D` | IME nonconvert |
| `VK_ACCEPT` | **30** | `0x1E` | IME accept |
| `VK_MODECHANGE` | **31** | `0x1F` | IME mode change request |
| `VK_SPACE` | **32** | `0x20` | SPACEBAR |
| `VK_PRIOR` | **33** | `0x21` | PAGE UP key |
| `VK_NEXT` | **34** | `0x22` | PAGE DOWN key |
| `VK_END` | **35** | `0x23` | END key |
| `VK_HOME` | **36** | `0x24` | HOME key |
| `VK_LEFT` | **37** | `0x25` | LEFT ARROW key |
| `VK_UP` | **38** | `0x26` | UP ARROW key |
| `VK_RIGHT` | **39** | `0x27` | RIGHT ARROW key |
| `VK_DOWN` | **40** | `0x28` | DOWN ARROW key |
| `VK_SELECT` | **41** | `0x29` | SELECT key |
| `VK_PRINT` | **42** | `0x2A` | PRINT key |
| `VK_EXECUTE` | **43** | `0x2B` | EXECUTE key |
| `VK_SNAPSHOT` | **44** | `0x2C` | PRINT SCREEN key |
| `VK_INSERT` | **45** | `0x2D` | INS (Insert) key |
| `VK_DELETE` | **46** | `0x2E` | DEL (Delete) key |
| `VK_HELP` | **47** | `0x2F` | HELP key |
| `VK_0` | **48** | `0x30` | 0 key (Top row) |
| `VK_1` | **49** | `0x31` | 1 key (Top row) |
| `VK_2` | **50** | `0x32` | 2 key (Top row) |
| `VK_3` | **51** | `0x33` | 3 key (Top row) |
| `VK_4` | **52** | `0x34` | 4 key (Top row) |
| `VK_5` | **53** | `0x35` | 5 key (Top row) |
| `VK_6` | **54** | `0x36` | 6 key (Top row) |
| `VK_7` | **55** | `0x37` | 7 key (Top row) |
| `VK_8` | **56** | `0x38` | 8 key (Top row) |
| `VK_9` | **57** | `0x39` | 9 key (Top row) |
| — | 58–64 | `0x3A–0x40` | Undefined |
| `VK_A` | **65** | `0x41` | A key |
| `VK_B` | **66** | `0x42` | B key |
| `VK_C` | **67** | `0x43` | C key |
| `VK_D` | **68** | `0x44` | D key |
| `VK_E` | **69** | `0x45` | E key |
| `VK_F` | **70** | `0x46` | F key |
| `VK_G` | **71** | `0x47` | G key |
| `VK_H` | **72** | `0x48` | H key |
| `VK_I` | **73** | `0x49` | I key |
| `VK_J` | **74** | `0x4A` | J key |
| `VK_K` | **75** | `0x4B` | K key |
| `VK_L` | **76** | `0x4C` | L key |
| `VK_M` | **77** | `0x4D` | M key |
| `VK_N` | **78** | `0x4E` | N key |
| `VK_O` | **79** | `0x4F` | O key |
| `VK_P` | **80** | `0x50` | P key |
| `VK_Q` | **81** | `0x51` | Q key |
| `VK_R` | **82** | `0x52` | R key |
| `VK_S` | **83** | `0x53` | S key |
| `VK_T` | **84** | `0x54` | T key |
| `VK_U` | **85** | `0x55` | U key |
| `VK_V` | **86** | `0x56` | V key |
| `VK_W` | **87** | `0x57` | W key |
| `VK_X` | **88** | `0x58` | X key |
| `VK_Y` | **89** | `0x59` | Y key |
| `VK_Z` | **90** | `0x5A` | Z key |
| `VK_LWIN` | **91** | `0x5B` | Left Windows key |
| `VK_RWIN` | **92** | `0x5C` | Right Windows key |
| `VK_APPS` | **93** | `0x5D` | Applications / Context Menu key |
| — | 94 | `0x5E` | Reserved |
| `VK_SLEEP` | **95** | `0x5F` | Computer Sleep key |
| `VK_NUMPAD0` | **96** | `0x60` | Numeric keypad 0 key |
| `VK_NUMPAD1` | **97** | `0x61` | Numeric keypad 1 key |
| `VK_NUMPAD2` | **98** | `0x62` | Numeric keypad 2 key |
| `VK_NUMPAD3` | **99** | `0x63` | Numeric keypad 3 key |
| `VK_NUMPAD4` | **100** | `0x64` | Numeric keypad 4 key |
| `VK_NUMPAD5` | **101** | `0x65` | Numeric keypad 5 key |
| `VK_NUMPAD6` | **102** | `0x66` | Numeric keypad 6 key |
| `VK_NUMPAD7` | **103** | `0x67` | Numeric keypad 7 key |
| `VK_NUMPAD8` | **104** | `0x68` | Numeric keypad 8 key |
| `VK_NUMPAD9` | **105** | `0x69` | Numeric keypad 9 key |
| `VK_MULTIPLY` | **106** | `0x6A` | Multiply key (`*` on numeric keypad) |
| `VK_ADD` | **107** | `0x6B` | Add key (`+` on numeric keypad) |
| `VK_SEPARATOR` | **108** | `0x6C` | Separator key |
| `VK_SUBTRACT` | **109** | `0x6D` | Subtract key (`-` on numeric keypad) |
| `VK_DECIMAL` | **110** | `0x6E` | Decimal key (`.` on numeric keypad) |
| `VK_DIVIDE` | **111** | `0x6F` | Divide key (`/` on numeric keypad) |
| `VK_F1` | **112** | `0x70` | F1 key |
| `VK_F2` | **113** | `0x71` | F2 key |
| `VK_F3` | **114** | `0x72` | F3 key |
| `VK_F4` | **115** | `0x73` | F4 key |
| `VK_F5` | **116** | `0x74` | F5 key |
| `VK_F6` | **117** | `0x75` | F6 key |
| `VK_F7` | **118** | `0x76` | F7 key |
| `VK_F8` | **119** | `0x77` | F8 key |
| `VK_F9` | **120** | `0x78` | F9 key |
| `VK_F10` | **121** | `0x79` | F10 key |
| `VK_F11` | **122** | `0x7A` | F11 key |
| `VK_F12` | **123** | `0x7B` | F12 key |
| `VK_F13` | **124** | `0x7C` | F13 key |
| `VK_F14` | **125** | `0x7D` | F14 key |
| `VK_F15` | **126** | `0x7E` | F15 key |
| `VK_F16` | **127** | `0x7F` | F16 key |
| `VK_F17` | **128** | `0x80` | F17 key |
| `VK_F18` | **129** | `0x81` | F18 key |
| `VK_F19` | **130** | `0x82` | F19 key |
| `VK_F20` | **131** | `0x83` | F20 key |
| `VK_F21` | **132** | `0x84` | F21 key |
| `VK_F22` | **133** | `0x85` | F22 key |
| `VK_F23` | **134** | `0x86` | F23 key |
| `VK_F24` | **135** | `0x87` | F24 key |
| — | 136–143 | `0x88–0x8F` | Unassigned |
| `VK_NUMLOCK` | **144** | `0x90` | NUM LOCK key |
| `VK_SCROLL` | **145** | `0x91` | SCROLL LOCK key |
| — | 146–150 | `0x92–0x96` | OEM specific |
| — | 151–159 | `0x97–0x9F` | Unassigned |
| `VK_LSHIFT` | **160** | `0xA0` | Left SHIFT key |
| `VK_RSHIFT` | **161** | `0xA1` | Right SHIFT key |
| `VK_LCONTROL` | **162** | `0xA2` | Left CONTROL key |
| `VK_RCONTROL` | **163** | `0xA3` | Right CONTROL key |
| `VK_LMENU` | **164** | `0xA4` | Left ALT key |
| `VK_RMENU` | **165** | `0xA5` | Right ALT key |
| `VK_BROWSER_BACK` | **166** | `0xA6` | Browser Back key |
| `VK_BROWSER_FORWARD` | **167** | `0xA7` | Browser Forward key |
| `VK_BROWSER_REFRESH` | **168** | `0xA8` | Browser Refresh key |
| `VK_BROWSER_STOP` | **169** | `0xA9` | Browser Stop key |
| `VK_BROWSER_SEARCH` | **170** | `0xAA` | Browser Search key |
| `VK_BROWSER_FAVORITES` | **171** | `0xAB` | Browser Favorites key |
| `VK_BROWSER_HOME` | **172** | `0xAC` | Browser Home key |
| `VK_VOLUME_MUTE` | **173** | `0xAD` | Volume Mute key |
| `VK_VOLUME_DOWN` | **174** | `0xAE` | Volume Down key |
| `VK_VOLUME_UP` | **175** | `0xAF` | Volume Up key |
| `VK_MEDIA_NEXT_TRACK` | **176** | `0xB0` | Next Track key |
| `VK_MEDIA_PREV_TRACK` | **177** | `0xB1` | Previous Track key |
| `VK_MEDIA_STOP` | **178** | `0xB2` | Stop Media key |
| `VK_MEDIA_PLAY_PAUSE` | **179** | `0xB3` | Play / Pause Media key |
| `VK_LAUNCH_MAIL` | **180** | `0xB4` | Start Mail key |
| `VK_LAUNCH_MEDIA_SELECT` | **181** | `0xB5` | Select Media key |
| `VK_LAUNCH_APP1` | **182** | `0xB6` | Start Application 1 key |
| `VK_LAUNCH_APP2` | **183** | `0xB7` | Start Application 2 key |
| — | 184–185 | `0xB8–0xB9` | Reserved |
| `VK_OEM_1` | **186** | `0xBA` | `;:` key (US standard keyboard) |
| `VK_OEM_PLUS` | **187** | `0xBB` | `+` / `=` key |
| `VK_OEM_COMMA` | **188** | `0xBC` | `,` / `<` key |
| `VK_OEM_MINUS` | **189** | `0xBD` | `-` / `_` key |
| `VK_OEM_PERIOD` | **190** | `0xBE` | `.` / `>` key |
| `VK_OEM_2` | **191** | `0xBF` | `/?` key (US standard keyboard) |
| `VK_OEM_3` | **192** | `0xC0` | `~`` (Tilde / Backtick) key |
| — | 193–215 | `0xC1–0xD7` | Reserved |
| — | 216–218 | `0xD8–0xDA` | Unassigned |
| `VK_OEM_4` | **219** | `0xDB` | `[{` key |
| `VK_OEM_5` | **220** | `0xDC` | `\|` (Backslash) key |
| `VK_OEM_6` | **221** | `0xDD` | `]}` key |
| `VK_OEM_7` | **222** | `0xDE` | `'"` (Single/Double Quote) key |
| `VK_OEM_8` | **223** | `0xDF` | Miscellaneous OEM character |
| — | 224 | `0xE0` | Reserved |
| — | 225 | `0xE1` | OEM specific |
| `VK_OEM_102` | **226** | `0xE2` | `<>` or `\|` key on 102-key keyboard |
| — | 227–228 | `0xE3–0xE4` | OEM specific |
| `VK_PROCESSKEY` | **229** | `0xE5` | IME PROCESS key |
| — | 230 | `0xE6` | OEM specific |
| `VK_PACKET` | **231** | `0xE7` | Unicode packet key |
| — | 232 | `0xE8` | Unassigned |
| — | 233–245 | `0xE9–0xF5` | OEM specific |
| `VK_ATTN` | **246** | `0xF6` | Attn key |
| `VK_CRSEL` | **247** | `0xF7` | CrSel key |
| `VK_EXSEL` | **248** | `0xF8` | ExSel key |
| `VK_EREOF` | **249** | `0xF9` | Erase EOF key |
| `VK_PLAY` | **250** | `0xFA` | Play key |
| `VK_ZOOM` | **251** | `0xFB` | Zoom key |
| `VK_NONAME` | **252** | `0xFC` | Reserved |
| `VK_PA1` | **253** | `0xFD` | PA1 key |
| `VK_OEM_CLEAR` | **254** | `0xFE` | Clear key |
| **Speeder Scroll Up** | **256** | `0x100` | Mouse Wheel Scroll Up (Speeder Custom) |
| **Speeder Scroll Down** | **257** | `0x101` | Mouse Wheel Scroll Down (Speeder Custom) |
