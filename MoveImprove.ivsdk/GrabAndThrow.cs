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
        private static bool fwdCounter;
        private static bool isTackling;
        private static int pedHandle;
        private static int currPed;
        private static float finisherTime;

        public static void Tick()
        {
            GET_CURRENT_CHAR_WEAPON(Main.PlayerHandle, out int pWeap);
            if (pWeap == 0 && (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_back") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_low_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_low_r")))
            {
                if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@low", "jack_perp_ds") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@low", "jack_perp_ps") && (NativeControls.IsGameKeyPressed(0, Main.GrabKey) || NativeControls.IsGameKeyPressed(0, Main.GrabKey)))
                {
                    if (!HAVE_ANIMS_LOADED("misspackie1"))
                        REQUEST_ANIMS("misspackie1");
                    if (!HAVE_ANIMS_LOADED("visemes@m_hi"))
                        REQUEST_ANIMS("visemes@m_hi");
                    if (!HAVE_ANIMS_LOADED("missray2"))
                        REQUEST_ANIMS("missray2");
                    //if (!HAVE_ANIMS_LOADED("veh@bike_dirt"))
                    //REQUEST_ANIMS("veh@bike_dirt");
                    //if (!HAVE_ANIMS_LOADED("veh@low"))
                    //REQUEST_ANIMS("veh@low");
                    if (!HAVE_ANIMS_LOADED("veh@std"))
                        REQUEST_ANIMS("veh@std");

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
                        _TASK_PLAY_ANIM(Main.PlayerHandle, "jack_perp_ds", "veh@std", 4.0f, 0, 0, 0, 0, -1);
                        leftCounter = true;
                    }
                    else if (NativeControls.IsGameKeyPressed(0, GameKey.MoveRight) || NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                    {
                        _TASK_PLAY_ANIM(Main.PlayerHandle, "jack_perp_ps", "veh@std", 4.0f, 0, 0, 0, 0, -1);
                        rightCounter = true;
                    }
                    else if (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) || NativeControls.IsGameKeyPressed(0, GameKey.MoveForward))
                    {
                        _TASK_PLAY_ANIM(Main.PlayerHandle, "gbge_throwrubbish", "missray2", 4.0f, 0, 0, 0, 0, -1);
                        fwdCounter = true;
                    }
                }
            }
            if (leftCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@std", "jack_perp_ds"))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std", "jack_perp_ds", 1.65f);
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@std", "jack_perp_ds", out float plyrTime);
                if (plyrTime < 0.2)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@std", "jack_perp_ds", 0.5f);
                }
                else if (plyrTime > 0.9 && plyrTime < 0.95)
                {
                    //SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@std", "jack_perp_ds", 0.95f);
                    leftCounter = false;
                }
            }
            else if (rightCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@std", "jack_perp_ps"))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std", "jack_perp_ps", 1.85f);
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@std", "jack_perp_ps", out float plyrTime);
                if (plyrTime < 0.2)
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@std", "jack_perp_ps", 0.3f);

                else if (plyrTime > 0.85 && plyrTime < 0.95)
                {
                    //_TASK_PLAY_ANIM(Main.PlayerHandle, "a", "visemes@m_hi", 8.0f, 0, 0, 0, 0, -1);
                    //SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "visemes@m_hi", "a", 1.0f);
                    rightCounter = false;
                }
            }
            else if (fwdCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "missray2", "gbge_throwrubbish"))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "missray2", "gbge_throwrubbish", 1.65f);
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "missray2", "gbge_throwrubbish", out float plyrTime);
                if (plyrTime < 0.2)
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "missray2", "gbge_throwrubbish", 0.35f);

                else if (plyrTime > 0.7 && plyrTime < 0.85)
                {
                    fwdCounter = false;
                }
            }

            if (DOES_CHAR_EXIST(currPed))
            {
                GET_CHAR_HEADING(Main.PlayerHandle, out float pHdng);
                if (!fwdCounter)
                    pHdng += 90;

                pHdng *= (float)Math.PI / 180f;
                float dirX = (float)(0.0 - Math.Sin(pHdng));
                float dirY = (float)Math.Cos(pHdng);
                float dirZ = 0f;

                if (leftCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@std", "jack_perp_ds"))
                {
                    if (!IS_PED_RAGDOLL(currPed))
                    {
                        SWITCH_PED_TO_RAGDOLL_WITH_FALL(currPed, 2500, 2500, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        APPLY_FORCE_TO_PED(currPed, 3, dirX * 4, dirY * 4, 0, 0, 0, 0, 0, 0, 1, 1);
                        currPed = -1;
                    }
                }
                else if (rightCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@std", "jack_perp_ps"))
                {
                    if (!IS_PED_RAGDOLL(currPed))
                    {
                        SWITCH_PED_TO_RAGDOLL_WITH_FALL(currPed, 2500, 2500, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        APPLY_FORCE_TO_PED(currPed, 3, dirX * -4, dirY * -4, 0, 0, 0, 0, 0, 0, 1, 1);
                        currPed = -1;
                    }
                }
                else if (fwdCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "missray2", "gbge_throwrubbish"))
                {
                    if (!IS_PED_RAGDOLL(currPed))
                    {
                        SWITCH_PED_TO_RAGDOLL_WITH_FALL(currPed, 2500, 2500, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        APPLY_FORCE_TO_PED(currPed, 3, dirX * 4, dirY * 4, 0, 0, 0.0f, 0, 0, 0, 1, 1);
                        currPed = -1;
                    }
                }
            }
        }
    }
}
