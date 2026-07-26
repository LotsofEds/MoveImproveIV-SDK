using CCL.GTAIV;
using IVSDKDotNet;
using IVSDKDotNet.Enums;
using IVSDKDotNet.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Runtime;
using System.Windows.Forms;
using static IVSDKDotNet.Native.Natives;

namespace MoveImprove.ivsdk
{
    internal class GetUpCrouched
    {
        private static List<int> pedList = new List<int>();
        private static List<uint> timerList = new List<uint>();

        public static void UnInit()
        {
            pedList.Clear();
            timerList.Clear();
        }
        public static void Tick()
        {
            foreach (var ped in PedHelper.PedHandles)
            {
                int pedHandle = ped.Value;
                if (IS_CHAR_GETTING_UP(pedHandle) && !IS_PED_RAGDOLL(pedHandle) && (pedHandle != Main.PlayerHandle || IS_CHAR_DUCKING(pedHandle)))
                {
                    if ((!Main.GetUpCrouchNPC && pedHandle == Main.PlayerHandle) || Main.GetUpCrouchNPC)
                    {
                        if (IS_CHAR_PLAYING_ANIM(pedHandle, "get_up", "get_up_fast"))
                            TriggerDucking(pedHandle, "get_up", "get_up_fast", 0.4f);

                        else if (IS_CHAR_PLAYING_ANIM(pedHandle, "get_up", "get_up_normal"))
                            TriggerDucking(pedHandle, "get_up", "get_up_normal", 0.55f);

                        else if (IS_CHAR_PLAYING_ANIM(pedHandle, "get_up", "get_up_slow"))
                            TriggerDucking(pedHandle, "get_up", "get_up_slow", 0.45f);

                        else if (IS_CHAR_PLAYING_ANIM(pedHandle, "get_up_back", "get_up_fast"))
                            TriggerDucking(pedHandle, "get_up_back", "get_up_fast", 0.4f);

                        else if (IS_CHAR_PLAYING_ANIM(pedHandle, "get_up_back", "get_up_normal"))
                            TriggerDucking(pedHandle, "get_up_back", "get_up_normal", 0.3f);

                        else if (IS_CHAR_PLAYING_ANIM(pedHandle, "get_up_back", "get_up_slow"))
                            TriggerDucking(pedHandle, "get_up_back", "get_up_slow", 0.5f);
                    }
                }
            }

            for (int i = 0; i < pedList.Count; i++)
            {
                if (!DOES_CHAR_EXIST(pedList[i]) || Main.gTimer >= timerList[i] + 40)
                {
                    if (DOES_CHAR_EXIST(pedList[i]))
                    {
                        BLEND_FROM_NM_WITH_ANIM(pedList[i], "move_crouch", "idle2crouchidle", 8, 0, 0, 0);
                        SET_CHAR_ANIM_CURRENT_TIME(pedList[i], "move_crouch", "idle2crouchidle", 0.6f);
                        if (!IS_CHAR_DUCKING(pedList[i]))
                        {
                            if (pedList[i] == Main.PlayerHandle)
                                SET_CHAR_DUCKING_TIMED(pedList[i], -1);
                            else
                                SET_CHAR_DUCKING_TIMED(pedList[i], 100);
                        }
                    }
                    pedList.RemoveAt(i);
                    timerList.RemoveAt(i);
                }
            }
        }
        private static void TriggerDucking(int ped, string animGroup, string animName, float timeToStop)
        {
            GET_CHAR_ANIM_CURRENT_TIME(ped, animGroup, animName, out float animTime);
            if (animTime > timeToStop)
            {
                SWITCH_PED_TO_RAGDOLL(ped, 0, 500, true, true, true, false);

                if (!pedList.Contains(ped))
                {
                    pedList.Add(ped);
                    timerList.Add(Main.gTimer);
                }
            }
        }
    }
}
