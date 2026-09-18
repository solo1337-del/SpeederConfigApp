# Speeder Macros & Waymarks Complete Reference Guide

Speeder features a high-performance automation engine driven by standard `.ini` configuration files. This document details both the **Waymarks Engine** (path traversal, unstick failsafes, node gathering) and the **Macro Scripting Engine** (combat rotations, targeting primitives, math/logic variables, functions).

---

## 1. Waymarks File Format (`.ini`)

Waymark files store navigation routes, actions, and unstick routines. They are loaded using the `-tw*` (run once) or `-twrepeat*` (loop route) console commands.

### File Comment Rules
- Speeder ignores any line that does **not** contain a `[` or `=` character.
- Lines without `[` or `=` are treated as natural comments (no `#` or `;` prefixes required).

### Structure Overview

```ini
[variables]
initialState=true
gatherTries=0

[unstick]
distance=5
keys=27|s500|32
timer=5000
timer2=2000
giveup=5
script=escape_route.ini

[0]
x=84.66
y=-78.00
z=832.10
wait time=30
keys=2d|s3000|2u|eq % initialState,true|dbg % Starting route!|store % initialState,false

[1]
x=90.34
y=-60.00
z=783.21
wait time=1
keys=nop

[2]
x=0.1
y=0
z=0
wait time=2000
keys=tcg*15,4,1|s1000|1|s2000

[3]
x=105.12
y=-55.00
z=712.44
wait time=30
script=next_zone.ini|5
```

### Waymark Properties Reference

#### Coordinate Fields: `x`, `y`, `z`
- `x`, `z`: In-game horizontal plane coordinates. Albion uses `X` (East/West) and `Z` (North/South).
- `y`: Elevation/height coordinate. In Albion Online pathing, elevation is ignored by the pathfinder, but keeping it standard is recommended.
- **Stop-Movement Trick (`x=0.1`)**:
  Setting `x=0.1` instructs Speeder **not** to move the character. This creates an **action-only waypoint** (for gathering, buffing, casting, or inventory interaction).

#### Timing Field: `wait time`
- `wait time`: Duration in milliseconds to pause at the waypoint after arriving before processing commands or advancing to the next waypoint.
- **Seamless Running Trick (`wait time=1`)**:
  Setting `wait time=1` instructs Speeder **not** to release the movement key upon arrival. This produces continuous, fluid running between waypoints without deceleration stutters.
  > **Note**: Do *not* set `wait time=1` on waypoints where the character needs to stop and interact with objects or channels.

