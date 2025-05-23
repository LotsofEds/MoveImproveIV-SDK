﻿using System;
using System.Collections.Generic;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;

namespace MoveImprove.ivsdk
{
    internal class ForceRunning
    {
        private static bool isWalking(IVPed ped) => ((IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_b") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_c") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_down") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_up") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_turn_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_turn_l2") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_turn_l3") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_turn_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_turn_r2") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "walk_turn_r3")) && !(IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_down") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_up") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_l2") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "run_turn_r2") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint_turn_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_player", "sprint_turn_r")));
        private static bool pressMoveKeys() => (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) || NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) || NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) || NativeControls.IsGameKeyPressed(0, GameKey.MoveRight));
        public static void Tick()
        {
            if (!HAVE_ANIMS_LOADED("move_fast"))
                REQUEST_ANIMS("move_fast");

            if (Main.SprintToVehicles == true && NativeControls.IsGameKeyPressed(0, GameKey.EnterCar) && !pressMoveKeys() && !IS_CHAR_DEAD(Main.PlayerHandle) && !IS_PED_RAGDOLL(Main.PlayerHandle) && !Main.PlayerPed.IsInVehicle())
                SET_ANIM_GROUP_FOR_CHAR(Main.PlayerHandle, "move_fast");

            if (Main.ForceRun == true)
                if (NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && !IS_PED_IN_COVER(Main.PlayerHandle))
                {
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(500), "Main", () =>
                    {
                        if (NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && isWalking(Main.PlayerPed))
                            SET_ANIM_GROUP_FOR_CHAR(Main.PlayerHandle, "move_fast");
                    });
                }

            if ((!NativeControls.IsGameKeyPressed(0, GameKey.EnterCar) || pressMoveKeys() || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_fast", "sprint")) && !NativeControls.IsGameKeyPressed(0, GameKey.Sprint))
                SET_ANIM_GROUP_FOR_CHAR(Main.PlayerHandle, "move_player");

            /*float moveState = Main.PlayerPed.PedMoveBlendOnFoot.MoveState;

            if (Main.SprintToVehicles == true && NativeControls.IsGameKeyPressed(0, GameKey.EnterCar) && !pressMoveKeys() && !IS_CHAR_DEAD(Main.PlayerHandle) && !IS_PED_RAGDOLL(Main.PlayerHandle) && !Main.PlayerPed.IsInVehicle())
            {
                if (moveState < 3.0f && moveState != 0)
                    moveState += 0.05f;

                moveState = Main.Clamp(moveState, 0.0f, 3.0f);
                Main.PlayerPed.PedMoveBlendOnFoot.MoveState = moveState;
            }*/

            /*if (Main.ForceRun == true)
            {
                if (NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && !IS_PED_IN_COVER(Main.PlayerHandle))
                {
                    if (Main.PlayerPed.PlayerInfo.Stamina > 0)
                    {
                        if (moveState < 3.0f && moveState != 0)
                            moveState += 0.2f;

                        moveState = Main.Clamp(moveState, 0.0f, 3.0f);
                    }
                    else
                    {
                        if (moveState < 2.0f && moveState != 0)
                            moveState += 0.2f;

                        moveState = Main.Clamp(moveState, 0.0f, 2.0f);
                    }
                    if (moveState == 3)
                    {
                        float pStamina = Main.PlayerPed.PlayerInfo.Stamina;
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(20), "Main", () =>
                        {
                            if (pStamina <= Main.PlayerPed.PlayerInfo.Stamina)
                            {
                                IVGame.ShowSubtitleMessage(moveState.ToString() + "  " + Main.PlayerPed.PlayerInfo.Stamina.ToString());
                                Main.PlayerPed.PlayerInfo.Stamina -= 1.25f;
                            }
                        });
                    }
                    Main.PlayerPed.PedMoveBlendOnFoot.MoveState = moveState;
                }
            }*/
        }
    }
}
