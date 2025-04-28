using System;
using System.Collections.Generic;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;
using IVSDKDotNet.Enums;
using System.Numerics;

namespace MoveImprove.ivsdk
{
    internal class AdvancedClimbing
    {
        private static float groundDist, objDist;
        private static int ObjHandle;
        private static int LdrHandle;
        private static UIntPtr ObjPtr;
        private static float hdng;
        private static bool IsGrabbingLedge;
        private static bool DeleteObj;
        private static bool DoClimbDown;
        private static bool CanClimbDown;
        private static bool onLadder;
        public static Vector3 ObjPos { get; set; }
        public static Dictionary<UIntPtr, int> ObjHandles { get; private set; } = new Dictionary<UIntPtr, int>();
        public static void Tick()
        {
            if (!DoClimbDown && Main.ClimbDown && ((NativeControls.IsGameKeyPressed(0, GameKey.RadarZoom) && NativeControls.IsGameKeyPressed(0, GameKey.LookBehind)) || IVGame.IsKeyPressed(Main.ClimbDownKey)) && !IS_CHAR_DEAD(Main.PlayerHandle) && !IS_PED_RAGDOLL(Main.PlayerHandle) && !IS_CHAR_IN_AIR(Main.PlayerHandle) && !IS_CHAR_GETTING_UP(Main.PlayerHandle) && !Main.PlayerPed.IsInVehicle() && !IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle) && !IS_CHAR_SWIMMING(Main.PlayerHandle))
            {
                if (!DOES_OBJECT_EXIST(ObjHandle))
                {
                    CREATE_OBJECT(GET_HASH_KEY("cj_dart_1"), Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z + 10f, out ObjHandle, true);
                    ATTACH_OBJECT_TO_PED(ObjHandle, Main.PlayerHandle, (uint)eBone.BONE_ROOT, 0.0f, 0.2f, 0.0f, 0f, 0f, 0f, 0);
                    SET_OBJECT_VISIBLE(ObjHandle, false);
                    DoClimbDown = true;
                }
            }
            if (DoClimbDown)
            {
                DoClimbDown = false;
                if (!DOES_OBJECT_EXIST(ObjHandle))
                    return;

                GET_OBJECT_COORDINATES(ObjHandle, out float objX, out float objY, out float objZ);
                GET_GROUND_Z_FOR_3D_COORD(objX, objY, objZ, out groundDist);
                GET_DISTANCE_BETWEEN_COORDS_3D(objX, objY, objZ, objX, objY, groundDist, out objDist);
                if (DOES_OBJECT_EXIST(ObjHandle) && objDist > 3.25f)
                {
                    CanClimbDown = true;
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(20), "Main", () =>
                    {
                        DETACH_OBJECT(ObjHandle, true);

                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(450), "Main", () =>
                    {
                        GET_OBJECT_VELOCITY(ObjHandle, out float VobjX, out float VobjY, out float VobjZ);
                        if (VobjZ < -4.0f)
                        {
                            CREATE_OBJECT(GET_HASH_KEY("bm_ladder"), Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z + 10f, out LdrHandle, true);
                            DELETE_OBJECT(ref ObjHandle);
                            SET_OBJECT_VISIBLE(LdrHandle, false);
                            Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(50), "Main", () =>
                            {
                                ATTACH_OBJECT_TO_PED(LdrHandle, Main.PlayerHandle, (uint)eBone.BONE_ROOT, 0.0f, 0.25f, -2.8f, 0f, 0f, -3.2f, 0);
                                Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(20), "Main", () =>
                                {
                                    DETACH_OBJECT(LdrHandle, true);
                                    _TASK_CLIMB_LADDER(Main.PlayerHandle, 0);
                                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(500), "Main", () =>
                                    {
                                        onLadder = true;
                                    });
                                });
                            });
                        }
                        else
                            CanClimbDown = false;
                    });
                    });
                }
            }
            if (onLadder)
            {
                if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "ladder_geton_top"))
                {
                    DELETE_OBJECT(ref LdrHandle);
                    _TASK_JUMP(Main.PlayerHandle, true);
                    DoClimbDown = false;
                    onLadder = false;
                }
            }
            else if (!CanClimbDown && !DoClimbDown && DOES_OBJECT_EXIST(ObjHandle))
            {
                DELETE_OBJECT(ref ObjHandle);
                DoClimbDown = false;
            }

            if (Main.JumpFromLedges == true && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "climb_idle"))
            {
                if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !IsGrabbingLedge)
                {
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(200), "Main", () =>
                    {
                        if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && !IsGrabbingLedge)
                        {
                            IsGrabbingLedge = true;
                            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "climb_idle"))
                            {
                                GET_CHAR_HEADING(Main.PlayerHandle, out hdng);
                            }
                        }
                    });
                }
                else if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && IsGrabbingLedge)
                {
                    JumpFromLedge();
                }
                else if (!NativeControls.IsGameKeyPressed(0, GameKey.Aim) && IsGrabbingLedge && (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "climb_idle")))
                {
                    IsGrabbingLedge = false;
                    CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                    SET_CHAR_HEADING(Main.PlayerHandle, hdng);
                    _TASK_SHIMMY(Main.PlayerHandle, 0);
                    FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                }
            }
        }

        private static void JumpFromLedge()
        {
            if (NativeControls.IsGameKeyPressed(0, GameKey.Aim) && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "climb_idle"))
            {
                FREEZE_CHAR_POSITION(Main.PlayerHandle, true);
                _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "climb_idle", "climb_std", 8.0f, 0, 1, 0, 0, -1);
                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "climb_idle", 0.01f);
                if (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && NativeControls.IsGameKeyPressed(0, GameKey.Action))
                {
                    IsGrabbingLedge = false;
                    CLEAR_CHAR_TASKS_IMMEDIATELY(Main.PlayerHandle);
                    NativeCamera cam = NativeCamera.GetGameCam();
                    Vector3 dir = cam.Direction;
                    GET_HEADING_FROM_VECTOR_2D(dir.X, dir.Y, out float camHdng);
                    SET_CHAR_HEADING(Main.PlayerHandle, camHdng);
                    FREEZE_CHAR_POSITION(Main.PlayerHandle, true);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(80), "Main", () =>
                    {
                        FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                        APPLY_FORCE_TO_PED(Main.PlayerHandle, 0, 0, 100, 200, 0, 0, 0, 0, 1, 1, 1);
                        //SET_CHAR_VELOCITY
                        //SET_CHAR_VELOCITY(Main.PlayerHandle, 0, 10f, 20f);
                        _TASK_SHIMMY(Main.PlayerHandle, 0);
                    });
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(250), "Main", () =>
                    {
                        //SET_CHAR_VELOCITY(Main.PlayerHandle, 0, 0, 2f);
                    });
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(500), "Main", () =>
                    {
                        if (DOES_OBJECT_EXIST(ObjHandle))
                            DELETE_OBJECT(ref ObjHandle);
                    });
                }
            }
        }
    }
}
