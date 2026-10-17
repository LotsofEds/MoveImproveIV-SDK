using CCL.GTAIV;
using IVSDKDotNet;
using IVSDKDotNet.Enums;
using IVSDKDotNet.Native;
using MoveImprove.ivsdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static IVSDKDotNet.Native.Natives;

namespace MoveImprove.ivsdk
{
    internal class ExtremeClimbing
    {
        private static int testObj;
        private static bool isClimbing;
        private static uint fTimer;
        private static bool speedUp;
        private static bool getUp;
        private static float movepntr;
        private static float groundPos;
        public static Vector3 plyrPos;
        public static void UnInit()
        {
            if (DOES_OBJECT_EXIST(testObj))
                DELETE_OBJECT(ref testObj);
        }
        public static void Tick()
        {
            if (!Main.veryExtremeClimbing)
                ProcessExtremeClimb();
            else
            {
                ProcessVeryExtremeClimb();
                ProcessStandUp();
            }
        }
        private static void ProcessExtremeClimb()
        {
            REQUEST_MODEL(GET_HASH_KEY("cj_game_cube_1"));

            Vector3 standPos;
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "vault_start") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "vault_start_r"))
                GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.0f, 0.8f, 0.5f), out standPos);
            else
                GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Main.PlayerHandle, new Vector3(0.0f, 0.5f, 1.5f), out standPos);

            GET_GROUND_Z_FOR_3D_COORD(standPos, out float pGround);

            ProcessVaultStand("vault_start", standPos, pGround);
            ProcessVaultStand("vault_start_r", standPos, pGround);
            ProcessVaultStand("hang_to_waist", standPos, pGround);

            /*IVGame.ShowSubtitleMessage((standPos.Z - pGround).ToString());
            
            REQUEST_MODEL(GET_HASH_KEY("ec_nf_ghostball"));

            if (!DOES_OBJECT_EXIST(testObj))
                CREATE_OBJECT(GET_HASH_KEY("ec_nf_ghostball"), Vector3.Zero, out testObj, true);
            else
            {
                SET_OBJECT_COLLISION(testObj, false);
                FREEZE_OBJECT_POSITION(testObj, false);
                SET_OBJECT_ALPHA(testObj, 255);
                SET_OBJECT_VISIBLE(testObj, true);
                SET_OBJECT_COORDINATES(testObj, standPos);
            }*/

            if (isClimbing)
            {
                if (!DOES_OBJECT_EXIST(testObj))
                {
                    CREATE_OBJECT_NO_OFFSET(GET_HASH_KEY("cj_game_cube_1"), new Vector3(standPos.X, standPos.Y, standPos.Z - 1.0f), out testObj, true);

                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "vault_start") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", "vault_start_r"))
                        ATTACH_OBJECT_TO_PED(testObj, Main.PlayerHandle, (int)eBone.BONE_ROOT, new Vector3(0, 0.8f, -0.5f), Vector3.Zero, 1);
                    else
                        ATTACH_OBJECT_TO_PED(testObj, Main.PlayerHandle, (int)eBone.BONE_ROOT, new Vector3(0, 0.8f, 0.5f), Vector3.Zero, 1);

                    SET_OBJECT_VISIBLE(testObj, false);
                    SET_OBJECT_ALPHA(testObj, 0);
                }
                else
                {
                    if (IS_OBJECT_ATTACHED(testObj))
                    {
                        DETACH_OBJECT(testObj, true);
                        FREEZE_OBJECT_POSITION(testObj, true);
                    }
                    if (!IS_PED_CLIMBING(Main.PlayerHandle))
                    {
                        if (DOES_OBJECT_EXIST(testObj))
                            DELETE_OBJECT(ref testObj);
                        isClimbing = false;
                    }
                }
            }
        }
        private static void ProcessVeryExtremeClimb()
        {
            if (!NativeControls.IsGameKeyPressed(0, GameKey.Sprint))
            {
                StandUp("vault_end", 0.07f, 0.27f);
                StandUp("vault_end_r", 0.05f, 0.25f);
                StandUp("waist_to_vault", 0.52f, 0.6f);
                StandUp("waist_to_vault_shallow", 0.52f, 0.6f);
            }
        }
        private static void StandUp(string animName, float timeMin, float timeMax)
        {
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", animName))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", animName, out movepntr);
                if (movepntr > timeMin && movepntr < timeMax && !NativeControls.IsGameKeyPressed(0, GameKey.Sprint)
                    && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", animName))
                {
                    if (!IS_PED_RAGDOLL(Main.PlayerHandle))
                    {
                        if (!HAVE_ANIMS_LOADED("move_crouch"))
                            REQUEST_ANIMS("move_crouch");
                        if (!HAVE_ANIMS_LOADED("ragdoll_trans"))
                            REQUEST_ANIMS("ragdoll_trans");
                        Main.PlayerPed.ActivateDrunkRagdoll(500);
                        GET_GAME_TIMER(out fTimer);
                        getUp = true;
                    }
                }
            }
        }
        private static void ProcessStandUp()
        {
            if (getUp)
            {
                if (Main.gTimer >= fTimer + 440)
                {
                    GET_GROUND_Z_FOR_3D_COORD(plyrPos, out float groundZ);
                    if ((plyrPos.Z - groundZ) <= 0.65f)
                        groundPos = (plyrPos.Z - groundZ);
                    else
                        groundPos = 0.65f;
                    FREEZE_CHAR_POSITION(Main.PlayerHandle, false);
                    SET_CHAR_COORDINATES(Main.PlayerHandle, new Vector3(plyrPos.X, plyrPos.Y, (plyrPos.Z - groundPos)));
                    _TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "recover_balance", "ragdoll_trans", 2, 0, 0, 0, 0, -1);
                    //REMOVE_ANIMS("ragdoll_trans");
                    speedUp = true;
                    getUp = false;
                }
                else if (Main.gTimer >= fTimer + 90 && !IS_PED_RAGDOLL(Main.PlayerHandle))
                    FREEZE_CHAR_POSITION(Main.PlayerHandle, true);
                else if (Main.gTimer >= fTimer + 40 && IS_PED_RAGDOLL(Main.PlayerHandle))
                {
                    plyrPos = Main.PlayerPos;
                    BLEND_FROM_NM_WITH_ANIM(Main.PlayerHandle, "move_crouch", "crouchidle2idle", 10, 0, 0, 0);
                }
            }
            if (speedUp)
            {
                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "ragdoll_trans", "recover_balance"))
                {
                    SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "ragdoll_trans", "recover_balance", 0.8f);
                    speedUp = false;
                }
            }
        }
        private static void ProcessVaultStand(string animName, Vector3 stdPos, float groundPos)
        {
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", animName))
            {
                GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "climb_std", animName, out float animTime);
                if (animTime > 0.5 && animTime < 0.9 && IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "climb_std", animName))
                {
                    if ((stdPos.Z - groundPos) < 1.0f && !isClimbing && !NativeControls.IsGameKeyPressed(0, GameKey.Sprint))
                        isClimbing = true;
                }
            }
        }
    }
}
