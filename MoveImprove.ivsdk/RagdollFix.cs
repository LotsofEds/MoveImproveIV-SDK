using CCL.GTAIV;
using IVSDKDotNet;
using IVSDKDotNet.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Runtime;
using System.Windows.Forms;
using static IVSDKDotNet.Native.Natives;

namespace MoveImprove.ivsdk
{
    internal class RagdollFix
    {
        public readonly static List<int> pList = new List<int>();
        public readonly static List<uint> tList = new List<uint>();
        private static bool isParachuting(int ped)=> (IS_CHAR_PLAYING_ANIM(ped, "parachute", "accelerate_2_idle") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "accelerate_loop") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "deccelerate") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "dec_2_acc") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "free_fall") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "free_fall_decelerate") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "free_fall_fast") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "free_fall_veer_left") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "free_fall_veer_right") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "full_brake_for_landing") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "full_brake_loop") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "hang_2_steer_l") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "hang_2_steer_r") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "hang_idle") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "hang_idle2") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "hang_2_steer_l") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "open_chute") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "steer_abwt_l") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "steer_abwt_r") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "steer_ab_l") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "steer_ab_r") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "steer_l") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "steer_l_less") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "steer_l_trans") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "steer_r") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "steer_r_less") || IS_CHAR_PLAYING_ANIM(ped, "parachute", "steer_r_trans"));
        private static bool isSlidingDown(int ped) => (IS_CHAR_PLAYING_ANIM(ped, "climb_std", "ladder_slide") || IS_CHAR_PLAYING_ANIM(ped, "climb_std", "ladder_jumpoff"));
        public static void Tick()
        {
            foreach (var ped in PedHelper.PedHandles)
            {
                int pedHandle = ped.Value;
                if (!DOES_CHAR_EXIST(pedHandle)) continue;
                //if (pList.Contains(pedHandle)) continue;
                if (!LOCATE_CHAR_ON_FOOT_3D(pedHandle, Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z, 30, 30, 30, false)) continue;

                if (!IS_PED_RAGDOLL(pedHandle))
                {
                    float counterTime = 0;

                    if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_right_2"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_right_2", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_right_3"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_right_3", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_left_2"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_left_2", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_left_3"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_left_3", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_back_2"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_back_2", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", "hit_counter_back_3"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", "hit_counter_back_3", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_left"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_baseball_extra", "hit_counter_left", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_right"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_baseball_extra", "hit_counter_right", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", "hit_counter_back"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_baseball_extra", "hit_counter_back", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_left"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_knife_extra", "hit_counter_left", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_right"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_knife_extra", "hit_counter_right", out counterTime);

                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", "hit_counter_back"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_knife_extra", "hit_counter_back", out counterTime);

                    float animTime = 0;
                    if (IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", "ko_back"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "dam_ko", "ko_back", out animTime);
                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", "ko_collapse"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "dam_ko", "ko_collapse", out animTime);
                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", "ko_front"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "dam_ko", "ko_front", out animTime);
                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", "ko_left"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "dam_ko", "ko_left", out animTime);
                    else if (IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", "ko_right"))
                        GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "dam_ko", "ko_right", out animTime);

                    //if (animTime > 0.1f)
                    //SWITCH_PED_TO_RAGDOLL_WITH_FALL(pedHandle, 1000, 1000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                    GET_CURRENT_CHAR_WEAPON(pedHandle, out int pWeap);
                    GET_CHAR_VELOCITY(pedHandle, out Vector3 pedVel);

                    if (pWeap == 41 && !pList.Contains(pedHandle))
                    {
                        pList.Add(pedHandle);
                        tList.Add(Main.gTimer);
                    }

                    //GET_CHAR_HEIGHT_ABOVE_GROUND(pedHandle, out float gHeight);
                    else if ((pedVel.Z < -10.0 || (animTime > 0.1f && animTime < 0.9f) || counterTime > 0.65f) && !IS_PED_RAGDOLL(pedHandle) && !isParachuting(pedHandle) && !isSlidingDown(pedHandle) && !pList.Contains(pedHandle))
                    {
                        //IVGame.ShowSubtitleMessage(pedVel.Z.ToString() + "  " + animTime.ToString() + "  " + counterTime.ToString());
                        SWITCH_PED_TO_RAGDOLL_WITH_FALL(pedHandle, 1000, 1000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        //pList.Add(pedHandle);
                        //vList.Add(pedVel.Z);
                        //aList.Add(counterTime);
                        //tList.Add(Main.gTimer);
                    }
                }
            }

            for (int i = 0; i < pList.Count; i++)
            {
                if (!DOES_CHAR_EXIST(pList[i]) || Main.gTimer >= tList[i] + 500)
                {
                    pList.RemoveAt(i);
                    tList.RemoveAt(i);
                }
                else
                {
                    GET_CURRENT_CHAR_WEAPON(pList[i], out int pWeap);
                    if (pWeap == 41)
                    {
                        GET_GAME_TIMER(out uint fTimer);
                        tList[i] = fTimer;
                    }
                }
            }
        }
        public static void UnInit()
        {
            pList.Clear();
            tList.Clear();
        }
    }
}
