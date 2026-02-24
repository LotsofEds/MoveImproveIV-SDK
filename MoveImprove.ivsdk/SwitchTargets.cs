using System.Collections.Generic;
using System.Windows.Forms;
using static IVSDKDotNet.Native.Natives;
using CCL.GTAIV;
using IVSDKDotNet.Enums;
using System.IO;
using System.Diagnostics;
using System;
using System.Numerics;
using System.Runtime;
using System.Drawing;
using IVSDKDotNet;

namespace MoveImprove.ivsdk
{
    internal class SwitchTargets
    {
        private static bool gotSetting;
        private static uint defaultSetting;
        public static void UnInit()
        {
            gotSetting = false;
            IVMenuManager.SetSetting(eSettings.SETTING_AUTO_AIMING, defaultSetting);
        }
        public static void Tick()
        {
            GET_CURRENT_CHAR_WEAPON(Main.PlayerHandle, out int currWeap);
            if (IVWeaponInfo.GetWeaponInfo((uint)currWeap).WeaponSlot <= 1)
            {
                //IVMenuManager.RemapOptions.SetValue();
                if (!gotSetting)
                {
                    defaultSetting = IVMenuManager.GetSetting(eSettings.SETTING_AUTO_AIMING);
                    gotSetting = true;
                }

                IVMenuManager.SetSetting(eSettings.SETTING_AUTO_AIMING, 1);
            }
            else
            {
                IVMenuManager.SetSetting(eSettings.SETTING_AUTO_AIMING, defaultSetting);
            }
        }
    }
}
