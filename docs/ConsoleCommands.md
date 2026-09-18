# Speeder Console Commands Reference

Type these commands directly into Speeder's console window or execute them dynamically within macros/waymarks using the `cc % [console command]` syntax.

---

## Quick Reference Table

| Command | Arguments | Category | Description | Related Config Line |
|---|---|---|---|---|
| `-d` | None | Hacks | Disables all hacks globally | — |
| `-e` | None | Hacks | Enables all hacks (only valid after `-d` was used) | — |
| `-allplayers` | None | Memory / Query | Dumps position, HP, and status for all players in memory | — |
| `-allgather` | None | Memory / Query | Dumps all active gathering nodes/objects in memory | — |
| `-allmobs` | None | Memory / Query | Dumps all mobs (names, distances, coordinates) in memory | — |
| `-closeg` | None | Memory / Query | Displays information for closest active gathering object | — |
| `-displaycd` | None | Combat / Debug | Displays cooldown IDs in console ~2 seconds after skill use | Line 29 (`cc` command) |
| `-cl` | None | Coordinates | Prints current player position `(x, z)` and copies to clipboard | Line 27 |
| `-mp` | None | Cursor / Mouse | Prints mouse position `(x, y)` and sets `CX`, `CY` variables | — |
| `-gpc` | None | Screen / Color | Prints pixel color (Hex/RGB) under cursor and copies to clipboard | Line 33 (DPI multiplier) |
| `-settings` | None | Config | Opens `config.txt` in default text editor | — |
| `-cf` | `[filename.txt]` | Config | Loads specified file as the active configuration file | — |
| `-esp` | None | Visuals | Toggles (opens or closes) the ESP overlay window | — |
| `-rad` | None | Visuals | Toggles (opens or closes) the Radar window | — |
| `-twrepeat*` | `[file.ini]` | Waymarks | Loops waymark file continuously starting at index 0 | Line 22 (Pause key) |
| `-tw*` | `[file.ini]` | Waymarks | Executes waymark file once from index 0 to last index (no repeat) | Line 22 (Pause key) |
| `-twresume` | None | Waymarks | Resumes `-twrepeat*` loop from the next waymark index | Line 22 (Pause key) |
| `-record` | `[dist],[wait]` *(optional)* | Recording | Records player movement path into `Recorded Waymarks.ini` | Line 27 (Record hotkey) |
| `-recordkeys` | `[stop],[mouse],[delay]` *(optional)* | Recording | Records keyboard and mouse inputs with timestamps to clipboard | Line 27 |
| `-cmd` | `[macro commands]` | Testing / Scripting | Executes macro/waymark command strings immediately | Line 29 |

---

## Detailed Command Specifications

### System & Hack Controls

#### `-d`
- **Description**: Immediately disables all active hacks and memory modifications.
- **Syntax**: `-d`
- **Arguments**: None.
- **Example**:
  ```text
  -d
  ```

#### `-e`
- **Description**: Re-enables all hacks.
- **Syntax**: `-e`
- **Arguments**: None.
- **Important Note**: This command must only be used after `-d` has been used to disable hacks. Using it without first disabling hacks has no effect.
- **Example**:
  ```text
  -e
  ```

#### `-settings`
- **Description**: Launches and opens the active `config.txt` file in the system default text editor (e.g., Notepad).
- **Syntax**: `-settings`
- **Arguments**: None.
- **Example**:
  ```text
  -settings
  ```

#### `-cf [filename.txt]`
- **Description**: Instructs Speeder to switch its configuration and read settings from the specified text file instead of the default `config.txt`.
- **Syntax**: `-cf [filename.txt]`
- **Arguments**:
  - `filename.txt`: The name or relative path of the configuration file to load.
- **Example**:
  ```text
  -cf farming_config.txt
  ```

---

### Visual Overlays

#### `-esp`
- **Description**: Toggles the ESP (Extra Sensory Perception) overlay window, opening it if closed or closing it if already open.
- **Syntax**: `-esp`
- **Arguments**: None.
- **Example**:
  ```text
  -esp
  ```

#### `-rad`
- **Description**: Toggles the Radar overlay window, opening it if closed or closing it if already open.
- **Syntax**: `-rad`
- **Arguments**: None.
- **Example**:
  ```text
  -rad
  ```

---

### Memory & Entity Inspection

#### `-allplayers`
- **Description**: Scans memory and outputs a complete list of all player entities currently loaded in memory. Outputs entity ID, character name, world coordinates `(X, Y, Z)`, distance from player, health points (HP), and status flags.
- **Syntax**: `-allplayers`
- **Arguments**: None.
- **Use Case**: Determining enemy and party member distance, checking player names for target locking, or verifying max HP values for `tphp` and `cpmhp` macro commands.
- **Example**:
  ```text
  -allplayers
  ```

#### `-allgather`
- **Description**: Scans memory and prints details for all harvestable gathering nodes and loot objects currently in memory. Displays object type name (e.g., `WOOD`, `ROCK`, `FIBER`, `HIDE`, `ORE`), tier (T1–T8), rarity enchantment (0–4), distance, and whether the resource node is active or depleted.
- **Syntax**: `-allgather`
- **Arguments**: None.
- **Use Case**: Finding exact node type strings for `tcg % [name],[dist]` and `cng % [name],[count],[dist]` commands.
- **Example**:
  ```text
  -allgather
  ```

