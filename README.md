# Speeder Config (WPF)

A modern, high-performance desktop configuration utility built with **WPF (.NET 10)**. Features a polished dark-mode interface, real-time input validation, and interactive tools for managing structured configuration files.

---

## ✨ Features & Highlights

- **Multi-Studio Desktop Environment**: Seamlessly toggle between 4 dedicated studios via a modern dark-mode segmented navigation bar:
  1. ⚙️ **Config (55 Lines)**: Graphical parameter manager for `config.txt` with categorized cards, live dual-view inspector, and screen clamp boundary calculator.
  2. 🎯 **Macro Studio (.ini)**: Interactive macro builder and editor linked to Line 29 (`MacroFileName`). Includes visual key pickers, execution mode selectors, multi-line action step editors, quick syntax chips, and 8 verified combat/gathering/speed templates.
  3. 🗺️ **Waymark Studio (.ini)**: Complete route generator for waymark `.ini` files. Supports waypoint coordinate editing, stop-movement (`x=0.1`), seamless running (`wait time=1`), loop script chaining, variables (`[variables]`), and unstick recovery failsafe configuration (`[unstick]`).
  4. 📖 **Commands & Syntax Reference**: Searchable in-app cheatsheet indexing all 20 Speeder console commands, 50+ macro syntax tokens, and full Windows virtual key codes with one-click copy actions.
- **Interactive Key Detector & Upgraded VirtualKey Engine**: Captures Windows Virtual Key codes with support for left/right modifiers (160–165), Speeder scroll keys (256, 257), and multimedia keys.
- **Color Studio**: Integrated RGB color picker supporting visual swatches, decimal RGB integers, and quick presets.
- **Automated Validation Suite**: Built-in 196-check automated verification suite guaranteeing data integrity, INI round-tripping, and formatting accuracy.
- **Comprehensive Documentation**: Complete offline guides available in `docs/`:
  - [`docs/ConsoleCommands.md`](docs/ConsoleCommands.md)
  - [`docs/MacrosAndWaymarks.md`](docs/MacrosAndWaymarks.md)
  - [`docs/VirtualKeyCodes.md`](docs/VirtualKeyCodes.md)

---

## 🚀 How to Run

### Option 1: Run via .NET CLI
```powershell
cd SpeederConfigApp
dotnet run
```

### Option 2: Run Built-In Verification Test Suite
```powershell
dotnet run -- --test
```

### Option 3: Pre-Built / Published Executable
Pre-built executables are available in:
- `publish/SpeederConfigApp.exe`
- `SpeederConfigApp/bin/Release/net10.0-windows/SpeederConfigApp.exe`

To rebuild or publish at any time:
```powershell
# Build Release configuration
dotnet build -c Release

# Publish ready-to-run package
dotnet publish -c Release -o "./publish"
```

---

## 🔒 Confidentiality Notice
Proprietary configuration files, definitions, and active preset bundles are strictly reserved for subscribers. Do not commit or share live configuration files publicly.
