using CCL.GTAIV;
using IVSDKDotNet;
using IVSDKDotNet.Enums;
using IVSDKDotNet.Native;
using System;
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
        public static void Tick()
        {
            foreach (var ped in PedHelper.PedHandles)
            {
                int pedHandle = ped.Value;
                if (IS_CHAR_GETTING_UP(pedHandle) && !IS_PED_RAGDOLL(pedHandle) && (pedHandle != Main.PlayerHandle || IS_CHAR_DUCKING(pedHandle)))
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
        private static void TriggerDucking(int ped, string animGroup, string animName, float timeToStop)
        {
            GET_CHAR_ANIM_CURRENT_TIME(ped, animGroup, animName, out float animTime);
            if (animTime > timeToStop)
            {
                SWITCH_PED_TO_RAGDOLL(ped, 0, 500, true, true, true, false);

                Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(40), "Main", () =>
                {
                    BLEND_FROM_NM_WITH_ANIM(ped, "move_crouch", "idle2crouchidle", 8, 0, 0, 0);
                    SET_CHAR_ANIM_CURRENT_TIME(ped, "move_crouch", "idle2crouchidle", 0.6f);
                    //SET_CHAR_ANIM_CURRENT_TIME(ped, animGroup, animName, 1.0f);
                    if (!IS_CHAR_DUCKING(ped))
                    {
                        if (ped == Main.PlayerHandle)
                            SET_CHAR_DUCKING_TIMED(ped, -1);
                        else
                            SET_CHAR_DUCKING_TIMED(ped, 100);
                    }
                });
            }
        }
    }
}
