using CCL;
using CCL.GTAIV;
using IVSDKDotNet;
using IVSDKDotNet.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static IVSDKDotNet.Native.Natives;

namespace MoveImprove.ivsdk
{
    internal class FasterJackingScript
    {
        private static uint fTimer;
        private static int pedHandle;
        private static Dictionary<string, float> JackingAnims;

        public static void Init()
        {
            JackingAnims = GetJackingAnims();
        }
        public static void UnInit()
        {
            fTimer = 0;
        }
        private static Dictionary<string, float> GetJackingAnims()
        {
            Dictionary<string, float> anims = new Dictionary<string, float>();

            string[] animNames = new string[] { "std_jack_unarmdc_ds", "low_jack_pistol_ds", "low_jack_pistol_ps", "low_jack_rifle_ds", "low_jack_rifle_ps",
                "std_jack_pistol_ds", "std_jack_pistol_ps", "std_jack_pistolb_ds", "std_jack_pistolb_ps", "std_jack_rifle_ds", "std_jack_rifle_ps",
                "std_jack_rifle_b_ds", "std_jack_rifle_c_ds" , "std_jack_rpg_ds", "std_jack_rpg_ps", "tru_jack_pistol_ds", "tru_jack_pistol_ps",
                "tru_jack_rifle_ds", "tru_jack_rifle_ps", "van_jack_pistol_ds", "van_jack_pistol_ps" };

            foreach (string anim in animNames)
            {
                anims[anim] = 1.2f;
            }
            anims["std_jack_unarmdc_ds"] = 1.25f;
            anims["low_jack_pistol_ds"] = 1.5f;
            anims["std_jack_pistol_ds"] = 1.3f;
            anims["van_jack_rifle_ps"] = 1.4f;

            return anims;
        }
        public static void Tick()
        {
            if (!Main.FasterJacking)
                return;

            if (Main.gTimer >= fTimer + 100)
            {
                GET_GAME_TIMER(out fTimer);

                if (IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle))
                {
                    if (!IS_PED_JACKING(Main.PlayerHandle))
                        SET_CHAR_ALL_ANIMS_SPEED(Main.PlayerHandle, (Main.EnterExitVehSpeed));

                    else
                    {
                        foreach (var anims in JackingAnims)
                        {
                            string trimmed;
                            if (anims.Key.EndsWith("_ds"))
                            {
                                trimmed = anims.Key.Substring(0, anims.Key.Length - 3);

                                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, trimmed, "jack_perp_ds", anims.Value);
                            }
                            else if (anims.Key.EndsWith("_ps"))
                            {
                                trimmed = anims.Key.Substring(0, anims.Key.Length - 3);

                                SET_CHAR_ANIM_SPEED(Main.PlayerHandle, trimmed, "jack_perp_ps", anims.Value);
                            }
                        }

                        if (IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle))
                        {
                            foreach (var ped in PedHelper.PedHandles)
                            {
                                pedHandle = ped.Value;
                                if (!DOES_CHAR_EXIST(pedHandle)) continue;
                                if (IS_CHAR_INJURED(pedHandle)) continue;
                                if (!LOCATE_CHAR_ANY_MEANS_3D(pedHandle, Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z, 5, 5, 5, false)) continue;

                                foreach (var anims in JackingAnims)
                                {
                                    string trimmed;
                                    if (anims.Key.EndsWith("_ds"))
                                    {
                                        trimmed = anims.Key.Substring(0, anims.Key.Length - 3);

                                        SET_CHAR_ANIM_SPEED(pedHandle, trimmed, "jack_driver_ds", anims.Value);
                                    }
                                    else if (anims.Key.EndsWith("_ps"))
                                    {
                                        trimmed = anims.Key.Substring(0, anims.Key.Length - 3);

                                        SET_CHAR_ANIM_SPEED(pedHandle, trimmed, "jack_driver_ps", anims.Value);
                                    }
                                }
                            }
                        }
                    }
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_chopper", "pickup_lhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_chopper", "pickup_rhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_chopper", "pullup_lhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_chopper", "pullup_rhs", 1.5f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_dirt", "pickup_lhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_dirt", "pickup_rhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_dirt", "pullup_lhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_dirt", "pullup_rhs", 1.5f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_freeway", "pickup_lhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_freeway", "pickup_rhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_freeway", "pullup_lhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_freeway", "pullup_rhs", 1.5f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_scooter", "pickup_lhs", 1.33f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_scooter", "pickup_rhs", 1.17f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_scooter", "pullup_lhs", 1.5f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_scooter", "pullup_rhs", 1.33f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_spt", "pickup_lhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_spt", "pickup_rhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_spt", "pullup_lhs", 1.25f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@bike_spt", "pullup_rhs", 1.5f);
                }
                if (IS_CHAR_IN_ANY_CAR(Main.PlayerHandle) && !IS_CHAR_SITTING_IN_ANY_CAR(Main.PlayerHandle) && !IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle))
                    SET_CHAR_ALL_ANIMS_SPEED(Main.PlayerHandle, (Main.EnterExitVehSpeed));
            }
        }
    }
}
