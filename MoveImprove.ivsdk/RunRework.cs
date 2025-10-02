using CCL;
using CCL.GTAIV;
using IVSDKDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static IVSDKDotNet.Native.Natives;
using static System.Windows.Forms.AxHost;

namespace MoveImprove.ivsdk
{
    internal class RunRework
    {
        // IniShit
        private static bool SprintToVehicles;
        private static bool ForceRun;
        private static bool ToggleSprint;

        private static float SprintDrain;
        private static float RunDrain;
        private static float WalkDrain;
        private static float StandDrain;
        private static float SwimFastDrain;
        private static float SwimDrain;
        private static float FloatDrain;
        private static float ClimbDrain;

        // OtherShit
        private static bool CapsPressed;
        private static bool IsCapsLockActive() => Control.IsKeyLocked(Keys.Capital);
        private static bool PressMoveKeys() => (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) || NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) || NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) || NativeControls.IsGameKeyPressed(0, GameKey.MoveRight));

        public static void Init(SettingsFile settings)
        {
            SprintToVehicles = settings.GetBoolean("STAMINA REWORK", "SprintToVehicles", false);
            ForceRun = settings.GetBoolean("STAMINA REWORK", "RunRework", false);
            ToggleSprint = settings.GetBoolean("STAMINA REWORK", "ToggleSprint", false);

            SprintDrain = settings.GetFloat("STAMINA REWORK", "SprintDrain", 20.0f);
            RunDrain = settings.GetFloat("STAMINA REWORK", "RunDrain", 2.0f);
            WalkDrain = settings.GetFloat("STAMINA REWORK", "WalkDrain", 2.0f);
            StandDrain = settings.GetFloat("STAMINA REWORK", "StandDrain", 2.0f);
            SwimFastDrain = settings.GetFloat("STAMINA REWORK", "SwimFastDrain", 20.0f);
            SwimDrain = settings.GetFloat("STAMINA REWORK", "SwimDrain", 5.0f);
            FloatDrain = settings.GetFloat("STAMINA REWORK", "FloatDrain", 5.0f);
            ClimbDrain = settings.GetFloat("STAMINA REWORK", "ClimbDrain", 2.0f);
        }
        public static void Tick()
        {
            GET_FRAME_TIME(out float frameTime);
            if (IS_PLAYER_CONTROL_ON((int)Main.PlayerIndex))
            {
                if (SprintToVehicles && !IS_CHAR_ON_FOOT(Main.PlayerHandle) && !IS_CHAR_IN_ANY_CAR(Main.PlayerHandle))
                {
                    float moveState = Main.PlayerPed.PedMoveBlendOnFoot.MoveState;
                    moveState += 10.0f * frameTime;
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Sprint))
                        moveState = Main.Clamp(moveState, 0.0f, 2.0f);

                    else
                        moveState = Main.Clamp(moveState, 0.0f, 3.0f);

                    Main.PlayerPed.PedMoveBlendOnFoot.MoveState = moveState;
                }

                if (!ForceRun)
                {
                    if (!IsCapsLockActive())
                    {
                        DISABLE_PLAYER_SPRINT((int)Main.PlayerIndex, true);
                        CapsPressed = false;
                    }
                    else if (!CapsPressed)
                    {
                        DISABLE_PLAYER_SPRINT((int)Main.PlayerIndex, false);
                        CapsPressed = true;
                    }
                }
                else
                {
                    DISABLE_PLAYER_SPRINT((int)Main.PlayerIndex, true);
                    if (NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && PressMoveKeys() && Main.PlayerPed.PlayerInfo.Stamina > -100)
                    {
                        float moveState = Main.PlayerPed.PedMoveBlendOnFoot.MoveState;
                        if (IsCapsLockActive() || !ToggleSprint)
                        {
                            moveState += 4.0f * frameTime;
                            moveState = Main.Clamp(moveState, 0.0f, 3.0f);
                        }
                        else
                        {
                            if (moveState <= 2)
                            {
                                moveState += 4.0f * frameTime;
                                moveState = Main.Clamp(moveState, 0.0f, 2.0f);
                            }
                        }
                        Main.PlayerPed.PedMoveBlendOnFoot.MoveState = moveState;
                    }
                }

                if (!IS_PLAYER_CLIMBING((int)Main.PlayerIndex) && !IS_CHAR_SWIMMING(Main.PlayerHandle))
                {
                    if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 2)
                        Main.PlayerPed.PlayerInfo.Stamina -= SprintDrain * frameTime;
                    else if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 1)
                        Main.PlayerPed.PlayerInfo.Stamina -= RunDrain * frameTime;
                    else if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 0)
                        Main.PlayerPed.PlayerInfo.Stamina -= WalkDrain * frameTime;
                    else
                        Main.PlayerPed.PlayerInfo.Stamina -= StandDrain * frameTime;
                }
                else if (IS_PLAYER_CLIMBING((int)Main.PlayerIndex))
                {
                    if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 1)
                        Main.PlayerPed.PlayerInfo.Stamina -= ClimbDrain * frameTime;
                    else if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState <= 1)
                        Main.PlayerPed.PlayerInfo.Stamina -= (ClimbDrain + 18f) * frameTime;
                }
                else if (IS_CHAR_SWIMMING(Main.PlayerHandle))
                {
                    if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 1)
                        Main.PlayerPed.PlayerInfo.Stamina -= SwimFastDrain * frameTime;
                    else if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 0)
                        Main.PlayerPed.PlayerInfo.Stamina -= SwimDrain * frameTime;
                    else
                        Main.PlayerPed.PlayerInfo.Stamina -= FloatDrain * frameTime;
                }
            }
        }
    }
}
