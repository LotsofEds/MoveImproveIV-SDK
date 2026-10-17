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
        // IniShit
        private static uint healthThresh;
        private static float staminaDrain;
        private static float fatigueDrain;

        private static bool leftCounter;
        private static bool rightCounter;
        private static bool fwdCounter;
        private static bool isTackling;
        private static int pedHandle;
        private static int currPed;
        private static float finisherTime;

        private static void ThrowPed(float dirX, float dirY, float dirZ)
        {
            if (!IS_PED_RAGDOLL(currPed))
            {
                SWITCH_PED_TO_RAGDOLL_WITH_FALL(currPed, 2500, 2500, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                APPLY_FORCE_TO_PED(currPed, 3, dirX, dirY, 0, 0, 0, 0, 0, 0, 1, 1);
                currPed = -1;
            }
        }
        public static void Init(SettingsFile settings)
        {
            healthThresh = settings.GetUInteger("MAIN", "GrabHealthThresh", 140);
            staminaDrain = settings.GetFloat("MAIN", "GrabStaminaDrain", 25);
            fatigueDrain = settings.GetFloat("MAIN", "GrabFatigueDrain", 1.0f);
        }
        public static void Tick()
        {
            GET_CURRENT_CHAR_WEAPON(Main.PlayerHandle, out int pWeap);
            if (pWeap == 0 && (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_back") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_r") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_low_l") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "dodge_low_r")))
            {
                if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@low", "jack_perp_ds") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@low", "jack_perp_ps") && (NativeControls.IsGameKeyPressed(0, Main.GrabKey) || NativeControls.IsGameKeyPressed(0, Main.GrabKey)))
                {
                    bool canThrow = false;

                    if (!HAVE_ANIMS_LOADED("missray2"))
                        REQUEST_ANIMS("missray2");
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
                        if (IS_CHAR_HEALTH_GREATER(pedHandle, healthThresh)) continue;

                        if (IS_PLAYER_TARGETTING_CHAR((int)Main.PlayerIndex, pedHandle))
                        {
                            if (Main.staminaBar >= staminaDrain)
                            {
                                canThrow = true;
                                currPed = pedHandle;
                                break;
                            }
                        }
                    }
                    if (canThrow)
                    {
                        if (NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) || NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft))
                        {
                            PlayThrowAnim("veh@std", "jack_perp_ds");
                            leftCounter = true;
                        }
                        else if (NativeControls.IsGameKeyPressed(0, GameKey.MoveRight) || NativeControls.IsGameKeyPressed(0, GameKey.MoveRight))
                        {
                            PlayThrowAnim("veh@std", "jack_perp_ps");
                            rightCounter = true;
                        }
                        else if (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) || NativeControls.IsGameKeyPressed(0, GameKey.MoveForward))
                        {
                            PlayThrowAnim("missray2", "gbge_throwrubbish");
                            fwdCounter = true;
                        }
                    }
                }
            }
            if (leftCounter)
            {
                if (ProcessThrowAnim("veh@std", "jack_perp_ds", 1.65f, 0.5f, 0.9f))
                    leftCounter = false;
            }
            else if (rightCounter)
            {
                if (ProcessThrowAnim("veh@std", "jack_perp_ps", 1.85f, 0.3f, 0.85f))
                    rightCounter = false;
            }
            else if (fwdCounter)
            {
                if (ProcessThrowAnim("missray2", "gbge_throwrubbish", 1.65f, 0.35f, 0.7f))
                    fwdCounter = false;
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
                    ThrowPed(dirX * 4, dirY * 4, 0);

                else if (rightCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "veh@std", "jack_perp_ps"))
                    ThrowPed(dirX * -4, dirY * -4, 0);

                else if (fwdCounter && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "missray2", "gbge_throwrubbish"))
                    ThrowPed(dirX * 4, dirY * 4, 0);
            }
        }

        private static void PlayThrowAnim(string animSet, string animName)
        {
            _TASK_PLAY_ANIM(Main.PlayerHandle, animName, animSet, 4.0f, 0, 0, 0, 0, -1);
            Main.staminaBar -= staminaDrain;
            Main.maxStamina -= fatigueDrain;
        }

        private static bool ProcessThrowAnim(string animSet, string animName, float speed, float timeSet, float timeEnd)
        {
            bool stopThrow = false;
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animSet, animName))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std", "jack_perp_ds", speed);
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@std", "jack_perp_ds", out float plyrTime);
                if (plyrTime < 0.2f)
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "veh@std", "jack_perp_ds", timeSet);
                }
                else if (plyrTime > timeEnd && plyrTime < 0.99)
                    stopThrow = true;
            }
            return stopThrow;
        }
    }
}
