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

        private static bool QuickTurnStop;
        private static bool Remove180Anim;
        private static void StopUnarmed()
        {
            if (QuickTurnStop)
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
        private static void StopArmed(string moveSet, string moveAnim)
        {
            if (QuickTurnStop)
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
        private static void StopAnims(string moveSet, string moveAnim)
        {
            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, moveSet, moveAnim, 1.0f);
            BLEND_OUT_CHAR_MOVE_ANIMS(Main.PlayerHandle);
        }
        public static void Init(SettingsFile settings)
        {
            QuickTurnStop = settings.GetBoolean("IMPROVE 180 TURN", "StopImmediately", false);
            Remove180Anim = settings.GetBoolean("IMPROVE 180 TURN", "Remove180Anim", false);
        }
        public static void Tick()
        {
            if (NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_turn_180_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_turn_180_r")))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_turn_180_l", 1.5f);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_turn_180_r", 1.5f);
            }
            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint_turn_180_r"))
            {
                if (Remove180Anim)
                    StopAnims("move_player", "sprint_turn_180_r");
                else
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
            }
            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint_turn_180_l"))
            {
                if (Remove180Anim)
                    StopAnims("move_player", "sprint_turn_180_l");
                else
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
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_180_r"))
            {
                if (Remove180Anim)
                    StopAnims("move_player", "run_turn_180_r");
                else
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
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_180_l"))
            {
                if (Remove180Anim)
                    StopAnims("move_player", "run_turn_180_l");
                else
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
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_180"))
            {
                if (Remove180Anim)
                    StopAnims("move_player", "run_turn_180");
                else
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
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r"))
            {
                if (Remove180Anim)
                    StopAnims("move_rifle", "sprint_turn_180_r");
                else
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r", out turnpntr);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed("move_rifle", "sprint_turn_180_r");

                    if (turnpntr > 0.5 && turnpntr < 0.7)
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r", 0.87f);
                        if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                            StopArmed("move_rifle", "sprint_turn_180_r");
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l"))
            {
                if (Remove180Anim)
                    StopAnims("move_rifle", "sprint_turn_180_l");
                else
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l", out turnpntr);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed("move_rifle", "sprint_turn_180_l");

                    if (turnpntr > 0.57 && turnpntr < 0.7)
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l", 0.84f);
                        if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                            StopArmed("move_rifle", "sprint_turn_180_l");
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180_r"))
            {
                if (Remove180Anim)
                    StopAnims("move_rifle", "run_turn_180_r");
                else
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_r", out turnpntr);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed("move_rifle", "run_turn_180_r");

                    if (turnpntr > 0.5 && turnpntr < 0.7)
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_r", 0.87f);
                        if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                            StopArmed("move_rifle", "run_turn_180_r");
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180_l"))
            {
                if (Remove180Anim)
                    StopAnims("move_rifle", "run_turn_180_l");
                else
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_l", out turnpntr);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed("move_rifle", "run_turn_180_l");

                    if (turnpntr > 0.57 && turnpntr < 0.7)
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_l", 0.84f);
                        if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                            StopArmed("move_rifle", "run_turn_180_l");
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180"))
            {
                if (Remove180Anim)
                    StopAnims("move_rifle", "run_turn_180");
                else
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180", out turnpntr);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed("move_rifle", "run_turn_180");

                    if (turnpntr > 0.65 && turnpntr < 0.7)
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180", 0.84f);
                        if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                            StopArmed("move_rifle", "run_turn_180");
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r"))
            {
                if (Remove180Anim)
                    StopAnims("move_rpg", "sprint_turn_180_r");
                else
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r", out turnpntr);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed("move_rpg", "sprint_turn_180_r");

                    if (turnpntr > 0.5 && turnpntr < 0.7)
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r", 0.87f);
                        if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                            StopArmed("move_rpg", "sprint_turn_180_r");
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l"))
            {
                if (Remove180Anim)
                    StopAnims("move_rpg", "sprint_turn_180_l");
                else
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l", out turnpntr);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed("move_rpg", "sprint_turn_180_l");

                    if (turnpntr > 0.57 && turnpntr < 0.7)
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l", 0.84f);
                        if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                            StopArmed("move_rpg", "sprint_turn_180_l");
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_180_r"))
            {
                if (Remove180Anim)
                    StopAnims("move_rpg", "run_turn_180_r");
                else
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_r", out turnpntr);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed("move_rpg", "run_turn_180_r");

                    if (turnpntr > 0.5 && turnpntr < 0.7)
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_r", 0.87f);
                        if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                            StopArmed("move_rpg", "run_turn_180_r");
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_180_l"))
            {
                if (Remove180Anim)
                    StopAnims("move_rpg", "run_turn_180_l");
                else
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_l", out turnpntr);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        StopArmed("move_rpg", "run_turn_180_l");

                    if (turnpntr > 0.57 && turnpntr < 0.7)
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_l", 0.84f);
                        if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                            StopArmed("move_rpg", "run_turn_180_l");
                    }
                }
            }
        }
    }
}