#### `-allmobs`
- **Description**: Scans memory and lists all mobs/NPCs currently spawned in range. Displays mob ID, internal name string (e.g., `T2_MOB_HIDE_SNAKE`, `T5_MOB_UNDEAD_GHOUL`), coordinates, distance, and current HP.
- **Syntax**: `-allmobs`
- **Arguments**: None.
- **Use Case**: Discovering exact mob names to plug into `tcm % [name],[dist]` and `cnm % [name],[count],[dist]` targeting filters.
- **Example**:
  ```text
  -allmobs
  ```

#### `-closeg`
- **Description**: Identifies the single closest active (non-depleted) gathering resource object to your character and displays its full properties.
- **Syntax**: `-closeg`
- **Arguments**: None.
- **Example**:
  ```text
  -closeg
  ```

#### `-displaycd`
- **Description**: Enters cooldown detection mode. When you cast any ability or spell in game, its internal Cooldown ID will be printed to the console window after approximately a 2-second delay.
- **Syntax**: `-displaycd`
- **Arguments**: None.
- **Use Case**: Discovering spell/skill cooldown IDs for use in macro rotation checks: `cc[cooldown ID]` or `cc[cooldown ID],[timer]`.
- **Workflow**:
  1. Type `-displaycd` in Speeder console.
  2. Switch to game window and press your skill (e.g., Q ability).
  3. Wait ~2 seconds; read the integer ID printed in the console (e.g., `2033222`).
  4. Use that ID in your macro: `keys=cc2033222|81|dbg % Q ability ready`.

---

### Coordinates, Cursor & Screen Commands

#### `-cl`
- **Description**: Displays the player character's current position in `X, Z` coordinates and automatically copies the result to your Windows clipboard.
- **Syntax**: `-cl`
- **Arguments**: None.
- **Note on Coordinates**: The `Y` coordinate (elevation/height) is ignored in Albion Online pathing calculations. Only `X` and `Z` are required for waymark navigation and distance checks.
- **Integration with Line 27**: When `-recordkeys` is active, executing `-cl` also attaches the recorded key string to the next recorded waymark.
- **Example**:
  ```text
  -cl
  ```

#### `-mp`
- **Description**: Displays the current cursor position in screen `X, Y` pixels.
- **Syntax**: `-mp`
- **Arguments**: None.
- **Global Variables Populated**:
  - `CX`: Stores current cursor X coordinate.
  - `CY`: Stores current cursor Y coordinate.
- **Use Case**: Finding exact pixel coordinates for UI clicking (`m[x],[y]`) or initializing cursor memory before running targeting macros.
- **Example**:
  ```text
  -mp
  ```

#### `-gpc`
- **Description**: Retrieves the color (Hex code and RGB values) of the pixel currently located under the mouse cursor and copies the value to the Windows clipboard.
- **Syntax**: `-gpc`
- **Arguments**: None.
- **Config Dependency**: Line 33 of `config.txt` defines the screen DPI/scaling multiplier. If your display scaling is 125% or 150%, ensure Line 33 is configured properly for accurate pixel sampling.
- **Example**:
  ```text
  -gpc
  ```

---

### Waymark Navigation & Execution

#### `-twrepeat* [waymarkfilename.ini]`
- **Description**: Loads the specified `.ini` waymark file and executes the waypoints sequentially starting from index `[0]`. When the final waymark in the file is completed, it automatically loops back to `[0]` and continues indefinitely.
- **Syntax**: `-twrepeat* [waymarkfilename.ini]`
- **Arguments**:
  - `waymarkfilename.ini`: The `.ini` file containing sequential waypoints `[0]`, `[1]`, `[2]`, etc.
- **Emergency Stop / Pause**: Make sure a Virtual Key code is assigned to **Line 22** in `config.txt`. Pressing this key pauses or resumes the waymark runner immediately.
- **Example**:
  ```text
  -twrepeat* SteppeRoute.ini
  ```

#### `-tw* [waymarkfilename.ini]`
- **Description**: Loads the specified `.ini` waymark file and walks the waypoints sequentially from index `[0]` to the last index. **Unlike `-twrepeat*`, execution stops upon reaching the final waymark and does NOT loop.**
- **Syntax**: `-tw* [waymarkfilename.ini]`
- **Arguments**:
  - `waymarkfilename.ini`: The route file to execute.
- **Pause Key**: Controlled by **Line 22** of `config.txt`.
- **Example**:
  ```text
  -tw* SwampToCity.ini
  ```

#### `-twresume`
- **Description**: Resumes waymark execution from the next waymark index after navigation was paused or interrupted.
- **Syntax**: `-twresume`
- **Arguments**: None.
- **Note**: Line 22 in `config.txt` can also be toggled in-game to achieve the same result without switching to the console.
- **Example**:
  ```text
  -twresume
  ```

---

### Route & Key Recording Commands

