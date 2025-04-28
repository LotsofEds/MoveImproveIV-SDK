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
        private static string moveSet = "";
        private static string moveAnim = "";

        private static void StopUnarmed()
        {
            if (Main.QuickTurnStop)
            {
                _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_player", 5.00f, 0, 1, 0, 0, -1);
                Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                {
                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "idle"))
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "idle", 1.0f);
                        CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                    }
                });
            }
            else
                BLEND_OUT_CHAR_MOVE_ANIMS(Main.PlayerHandle);
        }
        private static void StopArmed()
        {
            if (Main.QuickTurnStop)
            {
                _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", moveSet, 5.00f, 0, 1, 0, 0, -1);
                Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                {
                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, moveSet, "idle"))
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, moveSet, moveAnim, 1.0f);
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, moveSet, "sstop_l", 1.0f);
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, moveSet, "sstop_r", 1.0f);
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, moveSet, "rstop_l", 1.0f);
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, moveSet, "rstop_r", 1.0f);
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, moveSet, "idle", 1.0f);
                    }
                });
            }
            else
                StopUnarmed();
        }
        public static void Tick()
        {
            REQUEST_ANIMS("move_player");
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint_turn_180_r"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "sprint_turn_180_r", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopUnarmed();

                if (turnpntr > 0.5 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "sprint_turn_180_r", 0.87f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopUnarmed();
                }
            }
            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint_turn_180_l"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "sprint_turn_180_l", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopUnarmed();

                if (turnpntr > 0.57 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "sprint_turn_180_l", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopUnarmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_180_r"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "run_turn_180_r", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopUnarmed();

                if (turnpntr > 0.5 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "run_turn_180_r", 0.87f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopUnarmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_180_l"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "run_turn_180_l", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopUnarmed();

                if (turnpntr > 0.57 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "run_turn_180_l", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopUnarmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_180"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "run_turn_180", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopUnarmed();

                if (turnpntr > 0.65 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "run_turn_180", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopUnarmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r"))
            {
                moveSet = "move_rifle";
                moveAnim = "sprint_turn_180_r";
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopArmed();

                if (turnpntr > 0.5 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r", 0.87f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l"))
            {
                moveSet = "move_rifle";
                moveAnim = "sprint_turn_180_l";
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopArmed();

                if (turnpntr > 0.57 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180_r"))
            {
                moveSet = "move_rifle";
                moveAnim = "run_turn_180_r";
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_r", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopArmed();

                if (turnpntr > 0.5 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_r", 0.87f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180_l"))
            {
                moveSet = "move_rifle";
                moveAnim = "run_turn_180_l";
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_l", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopArmed();

                if (turnpntr > 0.57 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_l", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180"))
            {
                moveSet = "move_rifle";
                moveAnim = "run_turn_180";
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopArmed();

                if (turnpntr > 0.65 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r"))
            {
                moveSet = "move_rpg";
                moveAnim = "sprint_turn_180_r";
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopArmed();

                if (turnpntr > 0.5 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r", 0.87f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l"))
            {
                moveSet = "move_rpg";
                moveAnim = "sprint_turn_180_l";
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopArmed();

                if (turnpntr > 0.57 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_180_r"))
            {
                moveSet = "move_rpg";
                moveAnim = "run_turn_180_r";
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_r", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopArmed();

                if (turnpntr > 0.5 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_r", 0.87f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed();
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_180_l"))
            {
                moveSet = "move_rpg";
                moveAnim = "run_turn_180_l";
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_l", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    StopArmed();

                if (turnpntr > 0.57 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_l", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed();
                }
            }
        }
    }
}