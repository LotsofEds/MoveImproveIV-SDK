using IVSDKDotNet;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using static IVSDKDotNet.Native.Natives;
using static System.Net.Mime.MediaTypeNames;
using CCL.GTAIV;

namespace MoveImprove.ivsdk
{
    internal class MeleeStamina
    {
        // IniShit
        private static bool debug;
        private static float minSpeed;
        private static float tiredThresh;
        private static List<FightMoves> moveList = new List<FightMoves>();
        private static uint recoverTime;
        private static int recoverySpeed;
        private static bool fatigueEnable;
        private static float fatigueRecovery;
        private static float fatigueDrain;

        // OtherShit
        private static string currMoveSet;
        private static string currMoveName;
        //private static float staminaBar;
        private static uint fTimer;
        private static float animTime;
        private static int tex= 0;
        private static SettingsFile meleeSettings;
        public static void Init(SettingsFile settings)
        {
            moveList.Clear();
            Main.staminaBar = 100;
            Main.maxStamina = 100;

            debug = settings.GetBoolean("MELEE STAMINA", "Debug", false);
            minSpeed = settings.GetFloat("MELEE STAMINA", "TiredMinSpeed", 0.75f);
            tiredThresh = settings.GetFloat("MELEE STAMINA", "TiredStaminaThresh", 10);
            recoverTime = settings.GetUInteger("MELEE STAMINA", "RecoveryTime", 1000);
            recoverySpeed = settings.GetInteger("MELEE STAMINA", "StaminaRecoveryRate", 100);
            fatigueEnable = settings.GetBoolean("MELEE STAMINA", "FatigueEnable", false);
            fatigueRecovery = settings.GetFloat("MELEE STAMINA", "FatigueRecoveryRate", 1.0f);
            fatigueDrain = settings.GetFloat("MELEE STAMINA", "RunningFatigue", 1.0f);

            meleeSettings = new SettingsFile(string.Format("{0}\\IVSDKDotNet\\scripts\\MoveImprove\\MoveList.ini", IVGame.GameStartupPath));
            meleeSettings.Load();

            string[] listOfMoves = meleeSettings.GetSectionNames();

            for (int i = 0; i < listOfMoves.Count(); i++)
            {
                moveList.Add(new FightMoves());

                moveList[i].MoveSet = meleeSettings.GetValue(listOfMoves[i], "MoveSet", "");
                moveList[i].MoveName = meleeSettings.GetValue(listOfMoves[i], "MoveName", "");
                moveList[i].StaminaLoss = meleeSettings.GetFloat(listOfMoves[i], "StaminaLoss", 0);
                moveList[i].FatigueAmount = meleeSettings.GetFloat(listOfMoves[i], "FatiguePenalty", 0);
                moveList[i].LongAnim = meleeSettings.GetBoolean(listOfMoves[i], "Animation", false);
                moveList[i].LossEnd = meleeSettings.GetFloat(listOfMoves[i], "LossEnd", 0);
            }
        }
        public static void Uninit()
        {
            moveList.Clear();
        }
        public static void IngameStart()
        {
            fTimer = 0;
        }
        public static void Tick()
        {
            ProcessReduceStamina();

            if (Main.gTimer >= fTimer + recoverTime)
            {
                if (fatigueEnable)
                {
                    Main.maxStamina += Main.frameTime * fatigueRecovery;

                        GET_CHAR_SPEED(Main.PlayerHandle, out float pSpd);

                        pSpd = Main.Clamp(pSpd, 0.625f, 7.5f);

                    if (IS_CHAR_SITTING_IN_ANY_CAR(Main.PlayerHandle))
                        pSpd = 0.625f;

                    float drainSpd = (pSpd * fatigueDrain) / 2.5f;
                    Main.maxStamina -= Main.frameTime * drainSpd;
                }
                Main.staminaBar += Main.frameTime * recoverySpeed;
            }

            Main.maxStamina = Main.Clamp(Main.maxStamina, 10, 100);
            Main.staminaBar = Main.Clamp(Main.staminaBar, 0, Main.maxStamina);

            if (Main.staminaBar <= tiredThresh)
            {
                bool ignoreSlow = false;
                float speed = (1 - minSpeed) * ((tiredThresh - Main.staminaBar) / tiredThresh);
                for (int i = 0; i < moveList.Count; i++)
                {
                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, moveList[i].MoveSet, moveList[i].MoveName) && moveList[i].LongAnim)
                    {
                        ignoreSlow = true;
                        break;
                    }
                }
                if (tex <= 0)
                {
                    if (!HAS_STREAMED_TXD_LOADED("screenfx"))
                        LOAD_TXD("screenfx");
                    tex = GET_TEXTURE_FROM_STREAMED_TXD("screenfx", "vignette");
                }

                int alpha = (int)(((tiredThresh - Main.staminaBar) / tiredThresh) * 32);
                DRAW_SPRITE((uint)tex, 0.5f, 0.5f, 1.0f, 1.0f, 0, 200, 0, 0, alpha);
                if (!ignoreSlow && IS_CHAR_IN_MELEE_COMBAT(Main.PlayerHandle))
                    SET_CHAR_ALL_ANIMS_SPEED(Main.PlayerHandle, 1 - speed);
            }
            else
            {
                if (HAS_STREAMED_TXD_LOADED("screenfx"))
                    MARK_STREAMED_TXD_AS_NO_LONGER_NEEDED("screenfx");

                if (tex > 0)
                {
                    RELEASE_TEXTURE(tex);
                    tex = 0;
                }
            }
            if (!IS_CHAR_SITTING_IN_ANY_CAR(Main.PlayerHandle))
                ProcessStaminaDisplay();

