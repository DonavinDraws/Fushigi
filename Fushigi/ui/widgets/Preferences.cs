﻿using Fushigi.param;
using Fushigi.util;
using ImGuiNET;
using System.Numerics;

namespace Fushigi.ui.widgets
{
    class Preferences
    {
        static readonly Vector4 errCol = new Vector4(1f, 0, 0, 1);
        static bool romfsTouched = false;
        static bool modRomfsTouched = false;
        public static void Draw(ref bool continueDisplay)
        {
            if (ImGui.Begin("Preferences"))
            {
                var romfs = UserSettings.GetRomFSPath();
                var mod = UserSettings.GetModRomFSPath();

                ImGui.Indent();


                if (PathSelector.Show(
                    "RomFS Game Path",
                    ref romfs,
                    RomFS.IsValidRoot(romfs))
                    )
                {
                    romfsTouched = true;

                    RomFS.SetRoot(romfs);
                    UserSettings.SetRomFSPath(romfs);
                    
                    /* if our parameter database isn't set, set it */
                    if (!ParamDB.sIsInit)
                    {
                        ParamDB.Load();
                    }
                }

                Tooltip.Show("The game files which are stored under the romfs folder.");

                if (romfsTouched && !RomFS.IsValidRoot(romfs))
                {
                    ImGui.TextColored(errCol,
                        "The path you have selected is invalid. Please select a RomFS path that contains BancMapUnit, Model, and Stage.");
                }

                if (PathSelector.Show("Save Directory", ref mod, !string.IsNullOrEmpty(mod)))
                {
                    modRomfsTouched = true;

                    UserSettings.SetModRomFSPath(mod);
                }   

                Tooltip.Show("The save output where to save modified romfs files");

                if (modRomfsTouched && string.IsNullOrEmpty(mod))
                {
                    ImGui.TextColored(errCol,
                        "The path you have selected is invalid. Directory must not be empty.");
                }

                ImGui.Unindent();

                if (ImGui.Button("Close"))
                {
                    continueDisplay = false;
                }

                ImGui.End();
            }
        }
    }
}
