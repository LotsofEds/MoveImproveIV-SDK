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
        private static float SprintDrain;
        private static float RunDrain;
        private static float WalkDrain;

        // OtherShit
        private static bool CapsPressed;
        private static uint gTimer;
        private static uint fTimer;
        private static float pStam;
        private static bool IsCapsLockActive() => Control.IsKeyLocked(Keys.Capital);
        private static bool PressMoveKeys() => (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) || NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) || NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) || NativeControls.IsGameKeyPressed(0, GameKey.MoveRight));
        public static void Init(SettingsFile settings)
        {
            SprintDrain = settings.GetFloat("EXTENSIVE SETTINGS", "SprintDrain", 25.0f);
            RunDrain = settings.GetFloat("EXTENSIVE SETTINGS", "RunDrain", 7.5f);
            WalkDrain = settings.GetFloat("EXTENSIVE SETTINGS", "WalkDrain", 25.0f);
        }
        public static void Tick()
        {
            if (IS_PLAYER_CONTROL_ON((int)Main.PlayerIndex))
            {
                GET_FRAME_TIME(out float frameTime);
                if (Main.SprintToVehicles && !IS_CHAR_ON_FOOT(Main.PlayerHandle) && !IS_CHAR_IN_ANY_CAR(Main.PlayerHandle))
                {
                    float moveState = Main.PlayerPed.PedMoveBlendOnFoot.MoveState;
                    moveState += 10.0f * frameTime;
                    if (!NativeControls.IsGameKeyPressed(0, GameKey.Sprint))
                        moveState = Main.Clamp(moveState, 0.0f, 2.0f);

                    else
                        moveState = Main.Clamp(moveState, 0.0f, 3.0f);

                    Main.PlayerPed.PedMoveBlendOnFoot.MoveState = moveState;
                }

                if (Main.ToggleSprint && !IS_USING_CONTROLLER())
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
                if (Main.ForceRun && NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && PressMoveKeys() && Main.PlayerPed.PlayerInfo.Stamina > 0)
                {
                    GET_GAME_TIMER(out gTimer);
                    float moveState = Main.PlayerPed.PedMoveBlendOnFoot.MoveState;
                    if (IsCapsLockActive() || !Main.ToggleSprint)
                    {
                        moveState += 4.0f * frameTime;
                        moveState = Main.Clamp(moveState, -3.0f, 3.0f);
                    }
                    else
                    {
                        if (moveState <= 2)
                        {
                            moveState += 4.0f * frameTime;
                            moveState = Main.Clamp(moveState, -2.0f, 2.0f);
                        }
                    }
                    Main.PlayerPed.PedMoveBlendOnFoot.MoveState = moveState;

                    if (Main.PlayerPed.PlayerInfo.NeverTired < 1)
                    {
                        if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 2 && gTimer > fTimer + frameTime)
                        {
                            if (pStam <= Main.PlayerPed.PlayerInfo.Stamina || (pStam + (SprintDrain * frameTime) > 600.0f))
                            {
                                //IVGame.ShowSubtitleMessage(pStam.ToString() + "  " + Main.PlayerPed.PlayerInfo.Stamina.ToString());
                                Main.PlayerPed.PlayerInfo.Stamina -= SprintDrain * frameTime;
                            }
                            pStam = Main.PlayerPed.PlayerInfo.Stamina;
                            GET_GAME_TIMER(out fTimer);
                        }
                        else if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 1)
                        {
                            if (pStam + (WalkDrain * frameTime) <= Main.PlayerPed.PlayerInfo.Stamina || (pStam + (RunDrain * frameTime) > 600.0f))
                                Main.PlayerPed.PlayerInfo.Stamina -= RunDrain * frameTime;
                            pStam = Main.PlayerPed.PlayerInfo.Stamina;
                            GET_GAME_TIMER(out fTimer);
                            //IVGame.ShowSubtitleMessage(pStam.ToString() + "  " + Main.PlayerPed.PlayerInfo.Stamina.ToString());
                        }
                    }
                    /*else if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 0)
                    {
                        Main.PlayerPed.PlayerInfo.Stamina -= WalkDrain * frameTime;
                    }*/
                }
            }
        }
    }
}
