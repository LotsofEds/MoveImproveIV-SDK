using System;
using System.Collections.Generic;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;
using IVSDKDotNet.Enums;
using System.Numerics;
using System.Collections;
using System.Windows.Forms;

namespace MoveImprove.ivsdk
{
    internal class FlipsNShit
    {
        private static bool isFlipping;
        private static bool isTackling;
        private static float animTime;
        private static Vector3 pVel;
        public static void DoFlip()
        {
            if (!IS_CHAR_GETTING_UP(Main.PlayerHandle) && !IS_CHAR_SWIMMING(Main.PlayerHandle) && !IS_CHAR_SITTING_IN_ANY_CAR(Main.PlayerHandle) && !IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle) && !IS_PED_RAGDOLL(Main.PlayerHandle) && !IS_CHAR_IN_AIR(Main.PlayerHandle))
            {
                if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_land_roll") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_takeoff_l") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_takeoff_r") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_on_spot") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_rifle", "jump_takeoff_l") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_rifle", "jump_takeoff_r") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_rifle", "jump_on_spot"))
                {
                    //_TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "jump_on_spot", "jump_std", 4.0f, 0, 1, 1, 0, -2);
                    isFlipping = true;
                }
            }
        }
        public static void Tick()
        {
            if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && NativeControls.IsGameKeyPressed(0, GameKey.RadarZoom))
            {
                if (!IS_CHAR_GETTING_UP(Main.PlayerHandle) && !IS_CHAR_SWIMMING(Main.PlayerHandle) && !IS_CHAR_SITTING_IN_ANY_CAR(Main.PlayerHandle) && !IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle) && !IS_PED_RAGDOLL(Main.PlayerHandle) && !IS_CHAR_IN_AIR(Main.PlayerHandle) && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "jump_grab"))
                {
                    IVGame.ShowSubtitleMessage(Main.PlayerPed.GetHeading().ToString() + "   " + NativeCamera.GetGameCam().Rotation.Z + 180.ToString());
                    if (!HAVE_ANIMS_LOADED("misskbtruck"))
                        REQUEST_ANIMS("misskbtruck");
                    else
                    {
                        Main.PlayerPed.SetHeading(NativeCamera.GetGameCam().Rotation.Z);
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "jump_grab", "misskbtruck", 4.0f, 0, 1, 1, 0, -2);
                        isTackling = true;
                    }
                }
            }
            if (isTackling)
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "misskbtruck", "jump_grab", out animTime);
                if (animTime > 0.4)
                {
                    Main.PlayerPed.ActivateDrunkRagdoll(-1);
                    APPLY_FORCE_TO_PED(Main.PlayerHandle, 0, 0, 10, -10, 0, 0, 0, 0, 1, 1, 1);
                }
                GET_CHAR_VELOCITY(Main.PlayerHandle, out pVel);
                //IVGame.ShowSubtitleMessage(pVel.Length().ToString());
                Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(500), "Main", () =>
                {
                    if (IS_PED_RAGDOLL(Main.PlayerHandle) && pVel.Length() < 0.6)
                    {
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(350), "Main", () =>
                        {
                            SWITCH_PED_TO_ANIMATED(Main.PlayerHandle, false);
                            isTackling = false;
                        });
                    }
                });
            }
            if (isFlipping)
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "jump_std", "jump_land_roll", 1.5f);
                /*GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "jump_std", "jump_on_spot", out animTime);
                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_on_spot") && animTime > 0.25)
                {*/
                    if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_land_roll"))
                    {
                        Main.PlayerPed.ApplyForceRelative(new Vector3(0, -1.25f, 7.0f), new Vector3(0));
                        //APPLY_FORCE_TO_PED(Main.PlayerHandle, 0, 0, 0, 6.25f, 0, 0, 0, 0, 1, 1, 1);
                        _TASK_PLAY_ANIM_WITH_FLAGS(Main.PlayerHandle, "jump_land_roll", "jump_std", 8.0f, -1, (int)AnimationFlags.RemoveSound | (int)AnimationFlags.StayAtNewPosition);

                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(800), "Main", () =>
                        {
                            _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "idle", "move_player", 4.0f, 0, 1, 0, 0, -1);
                            Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(400), "Main", () =>
                            {
                                CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                            });
                            isFlipping = false;
                        });
                    //}
                }
            }
        }
    }
}
