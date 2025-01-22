using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;

namespace MoveImprove.ivsdk
{
    internal class FasterJackingScript
    {
        private static bool CheckDateTime;
        private static DateTime currentDateTime;
        private static int pedHandle;
        public static void Tick()
        {
            if (CheckDateTime == false)
            {
                currentDateTime = DateTime.Now;
                CheckDateTime = true;
            }

            if (DateTime.Now.Subtract(currentDateTime).TotalMilliseconds > 100.0)
            {
                CheckDateTime = false;
                if (Main.PlayerPed.IsInVehicle())
                {
                    if (!IS_PED_JACKING(Main.PlayerHandle))
                        SET_CHAR_ALL_ANIMS_SPEED(Main.PlayerHandle, (Main.EnterExitVehSpeed));

                    else if (Main.FasterJacking == true && IS_PED_JACKING(Main.PlayerHandle))
                    {
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_unarmdc", "jack_perp_ds", 1.25f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@low_jack_pistol", "jack_perp_ds", 1.5f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@low_jack_pistol", "jack_perp_ps", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@low_jack_rifle", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@low_jack_rifle", "jack_perp_ps", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@low_jack_rpg", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@low_jack_rpg", "jack_perp_ps", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_pistol", "jack_perp_ds", 1.3f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_pistol", "jack_perp_ps", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_pistolb", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_pistolb", "jack_perp_ps", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_rifle", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_rifle", "jack_perp_ps", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_rifle_b", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_rifle_c", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_rpg", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@std_jack_rpg", "jack_perp_ps", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@tru_jack_pistol", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@tru_jack_pistol", "jack_perp_ps", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@tru_jack_rifle", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@tru_jack_rifle", "jack_perp_ps", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@van_jack_pistol", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@van_jack_pistol", "jack_perp_ps", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@van_jack_rifle", "jack_perp_ds", 1.2f);
                        SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "veh@van_jack_rifle", "jack_perp_ps", 1.4f);

                        foreach (var ped in PedHelper.PedHandles)
                        {
                            pedHandle = ped.Value;
                            if (!DOES_CHAR_EXIST(pedHandle)) continue;
                            if (IS_CHAR_DEAD(pedHandle)) continue;
                            if (IS_CHAR_INJURED(pedHandle)) continue;
                            if (!IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle)) continue;

                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_unarmdc", "jack_driver_ds", 1.25f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@low_jack_pistol", "jack_driver_ds", 1.35f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@low_jack_pistol", "jack_driver_ps", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@low_jack_rifle", "jack_driver_ds", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@low_jack_rifle", "jack_driver_ps", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@low_jack_rpg", "jack_driver_ds", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@low_jack_rpg", "jack_driver_ps", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_pistol", "jack_driver_ds", 1.3f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_pistol", "jack_driver_ps", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_pistolb", "jack_driver_ds", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_pistolb", "jack_driver_ps", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_rifle", "jack_driver_ds", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_rifle", "jack_driver_ps", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_rifle_b", "jack_driver_ds", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_rifle_c", "jack_driver_ds", 1.3f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_rpg", "jack_driver_ds", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@std_jack_rpg", "jack_driver_ps", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@tru_jack_pistol", "jack_driver_ds", 1.0f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@tru_jack_pistol", "jack_driver_ps", 1.0f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@tru_jack_rifle", "jack_driver_ds", 1.0f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@tru_jack_rifle", "jack_driver_ps", 1.0f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@van_jack_pistol", "jack_driver_ds", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@van_jack_pistol", "jack_driver_ps", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@van_jack_rifle", "jack_driver_ds", 1.2f);
                            SET_CHAR_ANIM_SPEED(pedHandle, "veh@van_jack_rifle", "jack_driver_ps", 1.4f);
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
                if (Main.PlayerPed.IsInVehicle() && !IS_CHAR_SITTING_IN_ANY_CAR(Main.PlayerHandle) && !IS_CHAR_GETTING_IN_TO_A_CAR(Main.PlayerHandle))
                {
                    SET_CHAR_ALL_ANIMS_SPEED(Main.PlayerHandle, (Main.EnterExitVehSpeed));
                }
            }
        }
    }
}
