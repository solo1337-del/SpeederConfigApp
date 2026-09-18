# Speeder Console Commands, Waymarks & Macro Engine Integration Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Expand SpeederConfigApp into a comprehensive suite supporting the 55-line `config.txt` generator, an in-app Cheatsheet/Reference browser, a Macro (.ini) Studio linked to Line 29, a Waymark (.ini) Route & Unstick Editor, upgraded VirtualKey mappings, and markdown documentation.

**Architecture:** 
- `Models/VirtualKeyHelper.cs` is upgraded with left/right modifier keys (160-165), Speeder scroll keys (256-257), and Windows keys.
- `Models/MacroModel.cs` and `Models/WaymarkModel.cs` provide full serialization, parsing, and data-binding for `.ini` files.
- `ViewModels/MainViewModel.cs` is extended to host sub-models, commands, and active tab states.
- `MainWindow.xaml` adds a modern tabbed layout (`Config (55 Lines)`, `Macro Studio (.ini)`, `Waymark Studio (.ini)`, `Commands & Syntax Reference`) while preserving all existing styling and 55-line config capabilities.
- `Tests/SelfTestRunner.cs` adds tests for key mapping, macro serialization/parsing, and waymark serialization/parsing.
- `docs/` repository files provide standalone reference for console commands, macro syntax, and virtual key codes.

**Tech Stack:** C# .NET 10, WPF, MVVM, XAML Modern Dark Theme.

**Spec:** User provided console commands, waymarks specification, macro scripting syntax, virtual key table, andAlbion-specific targeting parameters.

## Global Constraints
- Target Framework: .NET 10.0 Windows (`net10.0-windows`).
- Zero external runtime NuGet dependencies (use pure standard library and existing WPF controls).
- All 50 existing automated tests in `SelfTestRunner.cs` must continue to pass without regression.
- Maintain documentation integrity and GitHub markdown links in all responses.

---

### Task 1: Documentation Files
**Files:**
- Create: `docs/ConsoleCommands.md`
- Create: `docs/MacrosAndWaymarks.md`
- Create: `docs/VirtualKeyCodes.md`

- [ ] **Step 1: Create `docs/ConsoleCommands.md`**
Document all Speeder console commands (`-d`, `-e`, `-allplayers`, `-allgather`, `-allmobs`, `-closeg`, `-tw*`, `-twrepeat*`, `-twresume`, `-record`, `-recordkeys`, `-cl`, `-mp`, `-gpc`, `-displaycd`, `-settings`, `-esp`, `-rad`, `-cmd`, `-cf`) with exact parameters, bypass syntax (e.g. `-record 9999,30`, `-recordkeys 113,114,100`), and usage notes.

- [ ] **Step 2: Create `docs/MacrosAndWaymarks.md`**
Document waymark file format (`[variables]`, `[unstick]`, `[0]`, `[1]`, `wait time=1` seamless trick, `x=0.1` stop-movement trick, `script=file.ini|count`), macro syntax (`keys`, `keys2`, `endkeys`, `repeat`, `interrupt`), function format (`[fFunctionName]`), targeting commands (`tcp`, `tcm`, `tcg`, `tphp`, `aim`, `grid`, `circle`), entity commands (`cng`, `cnp`, `cnm`, `cphp`), and variable/scripting commands (`store %`, `eq %`, `cmp`, `or %`, `add %`, `sub %`, `mul %`, `div %`, `rand`, `cc %`, `call %`, `dbg %`). Include the combat and gathering example macros.

- [ ] **Step 3: Create `docs/VirtualKeyCodes.md`**
Document all standard virtual key codes (1-254) plus Speeder-specific codes (256 Scroll Up, 257 Scroll Down) and modifier guidance (use 160-165 instead of 16-18).

- [ ] **Step 4: Verify files exist and format is clean**
Verify all three docs are well-formatted markdown.

---

### Task 2: Upgrade VirtualKeyHelper & Settings Tooltips
**Files:**
- Modify: `SpeederConfigApp/Models/VirtualKeyHelper.cs`
- Modify: `SpeederConfigApp/MainWindow.xaml`
- Test: `SpeederConfigApp/Tests/SelfTestRunner.cs`