            if (fatigueEnable)
            {
                if (IS_CHAR_IN_MELEE_COMBAT(Main.PlayerHandle))
                    Main.maxStamina -= Main.frameTime * 0.1f;
                //else
            }
        }
        private static void ProcessStaminaDisplay()
        {
            if (IS_CHAR_IN_MELEE_COMBAT(Main.PlayerHandle) || Main.staminaBar < Main.maxStamina || NativeControls.IsGameKeyPressed(0, GameKey.RadarZoom))
            {
                //DRAW_RECT(0.105f, 0.967f, 0.110f, 0.02f, 0, 0, 0, 120);

                float barSize = (int)Main.staminaBar / 1000.0f;
                float barPos = 0.105f - (0.1f - barSize) / 2;
                //int barCol = (int)(Main.staminaBar * 2.55f);

                float maxSize = (int)Main.maxStamina / 1000.0f;
                float maxPos = 0.105f - (0.1f - maxSize) / 2;

                int colR = (int)((153 - 87) * (-Main.staminaBar / 100));
                int colG = (int)((69 - 124) * (-Main.staminaBar / 100));
                int colB = (int)((69 - 88) * (-Main.staminaBar / 100));

                Color barCol= Color.FromArgb(255, 153 + colR, 69 + colG, 69 + colB);
                DRAW_RECT(0.105f, 0.967f, 0.104f, 0.015f, 0, 0, 0, 180);
                DRAW_RECT(maxPos, 0.967f, maxSize, 0.01f, 192, 192, 192, 40);
                DRAW_RECT(barPos, 0.967f, barSize, 0.01f, barCol.R, barCol.G, barCol.B, barCol.A);
                //DRAW_RECT(barPos, 0.967f, barSize, 0.01f, 255, barCol, barCol, 255);
            }
        }
        private static void ProcessReduceStamina()
        {
            bool isPlayingAnim = false;
            for (int i = 0; i < moveList.Count; i++)
            {
                if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, moveList[i].MoveSet, moveList[i].MoveName))
                {
                    GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, moveList[i].MoveSet, moveList[i].MoveName, out animTime);
                    if (!moveList[i].LongAnim)
                    {
                        if (animTime <= 0.2f && animTime >= 0 && (currMoveSet != moveList[i].MoveSet || currMoveName != moveList[i].MoveName))
                        {
                            Main.maxStamina -= moveList[i].FatigueAmount;
                            Main.staminaBar -= moveList[i].StaminaLoss;
                            currMoveSet = moveList[i].MoveSet;
                            currMoveName = moveList[i].MoveName;

                            if (debug)
                                IVGame.ShowSubtitleMessage(moveList[i].MoveSet + ", " + moveList[i].MoveName + ", " + animTime + ", " + currMoveSet + ", " + currMoveName, 100);
                            GET_GAME_TIMER(out fTimer);
                        }
                        else
                        {
                            if (animTime > 0.2)
                                ResetAnims();
                            GET_GAME_TIMER(out fTimer);
                        }
                    }
                    else
                    {
                        if (animTime >= 0.0f && animTime <= moveList[i].LossEnd)
                        {
                            if (debug)
                                IVGame.ShowSubtitleMessage(moveList[i].MoveSet + ", " + moveList[i].MoveName + ", " + animTime + ", " + moveList[i].LossEnd, 100);

                            if (Main.staminaBar == 0)
                                SET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, moveList[i].MoveSet, moveList[i].MoveName, 1.0f);

                            GET_CHAR_ANIM_TOTAL_TIME(Main.PlayerHandle, moveList[i].MoveSet, moveList[i].MoveName, out float totalTime);
                            Main.staminaBar -= (moveList[i].StaminaLoss * Main.frameTime * 1000) / (totalTime * moveList[i].LossEnd);
                            Main.maxStamina -= (moveList[i].FatigueAmount * Main.frameTime * 1000) / (totalTime * moveList[i].LossEnd);

                            GET_GAME_TIMER(out fTimer);
                            currMoveSet = moveList[i].MoveSet;
                            currMoveName = moveList[i].MoveName;
                        }
                        else if (animTime > moveList[i].LossEnd || animTime < 0)
                        {
                            if (currMoveSet == moveList[i].MoveSet && currMoveName == moveList[i].MoveName)
                                ResetAnims();
                        }
                    }
                    isPlayingAnim = true;
                    break;
                }
            }
            if (!isPlayingAnim)
                ResetAnims();   
        }
        private static void ResetAnims()
        {
            currMoveSet = "";
            currMoveName = "";
        }
    }
    public class FightMoves
    {
        public string MoveSet { get; set; }
        public string MoveName { get; set; }
        public float StaminaLoss { get; set; }
        public float FatigueAmount { get; set; }
        public bool LongAnim { get; set; }
        public float LossEnd { get; set; }
        public FightMoves()
        {
        }
    }
}
