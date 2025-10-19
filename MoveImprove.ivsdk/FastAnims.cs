using CCL;
using CCL.GTAIV;
using IVSDKDotNet;
using IVSDKDotNet.Enums;
using System;
using System.Collections.Generic;
using System.Numerics;
using static IVSDKDotNet.Native.Natives;

namespace MoveImprove.ivsdk
{
    internal class FastAnims
    {
        private static float movepntr;
        private static bool speedUp;
        public static Vector3 PlyrPos;
        private static List<string> animList = new List<string>();
        private static float groundPos;

        public static void Init(SettingsFile settings)
        {
            string animString = settings.GetValue("MAIN", "HolsterAnims", "");
            foreach (var animName in animString.Split(','))
                animList.Add(animName);
        }
        private static void StandUp()
        {
            if (!IS_PED_RAGDOLL(Main.PlayerHandle))
            {
                if (!HAVE_ANIMS_LOADED("move_crouch"))
                    REQUEST_ANIMS("move_crouch");
                if (!HAVE_ANIMS_LOADED("ragdoll_trans"))
                    REQUEST_ANIMS("ragdoll_trans");
                Main.PlayerPed.ActivateDrunkRagdoll(500);
                Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(40), "Main", () =>
                {
                    PlyrPos = Main.PlayerPos;
                    GET_GROUND_Z_FOR_3D_COORD(PlyrPos, out float groundZ);
                    BLEND_FROM_NM_WITH_ANIM(Main.PlayerHandle, "move_crouch", "crouchidle2idle", 10, 0, 0, 0);
                    //REMOVE_ANIMS("move_crouch");
                    Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(50), "Main", () =>
                    {
                        FREEZE_CHAR_POSITION(Main.PlayerHandle, true);
                        Main.TheDelayedCaller.Add(TimeSpan.FromMilliseconds(350), "Main", () =>
                        {
                            if ((PlyrPos.Z - groundZ) <= 0.65f)
                                groundPos = (PlyrPos.Z - groundZ);
                            else
                                groundPos = 0.65f;
                            //IVGame.ShowSubtitleMessage(PlyrPos.Z.ToString() + "  " + groundZ.ToString() + "  " + (PlyrPos.Z - groundZ).ToString());
                            FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                            SET_CHAR_COORDINATES(Main.PlayerHandle, new Vector3(PlyrPos.X, PlyrPos.Y, (PlyrPos.Z - groundPos)));
                            _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "recover_balance", "ragdoll_trans", 2, 0, 0, 0, 0, -1);
                            //REMOVE_ANIMS("ragdoll_trans");
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
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "ragdoll_trans", "recover_balance", 0.8f);
                    speedUp = false;
                }
            }
            if (!IS_PED_IN_COVER(Main.PlayerHandle))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "high_l_pistol", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "high_r_pistol", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "low_l_pistol", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "low_r_pistol", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "high_l_pistol_short", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "high_r_pistol_short", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "low_l_pistol_short", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "low_r_pistol_short", Main.GetInCoverSpeed);

                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "high_l_rifle", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "high_r_rifle", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "low_l_rifle", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "low_r_rifle", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "high_l_rifle_short", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "high_r_rifle_short", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "low_l_rifle_short", Main.GetInCoverSpeed);
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", "low_r_rifle_short", Main.GetInCoverSpeed);

                if (IS_CHAR_DUCKING(Main.PlayerHandle))
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
            }

