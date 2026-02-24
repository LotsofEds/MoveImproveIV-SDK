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
        private static bool isBackFlipping;
        private static bool ResetAnim;
        private static bool isTackling;
        private static bool getUp;
        private static float animTime;
        private static uint fTimer;
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
        public static void DoBackFlip()
        {
            if (!IS_CHAR_GETTING_UP(Main.PlayerHandle) && !IS_CHAR_SWIMMING(Main.PlayerHandle) && !IS_CHAR_SITTING_IN_ANY_CAR(Main.PlayerHandle) && !IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle) && !IS_PED_RAGDOLL(Main.PlayerHandle) && !IS_CHAR_IN_AIR(Main.PlayerHandle))
            {
                if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_land_roll") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_takeoff_l") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_takeoff_r") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_on_spot") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_rifle", "jump_takeoff_l") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_rifle", "jump_takeoff_r") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_rifle", "jump_on_spot"))
                {
                    //_TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "jump_on_spot", "jump_std", 4.0f, 0, 1, 1, 0, -2);
                    isBackFlipping = true;
                }
            }
        }
        public static void Tick()
        {
            if (Main.TackleEnable)
            {
                if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && NativeControls.IsGameKeyPressed(0, GameKey.RadarZoom))
                {
                    if (!IS_CHAR_GETTING_UP(Main.PlayerHandle) && !IS_CHAR_SWIMMING(Main.PlayerHandle) && !IS_CHAR_SITTING_IN_ANY_CAR(Main.PlayerHandle) && !IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle) && !IS_PED_RAGDOLL(Main.PlayerHandle) && !IS_CHAR_IN_AIR(Main.PlayerHandle) && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "jump_grab"))
                    {
                        if (!HAVE_ANIMS_LOADED("misskbtruck"))
                            REQUEST_ANIMS("misskbtruck");
                        else
                        {
                            Main.PlayerPed.SetHeading(NativeCamera.GetGameCam().Rotation.Z);
                            _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "jump_grab", "misskbtruck", 4.0f, 0, 1, 1, 0, -2);
                            REMOVE_ANIMS("misskbtruck");
                            GET_GAME_TIMER(out fTimer);
                            isTackling = true;
                        }
                    }
                }
            }
            if (isTackling)
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "misskbtruck", "jump_grab", out animTime);
                if (animTime > 0.4 && !IS_PED_RAGDOLL(Main.PlayerHandle))
                {
                    Main.PlayerPed.ActivateDrunkRagdoll(-1);
                    APPLY_FORCE_TO_PED(Main.PlayerHandle, 0, 0, 10, -10, 0, 0, 0, 0, 1, 1, 1);
                    GET_GAME_TIMER(out fTimer);
                }
                GET_CHAR_VELOCITY(Main.PlayerHandle, out pVel);

                if (Main.gTimer >= fTimer + 500)
                {
                    if (getUp && IS_PED_RAGDOLL(Main.PlayerHandle) && pVel.Length() >= 0.6)
                        getUp = false;
                    if (IS_PED_RAGDOLL(Main.PlayerHandle) && pVel.Length() < 0.6)
                    {
                        if (!getUp)
                        {
                            GET_GAME_TIMER(out fTimer);
                            getUp = true;
                        }
                        else
                        {
                            SWITCH_PED_TO_ANIMATED(Main.PlayerHandle, false);
                            isTackling = false;
                            getUp = false;
                        }
                    }
                    else if (!IS_PED_RAGDOLL(Main.PlayerHandle))
                    {
                        isTackling = false;
                        getUp = false;
                    }
                }
            }
            if (isFlipping)
            {
                GET_CHAR_SPEED(Main.PlayerHandle, out float pSpeed);

                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "jump_std", "jump_land_roll", 1.35f);
                if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_land_roll") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "ragdoll_trans", "recover_balance"))
                {
                    Main.PlayerPed.ApplyForceRelative(new Vector3(0, -0.5f * pSpeed, 6.9f), new Vector3(0));
                    _TASK_PLAY_ANIM_WITH_FLAGS(Main.PlayerHandle, "jump_land_roll", "jump_std", 8.0f, -1, (int)AnimationFlags.RemoveSound | (int)AnimationFlags.StayAtNewPosition);
                    GET_GAME_TIMER(out fTimer);
                }
                else if (Main.gTimer >= fTimer + 650)
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "ragdoll_trans", "recover_balance", out float getUpTime);

                    if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "ragdoll_trans", "recover_balance"))
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "recover_balance", "ragdoll_trans", 4, 0, 0, 0, 0, -1);

                    else if (getUpTime > 0.35f || !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "ragdoll_trans", "recover_balance"))
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "ragdoll_trans", "recover_balance", 1.0f);
                        //SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "move_player", "idle", 1.0f);
                        isFlipping = false;
                    }
                }
            }

            if (isBackFlipping)
            {
                GET_CHAR_SPEED(Main.PlayerHandle, out float pSpeed);

                if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_land_roll") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "ragdoll_trans", "recover_balance"))
                {
                    Main.PlayerPed.ApplyForceRelative(new Vector3(0, -0.5f * pSpeed, 6.9f), new Vector3(0));
                    _TASK_PLAY_ANIM_WITH_FLAGS(Main.PlayerHandle, "jump_land_roll", "jump_std", 4.0f, -1, (int)AnimationFlags.RemoveSound | (int)AnimationFlags.StayAtNewPosition);
                    GET_GAME_TIMER(out fTimer);
                }

                if (Main.gTimer >= fTimer + 650)
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "ragdoll_trans", "recover_balance", out float getUpTime);

                    if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "ragdoll_trans", "recover_balance"))
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "recover_balance", "ragdoll_trans", 4, 0, 0, 0, 0, -1);

                    else if (getUpTime > 0.35f || !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "ragdoll_trans", "recover_balance"))
                    {
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "ragdoll_trans", "recover_balance", 1.0f);
                        ResetAnim = false;
                        isBackFlipping = false;
                    }
                }
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_land_roll"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "jump_std", "jump_land_roll", out float animTime);
                    if (!ResetAnim)
                    {
                        ResetAnim = true;
                        SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "jump_std", "jump_land_roll", 0.5f);
                    }

                    else if (animTime < 0.9)
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "jump_std", "jump_land_roll", -1.35f);
                }
            }
        }
    }
}
