using System;
using System.IO;
using System.Linq;
using System.Windows.Media;
using SpeederConfigApp.Models;
using SpeederConfigApp.ViewModels;

namespace SpeederConfigApp.Tests
{
    public static class SelfTestRunner
    {
        public static int RunAllTests()
        {
            int passed = 0;
            int failed = 0;
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_output.log");
            using var logWriter = new StreamWriter(logPath, false);

            void Log(string msg)
            {
                Console.WriteLine(msg);
                logWriter.WriteLine(msg);
                logWriter.Flush();
            }

            void Assert(bool condition, string testName)
            {
                if (condition)
                {
                    Log($"[PASS] {testName}");
                    passed++;
                }
                else
                {
                    Log($"[FAIL] {testName}");
                    failed++;
                }
            }

            try
            {
                Log("=================================================");
                Log("  RUNNING SPEEDER CONFIG TEST SUITE");
                Log("=================================================");

            // Test 1: ConfigSettings default generation line count
            var settings = new ConfigSettings();
            var lines = settings.GenerateLines();
            Assert(lines.Length == 55, "Default config produces exactly 55 lines");

            // Test 2: Movement speed default
            Assert(lines[1] == "1.05", "Line 2 default is 1.05 (5% speed increase)");

            // Test 3: Screen clamp default
            Assert(lines[18] == "100|1820|100|980", "Line 19 default screen boundary clamp format");

            // Test 4: Screen clamp calculator
            var vm = new MainViewModel
            {
                CalcResWidth = 3840,
                CalcResHeight = 2160,
                CalcMargin = 100
            };
            vm.CalculateBoundsCommand.Execute(null);
            Assert(vm.Settings.ClampMinX == 100 && vm.Settings.ClampMaxX == 3740 &&
                   vm.Settings.ClampMinY == 100 && vm.Settings.ClampMaxY == 2060,
                   "Screen clamp calculator generates 100|3740|100|2060 for 4K with 100px margin");

            // Test 5: Color converter RGB to Int and Int to RGB
            var redColor = ColorConverterHelper.IntToColor(16711680);
            Assert(redColor.R == 255 && redColor.G == 0 && redColor.B == 0, "Color 16711680 is Pure Red RGB(255, 0, 0)");

            int redInt = ColorConverterHelper.ColorToInt(Color.FromRgb(255, 0, 0));
            Assert(redInt == 16711680, "ColorToInt(Red) returns 16711680");

            var blueColor = ColorConverterHelper.IntToColor(255);
            Assert(blueColor.R == 0 && blueColor.G == 0 && blueColor.B == 255, "Color 255 is Pure Blue RGB(0, 0, 255)");

            var grayColor = ColorConverterHelper.IntToColor(3618615);
            Assert(grayColor.R == 55 && grayColor.G == 55 && grayColor.B == 55, "Color 3618615 is Dark Gray RGB(55, 55, 55)");

            // Test 6: VirtualKeyHelper mapping
            var f2 = VirtualKeyHelper.GetKeyByCode(113);
            Assert(f2.Name == "F2" && f2.Code == 113, "VirtualKeyHelper resolves 113 to F2");

            var space = VirtualKeyHelper.GetKeyByCode(32);
            Assert(space.Name == "Space" && space.Code == 32, "VirtualKeyHelper resolves 32 to Space");

            var lshift = VirtualKeyHelper.GetKeyByCode(160);
            Assert(lshift.Name == "Left Shift" && lshift.Code == 160, "VirtualKeyHelper resolves 160 to Left Shift");

            var rshift = VirtualKeyHelper.GetKeyByCode(161);
            Assert(rshift.Name == "Right Shift" && rshift.Code == 161, "VirtualKeyHelper resolves 161 to Right Shift");

            var lctrl = VirtualKeyHelper.GetKeyByCode(162);
            Assert(lctrl.Name == "Left Ctrl" && lctrl.Code == 162, "VirtualKeyHelper resolves 162 to Left Ctrl");

            var rctrl = VirtualKeyHelper.GetKeyByCode(163);
            Assert(rctrl.Name == "Right Ctrl" && rctrl.Code == 163, "VirtualKeyHelper resolves 163 to Right Ctrl");

            var lalt = VirtualKeyHelper.GetKeyByCode(164);
            Assert(lalt.Name == "Left Alt" && lalt.Code == 164, "VirtualKeyHelper resolves 164 to Left Alt");

            var ralt = VirtualKeyHelper.GetKeyByCode(165);
            Assert(ralt.Name == "Right Alt" && ralt.Code == 165, "VirtualKeyHelper resolves 165 to Right Alt");

            var scrollUp = VirtualKeyHelper.GetKeyByCode(256);
            Assert(scrollUp.Name == "Scroll Up" && scrollUp.Code == 256, "VirtualKeyHelper resolves 256 to Scroll Up");

            var scrollDown = VirtualKeyHelper.GetKeyByCode(257);
            Assert(scrollDown.Name == "Scroll Down" && scrollDown.Code == 257, "VirtualKeyHelper resolves 257 to Scroll Down");

            var lwin = VirtualKeyHelper.GetKeyByCode(91);
            Assert(lwin.Name == "Left Windows" && lwin.Code == 91, "VirtualKeyHelper resolves 91 to Left Windows");

            var rwin = VirtualKeyHelper.GetKeyByCode(92);
            Assert(rwin.Name == "Right Windows" && rwin.Code == 92, "VirtualKeyHelper resolves 92 to Right Windows");

            var apps = VirtualKeyHelper.GetKeyByCode(93);
            Assert(apps.Name == "Menu / Applications" && apps.Code == 93, "VirtualKeyHelper resolves 93 to Menu / Applications");

            // Test 7: Load example config lines from prompt specification
            var sampleLines = new string[55];
            for (int i = 0; i < 55; i++) sampleLines[i] = "";
            sampleLines[0] = "112"; // Line 1: F1
            sampleLines[1] = "1.05"; // Line 2
            sampleLines[2] = "113"; // Line 3: F2
            sampleLines[3] = "0.50"; // Line 4
            sampleLines[4] = "114"; // Line 5: F3
            sampleLines[5] = "39"; // Line 6: Right Arrow
            sampleLines[6] = "37"; // Line 7: Left Arrow
            sampleLines[7] = "5"; // Line 8
            sampleLines[8] = "38"; // Line 9: Up Arrow
            sampleLines[9] = "40"; // Line 10: Down Arrow
            sampleLines[10] = "5"; // Line 11
            sampleLines[11] = "4"; // Line 12: Middle Mouse Button
            sampleLines[12] = "0"; // Line 13
            sampleLines[13] = "0"; // Line 14
            sampleLines[14] = ""; // Line 15: Deprecated
            sampleLines[15] = "1.5"; // Line 16
            sampleLines[16] = "0"; // Line 17
            sampleLines[17] = "0"; // Line 18
            sampleLines[18] = "100|3740|100|2060"; // Line 19
            sampleLines[19] = "MyDriver.sys"; // Line 20
            sampleLines[20] = "0.5|0.75"; // Line 21
            sampleLines[21] = "115"; // Line 22: F4
            sampleLines[22] = "3"; // Line 23
            sampleLines[23] = "0.05"; // Line 24
            sampleLines[24] = "5000"; // Line 25
            sampleLines[25] = "100"; // Line 26
            sampleLines[26] = "116"; // Line 27: F5
            sampleLines[27] = "117"; // Line 28: F6
            sampleLines[28] = "Recorded Waymarks.ini"; // Line 29
            sampleLines[29] = "1"; // Line 30
            sampleLines[30] = "200"; // Line 31
            sampleLines[31] = "-10"; // Line 32
            sampleLines[32] = "2"; // Line 33
            sampleLines[33] = "1000"; // Line 34
            sampleLines[34] = "1"; // Line 35
            sampleLines[35] = "118"; // Line 36: F7
            sampleLines[36] = "1|0|2000|\"C:\\Program Files\\VideoLAN\\VLC\\vlc\" --qt-start-minimized --play-and-exit C:\\soundfile.mp3"; // Line 37
            sampleLines[37] = ""; // Line 38
            sampleLines[38] = "1"; // Line 39
            sampleLines[39] = ""; // Line 40
            sampleLines[40] = ""; // Line 41
            sampleLines[41] = "1"; // Line 42
            sampleLines[42] = "0|0|1920|1080|1|1000|15|3618615|255|255|16711680|*|1|1|1|0|0|10|200"; // Line 43
            sampleLines[43] = "20|20|520|380|13|3618615|16777215|0|1|Consolas"; // Line 44
            sampleLines[44] = "119"; // Line 45: F8
            sampleLines[45] = "200|20|255|255|16711680|1|1|1|50"; // Line 46
            sampleLines[46] = "46.5|7|0.02"; // Line 47
            sampleLines[47] = "2"; // Line 48
            sampleLines[48] = "WOOD,160000,2,1|WOOD,0|FIBER,120000,3,2|FIBER,0|ROCK,0"; // Line 49
            sampleLines[49] = "1|60000|5000"; // Line 50
            sampleLines[50] = "T2_MOB_HIDE_SNAKE,120000|T1_MOB_HIDE_SWAMP_FROG,160000"; // Line 51
            sampleLines[51] = "* %hp%|* %dist%"; // Line 52
            sampleLines[52] = "* %name% (%num%)|* %name% (%num%)"; // Line 53
            sampleLines[53] = "* %hp%|* %name% (%dist%)"; // Line 54
            sampleLines[54] = "1000"; // Line 55

            var loadedSettings = new ConfigSettings();
            loadedSettings.LoadFromLines(sampleLines);

            Assert(loadedSettings.MasterToggleKey == 112, "Line 1 loaded: MasterToggleKey = 112");
            Assert(loadedSettings.ClampMinX == 100 && loadedSettings.ClampMaxX == 3740 &&
                   loadedSettings.ClampMinY == 100 && loadedSettings.ClampMaxY == 2060, "Line 19 loaded: Clamp bounds");
            Assert(loadedSettings.DriverName == "MyDriver.sys", "Line 20 loaded: DriverName");
            Assert(loadedSettings.FieldOfView == "0.5|0.75", "Line 21 loaded: FieldOfView = 0.5|0.75");
            Assert(loadedSettings.MacroFileName == "Recorded Waymarks.ini", "Line 29 loaded: MacroFileName");
            Assert(loadedSettings.InjectedKeysAllowed == true, "Line 30 loaded: InjectedKeysAllowed");
            Assert(loadedSettings.MouseSmoothing == 200, "Line 31 loaded: MouseSmoothing = 200");
            Assert(loadedSettings.GpcMultiplier == 2.0, "Line 33 loaded: GpcMultiplier = 2");
            Assert(loadedSettings.DetectionMinPlayers == 1 && loadedSettings.DetectionTimerMs == 2000, "Line 37 loaded: Detection players & timer");
            Assert(loadedSettings.RadarWidth == 1920 && loadedSettings.RadarHeight == 1080, "Line 43 loaded: Radar dimensions");
            Assert(loadedSettings.RadarBgColor == 3618615, "Line 43 loaded: Radar background color 3618615");
            Assert(loadedSettings.ConsoleEnabled && loadedSettings.ConsolePosX == 20 && loadedSettings.ConsoleWidth == 520 &&
                   loadedSettings.ConsoleBgColor == 3618615 && loadedSettings.ConsoleFont == "Consolas", "Line 44 loaded: Console window parameters");
            Assert(loadedSettings.EspTotalEntities == 200 && loadedSettings.EspRefreshRate == 50, "Line 46 loaded: ESP entities and refresh");
            Assert(loadedSettings.EspTargetingFov == 46.5 && loadedSettings.EspCameraYaw == 7.0 && loadedSettings.EspCameraPitch == 0.02, "Line 47 loaded: ESP 3D projection");
            Assert(loadedSettings.GatheringFilters.Count == 5, "Line 49 loaded: 5 Gathering filters parsed");
            Assert(loadedSettings.GatheringFilters[0].ResourceName == "WOOD" && loadedSettings.GatheringFilters[0].ColorValue == 160000, "Line 49 filter 0: WOOD, 160000");
            Assert(loadedSettings.MountedPlayerHighlightActive && loadedSettings.MountedPlayerColor == 60000 && loadedSettings.MountedPlayerDelayMs == 5000, "Line 50 loaded: Mounted player highlight");
            Assert(loadedSettings.MobColorFilters.Count == 2, "Line 51 loaded: 2 Mob filters parsed");
            Assert(loadedSettings.PlayerEspText == "* %hp%" && loadedSettings.PlayerRadarText == "* %dist%", "Line 52 loaded: Player text");
            Assert(loadedSettings.GatheringEspText == "* %name% (%num%)", "Line 53 loaded: Gathering text");
            Assert(loadedSettings.MobEspText == "* %hp%" && loadedSettings.MobRadarText == "* %name% (%dist%)", "Line 54 loaded: Mob text");
            Assert(loadedSettings.MobMinHpThreshold == 1000, "Line 55 loaded: MobMinHpThreshold = 1000");

            // Test 8: Re-generation matches original input (Round-trip integrity)
            var reGeneratedLines = loadedSettings.GenerateLines();
            Assert(reGeneratedLines.Length == 55, "Re-generated config has exactly 55 lines");
            Assert(reGeneratedLines[18] == sampleLines[18], "Line 19 round-trips identically");
            Assert(reGeneratedLines[36] == sampleLines[36], "Line 37 round-trips identically");
            Assert(reGeneratedLines[42] == sampleLines[42], "Line 43 round-trips identically");
            Assert(reGeneratedLines[43] == sampleLines[43], "Line 44 round-trips identically");
            Assert(reGeneratedLines[45] == sampleLines[45], "Line 46 round-trips identically");
            Assert(reGeneratedLines[46] == sampleLines[46], "Line 47 round-trips identically");
            Assert(reGeneratedLines[48] == sampleLines[48], "Line 49 round-trips identically");
            Assert(reGeneratedLines[49] == sampleLines[49], "Line 50 round-trips identically");
            Assert(reGeneratedLines[50] == sampleLines[50], "Line 51 round-trips identically");
            Assert(reGeneratedLines[51] == sampleLines[51], "Line 52 round-trips identically");
            Assert(reGeneratedLines[52] == sampleLines[52], "Line 53 round-trips identically");
            Assert(reGeneratedLines[53] == sampleLines[53], "Line 54 round-trips identically");
            Assert(reGeneratedLines[54] == sampleLines[54], "Line 55 round-trips identically");

            // UI Theme Tests
            var comboStyle = System.Windows.Application.Current?.FindResource(typeof(System.Windows.Controls.ComboBox)) as System.Windows.Style;
            Assert(comboStyle != null, "Themes/ModernTheme defines an implicit ComboBox Style");

            var itemStyle = System.Windows.Application.Current?.FindResource(typeof(System.Windows.Controls.ComboBoxItem)) as System.Windows.Style;
            Assert(itemStyle != null, "Themes/ModernTheme defines an implicit ComboBoxItem Style");

            var scrollBarStyle = System.Windows.Application.Current?.FindResource(typeof(System.Windows.Controls.Primitives.ScrollBar)) as System.Windows.Style;
            Assert(scrollBarStyle != null, "Themes/ModernTheme defines an implicit ScrollBar Style");

            var toggleStyle = System.Windows.Application.Current?.FindResource("ComboBoxToggleButtonStyle") as System.Windows.Style;
            Assert(toggleStyle != null, "Themes/ModernTheme defines ComboBoxToggleButtonStyle");

            // Test 9: Macro Engine Tests (Task 3)
            Log("-------------------------------------------------");
            Log("  MACRO ENGINE TESTS");
            Log("-------------------------------------------------");

            string sampleMacroIni = @"; Combat Cooldown Rotation
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

; Helper function test
[fTest]
keys=dbg % test line 1
keys2=dbg % test line 2
keys3=dbg % test line 3
";

            var macroModel = MacroFileModel.ParseIni(sampleMacroIni);
            Assert(macroModel != null, "MacroFileModel.ParseIni returns a valid model");
            Assert(macroModel!.Macros.Count == 2, "Parsed macro count is exactly 2");

            var m0 = macroModel.Macros[0];
            Assert(m0.TriggerKey == 113, "Macro 0 TriggerKey == 113 (F2)");
            Assert(!m0.IsFunction, "Macro 0 !IsFunction");
            Assert(m0.Repeat == 2, "Macro 0 Repeat == 2");
            Assert(m0.KeysLines.Count == 8, "Macro 0 KeysLines.Count == 8");
            Assert(m0.KeysLines[0] == "!cm|gt8" && m0.KeysLines[7] == "s10", "Macro 0 first and last KeysLines match");
            Assert(m0.SectionHeader == "113", "Macro 0 SectionHeader == \"113\"");
            Assert(m0.Description.Contains("Combat Cooldown Rotation"), "Macro 0 Description loaded from comments");
            Assert(m0.DisplayName.Contains("[F2 (113)]"), "Macro 0 DisplayName resolves F2 key code");

            var m1 = macroModel.Macros[1];
            Assert(m1.IsFunction, "Macro 1 IsFunction == true");
            Assert(m1.FunctionName == "fTest", "Macro 1 FunctionName == \"fTest\"");
            Assert(m1.KeysLines.Count == 3, "Macro 1 KeysLines.Count == 3");
            Assert(m1.KeysLines[0] == "dbg % test line 1", "Macro 1 KeysLines[0] matches");
            Assert(m1.SectionHeader == "fTest", "Macro 1 SectionHeader == \"fTest\"");
            Assert(m1.Description.Contains("Helper function test"), "Macro 1 Description loaded from comments");
            Assert(m1.DisplayName.Contains("[fTest]"), "Macro 1 DisplayName contains [fTest]");

            // Round-trip test
            string generatedIni = macroModel.GenerateIni();
            var roundTripModel = MacroFileModel.ParseIni(generatedIni);
            Assert(roundTripModel.Macros.Count == 2, "Round-trip preserves macro count (2)");

            var rt0 = roundTripModel.Macros[0];
            var rt1 = roundTripModel.Macros[1];
            Assert(rt0.TriggerKey == m0.TriggerKey && rt0.IsFunction == m0.IsFunction && rt0.Repeat == m0.Repeat &&
                   rt0.KeysLines.Count == m0.KeysLines.Count && rt0.KeysLines.SequenceEqual(m0.KeysLines),
                   "Round-trip macro 0 matches properties and all KeysLines");
            Assert(rt1.IsFunction == m1.IsFunction && rt1.FunctionName == m1.FunctionName &&
                   rt1.KeysLines.Count == m1.KeysLines.Count && rt1.KeysLines.SequenceEqual(m1.KeysLines),
                   "Round-trip macro 1 matches properties and all KeysLines");

            // Default templates test
            var defaultTemplates = MacroFileModel.CreateDefaultTemplates();
            Assert(defaultTemplates.Macros.Count == 8, "CreateDefaultTemplates returns exactly 8 items");
            Assert(defaultTemplates.Macros[0].TriggerKey == 113 && defaultTemplates.Macros[0].Repeat == 2,
                   "Template 0 is F2 Combat Rotation with Repeat=2");
            Assert(defaultTemplates.Macros[1].TriggerKey == 114 && defaultTemplates.Macros[1].EndKeys == "lt-",
                   "Template 1 is F3 Player Target Lock with endkeys=lt-");
            Assert(defaultTemplates.Macros[2].TriggerKey == 115 && defaultTemplates.Macros[2].KeysLines.Count == 9,
                   "Template 2 is F4 Target Lock & Combat Rotation with 9 KeysLines");
            Assert(defaultTemplates.Macros[3].TriggerKey == 116 && defaultTemplates.Macros[3].EndKeys == "m(VAR % CX),(VAR % CY)",
                   "Template 3 is F5 Quick Target & Return Cursor with cursor restore endkeys");
            Assert(defaultTemplates.Macros[4].TriggerKey == 117 && defaultTemplates.Macros[4].Repeat == 2,
                   "Template 4 is F6 Continuous Rock Gatherer with Repeat=2");
            Assert(defaultTemplates.Macros[5].TriggerKey == 102 && defaultTemplates.Macros[5].Repeat == 0,
                   "Template 5 is NUMPAD6 Speed Increase (+1%)");
            Assert(defaultTemplates.Macros[6].TriggerKey == 100 && defaultTemplates.Macros[6].Repeat == 0,
                   "Template 6 is NUMPAD4 Speed Decrease (-1%)");
            Assert(defaultTemplates.Macros[7].IsFunction && defaultTemplates.Macros[7].FunctionName == "fTest",
                   "Template 7 is fTest Reusable Helper Function");

            // Deep clone test
            var clone = m0.Clone();
            clone.TriggerKey = 120;
            clone.KeysLines.Add("s999");
            Assert(clone.TriggerKey == 120 && m0.TriggerKey == 113, "MacroItem.Clone modifies clone without altering original TriggerKey");
            Assert(clone.KeysLines.Count == 9 && m0.KeysLines.Count == 8, "MacroItem.Clone creates independent KeysLines collection");

            // Interrupt and EndKeys serialization / parsing
            var customMacro = new MacroItem
            {
                TriggerKey = 112,
                Repeat = 1,
                Interrupt = 0,
                EndKeys = "2u|store % moveCursor,0"
            };
            customMacro.KeysLines.Add("cc100|81");
            var customModel = new MacroFileModel();
            customModel.Macros.Add(customMacro);
            var customIni = customModel.GenerateIni();
            var parsedCustom = MacroFileModel.ParseIni(customIni);
            Assert(parsedCustom.Macros[0].Interrupt == 0 && parsedCustom.Macros[0].EndKeys == "2u|store % moveCursor,0" &&
                   parsedCustom.Macros[0].Repeat == 1,
                   "Custom macro serializes and parses Interrupt, EndKeys, and Repeat correctly");

            // Test 10: Waymark Engine Tests (Task 4)
            Log("-------------------------------------------------");
            Log("  WAYMARK ENGINE TESTS");
            Log("-------------------------------------------------");

            string sampleWaymarkIni = @"; Sample Waymark Route from Reference Docs
[variables]
variablename=true

[unstick]
distance=5
keys=27
timer=5000
timer2=2000
giveup=5
script=probablydead.ini

[0]
x=84.66
y=-78
z=832
wait time=30
keys=2d|s3000|2u|eq % variablename,true|dbg % starting waymarks!|store % variablename,false

[1]
x=90.34
y=-60
z=783.21
wait time=30
script=nextinifile.ini|5
";

            var waymarkModel = WaymarkFileModel.ParseIni(sampleWaymarkIni);
            Assert(waymarkModel != null, "WaymarkFileModel.ParseIni returns a valid model");

            // Test 2: Verify Variables
            Assert(waymarkModel!.Variables.Count == 1, "Parsed waymark variables count == 1");
            Assert(waymarkModel.Variables[0].Name == "variablename", "Variable name == \"variablename\"");
            Assert(waymarkModel.Variables[0].Value == "true", "Variable value == \"true\"");

            // Test 3: Verify Unstick
            Assert(waymarkModel.Unstick.Enabled, "Unstick is enabled");
            Assert(Math.Abs(waymarkModel.Unstick.Distance - 5.0) < 0.001, "Unstick distance == 5");
            Assert(waymarkModel.Unstick.Keys == "27", "Unstick keys == \"27\"");
            Assert(waymarkModel.Unstick.Timer == 5000, "Unstick timer == 5000");
            Assert(waymarkModel.Unstick.Timer2 == 2000, "Unstick timer2 == 2000");
            Assert(waymarkModel.Unstick.GiveUp == 5, "Unstick giveup == 5");
            Assert(waymarkModel.Unstick.Script == "probablydead.ini", "Unstick script == \"probablydead.ini\"");

            // Test 4: Verify Points
            Assert(waymarkModel.Points.Count == 2, "Parsed waymark points count == 2");

            var wp0 = waymarkModel.Points[0];
            Assert(Math.Abs(wp0.X - 84.66) < 0.001, "Point 0: X == 84.66");
            Assert(Math.Abs(wp0.Y - (-78)) < 0.001, "Point 0: Y == -78");
            Assert(Math.Abs(wp0.Z - 832) < 0.001, "Point 0: Z == 832");
            Assert(wp0.WaitTime == 30, "Point 0: WaitTime == 30");
            Assert(wp0.Keys.Contains("starting waymarks!"), "Point 0: Keys contains \"starting waymarks!\"");

            var wp1 = waymarkModel.Points[1];
            Assert(Math.Abs(wp1.X - 90.34) < 0.001, "Point 1: X == 90.34");
            Assert(Math.Abs(wp1.Y - (-60)) < 0.001, "Point 1: Y == -60");
            Assert(Math.Abs(wp1.Z - 783.21) < 0.001, "Point 1: Z == 783.21");
            Assert(wp1.WaitTime == 30, "Point 1: WaitTime == 30");
            Assert(wp1.Script == "nextinifile.ini|5", "Point 1: Script == \"nextinifile.ini|5\"");

            // Test 5: Verify IsStopMovement helper flag
            var testPoint = new WaymarkPoint { X = 50.0, WaitTime = 30 };
            Assert(!testPoint.IsStopMovement, "Default point IsStopMovement == false");
            testPoint.X = 0.1;
            Assert(testPoint.IsStopMovement, "Setting X to 0.1 triggers IsStopMovement == true");
            testPoint.IsStopMovement = false;
            Assert(!testPoint.IsStopMovement && Math.Abs(testPoint.X - 0.1) >= 0.001, "Setting IsStopMovement = false resets X away from 0.1");
            testPoint.IsStopMovement = true;
            Assert(testPoint.IsStopMovement && Math.Abs(testPoint.X - 0.1) < 0.001, "Setting IsStopMovement = true sets X = 0.1");

            // Test 6: Verify IsSeamlessMovement helper flag
            Assert(!testPoint.IsSeamlessMovement, "WaitTime 30 has IsSeamlessMovement == false");
            testPoint.WaitTime = 1;
            Assert(testPoint.IsSeamlessMovement, "Setting WaitTime = 1 triggers IsSeamlessMovement == true");
            testPoint.IsSeamlessMovement = false;
            Assert(!testPoint.IsSeamlessMovement && testPoint.WaitTime == 30, "Setting IsSeamlessMovement = false resets WaitTime to 30");
            testPoint.IsSeamlessMovement = true;
            Assert(testPoint.IsSeamlessMovement && testPoint.WaitTime == 1, "Setting IsSeamlessMovement = true sets WaitTime = 1");

            // Test 7: Round-trip test
            string generatedWaymarkIni = waymarkModel.GenerateIni();
            var roundTripWaymarkModel = WaymarkFileModel.ParseIni(generatedWaymarkIni);
            Assert(roundTripWaymarkModel.Variables.Count == waymarkModel.Variables.Count, "Round-trip preserves variables count");
            Assert(roundTripWaymarkModel.Variables[0].Name == waymarkModel.Variables[0].Name &&
                   roundTripWaymarkModel.Variables[0].Value == waymarkModel.Variables[0].Value, "Round-trip preserves variable name/value");
            Assert(roundTripWaymarkModel.Unstick.Enabled == waymarkModel.Unstick.Enabled &&
                   Math.Abs(roundTripWaymarkModel.Unstick.Distance - waymarkModel.Unstick.Distance) < 0.001 &&
                   roundTripWaymarkModel.Unstick.Keys == waymarkModel.Unstick.Keys &&
                   roundTripWaymarkModel.Unstick.Timer == waymarkModel.Unstick.Timer &&
                   roundTripWaymarkModel.Unstick.Timer2 == waymarkModel.Unstick.Timer2 &&
                   roundTripWaymarkModel.Unstick.GiveUp == waymarkModel.Unstick.GiveUp &&
                   roundTripWaymarkModel.Unstick.Script == waymarkModel.Unstick.Script, "Round-trip preserves all unstick fields");
            Assert(roundTripWaymarkModel.Points.Count == waymarkModel.Points.Count, "Round-trip preserves points count");
            Assert(Math.Abs(roundTripWaymarkModel.Points[0].X - wp0.X) < 0.001 &&
                   Math.Abs(roundTripWaymarkModel.Points[0].Y - wp0.Y) < 0.001 &&
                   Math.Abs(roundTripWaymarkModel.Points[0].Z - wp0.Z) < 0.001 &&
                   roundTripWaymarkModel.Points[0].WaitTime == wp0.WaitTime &&
                   roundTripWaymarkModel.Points[0].Keys == wp0.Keys, "Round-trip point 0 matches coordinates, wait time, and keys");
            Assert(Math.Abs(roundTripWaymarkModel.Points[1].X - wp1.X) < 0.001 &&
                   Math.Abs(roundTripWaymarkModel.Points[1].Y - wp1.Y) < 0.001 &&
                   Math.Abs(roundTripWaymarkModel.Points[1].Z - wp1.Z) < 0.001 &&
                   roundTripWaymarkModel.Points[1].WaitTime == wp1.WaitTime &&
                   roundTripWaymarkModel.Points[1].Script == wp1.Script, "Round-trip point 1 matches coordinates, wait time, and script");

            // Test 8: CreateSampleRoute verification
            var sampleRoute = WaymarkFileModel.CreateSampleRoute();
            Assert(sampleRoute.Variables.Count == 1 && sampleRoute.Variables[0].Name == "variablename", "CreateSampleRoute includes variablename");
            Assert(sampleRoute.Unstick.Enabled && sampleRoute.Unstick.Timer == 5000, "CreateSampleRoute includes unstick failsafe");
            Assert(sampleRoute.Points.Count == 4, "CreateSampleRoute includes 4 points forming a complete loop");
            Assert(sampleRoute.Points[3].IsSeamlessMovement, "Sample route point 3 demonstrates seamless movement (WaitTime=1)");

            // Test 9: ReindexPoints and AddPoint
            var reindexModel = new WaymarkFileModel();
            var addedP0 = reindexModel.AddPoint(10, 20, 30);
            var addedP1 = reindexModel.AddPoint(40, 50, 60);
            Assert(addedP0.Index == 0 && addedP1.Index == 1, "AddPoint sequentially assigns index 0 and 1");
            addedP0.Index = 99;
            reindexModel.ReindexPoints();
            Assert(reindexModel.Points[0].Index == 0 && reindexModel.Points[1].Index == 1, "ReindexPoints normalizes indices to 0, 1");

            // Test 10: Deep Clone
            var waymarkClone = sampleRoute.Clone();
            Assert(waymarkClone.Points.Count == 4 && waymarkClone.Variables.Count == 1, "Waymark clone has matching counts");
            waymarkClone.Points[0].X = 999.9;
            Assert(sampleRoute.Points[0].X != 999.9, "Modifying cloned point does not affect original route point");

            // Test 11: Disabled Unstick generation omission
            var noUnstickModel = new WaymarkFileModel();
            noUnstickModel.Unstick.Enabled = false;
            noUnstickModel.AddPoint(1, 2, 3);
            var noUnstickIni = noUnstickModel.GenerateIni();
            Assert(!noUnstickIni.Contains("[unstick]"), "WaymarkFileModel with Unstick.Enabled == false omits [unstick] section");

            // Test 12: MainViewModel Studio & Reference Engines (Task 5)
            Log("-------------------------------------------------");
            Log("  VIEWMODEL STUDIO & REFERENCE ENGINES TESTS");
            Log("-------------------------------------------------");

            var studioVm = new MainViewModel();

            // 1. ActiveTabIndex
            Assert(studioVm.ActiveTabIndex == 0, "MainViewModel initializes with ActiveTabIndex == 0");
            studioVm.ActiveTabIndex = 1;
            Assert(studioVm.ActiveTabIndex == 1, "ActiveTabIndex can be updated to 1 (Macro Studio)");
            studioVm.ActiveTabIndex = 2;
            Assert(studioVm.ActiveTabIndex == 2, "ActiveTabIndex can be updated to 2 (Waymark Studio)");
            studioVm.ActiveTabIndex = 3;
            Assert(studioVm.ActiveTabIndex == 3, "ActiveTabIndex can be updated to 3 (Reference Cheatsheet)");

            // 2. Macro Studio Initialization & Commands
            Assert(studioVm.MacroFile != null && studioVm.MacroFile.Macros.Count == 8,
                   "MainViewModel initializes with default MacroFile containing 8 macros");
            Assert(studioVm.SelectedMacro != null && studioVm.SelectedMacro.TriggerKey == 113,
                   "MainViewModel initializes with SelectedMacro set to template 0 (F2)");

            // Add macro command
            int initialMacroCount = studioVm.MacroFile!.Macros.Count;
            studioVm.AddMacroCommand.Execute(null);
            Assert(studioVm.MacroFile.Macros.Count == initialMacroCount + 1, "AddMacroCommand increments macro count");
            Assert(studioVm.SelectedMacro != null && studioVm.SelectedMacro.TriggerKey == 113,
                   "AddMacroCommand selects newly created macro");

            // Add & remove keys lines
            int initialKeyLines = studioVm.SelectedMacro!.KeysLines.Count;
            studioVm.AddKeysLineCommand.Execute("s500");
            Assert(studioVm.SelectedMacro!.KeysLines.Count == initialKeyLines + 1 &&
                   studioVm.SelectedMacro.KeysLines.Last() == "s500",
                   "AddKeysLineCommand appends line to SelectedMacro.KeysLines");
            studioVm.RemoveKeysLineCommand.Execute("s500");
            Assert(studioVm.SelectedMacro!.KeysLines.Count == initialKeyLines,
                   "RemoveKeysLineCommand removes specified line from SelectedMacro.KeysLines");

            // Clone selected macro
            var macroToClone = studioVm.SelectedMacro;
            studioVm.CloneSelectedMacroCommand.Execute(null);
            Assert(studioVm.SelectedMacro != macroToClone, "CloneSelectedMacroCommand selects newly cloned instance");
            Assert(studioVm.SelectedMacro!.Description.Contains("Copy"), "Cloned macro description contains '(Copy)'");

            // Delete selected macro
            int countBeforeDelete = studioVm.MacroFile.Macros.Count;
            studioVm.DeleteSelectedMacroCommand.Execute(null);
            Assert(studioVm.MacroFile.Macros.Count == countBeforeDelete - 1, "DeleteSelectedMacroCommand removes macro");

            // Apply macro template command
            studioVm.ApplyMacroTemplateCommand.Execute("combat");
            Assert(studioVm.SelectedMacro != null && studioVm.SelectedMacro.Description.Contains("Combat"),
                   "ApplyMacroTemplateCommand applies combat template");

            // Link macro to config
            studioVm.MacroFile.FileName = "PvP_Macros.ini";
            studioVm.LinkMacroToConfigCommand.Execute(null);
            Assert(studioVm.Settings.MacroFileName == "PvP_Macros.ini",
                   "LinkMacroToConfigCommand updates Settings.MacroFileName to MacroFile.FileName");
            Assert(studioVm.RawConfigPreview.Contains("PvP_Macros.ini"),
                   "LinkMacroToConfigCommand updates Line 29 in RawConfigPreview");

            // 3. Waymark Studio Initialization & Commands
            Assert(studioVm.WaymarkFile != null && studioVm.WaymarkFile.Points.Count == 4,
                   "MainViewModel initializes with default WaymarkFile containing 4 points");
            Assert(studioVm.SelectedWaymarkPoint != null && studioVm.SelectedWaymarkPoint.Index == 0,
                   "MainViewModel initializes with SelectedWaymarkPoint at index 0");
            Assert(studioVm.SelectedWaymarkVariable != null && studioVm.SelectedWaymarkVariable.Name == "variablename",
                   "MainViewModel initializes with SelectedWaymarkVariable");

            // Add waypoint maintaining sequential index
            int initialPointCount = studioVm.WaymarkFile!.Points.Count;
            studioVm.AddWaymarkPointCommand.Execute(null);
            Assert(studioVm.WaymarkFile.Points.Count == initialPointCount + 1, "AddWaymarkPointCommand increments point count");
            Assert(studioVm.SelectedWaymarkPoint != null && studioVm.SelectedWaymarkPoint.Index == initialPointCount,
                   "AddWaymarkPointCommand assigns sequential index to new waypoint");

            // Move waymark up and down
            studioVm.MoveWaymarkUpCommand.Execute(null);
            Assert(studioVm.SelectedWaymarkPoint!.Index == initialPointCount - 1,
                   "MoveWaymarkUpCommand moves point up and updates index");
            studioVm.MoveWaymarkDownCommand.Execute(null);
            Assert(studioVm.SelectedWaymarkPoint!.Index == initialPointCount,
                   "MoveWaymarkDownCommand moves point down and restores index");

            // Toggle stop movement (x=0.1)
            Assert(!studioVm.SelectedWaymarkPoint!.IsStopMovement, "New point initially has IsStopMovement == false");
            studioVm.ToggleStopMovementCommand.Execute(null);
            Assert(studioVm.SelectedWaymarkPoint!.IsStopMovement && Math.Abs(studioVm.SelectedWaymarkPoint.X - 0.1) < 0.001,
                   "ToggleStopMovementCommand sets x=0.1 on selected waypoint");
            studioVm.ToggleStopMovementCommand.Execute(null);
            Assert(!studioVm.SelectedWaymarkPoint!.IsStopMovement,
                   "ToggleStopMovementCommand toggles back to normal movement");

            // Toggle seamless movement (wait time=1)
            Assert(!studioVm.SelectedWaymarkPoint!.IsSeamlessMovement, "Point initially has IsSeamlessMovement == false");
            studioVm.ToggleSeamlessMovementCommand.Execute(null);
            Assert(studioVm.SelectedWaymarkPoint!.IsSeamlessMovement && studioVm.SelectedWaymarkPoint.WaitTime == 1,
                   "ToggleSeamlessMovementCommand sets wait time=1 on selected waypoint");
            studioVm.ToggleSeamlessMovementCommand.Execute(null);
            Assert(!studioVm.SelectedWaymarkPoint!.IsSeamlessMovement && studioVm.SelectedWaymarkPoint.WaitTime == 30,
                   "ToggleSeamlessMovementCommand restores default wait time (30ms)");

            // Delete waypoint and verify re-indexing
            studioVm.DeleteSelectedWaymarkPointCommand.Execute(null);
            Assert(studioVm.WaymarkFile.Points.Count == initialPointCount, "DeleteSelectedWaymarkPointCommand removes waypoint");
            bool isSequenced = true;
            for (int i = 0; i < studioVm.WaymarkFile.Points.Count; i++)
            {
                if (studioVm.WaymarkFile.Points[i].Index != i)
                {
                    isSequenced = false;
                    break;
                }
            }
            Assert(isSequenced, "Points collection remains strictly sequentially indexed after deletion");

            // Manage variables
            int initialVarCount = studioVm.WaymarkFile.Variables.Count;
            studioVm.AddVariableCommand.Execute(null);
            Assert(studioVm.WaymarkFile.Variables.Count == initialVarCount + 1, "AddVariableCommand increments variable count");
            studioVm.RemoveVariableCommand.Execute(studioVm.SelectedWaymarkVariable);
            Assert(studioVm.WaymarkFile.Variables.Count == initialVarCount, "RemoveVariableCommand decrements variable count");

            // 4. Reference Tab & Filtering Engine
            Assert(studioVm.FilteredConsoleCommands.Count == 20,
                   "FilteredConsoleCommands populates with all 20 console commands");
            Assert(studioVm.FilteredMacroCommands.Count >= 50,
                   "FilteredMacroCommands populates with rich macro syntax library (>= 50 items)");
            Assert(studioVm.FilteredVirtualKeys.Count >= 100,
                   "FilteredVirtualKeys populates with comprehensive virtual keys (>= 100 items)");
            Assert(studioVm.FilteredVirtualKeys.Any(k => k.Category == "Function" && k.Name == "F2" && k.Code == 113),
                   "Virtual keys repository categorizes F2 as Function key with code 113");

            // Search filter: "record"
            studioVm.SearchReferenceText = "record";
            Assert(studioVm.FilteredConsoleCommands.Count == 2 &&
                   studioVm.FilteredConsoleCommands.Any(c => c.Command == "-record") &&
                   studioVm.FilteredConsoleCommands.Any(c => c.Command == "-recordkeys"),
                   "Searching 'record' filters console commands to exactly -record and -recordkeys");

            // Search filter: "tcg %"
            studioVm.SearchReferenceText = "tcg %";
            Assert(studioVm.FilteredMacroCommands.Any(m => m.Command.Contains("tcg %")),
                   "Searching 'tcg %' finds node target by name in macro commands");

            // Search filter: "Shift"
            studioVm.SearchReferenceText = "Shift";
            Assert(studioVm.FilteredVirtualKeys.Any(k => k.Name.Contains("Shift")),
                   "Searching 'Shift' finds Shift keys in FilteredVirtualKeys");

            // Reset search filter
            studioVm.SearchReferenceText = "";
            Assert(studioVm.FilteredConsoleCommands.Count == 20 &&
                   studioVm.FilteredMacroCommands.Count >= 50 &&
                   studioVm.FilteredVirtualKeys.Count >= 100,
                   "Clearing search filter restores full collections for all 3 categories");

            // Copy snippet command execution test (headless safe)
            var snippetCommand = studioVm.FilteredConsoleCommands.First();
            studioVm.CopyReferenceSnippetCommand.Execute(snippetCommand);
            Assert(!string.IsNullOrEmpty(studioVm.StatusMessage),
                   "CopyReferenceSnippetCommand executes and updates StatusMessage without throwing");

            Log("=================================================");
            Log($"  TEST RESULTS: {passed} PASSED, {failed} FAILED");
            Log("=================================================");

            return failed == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                Log($"CRITICAL EXCEPTION: {ex}");
                return 1;
            }
        }
    }
}
