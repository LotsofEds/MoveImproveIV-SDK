using System;
using System.Collections.Generic;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;
using IVSDKDotNet.Enums;

namespace MoveImprove.ivsdk
{
    internal class CounterStrikes
    {
        private static bool IsCountering;
        private static float CounterTime;
        private static uint pedHealth;
        private static int pedHandle;
        private static float finisherTime;
        private static void DamageLow()
        {
            IsCountering = true;
            GET_CHAR_HEALTH(pedHandle, out pedHealth);
            SET_CHAR_HEALTH(pedHandle, (pedHealth - 12));
        }
        private static void DamageMed()
        {
            IsCountering = true;
            GET_CHAR_HEALTH(pedHandle, out pedHealth);
            SET_CHAR_HEALTH(pedHandle, (pedHealth - 18));
        }
        private static void DamageHigh()
        {
            IsCountering = true;
            GET_CHAR_HEALTH(pedHandle, out pedHealth);
            SET_CHAR_HEALTH(pedHandle, (pedHealth - 25));
        }
        private static void DamageBat()
        {
            IsCountering = true;
            GET_CHAR_HEALTH(pedHandle, out pedHealth);
            SET_CHAR_HEALTH(pedHandle, (pedHealth - 36));
        }
        private static void DamageKnife()
        {
            IsCountering = true;
            GET_CHAR_HEALTH(pedHandle, out pedHealth);
            SET_CHAR_HEALTH(pedHandle, (pedHealth - 70));
        }
        public static void Tick()
        {
            foreach (var ped in PedHelper.PedHandles)
            {
                pedHandle = ped.Value;

                if (!DOES_CHAR_EXIST(pedHandle)) continue;
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

                    if (!IsCountering && CounterTime < 0.27 && CounterTime > 0.24)
                        DamageMed();

                    if (IsCountering && (CounterTime > 0.27 || (CounterTime < 0.24 && CounterTime > 0.17)))
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_back_3") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_back_3"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_back_3", out CounterTime);
                    if (!IsCountering && CounterTime < 0.23 && CounterTime > 0.2)
                        DamageHigh();

                    if (IsCountering && CounterTime > 0.23)
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_left_2") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left_2"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_left_2", out CounterTime);
                    if (!IsCountering && CounterTime < 0.17 && CounterTime > 0.15)
                        DamageMed();

                    if (!IsCountering && CounterTime < 0.32 && CounterTime > 0.3)
                        DamageMed();

                    if (IsCountering && (CounterTime > 0.32 || (CounterTime < 0.3 && CounterTime > 0.17)))
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_left_3") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left_3"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_left_3", out CounterTime);
                    if (!IsCountering && CounterTime < 0.135 && CounterTime > 0.11)
                        DamageMed();

                    if (!IsCountering && CounterTime < 0.345 && CounterTime > 0.32)
                        DamageMed();

                    if (IsCountering && (CounterTime > 0.345 || (CounterTime < 0.32 && CounterTime > 0.135)))
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_right_3") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_right_3"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_right_3", out CounterTime);
                    if (!IsCountering && CounterTime < 0.135 && CounterTime > 0.11)
                        DamageMed();

                    if (!IsCountering && CounterTime < 0.355 && CounterTime > 0.33)
                        DamageMed();

                    if (IsCountering && (CounterTime > 0.355 || (CounterTime < 0.33 && CounterTime > 0.135)))
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_back_2") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_back_2"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_back_2", out CounterTime);
                    if (!IsCountering && CounterTime < 0.17 && CounterTime > 0.14)
                        DamageLow();

                    if (!IsCountering && CounterTime < 0.34 && CounterTime > 0.31)
                        DamageLow();

                    if (!IsCountering && CounterTime < 0.49 && CounterTime > 0.46)
                        DamageLow();

                    if (IsCountering && (CounterTime > 0.49 || (CounterTime < 0.46 && CounterTime > 0.34) || (CounterTime < 0.31 && CounterTime > 0.17)))
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_left") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_baseball_extra", "counter_left"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_baseball_extra", "counter_left", out CounterTime);
                    if (!IsCountering && CounterTime < 0.14 && CounterTime > 0.1)
                        DamageBat();

                    if (IsCountering && CounterTime > 0.14)
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_right") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_baseball_extra", "counter_right"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_baseball_extra", "counter_right", out CounterTime);
                    if (!IsCountering && CounterTime < 0.21 && CounterTime > 0.17)
                        DamageBat();

                    if (IsCountering && CounterTime > 0.21)
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_back") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_baseball_extra", "counter_back"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_baseball_extra", "counter_back", out CounterTime);
                    if (!IsCountering && CounterTime < 0.145 && CounterTime > 0.12)
                        DamageBat();

                    if (IsCountering && CounterTime > 0.145)
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_left") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_knife_extra", "counter_left"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_knife_extra", "counter_left", out CounterTime);
                    if (!IsCountering && CounterTime < 0.33 && CounterTime > 0.3)
                        DamageKnife();

                    if (IsCountering && CounterTime > 0.33)
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_right") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_knife_extra", "counter_right"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_knife_extra", "counter_right", out CounterTime);
                    if (!IsCountering && CounterTime < 0.33 && CounterTime > 0.3)
                        DamageKnife();

                    if (IsCountering && CounterTime > 0.33)
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_back") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_knife_extra", "counter_back"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_knife_extra", "counter_back", out CounterTime);
                    if (!IsCountering && CounterTime < 0.33 && CounterTime > 0.3)
                        DamageKnife();

                    if (IsCountering && CounterTime > 0.33)
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_back") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_back"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_back", out CounterTime);
                    if (!IsCountering && CounterTime < 0.205 && CounterTime > 0.17)
                        DamageLow();

                    if (!IsCountering && CounterTime < 0.365 && CounterTime > 0.33)
                        DamageLow();

                    if (IsCountering && (CounterTime > 0.365 || (CounterTime < 0.33 && CounterTime > 0.205)))
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_left") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_left", out CounterTime);
                    if (!IsCountering && CounterTime < 0.22 && CounterTime > 0.19)
                        DamageLow();

                    if (!IsCountering && CounterTime < 0.36 && CounterTime > 0.33)
                        DamageLow();

                    if (IsCountering && (CounterTime > 0.36 || (CounterTime < 0.33 && CounterTime > 0.22)))
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_right") && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_right"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_right", out CounterTime);
                    if (!IsCountering && CounterTime < 0.185 && CounterTime > 0.15)
                        DamageLow();

                    if (!IsCountering && CounterTime < 0.365 && CounterTime > 0.33)
                        DamageLow();

                    if (IsCountering && (CounterTime > 0.365 || (CounterTime < 0.33 && CounterTime > 0.185)))
                        IsCountering = false;
                }

                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_left_2"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_left_2", out finisherTime);
                    if (finisherTime > 0.65)
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "melee_counters", "counter_left_2", 1.75f);

                    if (finisherTime > 0.85)
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_left_2", 1.0f);
                }

                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_counters", "counter_right_3"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_right_3", out finisherTime);
                    if (finisherTime > 0.6)
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "melee_counters", "counter_right_3", 1.75f);

                    if (finisherTime > 0.85)
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_counters", "counter_right_3", 1.0f);
                }
            }
        }
    }
}
