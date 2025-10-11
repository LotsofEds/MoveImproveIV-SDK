using CCL;
using CCL.GTAIV;
using IVSDKDotNet;
using IVSDKDotNet.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using static IVSDKDotNet.Native.Natives;

namespace MoveImprove.ivsdk
{
    internal class Pills
    {
        //IniShit
        private static bool enable;
        private static uint adrenalineTime;
        private static uint painKillerTime;
        private static Keys adrenalineKey;
        private static Keys painKillerKey;

        private static bool keyPressed;
        private static bool adrenalineOn;
        private static bool painKillerOn;
        private static bool adrenalineActive;
        private static bool painKillerActive;
        private static bool takeAdrenaline;
        private static bool takePainKiller;
        private static bool takenPills;
        private static bool gotHealth;
        private static int ObjHandle;
        private static uint oldHealth;
        private static uint fTimer;
        private static uint effectTime;
        public static void Init(SettingsFile settings)
        {
            enable = settings.GetBoolean("PILLS", "Enable", false);
        }
        public static void Tick()
        {
            GET_GAME_TIMER(out uint gTimer);
            if (!takenPills)
            {
                if (IVGame.IsKeyPressed(Keys.K) && !keyPressed)
                {
                    keyPressed = true;
                    takeAdrenaline = true;
                }
                else if (IVGame.IsKeyPressed(Keys.L) && !keyPressed)
                {
                    keyPressed = true;
                    takePainKiller = true;
                }
                else if (!IVGame.IsKeyPressed(Keys.K) && !IVGame.IsKeyPressed(Keys.L))
                    keyPressed = false;
            }

            if (takeAdrenaline)
                Adrenaline();
            else if (takePainKiller)
                PainKiller();

            if (adrenalineOn && gTimer > fTimer + 3000)
                AdrenalineEffect();
            else if (painKillerOn && gTimer > fTimer + 3000)
                PainKillerEffect();

            if (adrenalineActive && gTimer > fTimer + effectTime)
            {
                CLEAR_TIMECYCLE_MODIFIER();
                IVGame.ShowSubtitleMessage("Off");
                SET_TIME_SCALE(1.0f);
                SET_CHAR_MOVE_ANIM_SPEED_MULTIPLIER(Main.PlayerHandle, 1f);
                adrenalineActive = false;
                takenPills = false;
            }

            else if (painKillerActive)
            {
                GET_CHAR_HEALTH(Main.PlayerHandle, out uint pHealth);

                if (!gotHealth)
                {
                    oldHealth = pHealth;
                    gotHealth = true;
                }

                if (oldHealth > pHealth && gotHealth)
                {
                    SET_CHAR_HEALTH(Main.PlayerHandle, pHealth + ((oldHealth - pHealth) / 2));
                    gotHealth = false;
                }

                if (gTimer > fTimer + effectTime)
                {
                    CLEAR_TIMECYCLE_MODIFIER();
                    IVGame.ShowSubtitleMessage("Off");
                    painKillerActive = false;
                    takenPills = false;
                }
            }
        }
        private static void RequestStuff()
        {
            if (!HAVE_ANIMS_LOADED("amb@sprunk_plyr"))
                REQUEST_ANIMS("amb@sprunk_plyr");

            if (!HAS_MODEL_LOADED(GET_HASH_KEY("cspillbottle")))
                REQUEST_MODEL(GET_HASH_KEY("cspillbottle"));
        }
        private static void Adrenaline()
        {
            if (HAVE_ANIMS_LOADED("amb@sprunk_plyr") && HAS_MODEL_LOADED(GET_HASH_KEY("cspillbottle")))
            {
                if (!takenPills)
                {
                    takenPills = true;
                    takeAdrenaline = false;
                    adrenalineOn = true;
                    CREATE_OBJECT(GET_HASH_KEY("cspillbottle"), Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z + 10f, out ObjHandle, true);
                    SET_OBJECT_COLLISION(ObjHandle, false);
                    ATTACH_OBJECT_TO_PED(ObjHandle, Main.PlayerHandle, (uint)eBone.BONE_RIGHT_HAND, 0.1f, 0.02f, -0.02f, 0f, 0f, 0f, 0);
                    _TASK_PLAY_ANIM_SECONDARY_NO_INTERRUPT(Main.PlayerHandle, "partial_drink", "amb@sprunk_plyr", 4, 0, 0, 0, 0, -1);
                    GET_GAME_TIMER(out fTimer);
                }
            }
            else
                RequestStuff();
        }
        private static void PainKiller()
        {
            if (HAVE_ANIMS_LOADED("amb@sprunk_plyr") && HAS_MODEL_LOADED(GET_HASH_KEY("cspillbottle")))
            {
                if (!takenPills)
                {
                    takenPills = true;
                    takePainKiller = false;
                    painKillerOn = true;
                    CREATE_OBJECT(GET_HASH_KEY("cspillbottle"), Main.PlayerPos.X, Main.PlayerPos.Y, Main.PlayerPos.Z + 10f, out ObjHandle, true);
                    SET_OBJECT_COLLISION(ObjHandle, false);
                    ATTACH_OBJECT_TO_PED(ObjHandle, Main.PlayerHandle, (uint)eBone.BONE_RIGHT_HAND, 0.1f, 0.02f, -0.02f, 0f, 0f, 0f, 0);
                    _TASK_PLAY_ANIM_SECONDARY_NO_INTERRUPT(Main.PlayerHandle, "partial_drink", "amb@sprunk_plyr", 4, 0, 0, 0, 0, -1);
                    GET_GAME_TIMER(out fTimer);
                }
            }
            else
                RequestStuff();
        }
        private static void AdrenalineEffect()
        {
            IVGame.ShowSubtitleMessage("On");
            DELETE_OBJECT(ref ObjHandle);

            effectTime = 20000;

            SET_TIMECYCLE_MODIFIER("waste");
            SET_TIME_SCALE(0.75f);
            SET_CHAR_MOVE_ANIM_SPEED_MULTIPLIER(Main.PlayerHandle, (float)(1 / 0.75));

            GET_GAME_TIMER(out fTimer);
            adrenalineOn = false;
            adrenalineActive = true;
        }
        private static void PainKillerEffect()
        {
            IVGame.ShowSubtitleMessage("On");
            DELETE_OBJECT(ref ObjHandle);

            effectTime = 20000;

            SET_TIMECYCLE_MODIFIER("waste");

            GET_GAME_TIMER(out fTimer);
            painKillerOn = false;
            painKillerActive = true;
        }
    }
}
