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
        private static bool boostToCover;

        private static string[] HolsterAnims;
        private static string[] CoverDiveAnims;
        private static string[] CoverAnims;
        private static string[] ClimbAnims;
        private static string[] CoverStrafeAnims;
        private static string[] CrouchAnims;
        private static string[] PickupAnims;
        public static void Init(SettingsFile settings)
        {
            string animString = settings.GetValue("MAIN", "HolsterAnims", "");
            foreach (var animName in animString.Split(','))
                Main.animList.Add(animName);

            HolsterAnims = GetHolsterAnims();
            CoverDiveAnims = GetCoverDiveAnims();
            CoverAnims = GetCoverAnims();
            ClimbAnims = GetClimbAnims();
            CoverStrafeAnims = GetCoverStrafeAnims();
            CrouchAnims = GetCrouchAnims();
            PickupAnims = GetPickupAnims();
        }
        private static string[] GetHolsterAnims()
        {
            string[] anims = new string[] { "aim_2_holster", "holster_2_aim", "holster", "holster_crouch", "unholster", "unholster_crouch" };

            return anims;
        }
        private static string[] GetCoverDiveAnims()
        {
            string[] anims = new string[] { "high_l_pistol", "low_l_pistol", "low_r_pistol", "high_l_pistol_short", "high_r_pistol_short","low_l_pistol_short",
                "low_r_pistol_short", "high_l_rifle", "high_r_rifle", "low_l_rifle", "low_r_rifle", "high_l_rifle_short", "high_r_rifle_short", "low_l_rifle_short",
                "low_r_rifle_short"};

            return anims;
        }
        public static string[] GetCoverAnims()
        {
            string[] anims = new string[] { "cover_l_low_centre", "cover_r_low_centre", "cover_l_high_centre", "cover_r_high_centre", "cover_l_low_corner",
                "cover_r_low_corner", "cover_l_high_corner", "cover_r_high_corner"};

            return anims;
        }
        private static string[] GetClimbAnims()
        {
            string[] anims = new string[] { "hang_to_waist", "waist_to_stand_unarmed", "landing_head_height", "landing_waist_height", "waist_to_vault",
                "waist_to_vault_shallow", "shimmy_l", "shimmy_r"};

            return anims;
        }
        private static string[] GetCoverStrafeAnims()
        {
            string[] anims = new string[] { "move_combat_strafe", "move_rifle", "move_rpg", "move_crouch", "move_crouch_rifle", "move_crouch_rpg"};

            return anims;
        }
        private static string[] GetCrouchAnims()
        {
            string[] anims = new string[] { "move_crouch", "move_crouch_rifle", "move_crouch_rpg" };

            return anims;
        }
        private static string[] GetPickupAnims()
        {
            string[] anims = new string[] { "pickup_high", "pickup_low", "pickup_med" };

            return anims;
        }
        private static void CancelAnimEarly(string animSet, string animName, float stopTime)
        {
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animSet, animName))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, animSet, animName, out movepntr);

                if (movepntr > stopTime && (NativeControls.IsGameKeyPressed(0, GameKey.MoveBackward) || NativeControls.IsGameKeyPressed(2, GameKey.MoveBackward)))
                    CLEAR_CHAR_TASKS(Main.PlayerHandle);
            }
        }
        private static void CoverBoost(string animSet, string animName)
        {
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animSet, animName))
            {
                GET_CHAR_VELOCITY(Main.PlayerHandle, out Vector3 pVel);

                Vector3 dir = Vector3.Normalize(pVel);

                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, animSet, animName, out movepntr);

                if (movepntr <= 0.25f)
                    boostToCover = false;
                if (movepntr > 0.25f && !boostToCover)
                {
                    boostToCover = true;
                    APPLY_FORCE_TO_PED(Main.PlayerHandle, 3, dir.X * 8, dir.Y * 8, 0, 0, 0, 0, 0, 0, 1, 1);
                }
            }
        }
        public static void Tick()
        {
            if (!IS_PED_IN_COVER(Main.PlayerHandle))
            {
                foreach (string anim in CoverDiveAnims)
                {
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "cover_dive", anim, Main.GetInCoverSpeed);
                    CoverBoost("cover_dive", anim);
                    CancelAnimEarly("cover_dive", "high_l_pistol", 0.83f);
                }

                foreach (string animSet in CrouchAnims)
                {
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, "crouchidle2idle", (Main.CrouchingSpeed));
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, "idle2crouchidle", (Main.CrouchingSpeed));
                }

                foreach (string animName in PickupAnims)
                {
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "pickup_object", animName, (Main.PickupObjectSpeed));
                }

                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "ev_dives", "plyr_roll_left", (Main.CombatRollSpeed));
                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "ev_dives", "plyr_roll_right", (Main.CombatRollSpeed));

                if (IS_PED_CLIMBING(Main.PlayerHandle))
                {
                    foreach (string animName in ClimbAnims)
                    {
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", animName, (Main.ClimbAndShimmySpeed));
                    }

                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "landing_stretch_height"))
                    {
                        GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", "landing_stretch_height", out movepntr);
                        if (movepntr > 0.6)
                        {
                            SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "climb_std", "landing_stretch_height", 2.0f);
                        }
                    }
                }
            }

            else if (IS_PED_IN_COVER(Main.PlayerHandle) && !IS_CHAR_SHOOTING(Main.PlayerHandle))
            {
                foreach (string animSet in CoverAnims)
                {
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, "pistol_flip_180", (Main.CoverTurnSpeed));
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, "unarmed_flip_180", (Main.CoverTurnSpeed));

                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, "pistol_peek", (Main.PeekFromCoverSpeed));
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, "rifle_peek", (Main.PeekFromCoverSpeed));
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, "pistol_normal_fire_intro", (Main.PeekFromCoverSpeed));
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, "rifle_normal_fire_intro", (Main.PeekFromCoverSpeed));
                }

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
                    foreach (string moveSet in CoverStrafeAnims)
                    {
                        if (moveSet.Contains("crouch"))
                            SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_combat_strafe", "walk", 1.75f);
                        else
                            SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "move_combat_strafe", "walk", 1.5f);
                    }

                    if (Main.StaminaDrain && isMovingInCover())
                        Main.PlayerPed.PlayerInfo.Stamina -= Main.RunDrain * Main.frameTime;
                }
            }

            foreach (string animSet in Main.animList)
            {
                foreach (string animName in HolsterAnims)
                {
                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animSet, animName))
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, animSet, animName, Main.HolsterSpeed);
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
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_combat_strafe", "walk") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rifle", "walk")
                || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_rpg", "walk") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_crouch", "walk")
                || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_crouch_rifle", "walk") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "move_crouch_rpg", "walk"))
                return true;
            else
                return false;
        }
    }
}