- [ ] **Step 1: Update `VirtualKeyHelper.cs`**
Add:
- Left/Right modifiers: 160 (`VK_LSHIFT`), 161 (`VK_RSHIFT`), 162 (`VK_LCONTROL`), 163 (`VK_RCONTROL`), 164 (`VK_LMENU` / Left Alt), 165 (`VK_RMENU` / Right Alt).
- Windows keys: 91 (`VK_LWIN`), 92 (`VK_RWIN`), 93 (`VK_APPS`).
- Special Speeder codes: 256 ("Scroll Up"), 257 ("Scroll Down").

- [ ] **Step 2: Update UI Tooltips and Line Descriptions in `MainWindow.xaml`**
Update tooltips for:
- Line 22: Mention pause/resume toggle for `-twrepeat*`, `-tw*`, `-twresume`.
- Line 24: Mention sleep jitter / randomization for waypoints and `s` command.
- Line 26: Mention cursor offset distance for `tcg` and `tcg*` targeting commands.
- Line 27: Mention record coordinates key for `-record`, `-recordkeys`, and `-cl`.
- Line 29: Mention macro `.ini` file name and link to Macro Studio.
- Line 31: Mention mouse smoothing for `m[x],[y]`, `grid`, and `circle`.
- Line 33: Mention DPI / scaling multiplier for `-gpc` command.
- Line 47: Mention camera projection (FOV/Yaw/Pitch) required for `tcp`, `tcm`, `tcg`, `aim`, `tphp`.

- [ ] **Step 3: Add unit tests in `SelfTestRunner.cs` for new key codes**
Add assertions that codes 160, 161, 162, 163, 164, 165, 256, and 257 resolve properly.

- [ ] **Step 4: Run test suite to verify**
Run `dotnet run -- --test` and ensure tests pass.

---

### Task 3: Macro Models, Parser & Templates
**Files:**
- Create: `SpeederConfigApp/Models/MacroFileModel.cs`
- Test: `SpeederConfigApp/Tests/SelfTestRunner.cs`

- [ ] **Step 1: Implement `MacroFileModel.cs`**
Create:
- `MacroItem`: `TriggerKey` (int), `FunctionName` (string for `[fName]`), `IsFunction` (bool), `KeysLines` (`ObservableCollection<string>`), `EndKeys` (string), `Repeat` (int), `Interrupt` (int), `Description` (string).
- `MacroFileModel`: `Macros` (`ObservableCollection<MacroItem>`), `ParseIni(string content)`, `GenerateIni()`, helper methods to add default combat rotation, target player/mob, gather rock, speed adjustment macros.

- [ ] **Step 2: Add Unit Tests in `SelfTestRunner.cs`**
Test parsing a macro `.ini` file with both numeric trigger sections (e.g. `[113]`) and functions (e.g. `[fTest]`), checking `repeat`, `endkeys`, and round-trip string generation.

- [ ] **Step 3: Run test suite to verify**
Run `dotnet run -- --test` and ensure tests pass.

---

### Task 4: Waymark Models, Parser & Unstick Engine
**Files:**
- Create: `SpeederConfigApp/Models/WaymarkFileModel.cs`
- Test: `SpeederConfigApp/Tests/SelfTestRunner.cs`

- [ ] **Step 1: Implement `WaymarkFileModel.cs`**
Create:
- `WaymarkPoint`: `Index` (int), `X` (double), `Y` (double), `Z` (double), `WaitTime` (int, default 30), `Keys` (string), `Script` (string), `IsMovementPrevented` (`X == 0.1`).
- `UnstickConfig`: `Distance` (double), `Keys` (string), `Timer` (int), `Timer2` (int), `GiveUp` (int), `Script` (string).
- `WaymarkFileModel`: `Variables` (`ObservableCollection<KeyValuePair<string, string>>`), `Unstick` (`UnstickConfig`), `Points` (`ObservableCollection<WaymarkPoint>`), `ParseIni(string content)`, `GenerateIni()`.

