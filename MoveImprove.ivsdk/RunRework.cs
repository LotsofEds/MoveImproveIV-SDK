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
        private static bool CapsPressed;
        private static uint gTimer;
        private static uint fTimer;
        private static float pStam;
        private static bool IsCapsLockActive() => Control.IsKeyLocked(Keys.Capital);
        private static bool PressMoveKeys() => (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) || NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) || NativeControls.IsGameKeyPressed(0, GameKey.MoveLeft) || NativeControls.IsGameKeyPressed(0, GameKey.MoveRight));
        public static void Tick()
        {
            if (IS_PLAYER_CONTROL_ON((int)Main.PlayerIndex))
            {
                if (Main.SprintToVehicles && !IS_CHAR_ON_FOOT(Main.PlayerHandle) && !IS_CHAR_IN_ANY_CAR(Main.PlayerHandle))
                {
                    float moveState = Main.PlayerPed.PedMoveBlendOnFoot.MoveState;
                    moveState += 10.0f * Main.frameTime;
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
                if (Main.ForceRun && NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && (IS_INTERIOR_SCENE() || IVPhoneInfo.ThePhoneInfo.State > 1000 || Main.PlayerPed.PedMoveBlendOnFoot.MoveState <= 1) && PressMoveKeys() && Main.PlayerPed.PlayerInfo.Stamina > 0)
                {
                    GET_GAME_TIMER(out gTimer);
                    float moveState = Main.PlayerPed.PedMoveBlendOnFoot.MoveState;
                    if (IsCapsLockActive() || !Main.ToggleSprint)
                    {
                        moveState += 4.0f * Main.frameTime;
                        moveState = Main.Clamp(moveState, -3.0f, 3.0f);
                    }
                    else
                    {
                        if (moveState <= 2)
                        {
                            moveState += 4.0f * Main.frameTime;
                            moveState = Main.Clamp(moveState, -2.0f, 2.0f);
                        }
                    }
                    if (moveState <= 1.2f)
                    {
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_b", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_c", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_up", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_down", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_turn_l", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_turn_l2", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_turn_l3", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_turn_r", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_turn_r2", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_player", "walk_turn_r3", 1.25f);
                    }
                    //IVGame.ShowSubtitleMessage(moveState.ToString());
                    Main.PlayerPed.PedMoveBlendOnFoot.MoveState = moveState;
                    /*else if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 0)
                    {
                        Main.PlayerPed.PlayerInfo.Stamina -= WalkDrain * Main.frameTime;
                    }*/
                }

                if (Main.PlayerPed.PlayerInfo.NeverTired < 1 && Main.StaminaDrain)
                {
                    if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 2 && gTimer > fTimer + Main.frameTime)
                    {
                        if (pStam <= Main.PlayerPed.PlayerInfo.Stamina || (pStam + (Main.SprintDrain * Main.frameTime) > 600.0f))
                        {
                            //IVGame.ShowSubtitleMessage(pStam.ToString() + "  " + Main.PlayerPed.PlayerInfo.Stamina.ToString());
                            Main.PlayerPed.PlayerInfo.Stamina -= Main.SprintDrain * Main.frameTime;
                        }
                        pStam = Main.PlayerPed.PlayerInfo.Stamina;
                        GET_GAME_TIMER(out fTimer);
                    }
                    else if (Main.PlayerPed.PedMoveBlendOnFoot.MoveState > 1)
                    {
                        if (pStam + (Main.WalkDrain * Main.frameTime) <= Main.PlayerPed.PlayerInfo.Stamina || (pStam + (Main.RunDrain * Main.frameTime) > 600.0f))
                            Main.PlayerPed.PlayerInfo.Stamina -= Main.RunDrain * Main.frameTime;
                        pStam = Main.PlayerPed.PlayerInfo.Stamina;
                        GET_GAME_TIMER(out fTimer);
                        //IVGame.ShowSubtitleMessage(pStam.ToString() + "  " + Main.PlayerPed.PlayerInfo.Stamina.ToString());
                    }
                }
            }
        }
    }
}
