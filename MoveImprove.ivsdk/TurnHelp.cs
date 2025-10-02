using CCL;
using CCL.GTAIV;
using IVSDKDotNet;
using IVSDKDotNet.Enums;
using System;
using System.Collections.Generic;
using System.Numerics;
using static IVSDKDotNet.Native.Natives;

namespace MoveImprove.ivsdk
{
    internal class TurnHelp
    {
        private static float frameTime;
        private static float turnAmount;
        private static float hdngMin;
        private static float hdngMax;

        public static void Init(SettingsFile settings)
        {
            turnAmount = settings.GetFloat("MAIN", "TurnAmount", 0);
        }
        public static void Tick()
        {
            if (IS_PLAYER_CONTROL_ON((int)Main.PlayerIndex))
            {
                GET_FRAME_TIME(out frameTime);
                NativeCamera cam = NativeCamera.GetGameCam();
                Vector3 dir = cam.Direction;
                GET_HEADING_FROM_VECTOR_2D(dir.X, dir.Y, out float camHdng);
                GET_CHAR_HEADING(Main.PlayerHandle, out float pHdng);

                if (camHdng >= 1)
                    hdngMin = camHdng - 1;
                else
                    hdngMin = 360 - camHdng;

                if (camHdng < 359)
                    hdngMax = camHdng + 1;
                else
                    hdngMax = camHdng - 359;
                //IVGame.ShowSubtitleMessage(pHdng.ToString() + "  " + camHdng.ToString() + "  " + hdngMin.ToString() + "  " + hdngMax.ToString());

                if (isTurningLeft() && !(pHdng > hdngMin && pHdng < hdngMax))
                    SET_CHAR_HEADING(Main.PlayerHandle, pHdng + turnAmount * frameTime);
                if (isTurningRight() && !(pHdng > hdngMin && pHdng < hdngMax))
                    SET_CHAR_HEADING(Main.PlayerHandle, pHdng - turnAmount * frameTime);
            }
        }
        private static bool isTurningLeft()
        {
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_l2") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint_turn_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_l2") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "sprint_turn_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_l2") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "sprint_turn_l"))
                return true;
            else
                return false;
        }
        private static bool isTurningRight()
        {
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_r2") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint_turn_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_r2") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "sprint_turn_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_r2") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "sprint_turn_r"))
                return true;
            else
                return false;
        }
    }
}