#### `-record` & `-record [distance],[wait_time]`
- **Description**: Starts recording player movement waypoints in real-time as your character walks through the game world. Entering `-record` a second time stops recording and compiles all points into `Recorded Waymarks.ini`.
- **Syntax Modes**:
  1. **Interactive Mode**:
     ```text
     -record
     ```
     Prompts for distance threshold and wait time.
  2. **Bypass Mode (Direct Arguments)**:
     ```text
     -record [distance],[wait_time]
     ```
     Bypasses interactive questions by providing comma-separated values directly.
- **Arguments**:
  - `distance`: Distance in game units traveled before a new waypoint is automatically generated.
  - `wait_time`: Pause duration (in milliseconds) at each waypoint before proceeding to the next.
- **Manual Recording via Line 27 Hotkey**:
  - If you configure a key code on **Line 27** in `config.txt` (e.g. `113` for F2), pressing that key manually captures a waypoint at your current coordinates.
  - **Pro-Tip (Manual Route Crafting)**:
    Set `distance` to `9999` using bypass syntax:
    ```text
    -record 9999,30
    ```
    With distance set to 9999, Speeder will never automatically record unwanted intermediate points. You can walk to exact strategic spots and press your Line 27 key to record only deliberate waypoints. Type `-record` again when finished to save.

#### `-recordkeys` & `-recordkeys [stop_key],[mouse_key],[static_delay]`
- **Description**: Captures all keyboard and mouse actions performed while the game client has focus, distinguishing between key press (`down`) and key release (`up`) events and calculating precise sleep times between each action.
- **Syntax Modes**:
  1. **Interactive Mode**:
     ```text
     -recordkeys
     ```
     Prompts for 3 parameters:
     - **key to stop**: Virtual Key code of the hotkey used to stop recording. Pressing this hotkey immediately copies the formatted command string (e.g. `81d|s120|81u|...`) to your Windows clipboard for pasting into `.ini` files. (Not needed if used concurrently with `-record`).
     - **mouse key**: Virtual Key code of the key you press to record current mouse coordinates (`m[x],[y]`).
     - **static delay**: Milliseconds delay to apply uniformly between events. If set to `0`, dynamic real-time intervals are recorded.
  2. **Bypass Mode (Direct Arguments)**:
     ```text
     -recordkeys [stop_key],[mouse_key],[static_delay]
     ```
     Supplies all three parameters directly without interactive prompts.
- **Example (Bypass)**:
  ```text
  -recordkeys 113,114,100
  ```
  *(Sets Stop Key to F2 [113], Mouse Sample Key to F3 [114], and Static Delay to 100ms).*
- **Dual Recording Workflow (`-record` + `-recordkeys`)**:
  You can run `-record` and `-recordkeys` simultaneously:
  1. Start movement recording: `-record 9999,30`.
  2. Start key recording: `-recordkeys 113,114,100`.
  3. Perform your gathering or combat sequence at the spot.
  4. Press the **Line 27** hotkey (or run `-cl`).
  5. Speeder automatically attaches all recorded keys directly into the `keys=` property of that newly recorded waymark.

---

### Command Execution & Macro Testing

#### `-cmd [macro/waymark commands]`
- **Description**: Directly executes any macro command string as if triggered from an active macro hotkey. Ideal for testing complex command chains, verifying targeting syntax, or setting global variables manually.
- **Syntax**: `-cmd [command1]|[command2]|...`
- **Arguments**: Any valid command string separated by `|`.
- **Examples**:
  - Test variable storage, equality comparison, and debug console output:
    ```text
    -cmd store % testvar,1|eq % testvar,1|dbg % Variable verified!
    ```
  - Test cursor placement:
    ```text
    -cmd m500,500|dbg % Mouse moved to center
    ```
  - Test target scanning:
    ```text
    -cmd tcm*15,0,1|dbg % Targeted closest mob within 15 units
    ```

---

## Config.txt Key Dependencies Summary

| Config Line | Setting Description | Interactions with Console Commands |
|---|---|---|
| **Line 22** | Waymarks Pause / Resume Hotkey | Pauses, halts, or resumes `-twrepeat*`, `-tw*`, and `-twresume` routes |
| **Line 24** | Sleep Randomization / Jitter | Modifies static wait timers in commands such as `s` and waypoint `wait time` |
| **Line 26** | Gathering Cursor Distance Offset | Sets step distance used by `tcg` when nudging cursor toward nodes |
| **Line 27** | Record Coordinates Hotkey | Manually logs current position during `-record` and dumps `-recordkeys` |
| **Line 29** | Macro File Name | Specifies the `.ini` macro file containing active key bindings |
| **Line 31** | Mouse Smoothing Factor | Controls cursor movement interpolation in `m[x],[y]`, `grid`, and `circle` |
| **Line 33** | DPI / Scaling Multiplier | Adjusts pixel coordinate calculation for `-gpc` and screen analysis |
| **Line 47** | Camera Projection (FOV, Yaw, Pitch) | Mandatory for 3D world-to-screen targeting (`tcp`, `tcm`, `tcg`, `aim`, `tphp`) |
