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

        private static string[] ParachuteAnims;
        private static string[] CounterAnims;
        private static string[] CounterAnims2;
        private static string[] KoAnims;

        public static void Init()
        {
            ParachuteAnims = GetParachuteAnims();
            CounterAnims = GetCounterAnims();
            CounterAnims2 = GetCounterAnims2();
            KoAnims = GetKoAnims();
        }
        public static void UnInit()
        {
            pList.Clear();
            tList.Clear();
        }
        private static bool isParachuting(int ped)
        {
            bool isPara = false;
            foreach (string anim in ParachuteAnims)
            {
                if (IS_CHAR_PLAYING_ANIM(ped, "parachute", anim))
                {
                    isPara = true;
                    break;
                }
            }
            return isPara;
        }
        private static bool isSlidingDown(int ped) => (IS_CHAR_PLAYING_ANIM(ped, "climb_std", "ladder_slide") || IS_CHAR_PLAYING_ANIM(ped, "climb_std", "ladder_jumpoff"));
        private static string[] GetParachuteAnims()
        {
            string[] anims = new string[] { "accelerate_2_idle", "accelerate_loop", "deccelerate", "dec_2_acc", "free_fall", "free_fall_deccelerate", "free_fall_fast",
                "free_fall_veer_left", "free_fall_veer_right", "full_brake_for_landing", "full_brake_loop", "hang_2_steer_l", "hang_2_steer_r", "hang_idle", "hang_idle2",
                "open_chute", "steer_abwt_l", "steer_abwt_r", "steer_ab_l", "steer_ab_r", "steer_l", "steer_l_less", "steer_l_trans", "steer_r", "steer_r_less", "steer_r_trans"};
            return anims;
        }
        private static string[] GetCounterAnims()
        {
            string[] anims = new string[] { "hit_counter_right_2", "hit_counter_right_3", "hit_counter_left_2", "hit_counter_left_3", "hit_counter_back_2", "hit_counter_back_3" };
            return anims;
        }
        private static string[] GetCounterAnims2()
        {
            string[] anims = new string[] { "hit_counter_right", "hit_counter_left", "hit_counter_back" };
            return anims;
        }
        private static string[] GetKoAnims()
        {
            string[] anims = new string[] { "ko_back", "ko_collapse", "ko_front", "ko_left", "ko_right" };
            return anims;
        }
        public static void Tick()
        {
            if (!Main.FixRagdoll)
                return;

            foreach (var ped in PedHelper.PedHandles)
            {
                int pedHandle = ped.Value;
                if (!DOES_CHAR_EXIST(pedHandle)) continue;
                //if (pList.Contains(pedHandle)) continue;
                if (!LOCATE_CHAR_ON_FOOT_3D(pedHandle, Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z, 30, 30, 30, false)) continue;

                if (!IS_PED_RAGDOLL(pedHandle))
                {
                    float counterTime = 0;

                    foreach (string animName in CounterAnims)
                    {
                        if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_counters", animName))
                            GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_counters", animName, out counterTime);
                    }

                    foreach (string animName in CounterAnims2)
                    {
                        if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_baseball_extra", animName))
                            GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_baseball_extra", animName, out counterTime);
                        
                        else if (IS_CHAR_PLAYING_ANIM(pedHandle, "melee_knife_extra", animName))
                            GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "melee_knife_extra", animName, out counterTime);
                    }

                    float animTime = 0;

                    foreach (string animName in KoAnims)
                    {
                        if (IS_CHAR_PLAYING_ANIM(pedHandle, "dam_ko", animName))
                            GET_CHAR_ANIM_CURRENT_TIME(pedHandle, "dam_ko", animName, out animTime);
                    }

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
                    else if ((pedVel.Z < -10.0 || (animTime > 0.1f && animTime < 0.9f) || counterTime > 0.65f) && !IS_PED_RAGDOLL(pedHandle)
                        && !isParachuting(pedHandle) && !isSlidingDown(pedHandle) && !pList.Contains(pedHandle))
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
    }
}
