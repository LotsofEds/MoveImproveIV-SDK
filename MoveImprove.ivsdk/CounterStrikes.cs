using System;
using System.Collections.Generic;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;
using IVSDKDotNet.Enums;
using System.Collections;

namespace MoveImprove.ivsdk
{
    internal class CounterStrikes
    {
        private static bool IsCountering;
        private static float CounterTime;
        private static uint pedHealth;
        private static int pedHandle;
        private static float finisherTime;

        private static uint lowDmg;
        private static uint medDmg;
        private static uint highDmg;
        private static uint batDmg;
        private static uint knifeDmg;
        private static void DamageLow()
        {
            IsCountering = true;
            GET_CHAR_HEALTH(pedHandle, out pedHealth);
            SET_CHAR_HEALTH(pedHandle, (pedHealth - lowDmg));
        }
        private static void DamageMed()
        {
            IsCountering = true;
            GET_CHAR_HEALTH(pedHandle, out pedHealth);
            SET_CHAR_HEALTH(pedHandle, (pedHealth - medDmg));
        }
        private static void DamageHigh()
        {
            IsCountering = true;
            GET_CHAR_HEALTH(pedHandle, out pedHealth);
            SET_CHAR_HEALTH(pedHandle, (pedHealth - highDmg));
        }
        private static void DamageBat()
        {
            IsCountering = true;
            GET_CHAR_HEALTH(pedHandle, out pedHealth);
            SET_CHAR_HEALTH(pedHandle, (pedHealth - batDmg));
        }
        private static void DamageKnife()
        {
            IsCountering = true;
            GET_CHAR_HEALTH(pedHandle, out pedHealth);
            SET_CHAR_HEALTH(pedHandle, (pedHealth - knifeDmg));
        }
        public static void Init(SettingsFile settings)
        {
            lowDmg = settings.GetUInteger("EXTENSIVE SETTINGS", "DamageLow", 10);
            medDmg = settings.GetUInteger("EXTENSIVE SETTINGS", "DamageMed", 15);
            highDmg = settings.GetUInteger("EXTENSIVE SETTINGS", "DamageHigh", 30);
            batDmg = settings.GetUInteger("EXTENSIVE SETTINGS", "DamageBat", 50);
            knifeDmg = settings.GetUInteger("EXTENSIVE SETTINGS", "DamageKnife", 70);
        }

