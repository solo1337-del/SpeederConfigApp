using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeederConfigApp.Models
{
    /// <summary>
    /// Represents a Speeder console command reference item.
    /// </summary>
    public class ConsoleCommandItem
    {
        public string Command { get; set; } = string.Empty;
        public string Arguments { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RelatedConfigLine { get; set; } = string.Empty;
        public string Syntax { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public string DisplayText => string.IsNullOrWhiteSpace(Arguments) ? Command : $"{Command} {Arguments}";

        public override string ToString() => DisplayText;
    }

    /// <summary>
    /// Represents a Speeder macro/waymark scripting syntax command reference item.
    /// </summary>
    public class MacroSyntaxItem
    {
        public string Command { get; set; } = string.Empty;
        public string Syntax { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public string DisplayText => $"{Command} - {Description}";

        public override string ToString() => DisplayText;
    }

    /// <summary>
    /// Static repository providing full reference seed data for Console Commands, Macro Syntax, and Virtual Keys.
    /// </summary>
    public static class ReferenceRepository
    {
        private static readonly List<ConsoleCommandItem> _consoleCommands = new List<ConsoleCommandItem>
        {
            new ConsoleCommandItem
            {
                Command = "-d",
                Arguments = "None",
                Category = "Hacks",
                Description = "Immediately disables all active hacks and memory modifications globally.",
                RelatedConfigLine = "—",
                Syntax = "-d",
                Example = "-d",
                Notes = "Use for emergency stop or safe zone transitions."
            },
            new ConsoleCommandItem
            {
                Command = "-e",
                Arguments = "None",
                Category = "Hacks",
                Description = "Re-enables all hacks globally (only valid after -d was used).",
                RelatedConfigLine = "—",
                Syntax = "-e",
                Example = "-e",
                Notes = "Must only be used after -d has disabled hacks; no effect otherwise."
            },
            new ConsoleCommandItem
            {
                Command = "-allplayers",
                Arguments = "None",
                Category = "Memory / Query",
                Description = "Scans memory and dumps coordinates (X, Y, Z), HP, distance, and status flags for all players in memory.",
                RelatedConfigLine = "—",
                Syntax = "-allplayers",
                Example = "-allplayers",
                Notes = "Useful for enemy/party distance analysis and target locking verification."
            },
            new ConsoleCommandItem
            {
                Command = "-allgather",
                Arguments = "None",
                Category = "Memory / Query",
                Description = "Scans memory and dumps all active gathering nodes and loot objects (type, tier, rarity, distance, active/depleted).",
                RelatedConfigLine = "—",
                Syntax = "-allgather",
                Example = "-allgather",
                Notes = "Useful to discover exact node names for tcg % and cng % commands."
            },
            new ConsoleCommandItem
            {
                Command = "-allmobs",
                Arguments = "None",
                Category = "Memory / Query",
                Description = "Scans memory and lists all mobs/NPCs in range (ID, internal name, coordinates, distance, current HP).",
                RelatedConfigLine = "—",
                Syntax = "-allmobs",
                Example = "-allmobs",
                Notes = "Useful to discover exact mob names for tcm % and cnm % commands."
            },
            new ConsoleCommandItem
            {
                Command = "-closeg",
                Arguments = "None",
                Category = "Memory / Query",
                Description = "Displays detailed information for the single closest active gathering object.",
                RelatedConfigLine = "—",
                Syntax = "-closeg",
                Example = "-closeg",
                Notes = "Quick inspection of closest harvestable resource."
            },
            new ConsoleCommandItem
            {
                Command = "-displaycd",
                Arguments = "None",
                Category = "Combat / Debug",
                Description = "Enters cooldown detection mode; prints cooldown IDs in console ~2 seconds after skill use.",
                RelatedConfigLine = "Line 29 (cc command)",
                Syntax = "-displaycd",
                Example = "-displaycd",
                Notes = "Use discovered integer ID in macro rotations via cc[ID] commands."
            },
            new ConsoleCommandItem
            {
                Command = "-cl",
                Arguments = "None",
                Category = "Coordinates",
                Description = "Displays player position (X, Z) and copies to Windows clipboard.",
                RelatedConfigLine = "Line 27 (Coordinates Hotkey)",
                Syntax = "-cl",
                Example = "-cl",
                Notes = "Y coordinate (elevation) is ignored in Albion Online pathing."
            },
            new ConsoleCommandItem
            {
                Command = "-mp",
                Arguments = "None",
                Category = "Cursor / Mouse",
                Description = "Displays current mouse cursor position (X, Y) in pixels and stores into global variables CX and CY.",
                RelatedConfigLine = "—",
                Syntax = "-mp",
                Example = "-mp",
                Notes = "Finds exact pixel coordinates for UI clicking (m[x],[y]) and targeting."
            },
            new ConsoleCommandItem
            {
                Command = "-gpc",
                Arguments = "None",
                Category = "Screen / Color",
                Description = "Prints pixel color (Hex and RGB) under mouse cursor and copies value to clipboard.",
                RelatedConfigLine = "Line 33 (DPI multiplier)",
                Syntax = "-gpc",
                Example = "-gpc",
                Notes = "Line 33 DPI scaling multiplier affects pixel sampling accuracy."
            },
            new ConsoleCommandItem
            {
                Command = "-settings",
                Arguments = "None",
                Category = "Config",
                Description = "Launches and opens active config.txt in system default text editor.",
                RelatedConfigLine = "—",
                Syntax = "-settings",
                Example = "-settings",
                Notes = "Opens config in Notepad or default editor."
            },
            new ConsoleCommandItem
            {
                Command = "-cf",
                Arguments = "[filename.txt]",
                Category = "Config",
                Description = "Switches Speeder configuration to read settings from the specified text file instead of default config.txt.",
                RelatedConfigLine = "—",
                Syntax = "-cf [filename.txt]",
                Example = "-cf farming_config.txt",
                Notes = "Hot-swap configuration profiles on the fly."
            },
            new ConsoleCommandItem
            {
                Command = "-esp",
                Arguments = "None",
                Category = "Visuals",
                Description = "Toggles (opens or closes) the ESP overlay window.",
                RelatedConfigLine = "Lines 46, 47, 48",
                Syntax = "-esp",
                Example = "-esp",
                Notes = "Overlay settings configured in Lines 46-48."
            },
            new ConsoleCommandItem
            {
                Command = "-rad",
                Arguments = "None",
                Category = "Visuals",
                Description = "Toggles (opens or closes) the Radar overlay window.",
                RelatedConfigLine = "Lines 43, 48",
                Syntax = "-rad",
                Example = "-rad",
                Notes = "Radar window dimensions and colors configured in Line 43."
            },
            new ConsoleCommandItem
            {
                Command = "-twrepeat*",
                Arguments = "[file.ini]",
                Category = "Waymarks",
                Description = "Loads specified .ini waymark file and loops route continuously starting at index [0].",
                RelatedConfigLine = "Line 22 (Pause key)",
                Syntax = "-twrepeat* [waymarkfilename.ini]",
                Example = "-twrepeat* SteppeRoute.ini",
                Notes = "Pausing and resuming handled by Line 22 hotkey."
            },
            new ConsoleCommandItem
            {
                Command = "-tw*",
                Arguments = "[file.ini]",
                Category = "Waymarks",
                Description = "Executes waymark file once sequentially from index [0] to the last index (does not loop).",
                RelatedConfigLine = "Line 22 (Pause key)",
                Syntax = "-tw* [waymarkfilename.ini]",
                Example = "-tw* SwampToCity.ini",
                Notes = "Stops automatically when reaching the final waypoint."
            },
            new ConsoleCommandItem
            {
                Command = "-twresume",
                Arguments = "None",
                Category = "Waymarks",
                Description = "Resumes -twrepeat* loop from the next waymark index after navigation was paused.",
                RelatedConfigLine = "Line 22 (Pause key)",
                Syntax = "-twresume",
                Example = "-twresume",
                Notes = "Line 22 hotkey in-game provides the same function."
            },
            new ConsoleCommandItem
            {
                Command = "-record",
                Arguments = "[dist],[wait] (optional)",
                Category = "Recording",
                Description = "Starts/stops real-time recording of player movement path into Recorded Waymarks.ini.",
                RelatedConfigLine = "Line 27 (Record hotkey)",
                Syntax = "-record or -record [distance],[wait_time]",
                Example = "-record 9999,30",
                Notes = "Setting distance to 9999 allows deliberate manual waypoint creation using Line 27 key."
            },
            new ConsoleCommandItem
            {
                Command = "-recordkeys",
                Arguments = "[stop],[mouse],[delay] (optional)",
                Category = "Recording",
                Description = "Records keyboard and mouse inputs with timestamps to Windows clipboard.",
                RelatedConfigLine = "Line 27 (Record hotkey)",
                Syntax = "-recordkeys or -recordkeys [stop_key],[mouse_key],[static_delay]",
                Example = "-recordkeys 113,114,100",
                Notes = "Dual recording workflow combines -record and -recordkeys simultaneously."
            },
            new ConsoleCommandItem
            {
                Command = "-cmd",
                Arguments = "[macro commands]",
                Category = "Testing / Scripting",
                Description = "Executes macro/waymark command strings immediately as if triggered by a macro hotkey.",
                RelatedConfigLine = "Line 29 (Macro File Name)",
                Syntax = "-cmd [command1]|[command2]|...",
                Example = "-cmd store % testvar,1|eq % testvar,1|dbg % Variable verified!",
                Notes = "Ideal for testing targeting syntax, variables, and math formulas directly."
            }
        };

        private static readonly List<MacroSyntaxItem> _macroCommands = new List<MacroSyntaxItem>
        {
            // Section & Control
            new MacroSyntaxItem
            {
                Command = "keys",
                Syntax = "keys=[cmd]|[cmd]...",
                Category = "Macro Structure",
                Description = "Primary command line executed when macro is triggered.",
                Example = "keys=1d|s100|1u",
                Notes = "Additional lines are named keys2, keys3, etc."
            },
            new MacroSyntaxItem
            {
                Command = "endkeys",
                Syntax = "endkeys=[cmd]|[cmd]...",
                Category = "Macro Structure",
                Description = "Cleanup commands executed when macro stops or trigger key is released.",
                Example = "endkeys=lt-|2u",
                Notes = "Essential for releasing sticky target lock (lt-) or un-pressing mouse buttons."
            },
            new MacroSyntaxItem
            {
                Command = "repeat",
                Syntax = "repeat=0|1|2",
                Category = "Macro Structure",
                Description = "Repetition mode: 0=execute once, 1=repeat while held, 2=continuous toggle loop.",
                Example = "repeat=2",
                Notes = "Repeat mode 2 loops continuously until the trigger key is pressed again."
            },
            new MacroSyntaxItem
            {
                Command = "interrupt",
                Syntax = "interrupt=0|1",
                Category = "Macro Structure",
                Description = "Preemption behavior: 1 allows other macros to interrupt; 0 forces completion before another starts.",
                Example = "interrupt=0",
                Notes = "Optional property on macro items."
            },

            // Keyboard & Mouse
            new MacroSyntaxItem
            {
                Command = "Key Tap",
                Syntax = "[key]",
                Category = "Keyboard & Mouse",
                Description = "Presses and immediately releases the specified Virtual Key code.",
                Example = "81",
                Notes = "81 taps the Q key."
            },
            new MacroSyntaxItem
            {
                Command = "Key Down",
                Syntax = "[key]d",
                Category = "Keyboard & Mouse",
                Description = "Holds virtual key down. Does not repeat if already down.",
                Example = "160d",
                Notes = "160 holds Left Shift."
            },
            new MacroSyntaxItem
            {
                Command = "Force Key Down",
                Syntax = "[key]d*",
                Category = "Keyboard & Mouse",
                Description = "Forces a key down event even if registered as already pressed.",
                Example = "160d*",
                Notes = "Useful after desyncs or transitions."
            },
            new MacroSyntaxItem
            {
                Command = "Key Up",
                Syntax = "[key]u",
                Category = "Keyboard & Mouse",
                Description = "Releases virtual key.",
                Example = "160u",
                Notes = "160 releases Left Shift."
            },
            new MacroSyntaxItem
            {
                Command = "Force Key Up",
                Syntax = "[key]u*",
                Category = "Keyboard & Mouse",
                Description = "Forces a key release event even if registered as up.",
                Example = "160u*",
                Notes = "Guarantees key release."
            },
            new MacroSyntaxItem
            {
                Command = "m",
                Syntax = "m[x],[y]",
                Category = "Keyboard & Mouse",
                Description = "Moves mouse cursor to screen pixel coordinate (x, y). Smoothing controlled by Line 31.",
                Example = "m960,540",
                Notes = "Accepts variables like m(VAR % CX),(VAR % CY)."
            },
            new MacroSyntaxItem
            {
                Command = "kd",
                Syntax = "kd[key]",
                Category = "Keyboard & Mouse",
                Description = "Checks if virtual key is down. Continues line if pressed; halts line if not.",
                Example = "kd81|dbg % Q is pressed",
                Notes = "Conditional key press guard."
            },
            new MacroSyntaxItem
            {
                Command = "!kd",
                Syntax = "!kd[key]",
                Category = "Keyboard & Mouse",
                Description = "Inverted check: continues line only if key is UP.",
                Example = "!kd81|81",
                Notes = "Safe tap if not currently pressed."
            },
            new MacroSyntaxItem
            {
                Command = "kd*",
                Syntax = "!kd*[key] or kd*[key]",
                Category = "Keyboard & Mouse",
                Description = "Checks physical hardware key status, ignoring simulated/synthetic state.",
                Example = "kd*160|dbg % Shift physically held",
                Notes = "Direct hardware polling."
            },
            new MacroSyntaxItem
            {
                Command = "c",
                Syntax = "c[rad]",
                Category = "Keyboard & Mouse",
                Description = "Rotates cursor around character in radians relative to camera (0=N, 1.57=E, 3.14=S, 4.71=W).",
                Example = "c1.57|81",
                Notes = "Useful for directional skill-shots."
            },

            // Timers & Delays
            new MacroSyntaxItem
            {
                Command = "s",
                Syntax = "s[ms]",
                Category = "Timers & Delays",
                Description = "Pauses execution for specified milliseconds. Adds randomized jitter if Line 24 is configured.",
                Example = "s300",
                Notes = "Basic sleep delay."
            },
            new MacroSyntaxItem
            {
                Command = "rs",
                Syntax = "rs[min],[max]",
                Category = "Timers & Delays",
                Description = "Pauses execution for a randomized duration between min and max milliseconds.",
                Example = "rs150,350",
                Notes = "Anti-detection humanizer delay."
            },

            // Targeting
            new MacroSyntaxItem
            {
                Command = "cm",
                Syntax = "cm",
                Category = "Targeting",
                Description = "Checks if a mob is currently targeted. Continues if mob is targeted; halts line if not.",
                Example = "cm|81",
                Notes = "Requires active mob target."
            },
            new MacroSyntaxItem
            {
                Command = "!cm",
                Syntax = "!cm",
                Category = "Targeting",
                Description = "Inverted check: continues if NO mob is targeted. Halts line if a mob is targeted.",
                Example = "!cm|gt8",
                Notes = "Useful for aborting rotation if mob is defeated."
            },
            new MacroSyntaxItem
            {
                Command = "ct",
                Syntax = "ct[type]",
                Category = "Targeting",
                Description = "Checks if cursor is hovering over target type (2=gathering/loot, 3=mob).",
                Example = "ct2|1d|s100|1u",
                Notes = "Hover confirmation before clicking."
            },
            new MacroSyntaxItem
            {
                Command = "!ct",
                Syntax = "!ct[type]",
                Category = "Targeting",
                Description = "Inverted hover check: continues only if cursor is NOT hovering over target type.",
                Example = "!ct3|dbg % not hovering mob",
                Notes = "Negative hover check."
            },
            new MacroSyntaxItem
            {
                Command = "it",
                Syntax = "it",
                Category = "Targeting",
                Description = "Checks if player currently has any active selected target.",
                Example = "it|tt*",
                Notes = "Target presence check."
            },
            new MacroSyntaxItem
            {
                Command = "!it",
                Syntax = "!it",
                Category = "Targeting",
                Description = "Inverted check: continues only if player does NOT have a selected target.",
                Example = "!it|tcp*50",
                Notes = "Target absence fallback."
            },
            new MacroSyntaxItem
            {
                Command = "cg",
                Syntax = "cg",
                Category = "Targeting",
                Description = "Continues only if gathering object under cursor is active and not depleted.",
                Example = "cg|1",
                Notes = "Ensures node is harvestable before interacting."
            },
            new MacroSyntaxItem
            {
                Command = "tcg",
                Syntax = "tcg[dist],[tier],[rarity]",
                Category = "Targeting",
                Description = "Nudges cursor toward closest active gathering node within dist. Step distance set by Line 26.",
                Example = "tcg10,3,1",
                Notes = "Smooth node tracking."
            },
            new MacroSyntaxItem
            {
                Command = "tcg*",
                Syntax = "tcg*[dist],[tier],[rarity]",
                Category = "Targeting",
                Description = "Directly snaps cursor onto closest active gathering node meeting tier and rarity criteria.",
                Example = "tcg*15,4,2",
                Notes = "Instant node targeting."
            },
            new MacroSyntaxItem
            {
                Command = "tcg %",
                Syntax = "tcg % [name],[dist],[tier],[rarity]",
                Category = "Targeting",
                Description = "Snaps cursor to node whose internal name matches name (e.g., WOOD, ROCK, FIBER).",
                Example = "tcg % ROCK,20",
                Notes = "Filter gathering by resource type."
            },
            new MacroSyntaxItem
            {
                Command = "tcp*",
                Syntax = "tcp*[dist],0,[lock],[ally]",
                Category = "Targeting",
                Description = "Snaps cursor to closest non-allied player. lock=1 locks cursor until lt- or death. ally=1 targets party.",
                Example = "tcp*50,0,1",
                Notes = "Sticky player target locking."
            },
            new MacroSyntaxItem
            {
                Command = "tcp",
                Syntax = "tcp[dist],[cursor_dist],0,[ally]",
                Category = "Targeting",
                Description = "Moves cursor toward closest player by cursor_dist pixels.",
                Example = "tcp50,5",
                Notes = "Nudges mouse toward target player."
            },
            new MacroSyntaxItem
            {
                Command = "tcpC",
                Syntax = "tcpC[dist],0,[lock],[ally]",
                Category = "Targeting",
                Description = "Targets player closest to current cursor position.",
                Example = "tcpC30,0,1",
                Notes = "Cursor-relative player target."
            },
            new MacroSyntaxItem
            {
                Command = "tcm*",
                Syntax = "tcm*[dist],0,[lock]",
                Category = "Targeting",
                Description = "Snaps cursor to closest mob within dist. lock=1 locks cursor until mob dies or lt- is called.",
                Example = "tcm*30,0,1",
                Notes = "Sticky mob target locking."
            },
            new MacroSyntaxItem
            {
                Command = "tcm",
                Syntax = "tcm[dist],[cursor_dist]",
                Category = "Targeting",
                Description = "Nudges cursor toward closest mob by cursor_dist pixels.",
                Example = "tcm30,5",
                Notes = "Gradual mouse movement toward mob."
            },
            new MacroSyntaxItem
            {
                Command = "tcmC",
                Syntax = "tcmC[dist],0,[lock]",
                Category = "Targeting",
                Description = "Targets mob closest to current cursor position.",
                Example = "tcmC25,0,1",
                Notes = "Cursor-relative mob target."
            },
            new MacroSyntaxItem
            {
                Command = "tcm %",
                Syntax = "tcm % [name],[dist] or tcm % [name],[dist],[x],[z]",
                Category = "Targeting",
                Description = "Snaps cursor to mob whose name contains name within dist (optional world coordinates x, z).",
                Example = "tcm % HIDE_SNAKE,15",
                Notes = "Target mob by specific internal name."
            },
            new MacroSyntaxItem
            {
                Command = "tphp",
                Syntax = "tphp[dist],[max_hp]",
                Category = "Targeting",
                Description = "Snaps cursor to lowest HP enemy player under max_hp within dist.",
                Example = "tphp25,1500",
                Notes = "Automatic execute targeting for PvP."
            },
            new MacroSyntaxItem
            {
                Command = "tphpP",
                Syntax = "tphpP[dist],[max_hp]",
                Category = "Targeting",
                Description = "Snaps cursor to lowest HP party member under max_hp within dist.",
                Example = "tphpP30,2000",
                Notes = "Priority ally heal target."
            },
            new MacroSyntaxItem
            {
                Command = "aim",
                Syntax = "aim[x],[y],[z]",
                Category = "Targeting",
                Description = "Projects 3D world coordinates (x, y, z) to screen pixels and moves mouse cursor there.",
                Example = "aim-377,0,-16",
                Notes = "Requires calibrated camera FOV/Pitch/Yaw in Line 47."
            },
            new MacroSyntaxItem
            {
                Command = "tt",
                Syntax = "tt[dist]",
                Category = "Targeting",
                Description = "Moves cursor toward currently selected target by dist pixels.",
                Example = "tt10",
                Notes = "Nudges cursor toward selected target."
            },
            new MacroSyntaxItem
            {
                Command = "tt*",
                Syntax = "tt*",
                Category = "Targeting",
                Description = "Snaps cursor directly onto currently selected target center.",
                Example = "tt*",
                Notes = "Direct snap to target."
            },
            new MacroSyntaxItem
            {
                Command = "lt-",
                Syntax = "lt-",
                Category = "Targeting",
                Description = "Releases sticky cursor lock initiated by lock=1 in tcp* or tcm*.",
                Example = "endkeys=lt-",
                Notes = "Place in endkeys or cleanup routines."
            },
            new MacroSyntaxItem
            {
                Command = "grid",
                Syntax = "grid[area],[precision],[delay],[type]",
                Category = "Targeting / Search",
                Description = "Searches screen in random grid pattern centered on window for target type (2=node, 3=mob).",
                Example = "grid2000,20,100,3",
                Notes = "Window center grid scan."
            },
            new MacroSyntaxItem
            {
                Command = "circle",
                Syntax = "circle[rad],[prec],[delay],[type]",
                Category = "Targeting / Search",
                Description = "Sweeps cursor in a circle around screen center searching for target type.",
                Example = "circle100,0.2,100,2",
                Notes = "Center radial sweep."
            },

            // Entity & Environment Query
            new MacroSyntaxItem
            {
                Command = "cng",
                Syntax = "cng[count],[dist],[tier],[rar]",
                Category = "Query Commands",
                Description = "Continues if at least count gathering nodes exist within dist matching tier and rarity.",
                Example = "cng2,10,3,2",
                Notes = "Node count condition."
            },
            new MacroSyntaxItem
            {
                Command = "cnp",
                Syntax = "cnp[count],[dist],[ally]",
                Category = "Query Commands",
                Description = "Continues if at least count players are within dist. ally=1 checks party members.",
                Example = "cnp1,15",
                Notes = "Player proximity check."
            },
            new MacroSyntaxItem
            {
                Command = "cnm",
                Syntax = "cnm[count],[dist] or cnm[count],[dist],[x],[z]",
                Category = "Query Commands",
                Description = "Continues if at least count mobs are within dist (optional world coordinates x, z).",
                Example = "cnm1,12",
                Notes = "Mob count condition."
            },
            new MacroSyntaxItem
            {
                Command = "cphp",
                Syntax = "cphp[pct],[dist]",
                Category = "Query Commands",
                Description = "Continues if any player within dist has HP fraction <= pct (0.5 = 50%).",
                Example = "cphp0.5,20",
                Notes = "Nearby player health percentage check."
            },

            // Player Status & Cooldowns
            new MacroSyntaxItem
            {
                Command = "hp",
                Syntax = "hp[pct] or !hp[pct]",
                Category = "Player Status",
                Description = "Continues if player HP is <= pct (0.0 to 1.0). !hp checks if greater than pct.",
                Example = "hp0.4|dbg % Low HP!",
                Notes = "Defensive skill or potion trigger."
            },
            new MacroSyntaxItem
            {
                Command = "mp",
                Syntax = "mp[pct] or !mp[pct]",
                Category = "Player Status",
                Description = "Continues if player Mana is <= pct (0.0 to 1.0). !mp checks if greater than pct.",
                Example = "!mp0.2|81",
                Notes = "Prevents casting if mana is exhausted."
            },
            new MacroSyntaxItem
            {
                Command = "cc",
                Syntax = "cc[id],[timer] or !cc[id]",
                Category = "Player Status",
                Description = "Continues if ability with ID is ready. timer is readiness fraction (0.95 = 95%). !cc checks on cooldown.",
                Example = "cc2033222|81",
                Notes = "Smart cooldown combat rotations."
            },

            // Variables, Math, Logic & Scripting
            new MacroSyntaxItem
            {
                Command = "gt",
                Syntax = "gt[#] or gt*",
                Category = "Scripting & Flow",
                Description = "Waymarks: jumps to waypoint index #. Macros: jumps to line keys# (gt1 jumps to keys, gt3 jumps to keys3).",
                Example = "gt1",
                Notes = "Branching and looping control."
            },
            new MacroSyntaxItem
            {
                Command = "store %",
                Syntax = "store % name,val",
                Category = "Variables & Logic",
                Description = "Stores val into variable name. Pipe characters in val must be escaped as '|.",
                Example = "store % count,0",
                Notes = "Variable definition and update."
            },
            new MacroSyntaxItem
            {
                Command = "eq %",
                Syntax = "eq % name,val or !eq % name,val",
                Category = "Variables & Logic",
                Description = "Continues line only if variable name equals val. !eq checks not equal.",
                Example = "eq % skip,false|81",
                Notes = "Conditional branch based on variable value."
            },
            new MacroSyntaxItem
            {
                Command = "cmp",
                Syntax = "cmp[v1],[v2] or !cmp[v1],[v2]",
                Category = "Variables & Logic",
                Description = "Continues if v1 < v2. !cmp checks if v1 >= v2.",
                Example = "cmp(VAR % hp),500|70",
                Notes = "Numerical comparison."
            },
            new MacroSyntaxItem
            {
                Command = "or %",
                Syntax = "or % c1'|c2'|...",
                Category = "Variables & Logic",
                Description = "Evaluates sub-commands sequentially; continues as soon as one is true. Delimiter is '|.",
                Example = "or % cnm1,5'|cnp1,5",
                Notes = "Short-circuit logical OR."
            },
            new MacroSyntaxItem
            {
                Command = "add %",
                Syntax = "add % name,val",
                Category = "Math & Variables",
                Description = "Calculates name = name + val.",
                Example = "add % counter,1",
                Notes = "Arithmetic addition."
            },
            new MacroSyntaxItem
            {
                Command = "sub %",
                Syntax = "sub % name,val",
                Category = "Math & Variables",
                Description = "Calculates name = name - val.",
                Example = "sub % counter,1",
                Notes = "Arithmetic subtraction."
            },
            new MacroSyntaxItem
            {
                Command = "mul %",
                Syntax = "mul % name,val",
                Category = "Math & Variables",
                Description = "Calculates name = name * val.",
                Example = "mul % speed,1.1",
                Notes = "Arithmetic multiplication."
            },
            new MacroSyntaxItem
            {
                Command = "div %",
                Syntax = "div % name,val",
                Category = "Math & Variables",
                Description = "Calculates name = name / val.",
                Example = "div % distance,2",
                Notes = "Arithmetic division."
            },
            new MacroSyntaxItem
            {
                Command = "rand",
                Syntax = "rand[min],[max]",
                Category = "Math & Variables",
                Description = "Generates random integer between min and max inclusive and stores into global variable RAND.",
                Example = "rand1,100",
                Notes = "Random number generator."
            },
            new MacroSyntaxItem
            {
                Command = "cc %",
                Syntax = "cc % [cmd]",
                Category = "Scripting & Flow",
                Description = "Executes any Speeder console command directly from within a macro or waypoint.",
                Example = "cc % -record 9999,30",
                Notes = "Console command automation hook."
            },
            new MacroSyntaxItem
            {
                Command = "call %",
                Syntax = "call % [func]",
                Category = "Scripting & Flow",
                Description = "Calls reusable function subroutine defined in [fName]. Executes all lines of function.",
                Example = "call % fCombat",
                Notes = "Modular function call."
            },
            new MacroSyntaxItem
            {
                Command = "dbg %",
                Syntax = "dbg % [text]",
                Category = "Debugging",
                Description = "Prints text to Speeder console window. Highly recommended for troubleshooting scripts.",
                Example = "dbg % Step 1 Complete",
                Notes = "Diagnostic logging."
            },
            new MacroSyntaxItem
            {
                Command = "nop",
                Syntax = "nop",
                Category = "Scripting & Flow",
                Description = "No Operation: dummy command that performs no action. Safe target for jump (gt) commands.",
                Example = "keys3=nop",
                Notes = "Null statement."
            }
        };

        public static IReadOnlyList<ConsoleCommandItem> GetAllConsoleCommands() => _consoleCommands;

        public static IReadOnlyList<MacroSyntaxItem> GetAllMacroSyntax() => _macroCommands;

        public static IReadOnlyList<VirtualKeyInfo> GetAllVirtualKeys() => VirtualKeyHelper.AllKeys;
    }

    /// <summary>
    /// Alias/holder for reference documentation items.
    /// </summary>
    public static class ReferenceDocItem
    {
        public static IReadOnlyList<ConsoleCommandItem> ConsoleCommands => ReferenceRepository.GetAllConsoleCommands();
        public static IReadOnlyList<MacroSyntaxItem> MacroSyntax => ReferenceRepository.GetAllMacroSyntax();
        public static IReadOnlyList<VirtualKeyInfo> VirtualKeys => ReferenceRepository.GetAllVirtualKeys();
    }
}
