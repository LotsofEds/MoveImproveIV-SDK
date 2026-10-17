using CCL.GTAIV;
using IVSDKDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static IVSDKDotNet.Native.Natives;
using IVSDKDotNet.Enums;

namespace MoveImprove.ivsdk
{
    internal class MeleeGun
    {
        private static bool noMeleeGun = false;
        private static int currPed;
        private static void CancelAnim()
        {
            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_gun", "melee", 1.0f);
            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_gun", "melee_crouch", 1.0f);
            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_gun", "melee_ak", 1.0f);
            SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "melee_gun", "melee_ak_crouch", 1.0f);
        }
        private static void CheckIfHitPed(string animSet)
        {
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animSet, "melee") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animSet, "melee_crouch"))
            {
                foreach (var ped in PedHelper.PedHandles)
                {
                    int pedHandle = ped.Value;

                    if (!DOES_CHAR_EXIST(pedHandle)) continue;
                    if (!LOCATE_CHAR_ON_FOOT_3D(pedHandle, Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z, 2, 2, 2, false)) continue;

                    if (DOES_CHAR_EXIST(pedHandle) && IS_CHAR_TOUCHING_CHAR(Main.PlayerHandle, pedHandle))
                    {
                        DAMAGE_CHAR(pedHandle, 10, true);
                        if (!IS_PED_IN_COMBAT(pedHandle))
                        {
                            GET_CHAR_HEADING(Main.PlayerHandle, out float pHdng);

                            pHdng *= (float)Math.PI / 180f;
                            float dirX = (float)(0.0 - Math.Sin(pHdng));
                            float dirY = (float)Math.Cos(pHdng);
                            float dirZ = 0f;

                            APPLY_FORCE_TO_PED(pedHandle, 3, dirX, dirY, 0, 0, 0, 0, 0, 0, 1, 1);
                        }
                    }
                }
            }
        }
        public static void Tick()
        {
            /*if (IS_CONTROL_JUST_PRESSED(0, (int)GameKey.RadarZoom))
            {
                noMeleeGun = !noMeleeGun;
            }*/
            /*foreach (string animName in Main.animList)
            {
                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, animName, "wall_block_idle"))
                {
                    if (IS_CONTROL_JUST_PRESSED(0, (int)GameKey.EnterCar))
                    {
                        if (IS_CHAR_DUCKING(Main.PlayerHandle))
                            _TASK_PLAY_ANIM(Main.PlayerHandle, "melee_crouch", animName, 4.0f, 0, 0, 1, 0, -1);
                        else
                            _TASK_PLAY_ANIM(Main.PlayerHandle, "melee", animName, 4.0f, 0, 0, 1, 0, -1);
                    }
                }
                CheckIfHitPed(animName);
            }*/
            GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0, 0.7f, 0), out Vector3 offsetPos);
            bool closePedExists = false;
            foreach (var ped in PedHelper.PedHandles)
            {
                int pedHandle = ped.Value;

                if (!DOES_CHAR_EXIST(pedHandle)) continue;
                if (!LOCATE_CHAR_ON_FOOT_3D(pedHandle, offsetPos.X, offsetPos.Y, offsetPos.Z, 0.6f, 0.6f, 0.6f, false)) continue;

                if (DOES_CHAR_EXIST(pedHandle))
                {
                    closePedExists = true;
                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_gun", "melee") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_gun", "melee_crouch"))
                    {
                        if (IS_CHAR_TOUCHING_CHAR(Main.PlayerHandle, pedHandle) && currPed != pedHandle)
                        {
                            SET_CHAR_RELATIONSHIP(pedHandle, (int)eRelationship.RELATIONSHIP_HATE, (int)eRelationshipGroup.RELATIONSHIP_GROUP_PLAYER);
                            DAMAGE_CHAR(pedHandle, 10, true);

                            if (!IS_PED_IN_COMBAT(pedHandle))
                            {
                                GET_CHAR_HEADING(Main.PlayerHandle, out float pHdng);

                                pHdng *= (float)Math.PI / 180f;
                                float dirX = (float)(0.0 - Math.Sin(pHdng));
                                float dirY = (float)Math.Cos(pHdng);
                                float dirZ = 0f;

                                APPLY_FORCE_TO_PED(pedHandle, 3, dirX, dirY, 0, 0, 0, 0, 0, 0, 1, 1);
                            }
                        }
                    }
                    else
                        currPed = 0;
                }
            }
            if (closePedExists)
            {
                if (IS_CONTROL_JUST_PRESSED(0, (int)GameKey.EnterCar))
                {
                    if (IS_CHAR_DUCKING(Main.PlayerHandle))
                        _TASK_PLAY_ANIM(Main.PlayerHandle, "melee_crouch", "melee_gun", 4.0f, 0, 0, 1, 0, -1);
                    else
                        _TASK_PLAY_ANIM(Main.PlayerHandle, "melee", "melee_gun", 4.0f, 0, 0, 1, 0, -1);
                }
            }

            /*if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_gun", "melee") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_gun", "melee_crouch") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_gun", "melee_ak") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "melee_gun", "melee_ak_crouch"))
            {
                if (noMeleeGun)
                    CancelAnim();
            }*/
        }
    }
}
