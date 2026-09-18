# Speeder Config (WPF)

A modern, high-performance desktop configuration utility built with **WPF (.NET 10)**. Features a polished dark-mode interface, real-time input validation, and interactive tools for managing structured configuration files.

---

## ✨ Features & Highlights

- **Intuitive GUI**: Clean categorized layout for rapid parameter management across core systems.
- **Interactive Key Detector**: Built-in keyboard listener capturing Windows Virtual Key codes with one-click recording.
- **Color Studio**: Integrated RGB color picker supporting visual swatches, decimal RGB integers, and quick presets.
- **Live Output Inspector**: Real-time dual-view panel showing annotated settings alongside formatted raw output.
- **Automated Validation Suite**: Built-in 50-check test runner guaranteeing data integrity and formatting accuracy.

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
