using System;
using System.Collections.Generic;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;
using System.Numerics;

namespace MoveImprove.ivsdk
{
    internal class FastAnims
    {
        private static float movepntr;
        private static bool speedUp;
        public static Vector3 PlyrPos;
        private static void StandUp()
        {
            if (!IS_PED_RAGDOLL(Main.PlayerHandle))
            {
                REQUEST_ANIMS("move_crouch");
                REQUEST_ANIMS("get_up");
                Main.PlayerPed.ActivateDrunkRagdoll(500);
                Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(40), "Main", () =>
                {
                    PlyrPos = Main.PlayerPos;
                    BLEND_FROM_NM_WITH_ANIM(Main.PlayerHandle, "move_crouch", "crouchidle2idle", 10, 0, 0, 0);
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(50), "Main", () =>
                    {
                        FREEZE_CHAR_POSITION(Main.PlayerHandle, true);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(350), "Main", () =>
                        {
                            FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                            SET_CHAR_COORDINATES(Main.PlayerHandle, new Vector3(PlyrPos.X, PlyrPos.Y, (PlyrPos.Z - 0.5f)));
                            _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "recover_balance", "ragdoll_trans", 2, 0, 0, 0, 0, -1);
                            speedUp = true;
                        });
                    });
                });
            }
        }
        public static void Tick()
        {
            if (speedUp)
            {
                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "ragdoll_trans", "recover_balance"))
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "ragdoll_trans", "recover_balance", 3.0f);

                else
                    speedUp = false;
            }
            if (IS_CHAR_DUCKING(Main.PlayerHandle) && !IS_PED_IN_COVER(Main.PlayerHandle))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch", "idle2crouchidle", (Main.CrouchingSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch_rifle", "idle2crouchidle", (Main.CrouchingSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch_rpg", "idle2crouchidle", (Main.CrouchingSpeed));
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_crouch", "crouchidle2idle") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_crouch_rifle", "crouchidle2idle") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "pickup_object", "pickup_high") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "pickup_object", "pickup_low") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "pickup_object", "pickup_med") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "ev_dives", "plyr_roll_left") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "ev_dives", "plyr_roll_right"))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "pickup_object", "pickup_high", (Main.PickupObjectSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "pickup_object", "pickup_low", (Main.PickupObjectSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "pickup_object", "pickup_med", (Main.PickupObjectSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "ev_dives", "plyr_roll_left", (Main.CombatRollSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "ev_dives", "plyr_roll_right", (Main.CombatRollSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch", "crouchidle2idle", (Main.CrouchingSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch_rifle", "crouchidle2idle", (Main.CrouchingSpeed));
            }

            else if (IS_PED_CLIMBING(Main.PlayerHandle))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "hang_to_waist", (Main.ClimbAndShimmySpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "waist_to_stand_rifle", (Main.ClimbAndShimmySpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "waist_to_stand_unarmed", (Main.ClimbAndShimmySpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "landing_head_height", (Main.ClimbAndShimmySpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "landing_waist_height", (Main.ClimbAndShimmySpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "waist_to_vault", (Main.ClimbAndShimmySpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "waist_to_vault_shallow", (Main.ClimbAndShimmySpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "shimmy_l", (Main.ClimbAndShimmySpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "shimmy_r", (Main.ClimbAndShimmySpeed));

                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "landing_stretch_height"))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "landing_stretch_height", out movepntr);
                    if (movepntr > 0.6)
                    {
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "landing_stretch_height", 2.0f);
                    }
                }
                if (Main.ExtremeClimbing == true && !NativeControls.IsGameKeyPressed(0, GameKey.Sprint))
                {
                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "vault_end"))
                    {
                        GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "vault_end", out movepntr);
                        if (movepntr < 0.27 && !NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "vault_end"))
                        {
                            GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "vault_end", out movepntr);
                            if (movepntr > 0.07 && movepntr < 0.27)
                                StandUp();
                        }
                    }

                    else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "vault_end_r"))
                    {
                        GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "vault_end_r", out movepntr);
                        if (movepntr < 0.25 && !NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "vault_end_r"))
                        {
                            GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "vault_end_r", out movepntr);
                            if (movepntr > 0.05 && movepntr < 0.25)
                                StandUp();
                        }
                    }

                    else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "waist_to_vault"))
                    {
                        GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "waist_to_vault", out movepntr);
                        if (movepntr > 0.52 && movepntr < 0.6 && !NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "waist_to_vault"))
                            StandUp();
                    }

                    else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "waist_to_vault_shallow"))
                    {
                        GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "waist_to_vault_shallow", out movepntr);
                        if (movepntr > 0.52 && movepntr < 0.6 && !NativeControls.IsGameKeyPressed(0, GameKey.Sprint) && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "waist_to_vault_shallow"))
                            StandUp();
                    }
                }
            }

            if (IS_PED_IN_COVER(Main.PlayerHandle))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_centre", "pistol_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_centre", "rifle_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_centre", "pistol_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_centre", "rifle_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_corner", "pistol_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_corner", "rifle_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_corner", "pistol_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_corner", "rifle_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_corner", "pistol_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_corner", "rifle_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_corner", "pistol_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_corner", "rifle_peek", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_centre", "pistol_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_centre", "rifle_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_centre", "pistol_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_centre", "rifle_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_corner", "pistol_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_corner", "rifle_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_corner", "pistol_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_corner", "rifle_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_corner", "pistol_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_corner", "rifle_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_corner", "pistol_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_corner", "rifle_normal_fire_intro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_centre", "pistol_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_centre", "rifle_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_centre", "pistol_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_centre", "rifle_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_corner", "pistol_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_corner", "rifle_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_corner", "pistol_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_corner", "rifle_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_corner", "pistol_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_corner", "rifle_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_corner", "pistol_normal_fire_outro", (Main.PeekFromCoverSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_corner", "rifle_normal_fire_outro", (Main.PeekFromCoverSpeed));

                if (NativeControls.IsGameKeyPressed(0, GameKey.Sprint))
                {
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_combat_strafe", "walk", 1.5f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_rpg", "walk", 1.5f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch", "walk", 1.75f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch_rifle", "walk", 1.75f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch_rpg", "walk", 1.75f);
                }
            }

            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "fall_land"))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "fall_land", 1.5f);
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "fall_land", out movepntr);
                if (movepntr > 0.3)
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "fall_land", 2.0f);
            }

            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_on_spot"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "jump_std", "jump_on_spot", out movepntr);
                if (movepntr > 0.5)
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "jump_std", "jump_on_spot", 2.0f);
            }

            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_rifle", "jump_on_spot"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "jump_rifle", "jump_on_spot", out movepntr);
                if (movepntr > 0.6)
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "jump_rifle", "jump_on_spot", 2.0f);
            }
        }
    }
}