#### Command Execution Field: `keys`
- `keys`: A pipe-delimited (`|`) sequence of Virtual Key codes, macro commands, delays, and logic checks to execute at this waypoint. See the [Commands Reference](#3-commands-reference) below.

#### Script Branching & Chaining: `script`
- `script`: Can specify an external program executable or another waymark `.ini` file to load.
- **Loop Counter Syntax (`script=filename.ini|count`)**:
  Appends a loop threshold. Speeder will loop the current waymark file `count` times before switching to `filename.ini`.
  - Example: `script=nextinifile.ini|5` (switches to `nextinifile.ini` starting at index `[0]` after 5 complete cycles).

---

### Unstick Failsafe Routine (`[unstick]`)

The optional `[unstick]` section provides autonomous recovery if your character gets blocked by terrain, mobs, or obstacles.

| Setting | Type | Default | Description |
|---|---|---|---|
| `distance` | Decimal | `5` | Minimum distance threshold your character must cover to be considered "moving". |
| `timer` | Integer (ms) | `5000` | How long Speeder waits with no progress before triggering an unstick attempt. |
| `timer2` | Integer (ms) | `2000` | Delay after an unstick attempt before verifying progress. If still stuck after `timer2`, the `keys` sequence is triggered. If set to `0`, `keys` run immediately. |
| `keys` | String | `27` | Key codes / commands to execute to free the character (e.g. `27` for Esc, dashes, mounts, jump). |
| `giveup` | Integer | `5` | Number of consecutive failed unstick attempts before aborting. |
| `script` | String | None | Emergency fallback waymark file to load once `giveup` threshold is reached (e.g. `teleport_home.ini` or `recall.ini`). |

---

## 2. Macro File Format (`.ini`)

Macros are stored in the `.ini` file configured on **Line 29** of `config.txt`. They bind keys to automated sequences, combat rotations, and target locking.

### Section Header

Every macro begins with a section header matching the Virtual Key code:
- Triggered by Key: `[VK_CODE]` (e.g., `[113]` for F2, `[100]` for Numpad 4).
- Function Definition: `[fFunctionName]` (e.g., `[fCombatRotation]`, `[fDepositLoot]`).

### Macro Properties

```ini
[113]
keys=tcp*50,0,1|gt3
keys2=gt9
keys3=cc1|81|dbg % Q key|s300
keys4=cc2|87|dbg % W key|s300
endkeys=lt-
repeat=2
interrupt=0
```

| Property | Format / Values | Description |
|---|---|---|
| `keys` | `[cmd]\|[cmd]...` | First line of commands to execute when macro fires. |
| `keys2`, `keys3`, ... | `[cmd]\|[cmd]...` | Sequential command lines. Useful for multi-step branches or jumping via `gt[#]`. |
| `endkeys` | `[cmd]\|[cmd]...` | Cleanup commands executed when the macro finishes or the trigger key is released (e.g. `lt-` to unlock target, `2u` to release right mouse). |
| `repeat` | `0`, `1`, `2` | Execution repetition mode: <br>• `0` = Execute once on press.<br>• `1` = Repeat continuously while the trigger key is physically held down.<br>• `2` = Continuous toggle loop (press once to start, press again to stop). |
| `interrupt` | `0` or `1` | `1` allows other triggered macros to preempt this macro; `0` forces this macro to finish before another can begin. |

---

## 3. Commands Reference

Every command string inside `keys`, `keys2`, `endkeys`, or waypoint `keys` is a sequence of actions separated by the pipe character (`|`).

### 3.1 Keyboard & Mouse Input

| Command | Syntax | Description | Example |
|---|---|---|---|
| Key Down | `[key]d` | Holds virtual key down. Does not repeat if already down. | `160d` (Hold Left Shift) |
| Force Key Down | `[key]d*` | Forces a key down event even if Speeder thinks it's already pressed. | `160d*` |
| Key Up | `[key]u` | Releases virtual key. | `160u` (Release Left Shift) |
| Force Key Up | `[key]u*` | Forces a key release event even if thought to be up. | `160u*` |
| Key Tap | `[key]` | Presses and immediately releases a key. | `81` (Tap Q key) |
| Mouse Move | `m[x],[y]` | Moves mouse cursor to screen pixel `x, y`. Smoothing controlled by Line 31. | `m960,540` |
| Is Key Down | `kd[key]` | Checks if key is pressed. If true, continues line; if false, halts line. | `kd81\|dbg % Q is down` |
| Is Key Not Down | `!kd[key]` | Inverted check: continues line only if key is UP. | `!kd81\|81` |
| Physical Key Down | `kd*[key]` | Checks if the physical hardware key is held down (ignores synthetic state). | `kd*160\|dbg % Shift held` |
| Cursor Angle | `c[rad]` | Rotates cursor around character in radians relative to camera (`0` = N, `1.57` = E, `3.14` = S, `4.71` = W). | `c1.57\|81` (Cast Q to East) |

### 3.2 Timers & Delays

| Command | Syntax | Description | Example |
|---|---|---|---|
| Static Sleep | `s[ms]` | Pauses execution for `ms` milliseconds. Random jitter added if Line 24 is set. | `s300` (Sleep 300ms) |
| Random Sleep | `rs[min],[max]` | Pauses execution for a randomized duration between `min` and `max` milliseconds. Essential for anti-ban. | `rs150,350` |

### 3.3 Targeting Commands

> **Notice**: All 3D projection targeting commands (`tcp`, `tcm`, `tcg`, `tphp`, `aim`) require **Line 47** (Camera FOV/Pitch/Yaw) in `config.txt` to be calibrated.

| Command | Syntax | Description | Example |
|---|---|---|---|
| Is Mob Targeted | `cm` / `!cm` | Checks if a mob is currently targeted. `!cm` inverts check. | `!cm\|gt8` |
| Cursor Hover Check | `ct[type]` / `!ct[type]` | Checks if cursor is hovering over target type (`2` = gathering/loot, `3` = mob). | `ct2\|1d\|s100\|1u` |
| Has Selected Target | `it` / `!it` | Checks if player currently has a selected target. | `it\|tt*` |
| Is Gather Node Active | `cg` | Continues only if gathering object under cursor is not depleted. | `cg\|1` |
| Move Cursor to Node | `tcg[dist],[tier],[rarity]` | Nudges cursor toward closest active node within `dist`. Tier and rarity are optional filters. Nudge distance set by Line 26. | `tcg10,3,1` |
| Target Node Direct | `tcg*[dist],[tier],[rarity]` | Directly snaps cursor onto closest active node meeting criteria. | `tcg*15,4,2` |
| Target Node by Name | `tcg % [name],[dist],[tier],[rarity]` | Snaps cursor to node whose internal name matches `name` (e.g. `WOOD`, `ROCK`, `FIBER`). | `tcg % ROCK,20` |
| Target Player Direct | `tcp*[dist],0,[lock],[ally]` | Snaps cursor to closest non-allied player. Parameter 2 is always `0`. `lock=1` locks cursor until `lt-` or death. `ally=1` targets party members. | `tcp*50,0,1` |
| Move Cursor to Player | `tcp[dist],[cursor_dist],0,[ally]` | Moves cursor toward closest player by `cursor_dist` pixels. | `tcp50,5` |
| Target Player by Cursor | `tcpC[dist],0,[lock],[ally]` | Targets player closest to the current cursor position. | `tcpC30,0,1` |
| Target Mob Direct | `tcm*[dist],0,[lock]` | Snaps cursor to closest mob within `dist`. `lock=1` keeps cursor on mob until dead or `lt-`. | `tcm*30,0,1` |
| Move Cursor to Mob | `tcm[dist],[cursor_dist]` | Nudges cursor toward closest mob by `cursor_dist` pixels. | `tcm30,5` |
| Target Mob by Cursor | `tcmC[dist],0,[lock]` | Targets mob closest to current cursor position. | `tcmC25,0,1` |
| Target Mob by Name | `tcm % [name],[dist]` | Snaps cursor to mob whose name contains `name` within `dist`. | `tcm % HIDE_SNAKE,15` |
| Target Mob Static Anchor | `tcm % [name],[dist],[x],[z]` | Snaps cursor to mob matching `name` within `dist` of world coordinates `x, z`. | `tcm % SNAKE,15,120,450` |
| Target Lowest HP Player | `tphp[dist],[max_hp]` | Snaps cursor to lowest HP enemy player under `max_hp` within `dist`. | `tphp25,1500` |
| Target Lowest HP Ally | `tphpP[dist],[max_hp]` | Snaps cursor to lowest HP party member under `max_hp` within `dist`. | `tphpP30,2000` |
| Aim World Position | `aim[x],[y],[z]` | Projects 3D world coordinates `x, y, z` to screen pixels and moves cursor there. | `aim-377,0,-16` |
| Target Track Cursor | `tt[dist]` / `tt*` | Moves cursor toward currently selected target by `dist` pixels. `tt*` snaps cursor directly onto target center. | `tt*` |
| Unlock Target | `lt-` | Releases the sticky cursor lock initiated by `lock=1` in `tcp*` or `tcm*`. | `endkeys=lt-` |
| Grid Search | `grid[area],[precision],[delay],[type]` | Searches screen in random grid pattern centered on window for `type` (`2` = node, `3` = mob). `!grid` inverts. | `grid2000,20,100,3` |
| Grid Search Cursor | `grid*[area],[prec],[delay],[type]` | Same as `grid`, but centered around the cursor's current position. | `grid*1000,10,100,2` |
| Grid Search Fixed XY | `gridc[area],[prec],[delay],[type],[x],[y]` | Same as `grid`, but centered around screen coordinates `x, y`. | `gridc1500,15,80,3,960,540` |
| Circle Search | `circle[rad],[prec],[delay],[type]` | Sweeps cursor in circle around screen center searching for `type`. `rad` is radius, `prec` is radian step (~0.2). | `circle100,0.2,100,2` |
| Circle Search Cursor | `circle*[rad],[prec],[delay],[type]` | Same as `circle`, but centered around cursor position. | `circle*120,0.2,80,2` |
| Circle Search Fixed | `circlec[rad],[prec],[delay],[type],[x],[y]` | Same as `circle`, but centered around screen coordinates `x, y`. | `circlec100,0.2,100,3,960,540` |

### 3.4 Entity & Environment Query Commands

| Command | Syntax | Description | Example |
|---|---|---|---|
| Count Nodes Character | `cng[count],[dist],[tier],[rar]` | Continues if at least `count` gathering nodes exist within `dist` matching tier/rarity. | `cng2,10,3,2` |
| Count Nodes Cursor | `cng*[count],[dist],[tier],[rar]` | Continues if at least `count` nodes exist within `dist` of cursor. | `cng*1,10,2,1` |
| Count Nodes by Name | `cng % [name],[count],[dist],[tier],[rar]` | Continues if at least `count` nodes matching `name` exist within `dist`. | `cng % WOOD,1,10,3,2` |
| Count Players | `cnp[count],[dist],[ally]` | Continues if at least `count` players are within `dist`. `ally=1` checks party members. | `cnp1,15` |
| Count Players Cursor | `cnp*[count],[dist],[ally]` | Continues if at least `count` players are within `dist` of cursor. | `cnp*1,10` |
| Count Mobs Character | `cnm[count],[dist]` | Continues if at least `count` mobs are within `dist`. | `cnm1,12` |
| Count Mobs Cursor | `cnm*[count],[dist]` | Continues if at least `count` mobs are within `dist` of cursor. | `cnm*1,8` |
| Count Mobs Fixed XY | `cnm[count],[dist],[x],[z]` | Continues if at least `count` mobs are within `dist` of coordinates `x, z`. | `cnm1,10,34.5,57.3` |
| Count Mobs by Name | `cnm % [name],[count],[dist]` | Continues if at least `count` mobs matching `name` are within `dist`. | `cnm % TOAD,1,10` |
| Count Mobs Name Anchor | `cnm % [name],[count],[dist],[x],[z]` | Continues if at least `count` mobs matching `name` are within `dist` of `x, z`. | `cnm % TOAD,1,10,20.3,30.5` |
| Player HP Percentage | `cphp[pct],[dist]` | Continues if any player within `dist` has HP fraction `<= pct` (`0.5` = 50%). | `cphp0.5,20` |
| Player HP Literal | `cphp*[hp],[dist]` | Continues if any player within `dist` has literal HP `<= hp`. | `cphp*1000,20` |
| Player Max HP Check | `cpmhp[max_hp],[dist]` | Continues if any player within `dist` has max HP `<= max_hp`. | `cpmhp1500,20` |

### 3.5 Player Status & Cooldowns

| Command | Syntax | Description | Example |
|---|---|---|---|
| Self HP Check | `hp[pct]` / `!hp[pct]` | Continues if player HP is `<= pct` (0.0 to 1.0). `!hp` checks if greater. | `hp0.4\|dbg % Low HP!` |
| Self MP Check | `mp[pct]` / `!mp[pct]` | Continues if player Mana is `<= pct` (0.0 to 1.0). `!mp` checks if greater. | `!mp0.2\|81` (Cast Q if > 20% MP) |
| Cooldown Ready | `cc[id],[timer]` / `!cc[id]` | Continues if ability with `id` is ready. `timer` is readiness threshold (e.g. `0.95` = 95% ready). | `cc2033222\|81` |

### 3.6 Data Retrieval & Movement

| Command | Syntax | Description | Example |
|---|---|---|---|
| Get Cursor Position | `get % cursor` | Stores current screen mouse coordinates into global variables `CX` and `CY`. | `get % cursor` |
| Move To Coordinates | `mt[x],[z]` | Commands character to walk to coordinates `x, z`. | `mt150.2,320.8` |

### 3.7 Variables, Math, Logic & Scripting Control

| Command | Syntax | Description | Example |
|---|---|---|---|
| Jump to Line / Index | `gt[#]` | Waymarks: jumps to waypoint index `#`. Macros: jumps to line `keys#` (`gt1` jumps to `keys`, `gt3` jumps to `keys3`). | `gt1` |
| Read Variable | `(VAR % name)` | Replaces token with stored value of `name`. Can be nested. | `m(VAR % CX),(VAR % CY)` |
| Store Variable | `store % name,val` | Stores `val` into variable `name`. Pipe characters must be escaped as `'\|`. | `store % count,0` |
| Equal Check | `eq % name,val` / `!eq` | Continues line only if variable `name` equals `val`. `!eq` checks not equal. | `eq % skip,false\|81` |
| Compare Less Than | `cmp[v1],[v2]` / `!cmp` | Continues if `v1 < v2`. `!cmp` checks if `v1 >= v2`. | `cmp(VAR % hp),500\|70` |
| Logical OR | `or % c1'\|c2'\|...` | Evaluates sub-commands sequentially; continues as soon as one is true. Delimiter is `'\|`. | `or % cnm1,5'\|cnp1,5` |
| Add Math | `add % name,val` | Calculates `name = name + val`. | `add % counter,1` |
| Subtract Math | `sub % name,val` | Calculates `name = name - val`. | `sub % counter,1` |
| Multiply Math | `mul % name,val` | Calculates `name = name * val`. | `mul % speed,1.1` |
| Divide Math | `div % name,val` | Calculates `name = name / val`. | `div % distance,2` |
| Random Number | `rand[min],[max]` | Generates random integer between `min` and `max` inclusive and stores into global variable `RAND`. | `rand1,100` |
| Console Command | `cc % [cmd]` | Executes any Speeder console command that requires no interactive prompts. | `cc % -record 9999,30` |
| Call Function | `call % [func]` | Calls function defined in `[fName]`. Runs all lines of the function. | `call % fCombat` |
| Console Debug Print | `dbg % [text]` | Prints `text` to Speeder console. Highly recommended for troubleshooting. | `dbg % Step 1 Complete` |
| No Operation | `nop` | Dummy placeholder command that does nothing. Serves as a safe jump target. | `keys3=nop` |

---

## 4. Functions (`[fFunctionName]`)

Functions provide reusable subroutines that can be called from macros, waymarks, or other functions via `call % [function name]`.

### Syntax Rules
- Section header must start with lowercase `f` followed by alphanumeric characters: `[fName]`.
- Contains sequential command lines `keys`, `keys2`, `keys3`, etc.
- **Waymark Integration**: Functions can be declared directly inside waymark files. This allows an unlimited number of commands at a single waypoint!

```ini
[119]
keys=call % fBuffAndHeal
repeat=0

[fBuffAndHeal]
keys=hp0.6|dbg % Healing character|82|s500
keys2=mp0.5|dbg % Replenishing mana|70|s500
keys3=dbg % Function execution complete
```

---

## 5. Complete Copy-Paste Macro Examples

### 5.1 Combat Cooldown Rotation (F2)
Uses cooldown abilities in `Q -> W -> E -> R -> F -> G` sequence whenever targeting an active mob. Cooldown IDs are checked before casting.

```ini
[113]
keys=!cm|gt8
keys2=cc1|81|dbg % Q key|s300
keys3=cc2|87|dbg % W key|s300
keys4=cc3|69|dbg % E key|s300
keys5=cc4|82|dbg % R key|s300
keys6=cc5|70|dbg % F key|s300
keys7=cc6|71|dbg % G key|s300
keys8=s10
repeat=2
```

---

### 5.2 Player Sticky Target Lock (F3)
Locks the cursor onto the closest enemy player within 50 units. Releases lock when stopped or target dies.

```ini
[114]
keys=tcp*50,0,1|dbg % locked target on closest player within 50 distance
endkeys=lt-
repeat=0
```

---

### 5.3 Mob Sticky Target Lock (F3)
Locks the cursor onto the closest mob within 50 units. Releases lock when stopped or mob dies.

```ini
[114]
keys=tcm*50,0,1|dbg % locked target on closest mob within 50 distance
endkeys=lt-
repeat=0
```

---

### 5.4 Player Lock & Combat Rotation Combo (F4)
Combines sticky target locking with the full automated cooldown rotation.

```ini
[115]
keys=tcp*50,0,1|gt3
keys2=gt9
keys3=cc1|81|dbg % Q key|s300
keys4=cc2|87|dbg % W key|s300
keys5=cc3|69|dbg % E key|s300
keys6=cc4|82|dbg % R key|s300
keys7=cc5|70|dbg % F key|s300
keys8=cc6|71|dbg % G key|s300
keys9=s10
repeat=2
```

---

### 5.5 Quick Target & Return Cursor (F5)
Saves your current cursor position, snaps to the nearest player within 50 units, and automatically returns the mouse cursor back to its exact original position when F5 is released.

```ini
[116]
keys=get % cursor
keys2=tcp*50
endkeys=m(VAR % CX),(VAR % CY)
repeat=0
```

---

### 5.6 Continuous Rock Gathering Loop (F6)
Repeatedly detects and gathers `ROCK` nodes within 20 units. Clicks node, simulates human-like randomized hold duration (150–200ms), and loops continuously.

```ini
[117]
keys=cng % ROCK,1,20|tcg % ROCK,20|dbg % targeting ROCK|1d|rs150,200|1u
keys2=s10
repeat=2
```

---

### 5.7 Movement Speed Adjusters (Numpad 6 & 4)

#### Speed Up (Numpad 6: [102])
Increases character speed by 1% (0.01) increments:

```ini
[102]
keys=cmp(VAR % movementSpeed),1.0|store % movementSpeed,1.01|gt3
keys2=add % movementSpeed,0.01
keys3=ms(VAR % movementSpeed)|dbg % movement speed: (VAR % movementSpeed)
repeat=0
```

#### Speed Down (Numpad 4: [100])
Decreases character speed by 1% increments, disabling hack if speed drops below 1.0:

```ini
[100]
keys=!cmp(VAR % movementSpeed),1.005|sub % movementSpeed,0.01|gt3
keys2=store % movementSpeed,0
keys3=ms(VAR % movementSpeed)|dbg % movement speed: (VAR % movementSpeed)
repeat=0
```

---

### 5.8 Advanced Cursor Memory & Auto-Kiting Macro (F1: [112])
An advanced macro that executes two combat cooldowns when ready, stores initial cursor position, snaps to mob, fires skill, restores cursor position, and resumes holding right-click to continue moving in the original travel direction.

```ini
[112]
keys=cc3695810|cnm1,13|2u|get % cursor|tcm*13|s100|81|dbg % Q key|store % moveCursor,1|gt*
keys2=cc3695788|cnm1,15|2u|get % cursor|tcm*15|s100|87|dbg % W key|store % moveCursor,1|gt*
keys3=eq % moveCursor,1|m(VAR % oCX),(VAR % oCY)|store % moveCursor,0|store % storeCursor,0
keys4=2d
keys5=s10|eq % moveCursor,1|!eq % storeCursor,1|store % oCX,(VAR % CX)|store % oCY,(VAR % CY)|store % storeCursor,1
endkeys=2u|store % moveCursor,0|store % storeCursor,0
repeat=1
```