        public static void Tick()
        {
            foreach (var ped in PedHelper.PedHandles)
            {
                pedHandle = ped.Value;

                if (!DOES_CHAR_EXIST(pedHandle)) continue;
                if (!LOCATE_CHAR_ON_FOOT_3D(pedHandle, Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z, 5, 5, 5, false)) continue;
                if (IS_CHAR_DEAD(pedHandle)) continue;
                if (IS_CHAR_INJURED(pedHandle)) continue;
                if (IS_CHAR_SITTING_IN_ANY_CAR(pedHandle)) continue;

                if (!IS_PED_IN_COMBAT(pedHandle))
                   SET_CHAR_READY_TO_BE_STUNNED(pedHandle, true);

                else
                    SET_CHAR_READY_TO_BE_STUNNED(pedHandle, false);

                if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_right_2") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left_2") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_back_2") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_right_3") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left_3") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_back_3") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_right") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_back") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_baseball_extra", "counter_left") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_baseball_extra", "counter_right") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_baseball_extra", "counter_back") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_knife_extra", "counter_left") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_knife_extra", "counter_right") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_knife_extra", "counter_back"))
                    IsCountering = false;

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_right_2") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_right_2"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_right_2", out CounterTime);
                    if (!IsCountering && CounterTime < 0.17 && CounterTime > 0.14)
                        DamageMed();

                    else if (!IsCountering && CounterTime < 0.27 && CounterTime > 0.24)
                        DamageMed();

                    else if (IsCountering && (CounterTime > 0.27 || (CounterTime < 0.24 && CounterTime > 0.17)))
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_back_3") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_back_3"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_back_3", out CounterTime);
                    if (!IsCountering && CounterTime < 0.23 && CounterTime > 0.2)
                        DamageHigh();

                    else if (IsCountering && CounterTime > 0.23)
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_left_2") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left_2"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_left_2", out CounterTime);
                    if (!IsCountering && CounterTime < 0.17 && CounterTime > 0.15)
                        DamageMed();

                    else if (!IsCountering && CounterTime < 0.32 && CounterTime > 0.3)
                        DamageMed();

                    else if (IsCountering && (CounterTime > 0.32 || (CounterTime < 0.3 && CounterTime > 0.17)))
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_left_3") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left_3"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_left_3", out CounterTime);
                    if (!IsCountering && CounterTime < 0.135 && CounterTime > 0.11)
                        DamageMed();

                    else if (!IsCountering && CounterTime < 0.345 && CounterTime > 0.32)
                        DamageMed();

                    else if (IsCountering && (CounterTime > 0.345 || (CounterTime < 0.32 && CounterTime > 0.135)))
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_right_3") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_right_3"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_right_3", out CounterTime);
                    if (!IsCountering && CounterTime < 0.135 && CounterTime > 0.11)
                        DamageMed();

                    else if (!IsCountering && CounterTime < 0.355 && CounterTime > 0.33)
                        DamageMed();

                    else if (IsCountering && (CounterTime > 0.355 || (CounterTime < 0.33 && CounterTime > 0.135)))
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_back_2") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_back_2"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_back_2", out CounterTime);
                    
                    if (!IsCountering && CounterTime < 0.17 && CounterTime > 0.14)
                        DamageLow();

                    else if (!IsCountering && CounterTime < 0.34 && CounterTime > 0.31)
                        DamageLow();

                    else if (!IsCountering && CounterTime < 0.49 && CounterTime > 0.46)
                        DamageLow();

                    else if (IsCountering && (CounterTime > 0.49 || (CounterTime < 0.46 && CounterTime > 0.34) || (CounterTime < 0.31 && CounterTime > 0.17)))
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_left") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_baseball_extra", "counter_left"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_baseball_extra", "counter_left", out CounterTime);
                    if (!IsCountering && CounterTime < 0.14 && CounterTime > 0.1)
                        DamageBat();

                    else if (IsCountering && CounterTime > 0.14)
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_right") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_baseball_extra", "counter_right"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_baseball_extra", "counter_right", out CounterTime);
                    if (!IsCountering && CounterTime < 0.21 && CounterTime > 0.17)
                        DamageBat();

                    else if (IsCountering && CounterTime > 0.21)
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_back") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_baseball_extra", "counter_back"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_baseball_extra", "counter_back", out CounterTime);
                    if (!IsCountering && CounterTime < 0.145 && CounterTime > 0.12)
                        DamageBat();

                    else if (IsCountering && CounterTime > 0.145)
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_left") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_knife_extra", "counter_left"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_knife_extra", "counter_left", out CounterTime);
                    if (!IsCountering && CounterTime < 0.33 && CounterTime > 0.3)
                        DamageKnife();

                    else if (IsCountering && CounterTime > 0.33)
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_right") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_knife_extra", "counter_right"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_knife_extra", "counter_right", out CounterTime);
                    if (!IsCountering && CounterTime < 0.33 && CounterTime > 0.3)
                        DamageKnife();

                    else if (IsCountering && CounterTime > 0.33)
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_back") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_knife_extra", "counter_back"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_knife_extra", "counter_back", out CounterTime);
                    if (!IsCountering && CounterTime < 0.33 && CounterTime > 0.3)
                        DamageKnife();

                    else if (IsCountering && CounterTime > 0.33)
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_back") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_back"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_back", out CounterTime);
                    if (!IsCountering && CounterTime < 0.205 && CounterTime > 0.17)
                        DamageMed();

                    else if (!IsCountering && CounterTime < 0.365 && CounterTime > 0.33)
                        DamageMed();

                    else if (IsCountering && (CounterTime > 0.365 || (CounterTime < 0.33 && CounterTime > 0.205)))
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_left") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_left", out CounterTime);
                    if (!IsCountering && CounterTime < 0.22 && CounterTime > 0.19)
                        DamageMed();

                    else if (!IsCountering && CounterTime < 0.36 && CounterTime > 0.33)
                        DamageMed();

                    else if (IsCountering && (CounterTime > 0.36 || (CounterTime < 0.33 && CounterTime > 0.22)))
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_right") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_right"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_right", out CounterTime);
                    if (!IsCountering && CounterTime < 0.185 && CounterTime > 0.15)
                        DamageMed();

                    else if (!IsCountering && CounterTime < 0.365 && CounterTime > 0.33)
                        DamageMed();

                    else if (IsCountering && (CounterTime > 0.365 || (CounterTime < 0.33 && CounterTime > 0.185)))
                        IsCountering = false;
                }

                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left_2"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_left_2", out finisherTime);
                    if (finisherTime > 0.65)
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "melee_counters", "counter_left_2", 1.75f);

                    else if (finisherTime > 0.85)
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_left_2", 1.0f);
                }

                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_right_3"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_right_3", out finisherTime);
                    if (finisherTime > 0.6)
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "melee_counters", "counter_right_3", 1.75f);

                    else if (finisherTime > 0.85)
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_right_3", 1.0f);
                }
            }
        }
    }
}
