using CCL;
using CCL.GTAIV;
using IVSDKDotNet;
using IVSDKDotNet.Enums;
using System;
using System.Collections.Generic;
using System.Numerics;
using static IVSDKDotNet.Native.Natives;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace MoveImprove.ivsdk
{
    internal class AdvancedClimbing
    {
        private static float groundDist, objDist;
        private static int ObjHandle;
        private static int LdrHandle;
        private static int cam;
        private static uint fTimer;
        private static bool IsGrabbingLedge;
        private static bool DoClimbDown;
        private static bool canClimbDown;
        private static bool onLadder;
        private static Vector3 camRot;

        private static float pHdng;
        private static bool checkForFall;
        public static Dictionary<UIntPtr, int> ObjHandles { get; private set; } = new Dictionary<UIntPtr, int>();

        public static void UnInit()
        {
            if (DOES_CAM_EXIST(cam))
            {
                DESTROY_CAM(cam);
                ACTIVATE_SCRIPTED_CAMS(false, false);
            }
            DELETE_OBJECT(ref ObjHandle);
            DELETE_OBJECT(ref LdrHandle);
        }
        public static void Tick()
        {
            /*if (Main.ClimbDown)
            {
                GET_CHAR_HEIGHT_ABOVE_GROUND(Main.PlayerHandle, out float pHeight);
                if (pHeight >= 3.25f)
                    IVGame.ShowSubtitleMessage(pHeight.ToString());
                if (pHeight < 3.25f && !canClimbDown)
                {
                    if (DOES_OBJECT_EXIST(LdrHandle))
                        DELETE_OBJECT(ref LdrHandle);
                    onLadder = false;
                    GET_GAME_TIMER(out fTimer);
                    //checkForFall = true;
                }
                else if (Main.gTimer <= fTimer + 50 && !canClimbDown)
                {
                    CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                    //FREEZE_CHAR_POSITION(Main.PlayerHandle, true);
                    if (DOES_OBJECT_EXIST(LdrHandle))
                        DELETE_OBJECT(ref LdrHandle);
                    GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.0f, 0.0f, -2.85f), out Vector3 pOffset);
                    GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.0f, -0.2f, -0.7f), out Vector3 tOffset);
                    CREATE_OBJECT(GET_HASH_KEY("bm_ladder"), Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z + 10f, out LdrHandle, true);
                    SET_OBJECT_VISIBLE(LdrHandle, false);
                    SET_OBJECT_COORDINATES(LdrHandle, pOffset);
                    GET_CHAR_HEADING(Main.PlayerHandle, out float pHding);
                    SET_OBJECT_HEADING(LdrHandle, pHding - 180);
                    SET_CHAR_COORDINATES(Main.PlayerHandle, tOffset);
                    //_TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "ladder_geton_top", "climb_std", 8.0f, 0, 1, 1, 1, -1);
                    _TASK_CLIMB_LADDER(Main.PlayerHandle, 0);
                    GET_GAME_TIMER(out fTimer);
                    //checkForFall = true;
                    canClimbDown = true;
                }
                /*if (IS_CHAR_IN_AIR(Main.PlayerHandle) && Main.gTimer <= fTimer + 1000)
                {
                    GET_CHAR_VELOCITY(Main.PlayerHandle, out Vector3 pVel);
                    if (pVel.Z < -6.5f)
                    {
                        CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                        GET_CHAR_HEADING(Main.PlayerHandle, out float pHdng);
                        SET_CHAR_HEADING(Main.PlayerHandle, pHdng + 180);
                        APPLY_FORCE_TO_PED(Main.PlayerHandle, 3, 0, 25.0f, 7.5f, 0.0f, 0, 0, 0, 1, 1, 1);
                        _TASK_SHIMMY(Main.PlayerHandle, 0);
                    }
                }
            }*/
            if (!DoClimbDown)
            {
                if (DOES_OBJECT_EXIST(ObjHandle))
                    DELETE_OBJECT(ref ObjHandle);
                if (Main.ClimbDown && ((NativeControls.IsGameKeyPressed(0, GameKey.RadarZoom) && NativeControls.IsGameKeyPressed(0, GameKey.LookBehind)) || IVGame.IsKeyPressed(Main.ClimbDownKey)) && !IS_CHAR_DEAD(Main.PlayerHandle) && !IS_PED_RAGDOLL(Main.PlayerHandle) && !IS_CHAR_GETTING_UP(Main.PlayerHandle) && !Main.PlayerPed.IsInVehicle() && !IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle) && !IS_CHAR_IN_WATER(Main.PlayerHandle))
                {
                    GET_CHAR_HEIGHT_ABOVE_GROUND(Main.PlayerHandle, out float pHeight);
                    if (!DOES_OBJECT_EXIST(ObjHandle) && pHeight < 1.1f)
                    {
                        DoClimbDown = true;
                        GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, 0f, 0.2f, 0f, out float pOffX, out float pOffY, out float pOffZ);
                        CREATE_OBJECT(GET_HASH_KEY("ec_nf_ghostball"), pOffX, pOffY, pOffZ, out ObjHandle, true);
                        SET_OBJECT_VISIBLE(ObjHandle, false);

                        //SET_OBJECT_RECORDS_COLLISIONS(ObjHandle, true);
                        GET_OBJECT_COORDINATES(ObjHandle, out float objX, out float objY, out float objZ);
                        GET_GROUND_Z_FOR_3D_COORD(objX, objY, objZ, out groundDist);
                        GET_DISTANCE_BETWEEN_COORDS_3D(objX, objY, objZ, objX, objY, groundDist, out objDist);
                        SET_OBJECT_COLLISION(ObjHandle, false);

                        if (objDist > 3.35f)
                        {
                            //CLEAR_CHAR_TASKS(Main.PlayerHandle);
                            FREEZE_CHAR_POSITION(Main.PlayerHandle, true);
                            CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                            GET_GAME_TIMER(out fTimer);
                            canClimbDown = true;
                        }
                        else
                            DoClimbDown = false;
                    }
                }
            }
            if (canClimbDown && !onLadder)
            {
                if (DOES_OBJECT_EXIST(LdrHandle))
                {
                    if (DOES_OBJECT_EXIST(ObjHandle))
                    {
                        _TASK_CLIMB_LADDER(Main.PlayerHandle, 0);
                        DELETE_OBJECT(ref ObjHandle);
                    }
                    else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "ladder_geton_top"))
                    {
                        GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "ladder_geton_top", out float ldrTime);
                        if (ldrTime > 0.75f)
                        {
                            //Main.PlayerPed.ActivateDrunkRagdoll(-1);
                            onLadder = true;
                        }
                    }
                    /*else if (Main.gTimer >= fTimer + 500)
                    {
                        DELETE_OBJECT(ref LdrHandle);
                        canClimbDown = false;
                    }*/
                }
                else if (Main.gTimer >= fTimer + 0 && !DOES_OBJECT_EXIST(LdrHandle))
                {
                    //GET_OBJECT_VELOCITY(ObjHandle, out float VobjX, out float VobjY, out float VobjZ);
                    //if (VobjZ < -4.0f)
                    GET_CHAR_HEIGHT_ABOVE_GROUND(Main.PlayerHandle, out float pHeight);
                    FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                    if (!HAS_OBJECT_COLLIDED_WITH_ANYTHING(ObjHandle) && pHeight < 1.1f)
                    {
                        GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.0f, 0.25f, -2.85f), out Vector3 pOffset);
                        CREATE_OBJECT(GET_HASH_KEY("bm_ladder"), Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z + 10f, out LdrHandle, true);
                        SET_OBJECT_VISIBLE(LdrHandle, false);
                        SET_OBJECT_COORDINATES(LdrHandle, pOffset);
                        GET_CHAR_HEADING(Main.PlayerHandle, out float pHding);
                        SET_OBJECT_HEADING(LdrHandle, pHding - 180);
                    }
                    else
                    {
                        DoClimbDown = false;
                        canClimbDown = false;
                        onLadder = false;
                    }
                }
            }
            if (onLadder)
            {
                /*if (IS_PED_RAGDOLL(Main.PlayerHandle))
                {
                    FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                    DELETE_OBJECT(ref LdrHandle);
                    //SWITCH_PED_TO_ANIMATED(Main.PlayerHandle, true);
                    //CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                    BLEND_FROM_NM_WITH_ANIM(Main.PlayerHandle, "climb_std", "waist_to_hang", 4, 0, 0, 0);
                    //_TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "waist_to_hang", "climb_std", 4.0f, 0, 1, 1, 0, -2);
                }
                else if (!IS_PED_RAGDOLL(Main.PlayerHandle) && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "waist_to_hang"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "waist_to_hang", out float animTime);
                    if (animTime > 0.75f)
                    FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                    else
                        FREEZE_CHAR_POSITION(Main.PlayerHandle, true);
                }
                else if (!IS_PED_RAGDOLL(Main.PlayerHandle) && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "ladder_geton_top") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "waist_to_hang"))
                {
                    FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                    //GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3 (0.0f, -0.5f, -1.0f), out Vector3 pOff);
                    //SET_CHAR_COORDINATES(Main.PlayerHandle, pOff);
                    CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                    //APPLY_FORCE_TO_PED(Main.PlayerHandle, 3, 0, 0.1f, 2.0f, 0, 0, 0, 0, 1, 1, 1);
                    _TASK_SHIMMY(Main.PlayerHandle, 0);
                    //_TASK_JUMP(Main.PlayerHandle, false);
                    DoClimbDown = false;
                    canClimbDown = false;
                    onLadder = false;
                }*/
                if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "ladder_geton_top") && DOES_OBJECT_EXIST(LdrHandle))
                {
                    DELETE_OBJECT(ref LdrHandle);
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) && !NativeControls.IsGameKeyPressed(2, GameKey.MoveBackward))
                        _TASK_SHIMMY(Main.PlayerHandle, 0);
                    GET_GAME_TIMER(out fTimer);
                }
                if (Main.gTimer > fTimer + 500 && !DOES_OBJECT_EXIST(LdrHandle))
                {
                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "ladder_idle"))
                        CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                    DoClimbDown = false;
                    canClimbDown = false;
                    onLadder = false;
                }
            }

            if (Main.JumpFromLedges == true && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "climb_idle"))
            {
                if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !IsGrabbingLedge)
                {
                    if (!DOES_CAM_EXIST(cam))
                    {
                        GET_ROOT_CAM(out int rCam);
                        GET_CAM_ROT(rCam, out Vector3 rCamRot);
                        CREATE_CAM((int)eCamType.CAM_SCRIPTED, out cam);
                        SET_CAM_FOV(cam, 60);
                        GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3 (0.0f, -0.08f, 0.8f), out Vector3 pOff);
                        SET_CAM_POS(cam, pOff);
                        SET_CAM_ROT(cam, rCamRot);
                        camRot = rCamRot;
                        SET_CAM_PROPAGATE(cam, true);
                        SET_CAM_ACTIVE(cam, true);
                        ACTIVATE_SCRIPTED_CAMS(true, true);
                    }
                    IsGrabbingLedge = true;
                }
                else if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && IsGrabbingLedge)
                    JumpFromLedge();

                else if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && IsGrabbingLedge && (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "climb_idle")))
                {
                    if (DOES_CAM_EXIST(cam))
                    {
                        DESTROY_CAM(cam);
                        ACTIVATE_SCRIPTED_CAMS(false, false);
                    }
                    IsGrabbingLedge = false;
                }
            }
        }
        private static void JumpFromLedge()
        {
            if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "climb_idle"))
            {
                GET_MOUSE_INPUT(out int mX, out int mY);
                float mSens = GET_MOUSE_SENSITIVITY();

                camRot += new Vector3(-mY * mSens * 4, 0, -mX * mSens * 4);
                camRot.X = Main.Clamp(camRot.X, -70, 70);
                SET_CAM_ROT(cam, camRot);
                if (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && NativeControls.IsGameKeyPressed(0, GameKey.Action))
                {
                    IsGrabbingLedge = false;
                    CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                    SET_CHAR_HEADING(Main.PlayerHandle, camRot.Z);
                    APPLY_FORCE_TO_PED(Main.PlayerHandle, 3, 0, 2.5f, 2.5f, 0.0f, 0, 0, 0, 1, 1, 1);
                    _TASK_SHIMMY(Main.PlayerHandle, 0);
                    if (DOES_CAM_EXIST(cam))
                    {
                        DESTROY_CAM(cam);
                        ACTIVATE_SCRIPTED_CAMS(false, false);
                    }
                    SET_CAM_BEHIND_PED(Main.PlayerHandle);
                }
            }
        }
    }
}