            else if (IS_PED_IN_COVER(Main.PlayerHandle) && !IS_CHAR_SHOOTING(Main.PlayerHandle))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_centre", "pistol_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_centre", "unarmed_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_centre", "rifle_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_centre", "pistol_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_centre", "unarmed_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_centre", "rifle_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_centre", "pistol_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_centre", "unarmed_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_centre", "rifle_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_centre", "pistol_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_centre", "unarmed_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_centre", "rifle_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_corner", "pistol_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_corner", "unarmed_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_low_corner", "rifle_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_corner", "pistol_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_corner", "unarmed_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_low_corner", "rifle_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_corner", "pistol_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_corner", "unarmed_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_l_high_corner", "rifle_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_corner", "pistol_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_corner", "unarmed_flip_180", (Main.CoverTurnSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_r_high_corner", "rifle_flip_180", (Main.CoverTurnSpeed));

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

                // l_high_corner
                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_high_corner", "ak47_blindfire"))
                    SpeedUpBlindfire("cover_l_high_corner", "ak47_blindfire", 0.1067f, 0.84f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_high_corner", "pistol_blindfire"))
                    SpeedUpBlindfire("cover_l_high_corner", "pistol_blindfire", 0.1063f, 0.7872f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_high_corner", "rifle_blindfire"))
                    SpeedUpBlindfire("cover_l_high_corner", "rifle_blindfire", 0.2131f, 0.7868f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_high_corner", "rocket_blindfire"))
                    SpeedUpBlindfire("cover_l_high_corner", "rocket_blindfire", 0.2280f, 0.7017f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_high_corner", "shotgun_blindfire"))
                    SpeedUpBlindfire("cover_l_high_corner", "shotgun_blindfire", 0.2f, 0.8f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_high_corner", "uzi_blindfire"))
                    SpeedUpBlindfire("cover_l_high_corner", "uzi_blindfire", 0.0297f, 0.9345f);

                // r_high_corner
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_high_corner", "ak47_blindfire"))
                    SpeedUpBlindfire("cover_r_high_corner", "ak47_blindfire", 0.0406f, 0.9069f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_high_corner", "pistol_blindfire"))
                    SpeedUpBlindfire("cover_r_high_corner", "pistol_blindfire", 0.1086f, 0.7826f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_high_corner", "rifle_blindfire"))
                    SpeedUpBlindfire("cover_r_high_corner", "rifle_blindfire", 0.3137f, 0.7058f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_high_corner", "rocket_blindfire"))
                    SpeedUpBlindfire("cover_r_high_corner", "rocket_blindfire", 0.3061f, 0.7346f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_high_corner", "shotgun_blindfire"))
                    SpeedUpBlindfire("cover_r_high_corner", "shotgun_blindfire", 0.2058f, 0.7941f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_high_corner", "uzi_blindfire"))
                    SpeedUpBlindfire("cover_r_high_corner", "uzi_blindfire", 0.02994f, 0.9281f);

                // l_low_center
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_centre", "ak47_blindfire"))
                    SpeedUpBlindfire("cover_l_low_centre", "ak47_blindfire", 0.0558f, 0.8882f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_centre", "pistol_blindfire"))
                    SpeedUpBlindfire("cover_l_low_centre", "pistol_blindfire", 0.1f, 0.76f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_centre", "rifle_blindfire"))
                    SpeedUpBlindfire("cover_l_low_centre", "rifle_blindfire", 0.25f, 0.7236f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_centre", "rocket_blindfire"))
                    SpeedUpBlindfire("cover_l_low_centre", "rocket_blindfire", 0.2826f, 0.7391f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_centre", "shotgun_blindfire"))
                    SpeedUpBlindfire("cover_l_low_centre", "shotgun_blindfire", 0.225f, 0.7625f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_centre", "uzi_blindfire"))
                    SpeedUpBlindfire("cover_l_low_centre", "uzi_blindfire", 0.2f, 0.6571f);

                // r_low_center
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_centre", "ak47_blindfire"))
                    SpeedUpBlindfire("cover_r_low_centre", "ak47_blindfire", 0.0561f, 0.9044f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_centre", "pistol_blindfire"))
                    SpeedUpBlindfire("cover_r_low_centre", "pistol_blindfire", 0.1f, 0.76f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_centre", "rifle_blindfire"))
                    SpeedUpBlindfire("cover_r_low_centre", "rifle_blindfire", 0.1976f, 0.8023f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_centre", "rocket_blindfire"))
                    SpeedUpBlindfire("cover_r_low_centre", "rocket_blindfire", 0.3469f, 0.6734f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_centre", "shotgun_blindfire"))
                    SpeedUpBlindfire("cover_r_low_centre", "shotgun_blindfire", 0.2112f, 0.7746f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_centre", "uzi_blindfire"))
                    SpeedUpBlindfire("cover_r_low_centre", "uzi_blindfire", 0.09375f, 0.84375f);

                // l_low_corner
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_corner", "ak47_blindfire"))
                    SpeedUpBlindfire("cover_l_low_corner", "ak47_blindfire", 0.0847f, 0.8389f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_corner", "pistol_blindfire"))
                    SpeedUpBlindfire("cover_l_low_corner", "pistol_blindfire", 0.0888f, 0.7333f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_corner", "rifle_blindfire"))
                    SpeedUpBlindfire("cover_l_low_corner", "rifle_blindfire", 0.2236f, 0.7368f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_corner", "rocket_blindfire"))
                    SpeedUpBlindfire("cover_l_low_corner", "rocket_blindfire", 0.3333f, 0.6315f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_corner", "shotgun_blindfire"))
                    SpeedUpBlindfire("cover_l_low_corner", "shotgun_blindfire", 0.2179f, 0.7564f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_l_low_corner", "uzi_blindfire"))
                    SpeedUpBlindfire("cover_l_low_corner", "uzi_blindfire", 0.1794f, 0.6923f);

                // r_low_corner
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_corner", "ak47_blindfire"))
                    SpeedUpBlindfire("cover_r_low_corner", "ak47_blindfire", 0.0588f, 0.9058f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_corner", "pistol_blindfire"))
                    SpeedUpBlindfire("cover_r_low_corner", "pistol_blindfire", 0.1041f, 0.75f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_corner", "rifle_blindfire"))
                    SpeedUpBlindfire("cover_r_low_corner", "rifle_blindfire", 0.2112f, 0.7887f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_corner", "rocket_blindfire"))
                    SpeedUpBlindfire("cover_r_low_corner", "rocket_blindfire", 0.2745f, 0.7058f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_corner", "shotgun_blindfire"))
                    SpeedUpBlindfire("cover_r_low_corner", "shotgun_blindfire", 0.2142f, 0.7714f);
                else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "cover_r_low_corner", "uzi_blindfire"))
                    SpeedUpBlindfire("cover_r_low_corner", "uzi_blindfire", 0.0355f, 0.9289f);

                if (NativeControls.IsGameKeyPressed(0, GameKey.Sprint))
                {
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_combat_strafe", "walk", 1.5f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_rifle", "walk", 1.5f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_rpg", "walk", 1.5f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch", "walk", 1.75f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch_rifle", "walk", 1.75f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_crouch_rpg", "walk", 1.75f);

                    if (Main.StaminaDrain && isMovingInCover())
                        Main.PlayerPed.PlayerInfo.Stamina -= Main.RunDrain * Main.frameTime;
                }
            }

            foreach (string animName in animList)
            {
                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animName, "aim_2_holster") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animName, "holster") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animName, "holster_2_aim") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animName, "holster_crouch") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animName, "unholster_crouch") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animName, "unholster"))
                {
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animName, "aim_2_holster", Main.HolsterSpeed);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animName, "holster", Main.HolsterSpeed);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animName, "holster_2_aim", Main.HolsterSpeed);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animName, "holster_crouch", Main.HolsterSpeed);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animName, "unholster_crouch", Main.HolsterSpeed);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animName, "unholster", Main.HolsterSpeed);
                }
            }

            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "fall_land"))
            {
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "fall_land", 1.5f);
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "fall_land", out movepntr);
                if (movepntr > 0.3)
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "fall_land", 2.0f);
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_std", "jump_on_spot"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "jump_std", "jump_on_spot", out movepntr);
                if (movepntr > 0.5)
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "jump_std", "jump_on_spot", 2.0f);
                else
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "jump_std", "jump_on_spot", 1.0f);
            }

            else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "jump_rifle", "jump_on_spot"))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "jump_rifle", "jump_on_spot", out movepntr);
                if (movepntr > 0.6)
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "jump_rifle", "jump_on_spot", 2.0f);
                else
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "jump_rifle", "jump_on_spot", 1.0f);
            }
        }
        private static void SpeedUpBlindfire(string animSet, string animName, float startTime, float endTime)
        {
            GET_CHAR_ANIM_TOTAL_TIME(Main.PlayerHandle, animSet, animName, out float totalTime);
            GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, animSet, animName, out float animTime);

            float startSpeed = (Main.BlindFireSpeed * startTime * (totalTime / 250));
            float endSpeed = (Main.BlindFireSpeed * (1 - endTime) * (totalTime / 250));

            //IVGame.ShowSubtitleMessage(startSpeed.ToString() + "  " + endSpeed.ToString());
            if (animTime < startTime)
            {
                if (startSpeed > Main.BlindfireMaxSpd)
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, animName, Main.BlindfireMaxSpd);
                else if (startSpeed > Main.BlindFireSpeed)
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, animName, startSpeed);
                else
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, animName, Main.BlindFireSpeed);
            }

            else if (animTime > endTime)
            {
                if (endSpeed > Main.BlindfireMaxSpd)
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, animName, Main.BlindfireMaxSpd);
                if (endSpeed > Main.BlindFireSpeed)
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, animName, endSpeed);
                else
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, animName, Main.BlindFireSpeed);
            }
        }
        private static bool isMovingInCover()
        {
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_combat_strafe", "walk") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "walk") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "walk") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_crouch", "walk") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_crouch_rifle", "walk") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_crouch_rpg", "walk"))
                return true;
            else
                return false;
        }
    }
}
