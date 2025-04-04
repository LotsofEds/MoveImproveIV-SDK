
using System.Collections.Generic;
using System.Windows.Forms;
using static IVSDKDotNet.Native.Natives;
using CCL.GTAIV;
using IVSDKDotNet.Enums;
using System.IO;
using System.Diagnostics;
using System;
using System.Numerics;
using System.Runtime;
using System.Drawing;
using IVSDKDotNet;
using System.Diagnostics.PerformanceData;

namespace MoveImprove.ivsdk
{
    internal class RagdollFix
    {
        IVPed myPed;
        private static bool CheckDateTime;
        private static DateTime currentDateTime;
        private readonly static List<int> pList = new List<int>();
        private readonly static List<float> vList = new List<float>();
        private static List<float> aList = new List<float>();
        private static bool myRagdoll;
        public static void Tick()
        {
            if (CheckDateTime == false)
            {
                currentDateTime = DateTime.Now;
                CheckDateTime = true;
            }
            if (DateTime.Now.Subtract(currentDateTime).TotalMilliseconds > 50.0)
            {
                CheckDateTime = false;
                foreach (var ped in PedHelper.PedHandles)
                {
                    int pedHandle = ped.Value;
                    if (pList.Contains(pedHandle)) continue;
                    //myPed = NativeWorld.GetPedInstanceFromHandle(pedHandle);
                    float CounterTime = 0;

                    if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_right_2"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_right_2", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_right_3"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_right_3", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_left_2"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_left_2", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_left_3"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_left_3", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_back_2"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_back_2", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_back_3"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_back_3", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_left"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_baseball_extra", "hit_counter_left", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_right"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_baseball_extra", "hit_counter_right", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_back"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_baseball_extra", "hit_counter_back", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_left"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_knife_extra", "hit_counter_left", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_right"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_knife_extra", "hit_counter_right", out CounterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_back"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_knife_extra", "hit_counter_back", out CounterTime);

                    if (!IS_PED_RAGDOLL(pedHandle) && (IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", "ko_back") || IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", "ko_collapse") || IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", "ko_front") || IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", "ko_left") || IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", "ko_right")))
                    {
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(100), "Main", () =>
                        {
                            SWITCH_PED_TO_RAGDOLL_WITH_FALL(pedHandle, 1000, 1000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        });
                    }

                    GET_CHAR_VELOCITY(pedHandle, out Vector3 pedVel);
                    GET_CHAR_HEIGHT_ABOVE_GROUND(pedHandle, out float gHeight);
                    if ((pedVel.Z < -10.0 && !IS_PED_RAGDOLL(pedHandle) && gHeight > 4) || CounterTime > 0.65)
                    {
                        pList.Add(pedHandle);
                        vList.Add(pedVel.Z);
                        aList.Add(CounterTime);
                    }

                }
            }

            if (pList.Count > 0)
            {
                for (int i = pList.Count - 1; i >= 0; i--)
                {
                    if (!DOES_CHAR_EXIST(pList[i]) || IS_CHAR_DEAD(pList[i]) || IS_PED_RAGDOLL(pList[i]))
                    {
                        pList.Remove(pList[i]);
                        vList.Remove(vList[i]);
                        aList.Remove(aList[i]);
                    }
                }
            }

            foreach (var vped in pList)
            {
                if (aList[pList.IndexOf(vped)] > 0.65)
                {
                    SWITCH_PED_TO_RAGDOLL_WITH_FALL(vped, 1000, 1000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                }
                else if (vList[pList.IndexOf(vped)] < -10.0 && vped != Main.PlayerHandle)
                {
                    SWITCH_PED_TO_RAGDOLL_WITH_FALL(vped, 9999, 9999, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                }
            }

            if (!IS_PED_RAGDOLL(Main.PlayerHandle) && (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "dam_ko", "ko_back") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "dam_ko", "ko_collapse") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "dam_ko", "ko_front") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "dam_ko", "ko_left") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "dam_ko", "ko_right")))
                SWITCH_PED_TO_RAGDOLL_WITH_FALL(Main.PlayerHandle, 1000, 1000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            GET_CHAR_VELOCITY(Main.PlayerHandle, out Vector3 pVel);
            if (pVel.Z < -10.0 && !IS_PED_RAGDOLL(Main.PlayerHandle))
            {
                SWITCH_PED_TO_RAGDOLL_WITH_FALL(Main.PlayerHandle, -1, -1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(1000), "Main", () =>
                {
                    myRagdoll = true;
                });
            }
            if (((pVel.X < 1.0 && pVel.X > -1.0 && pVel.Y < 1.0 && pVel.Y > -1.0 && pVel.Z < 1.0 && pVel.Z > -1.0) || IS_CHAR_SWIMMING(Main.PlayerHandle)) && myRagdoll)
            {
                Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(800), "Main", () =>
                {
                    if (pVel.X < 1.0 && pVel.X > -1.0 && pVel.Y < 1.0 && pVel.Y > -1.0 && pVel.Z < 1.0 && pVel.Z > -1.0 && myRagdoll)
                    {
                        SWITCH_PED_TO_ANIMATED(Main.PlayerHandle, false);
                        myRagdoll = false;
                    }
                });
            }
        }

        public static void Init()
        {
            pList.Clear();
            vList.Clear();
            aList.Clear();
        }
    }
}
