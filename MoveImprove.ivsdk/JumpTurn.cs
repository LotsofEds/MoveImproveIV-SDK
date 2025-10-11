using System;
using System.Collections.Generic;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;
using IVSDKDotNet.Enums;
using System.Numerics;

namespace MoveImprove.ivsdk
{
    internal class JumpTurn
    {
        private static float turnAmount;
        public static void Init(SettingsFile settings)
        {
            turnAmount = settings.GetFloat("MAIN", "JumpTurnAmount", 0);
        }
        public static void Tick()
        {
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_inair_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_inair_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_rifle", "jump_inair_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_rifle", "jump_inair_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "fall_glide") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "fall_fall"))
            {
                GET_CHAR_HEADING(Main.PlayerHandle, out float pHdng);
                if (NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) || NativeControls.IsGameKeyPressed(2, GameKey.MoveLeft))
                    SET_CHAR_HEADING(Main.PlayerHandle, pHdng + turnAmount * Main.frameTime);
                else if (NativeControls.IsGameKeyPressed(0, GameKey.MoveRight) || NativeControls.IsGameKeyPressed(2, GameKey.MoveRight))
                    SET_CHAR_HEADING(Main.PlayerHandle, pHdng - turnAmount * Main.frameTime);
            }
        }
    }
}
