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
        private static int LdrHandle;
        private static int cam;
        private static uint fTimer;
        private static bool IsGrabbingLedge;
        private static bool DoClimbDown;
        private static bool canClimbDown;
        private static bool canClimbUp;
        private static bool onLadder;
        private static Vector3 camRot;

        private static Vector3[] offsets;
        private static float[] groundPos;
        //private static bool[] isBlocked;
        private static int[] debugBalls;

        private static float pHdng;
        private static bool checkForFall;

        public static void UnInit()
        {
            if (DOES_CAM_EXIST(cam))
            {
                DESTROY_CAM(cam);
                ACTIVATE_SCRIPTED_CAMS(false, false);
            }
            DELETE_OBJECT(ref LdrHandle);

            for (int i = 0; i < 4; i++)
            {
                if (DOES_OBJECT_EXIST(debugBalls[i]))
                    DELETE_OBJECT(ref debugBalls[i]);
            }
        }
        public static void Init()
        {
            offsets = new Vector3[4];
            groundPos = new float[4];
            //isBlocked = new bool[4];
            debugBalls = new int[4];
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
            //ProcessClimbUp();
            ProcessClimbDown();
            ProcessJumpFromLedge();
        }
        private static void ProcessClimbUp()
        {
            if (canClimbUp)
            {
                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "hang_to_waist"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "hang_to_waist", out float animTime);
                    if (animTime > 0.75f)
                    {
                        //SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "hang_to_waist", 1.0f);
                        SET_CHAR_COORDINATES(Main.PlayerHandle, new Vector3(Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z + 1.0f));
                        CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                    }
                }
                else
                {
                    if (!IS_PED_CLIMBING(Main.PlayerHandle))
                    {
                        IVGame.ShowSubtitleMessage("fuckyou");
                        FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                        SET_CHAR_VELOCITY(Main.PlayerHandle, Vector3.Zero);
                        _TASK_SHIMMY(Main.PlayerHandle, 0);
                    }
                    else
                    {
                        canClimbUp = false;
                    }
                }
            }
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "climb_idle"))
            {
                GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.25f, 0f, 1.5f), out offsets[0]);
                GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(-0.25f, 0f, 1.5f), out offsets[1]);
                GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.25f, 0f, 2.25f), out offsets[2]);
                GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(-0.25f, 0f, 2.25f), out offsets[3]);

                /*for (int i = 0; i < 4; i++)
                {
                    if (!DOES_OBJECT_EXIST(debugBalls[i]))
                        CREATE_OBJECT(GET_HASH_KEY("ec_nf_ghostball"), Vector3.Zero, out debugBalls[i], true);
                    else
                        SET_OBJECT_COORDINATES(debugBalls[i], offsets[i]);
                }*/

                if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && IS_CONTROL_JUST_PRESSED(0, (int)GameKey.Jump))
                {
                    bool isBlocked = false;
                    if (IVWorld.ProcessLineOfSight(offsets[0], offsets[2], out IVLineOfSightResults pResults, 0))
                        isBlocked = true;
                    if (IVWorld.ProcessLineOfSight(offsets[1], offsets[3], out IVLineOfSightResults pResults2, 0))
                        isBlocked = true;

                    if (!isBlocked)
                    {
                        FREEZE_CHAR_POSITION(Main.PlayerHandle, true);
                        _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "hang_to_waist", "climb_std", 4.0f, 0, 0, 1, 0, -1);
                        canClimbUp = true;
                    }
                }
            }
        }
        private static void ProcessClimbDown()
        {
            if (!HAS_MODEL_LOADED(GET_HASH_KEY("bm_ladder")))
                REQUEST_MODEL(GET_HASH_KEY("bm_ladder"));

            if (!DoClimbDown)
            {
                if (Main.ClimbDown && (NativeControls.IsGameKeyPressed(0, GameKey.LookBehind) || IVGame.IsKeyPressed(Main.ClimbDownKey))
                    && !IS_CHAR_DEAD(Main.PlayerHandle) && !IS_PED_RAGDOLL(Main.PlayerHandle) && !IS_CHAR_GETTING_UP(Main.PlayerHandle)
                    && !Main.PlayerPed.IsInVehicle() && !IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle) && !IS_CHAR_IN_WATER(Main.PlayerHandle))
                {
                    GET_CHAR_HEIGHT_ABOVE_GROUND(Main.PlayerHandle, out float pHeight);

                    GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.25f, 0.15f, 0f), out offsets[0]);
                    GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(-0.25f, 0.15f, 0f), out offsets[1]);
                    GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(-0.25f, 0.8f, 0f), out offsets[2]);
                    GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.25f, 0.8f, 0f), out offsets[3]);

                    /*for (int i = 0; i < 4; i++)
                    {
                        if (!DOES_OBJECT_EXIST(debugBalls[i]))
                            CREATE_OBJECT(GET_HASH_KEY("ec_nf_ghostball"), Vector3.Zero, out debugBalls[i], true);
                        else
                            SET_OBJECT_COORDINATES(debugBalls[i], offsets[i]);
                    }*/

                    if (pHeight < 1.1f && !canClimbDown)
                    {
                        DoClimbDown = true;

                        bool canGoDown = true;
                        for (int i = 0; i < 4; i++)
                        {
                            GET_GROUND_Z_FOR_3D_COORD(offsets[i], out groundPos[i]);
                            GET_DISTANCE_BETWEEN_COORDS_3D(offsets[i].X, offsets[i].Y, offsets[i].Z, offsets[i].X, offsets[i].Y, groundPos[i], out float groundDist);

                            bool isBlocked = false;
                            if (i < 3)
                            {
                                if (IVWorld.ProcessLineOfSight(offsets[i], offsets[i + 1], out IVLineOfSightResults pResults, 0))
                                    isBlocked = true;
                            }
                            else
                            {
                                if (IVWorld.ProcessLineOfSight(offsets[i], offsets[0], out IVLineOfSightResults pResults, 0))
                                    isBlocked = true;
                            }

                            if (groundDist > 3.35f && !isBlocked)
                                continue;

                            canGoDown = false;
                            break;
                        }
                        for (int i = 3; i > -1; i--)
                        {
                            bool isBlocked = false;
                            if (i > 0)
                            {
                                if (IVWorld.ProcessLineOfSight(offsets[i], offsets[i - 1], out IVLineOfSightResults pResults, 0))
                                    isBlocked = true;
                            }
                            else
                            {
                                if (IVWorld.ProcessLineOfSight(offsets[i], offsets[3], out IVLineOfSightResults pResults, 0))
                                    isBlocked = true;
                            }

                            if (!isBlocked)
                                continue;

                            canGoDown = false;
                            break;
                        }

                        if (canGoDown)
                        {
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
                if (!DOES_OBJECT_EXIST(LdrHandle))
                {
                    FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                    GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.0f, 0.25f, -2.85f), out Vector3 pOffset);
                    CREATE_OBJECT(GET_HASH_KEY("bm_ladder"), Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z + 10f, out LdrHandle, true);
                    SET_OBJECT_VISIBLE(LdrHandle, false);
                    SET_OBJECT_COORDINATES(LdrHandle, pOffset);
                    GET_CHAR_HEADING(Main.PlayerHandle, out float pHding);
                    SET_OBJECT_HEADING(LdrHandle, pHding - 180);
                }
                else
                {
                    if (!IS_PED_CLIMBING(Main.PlayerHandle))
                        _TASK_CLIMB_LADDER(Main.PlayerHandle, 0);
                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "ladder_geton_top"))
                    {
                        GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "ladder_geton_top", out float animTime);
                        if (animTime > 0.75f)
                        {
                            //Main.PlayerPed.ActivateDrunkRagdoll(-1);
                            onLadder = true;
                        }
                    }
                    else if (Main.gTimer >= fTimer + 1000)
                    {
                        DELETE_OBJECT(ref LdrHandle);
                        canClimbDown = false;
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
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward))
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
        }
        private static void ProcessJumpFromLedge()
        {
            if (Main.JumpFromLedges == true)
            {
                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "climb_idle"))
                {
                    if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !IsGrabbingLedge)
                    {
                        if (!DOES_CAM_EXIST(cam))
                        {
                            GET_ROOT_CAM(out int rCam);
                            GET_CAM_ROT(rCam, out Vector3 rCamRot);
                            CREATE_CAM((int)eCamType.CAM_SCRIPTED, out cam);
                            SET_CAM_FOV(cam, 60);
                            GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.0f, -0.08f, 0.8f), out Vector3 pOff);
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

                    else if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && IsGrabbingLedge)
                        DeleteCam();
                }

                else if (IsGrabbingLedge)
                    DeleteCam();
            }
        }
        private static void DeleteCam()
        {
            if (DOES_CAM_EXIST(cam))
            {
                DESTROY_CAM(cam);
                ACTIVATE_SCRIPTED_CAMS(false, false);
            }
            IsGrabbingLedge = false;
        }
        private static void JumpFromLedge()
        {
            if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "climb_idle"))
            {
                GET_MOUSE_INPUT(out int mX, out int mY);
                float mSens = GET_MOUSE_SENSITIVITY();

                GET_POSITION_OF_ANALOGUE_STICKS(0, out int leftX, out int leftY, out int rightX, out int rightY);

                if (IS_USING_CONTROLLER())
                    camRot += new Vector3(-rightY * Main.stickSens * Main.frameTime, 0, -rightX * Main.stickSens * Main.frameTime);
                camRot += new Vector3(-mY * mSens * Main.mouseSens, 0, -mX * mSens * Main.mouseSens);
                camRot.X = Main.Clamp(camRot.X, -70, 70);
                SET_CAM_ROT(cam, camRot);
                if (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && NativeControls.IsGameKeyPressed(0, GameKey.Action))
                {
                    IsGrabbingLedge = false;
                    CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                    SET_CHAR_HEADING(Main.PlayerHandle, camRot.Z);
                    float jumpAmt = (Main.Clamp(camRot.X, -50, 50) + 50) / 100;
                    APPLY_FORCE_TO_PED(Main.PlayerHandle, 3, 0, 3.5f * jumpAmt, 3.0f * jumpAmt, 0.0f, 0, 0, 0, 1, 1, 1);
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
