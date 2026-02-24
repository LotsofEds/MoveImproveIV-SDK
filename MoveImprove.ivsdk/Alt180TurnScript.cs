using System;
using System.Collections.Generic;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;
using IVSDKDotNet.Enums;

namespace MoveImprove.ivsdk
{
    internal class Alt180TurnScript
    {
        private static float turnpntr;
        private static bool Remove180Anim;
        private static void Process180Turn(string animSet, string animName, float skipStart, float skipEnd)
        {
            if (Remove180Anim)
            {
                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, animSet, animName, 1.0f);
                BLEND_OUT_CHAR_MOVE_ANIMS(Main.PlayerHandle);
            }
            else
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, animSet, animName, out turnpntr);

                if (turnpntr > skipStart)
                {
                    if (turnpntr < skipEnd)
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, animSet, animName, skipEnd);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        BLEND_OUT_CHAR_MOVE_ANIMS(Main.PlayerHandle);
                }
            }
        }
        public static void Init(SettingsFile settings)
        {
            Remove180Anim = settings.GetBoolean("MAIN", "Remove180Anim", false);
        }
        public static void Tick()
        {
            if (NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_turn_180_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_turn_180_r")))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_turn_180_l", 1.5f);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_turn_180_r", 1.5f);
            }
            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint_turn_180_r"))
                Process180Turn("move_player", "sprint_turn_180_r", 0.5f, 0.87f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint_turn_180_l"))
                Process180Turn("move_player", "sprint_turn_180_l", 0.57f, 0.84f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_180_r"))
                Process180Turn("move_player", "run_turn_180_r", 0.5f, 0.87f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_180_l"))
                Process180Turn("move_player", "run_turn_180_l", 0.57f, 0.84f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_180"))
                Process180Turn("move_player", "run_turn_180", 0.65f, 0.84f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r"))
                Process180Turn("move_rifle", "sprint_turn_180_r", 0.5f, 0.87f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l"))
                Process180Turn("move_rifle", "sprint_turn_180_l", 0.57f, 0.84f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180_r"))
                Process180Turn("move_rifle", "run_turn_180_r", 0.5f, 0.87f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180_l"))
                Process180Turn("move_rifle", "run_turn_180_l", 0.57f, 0.84f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180"))
                Process180Turn("move_rifle", "run_turn_180", 0.65f, 0.84f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r"))
                Process180Turn("move_rpg", "sprint_turn_180_r", 0.5f, 0.87f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l"))
                Process180Turn("move_rpg", "sprint_turn_180_l", 0.57f, 0.84f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_180_r"))
                Process180Turn("move_rpg", "run_turn_180_r", 0.5f, 0.87f);

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_180_l"))
                Process180Turn("move_rpg", "run_turn_180_l", 0.57f, 0.84f);
        }
    }
}