- [ ] **Step 2: Add Unit Tests in `SelfTestRunner.cs`**
Test parsing waymark `.ini` file containing `[variables]`, `[unstick]`, and sequential waypoints `[0]`, `[1]`, with `wait time=1` and `x=0.1` handling, verifying round-trip generation.

- [ ] **Step 3: Run test suite to verify**
Run `dotnet run -- --test` and ensure tests pass.

---

### Task 5: ViewModel Expansion & Reference Data
**Files:**
- Create: `SpeederConfigApp/Models/ReferenceDocItem.cs`
- Modify: `SpeederConfigApp/ViewModels/MainViewModel.cs`

- [ ] **Step 1: Create `ReferenceDocItem.cs`**
Data structures for console commands (`Command`, `Parameters`, `Description`, `Example`), macro commands (`Syntax`, `Category`, `Description`, `Example`), and virtual key codes for fast searching/filtering.

- [ ] **Step 2: Extend `MainViewModel.cs`**
Add:
- `ActiveTabIndex` property.
- `MacroFileModel MacroFile` property with Load/Save/New/Add Macro commands.
- `WaymarkFileModel WaymarkFile` property with Load/Save/New/Add Waypoint commands.
- Search/filter properties for Commands Reference and VK Reference.
- Quick link between Line 29 (`MacroFileName`) and the Macro Studio.

---

### Task 6: UI Integration - Modern Tabs & Studios in `MainWindow.xaml`
**Files:**
- Modify: `SpeederConfigApp/MainWindow.xaml`
- Modify: `SpeederConfigApp/Themes/ModernTheme.xaml` (if additional tab styles needed)

- [ ] **Step 1: Update Top Navigation with Tab Selector**
Add modern segmented tab switcher in the header:
- `⚙️ Config (55 Lines)`
- `🎯 Macro Studio (.ini)`
- `🗺️ Waymark Studio (.ini)`
- `📖 Commands & Syntax Reference`

- [ ] **Step 2: Add Macro Studio View**
UI layout with:
- Left pane: Macro / Function list with Trigger Key or Function Name badge, Add/Delete/Clone buttons, Pre-built templates dropdown (Combat rotation, Target lock, Gathering, Movement speed).
- Center/Right pane: Macro details editor (`Trigger Key`, `Repeat Mode`, `Interrupt`, `EndKeys`), list of `keys=` lines with Add/Delete/Move Up/Move Down, and Live `.ini` preview with Copy/Save.

- [ ] **Step 3: Add Waymark Studio View**
UI layout with:
- Top bar: Route summary (Total waypoints, Unstick enabled, Actions), New/Open/Save buttons.
- Left pane: Waypoints DataGrid/List with index, X, Y, Z, Wait Time, Keys, Script. Buttons to Add Waypoint, Delete, Move Up/Down, Set Stop-Movement (`x=0.1`), Set Seamless (`wait time=1`).
- Right pane: Unstick failsafe settings panel (`Distance`, `Keys`, `Timer ms`, `Timer2 ms`, `GiveUp attempts`, `Script`) + Variables table (`[variables]`) + Live `.ini` preview.

- [ ] **Step 4: Add Commands & Key Codes Reference View**
UI layout with:
- Search filter box.
- Category tabs or grouped list:
  1. Console Commands (table with Command, Parameters, Description, Example).
  2. Macro & Waymark Syntax (Command, Arguments, Category, Description, Example).
  3. Virtual Key Codes (Code, Name, Modifier instructions, Special Speeder codes).
- Quick "Copy Command / Example" button.

---

### Task 7: Build, Verification & Published Executable
**Files:**
- Test: `SpeederConfigApp/Tests/SelfTestRunner.cs`
- Build: `dotnet build -c Release`
- Publish: `dotnet publish -c Release -o "./publish"`

- [ ] **Step 1: Run comprehensive automated test suite**
Run `dotnet run -- --test` and verify all tests pass (50 original + new key tests + macro tests + waymark tests).

- [ ] **Step 2: Build Release**
Run `dotnet build -c Release` and confirm zero errors, zero warnings.

- [ ] **Step 3: Publish updated executable**
Run `dotnet publish -c Release -o "./publish"`.
