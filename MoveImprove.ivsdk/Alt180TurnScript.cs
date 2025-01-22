using System;
using System.Collections.Generic;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;

namespace MoveImprove.ivsdk
{
    internal class Alt180TurnScript
    {
        private static float turnpntr;

        private static void StopUnarmed()
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
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                {
                    _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rifle", 5.00f, 0, 1, 0, 0, -1);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                    {
                        if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "idle"))
                        {
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "idle", 1.0f);
                        }
                    });
                }

                if (turnpntr > 0.5 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r", 0.87f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rifle", 5.00f, 0, 1, 0, 0, -1);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                        {
                            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "idle"))
                            {
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "idle", 1.0f);
                            }
                        });
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                {
                    _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rifle", 5.00f, 0, 1, 0, 0, -1);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                    {
                        if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "idle"))
                        {
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "idle", 1.0f);
                        }
                    });
                }

                if (turnpntr > 0.57 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rifle", 5.00f, 0, 1, 0, 0, -1);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                        {
                            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "idle"))
                            {
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sprint_turn_180_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "idle", 1.0f);
                            }
                        });
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180_r"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_r", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                {
                    _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rifle", 5.00f, 0, 1, 0, 0, -1);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                    {
                        if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "idle"))
                        {
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "idle", 1.0f);
                        }
                    });
                }

                if (turnpntr > 0.5 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_r", 0.87f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rifle", 5.00f, 0, 1, 0, 0, -1);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                        {
                            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "idle"))
                            {
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "idle", 1.0f);
                            }
                        });
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180_l"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_l", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                {
                    _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rifle", 5.00f, 0, 1, 0, 0, -1);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                    {
                        if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "idle"))
                        {
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "idle", 1.0f);
                        }
                    });
                }

                if (turnpntr > 0.57 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_l", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rifle", 5.00f, 0, 1, 0, 0, -1);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                        {
                            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "idle"))
                            {
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "idle", 1.0f);
                            }
                        });
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "run_turn_180"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                {
                    _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rifle", 5.00f, 0, 1, 0, 0, -1);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                    {
                        if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "idle"))
                        {
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "idle", 1.0f);
                        }
                    });
                }

                if (turnpntr > 0.65 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rifle", 5.00f, 0, 1, 0, 0, -1);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                        {
                            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "idle"))
                            {
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "run_turn_180", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "sstop_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rifle", "idle", 1.0f);
                            }
                        });
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                {
                    _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rpg", 5.00f, 0, 1, 0, 0, -1);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                    {
                        if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "idle"))
                        {
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "idle", 1.0f);
                        }
                    });
                }

                if (turnpntr > 0.5 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r", 0.87f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rpg", 5.00f, 0, 1, 0, 0, -1);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                        {
                            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "idle"))
                            {
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "idle", 1.0f);
                            }
                        });
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                {
                    _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rpg", 5.00f, 0, 1, 0, 0, -1);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                    {
                        if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "idle"))
                        {
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "idle", 1.0f);
                        }
                    });
                }

                if (turnpntr > 0.57 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rpg", 5.00f, 0, 1, 0, 0, -1);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                        {
                            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "idle"))
                            {
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sprint_turn_180_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "idle", 1.0f);
                            }
                        });
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_180_r"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_r", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                {
                    _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rpg", 5.00f, 0, 1, 0, 0, -1);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                    {
                        if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "idle"))
                        {
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "idle", 1.0f);
                        }
                    });
                }

                if (turnpntr > 0.5 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_r", 0.87f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rpg", 5.00f, 0, 1, 0, 0, -1);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                        {
                            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "idle"))
                            {
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "idle", 1.0f);
                            }
                        });
                    }
                }
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "run_turn_180_l"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_l", out turnpntr);
                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                {
                    _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rpg", 5.00f, 0, 1, 0, 0, -1);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                    {
                        if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "idle"))
                        {
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_l", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_r", 1.0f);
                            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "idle", 1.0f);
                        }
                    });
                }

                if (turnpntr > 0.57 && turnpntr < 0.7)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_l", 0.84f);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_rpg", 5.00f, 0, 1, 0, 0, -1);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                        {
                            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "idle"))
                            {
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "run_turn_180_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_l", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "sstop_r", 1.0f);
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_rpg", "idle", 1.0f);
                            }
                        });
                    }
                }
            }
        }
    }
}