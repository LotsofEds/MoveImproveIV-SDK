using System;
using System.Collections.Generic;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;
using IVSDKDotNet.Enums;
using System.Collections;
using System.Numerics;

namespace MoveImprove.ivsdk
{
    internal class GrabAndThrow
    {
        private static bool leftCounter;
        private static bool rightCounter;
        private static bool isTackling;
        private static int pedHandle;
        private static int currPed;
        private static float finisherTime;

        public static void Tick()
        {
            GET_CURRENT_CHAR_WEAPON(Main.PlayerHandle, out int pWeap);
            if (pWeap == 0 && (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_back") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_r")))
            {
                if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@low", "jack_perp_ds") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@low", "jack_perp_ps") && (NativeControls.IsGameKeyPressed(0, GameKey.Action) || NativeControls.IsGameKeyPressed(0, GameKey.Action)))
                {
                    //if (!HAVE_ANIMS_LOADED("misspackie1"))
                    //REQUEST_ANIMS("misspackie1");
                    if (!HAVE_ANIMS_LOADED("veh@low"))
                        REQUEST_ANIMS("veh@low");

                    foreach (var ped in PedHelper.PedHandles)
                    {
                        pedHandle = ped.Value;

                        if (!DOES_CHAR_EXIST(pedHandle)) continue;
                        if (!LOCATE_CHAR_ON_FOOT_3D(pedHandle, Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z, 5, 5, 5, false)) continue;
                        if (IS_CHAR_DEAD(pedHandle)) continue;
                        if (IS_CHAR_INJURED(pedHandle)) continue;
                        if (IS_CHAR_SITTING_IN_ANY_CAR(pedHandle)) continue;

                        if (IS_PLAYER_TARGETTING_CHAR((int)Main.PlayerIndex, pedHandle))
                            currPed = pedHandle;
                    }
                    if (NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) || NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft))
                    {
                        _TASK_PLAY_ANIM(Main.PlayerHandle, "jack_perp_ds", "veh@low", 8.0f, 0, 0, 0, 0, -1);
                        leftCounter = true;
                    }
                    else if (NativeControls.IsGameKeyPressed(0, GameKey.MoveRight) || NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM(Main.PlayerHandle, "jack_perp_ps", "veh@low", 8.0f, 0, 0, 0, 0, -1);
                        rightCounter = true;
                    }
                }
            }
            if (leftCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@low", "jack_perp_ds"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@low", "jack_perp_ds", out float plyrTime);
                if (plyrTime < 0.2)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@low", "jack_perp_ds", 0.5f);
                }
                else if (plyrTime > 0.9 && plyrTime < 0.95)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@low", "jack_perp_ds", 0.95f);
                    leftCounter = false;
                }
            }
            else if (rightCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@low", "jack_perp_ps"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@low", "jack_perp_ps", out float plyrTime);
                if (plyrTime < 0.2)
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@low", "jack_perp_ps", 0.35f);

                else if (plyrTime > 0.7 && plyrTime < 0.85)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@low", "jack_perp_ps", 0.95f);
                    rightCounter = false;
                }
            }

            if (DOES_CHAR_EXIST(currPed))
            {
                if (leftCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@low", "jack_perp_ds"))
                {
                    if (!IS_PED_RAGDOLL(currPed))
                        SWITCH_PED_TO_RAGDOLL_WITH_FALL(currPed, 2500, 2500, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    else
                    {
                        APPLY_FORCE_TO_PED(currPed, 3, 4, -1, 0, 0, 0, 0, 0, 1, 1, 1);
                        currPed = -1;
                    }
                }
                else if (rightCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@low", "jack_perp_ps"))
                {
                    if (!IS_PED_RAGDOLL(currPed))
                        SWITCH_PED_TO_RAGDOLL_WITH_FALL(currPed, 2500, 2500, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    else
                    {
                        APPLY_FORCE_TO_PED(currPed, 3, -4, -1, 0, 0, 0, 0, 0, 1, 1, 1);
                        currPed = -1;
                    }
                }
            }
        }
    }
}
