using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;

namespace MoveImprove.ivsdk
{
    public class Main : Script
    {
        // IniShit
        public static Keys ClimbDownKey;
        public static GameKey GrabKey;
        public static bool Alt180Turn;
        public static bool QuickTurnStop;
        public static bool SprintToVehicles;
        public static bool ForceRun;
        public static bool ExtremeClimbing;
        public static bool ClimbDown;
        public static bool JumpFromLedges;
        public static bool FasterJacking;
        public static bool FixRagdoll;
        public static bool OldLedgeMethod;
        public static bool GrabEnable;
        public static bool FlipEnable;
        public static float CombatRollSpeed;
        public static float PickupObjectSpeed;
        public static float ClimbAndShimmySpeed;
        public static float PeekFromCoverSpeed;
        public static float CoverTurnSpeed;
        public static float GetInCoverSpeed;
        public static float EnterExitVehSpeed;
        public static float CrouchingSpeed;
        public static float HolsterSpeed;
        public static int NumOfWeapIDs;

        // OtherShit
        public static DelayedCalling TheDelayedCaller;
        public static IVPed PlayerPed { get; set; }
        public static uint PlayerIndex { get; set; }
        public static int PlayerHandle { get; set; }
        public static Vector3 PlayerPos { get; set; }
        public Main()
        {
            Uninitialize += Main_Uninitialize;
            Initialized += Main_Initialized;
            Tick += Main_Tick;
            KeyDown += Main_KeyDown;
            TheDelayedCaller = new DelayedCalling();
        }

        public static void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (FlipEnable)
            {
                if (e.KeyValue == (int)Keys.M)
                    FlipsNShit.DoFlip();

                if (e.KeyValue == (int)Keys.N)
                    FlipsNShit.DoBackFlip();
            }
        }

        private void Main_Uninitialize(object sender, EventArgs e)
        {
            if (TheDelayedCaller != null)
            {
                TheDelayedCaller.ClearAll();
                TheDelayedCaller = null;
            }
            RagdollFix.pList.Clear();
            RagdollFix.vList.Clear();
            RagdollFix.aList.Clear();
        }

        private void Main_Initialized(object sender, EventArgs e)
        {
            LoadSettings(Settings);
            FastAnims.Init(Settings);
            RagdollFix.Init();
            //Prone.Init();
        }

        private void Main_Tick(object sender, EventArgs e)
        {
            PlayerPed = IVPed.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());
            PlayerIndex = GET_PLAYER_ID();
            PlayerHandle = PlayerPed.GetHandle();
            PlayerPos = PlayerPed.Matrix.Pos;

            GET_CURRENT_CHAR_WEAPON(PlayerHandle, out int pWeap);

            TheDelayedCaller.Process();
            PedHelper.GrabAllPeds();
            FastAnims.Tick();
            CounterStrikes.Tick();
            //Prone.Tick();
            if (FlipEnable)
                FlipsNShit.Tick();
            AdvancedClimbing.Tick();
            if (FixRagdoll)
                RagdollFix.Tick();
            if (Alt180Turn)
                Alt180TurnScript.Tick();
            if (SprintToVehicles || ForceRun)
                ForceRunning.Tick();
            if (FasterJacking)
                FasterJackingScript.Tick();
            if (GrabEnable)
                GrabAndThrow.Tick();
        }
        /*public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }*/
        private void LoadSettings(SettingsFile settings)
        {
            // Booleans
            SprintToVehicles = settings.GetBoolean("MAIN", "SprintToVehicles", false);
            ForceRun = settings.GetBoolean("MAIN", "ForceRun", false);
            FasterJacking = settings.GetBoolean("MAIN", "FasterJacking", false);
            Alt180Turn = settings.GetBoolean("MAIN", "Alt180Turn", false);
            QuickTurnStop = settings.GetBoolean("MAIN", "StopImmediately", false);
            OldLedgeMethod = settings.GetBoolean("EXPERIMENTAL FEATURES", "OldJumpFromLedgeMethod", false);
            ExtremeClimbing = settings.GetBoolean("EXPERIMENTAL FEATURES", "ExtremeClimbing", false);
            ClimbDown = settings.GetBoolean("EXPERIMENTAL FEATURES", "ClimbDown", false);
            JumpFromLedges = settings.GetBoolean("EXPERIMENTAL FEATURES", "JumpFromLedges", false);
            FixRagdoll = settings.GetBoolean("MAIN", "RagdollFix", false);
            GrabEnable = settings.GetBoolean("EXPERIMENTAL FEATURES", "GrabEnable", false);
            FlipEnable = settings.GetBoolean("EXPERIMENTAL FEATURES", "FlipAndTackle", false);

            // AnimSpeeds
            CombatRollSpeed = settings.GetFloat("ANIMATION SPEED", "CombatRoll", 1.0f);
            PickupObjectSpeed = settings.GetFloat("ANIMATION SPEED", "PickupObject  ", 1.0f);
            ClimbAndShimmySpeed = settings.GetFloat("ANIMATION SPEED", "ClimbAndShimmy", 1.0f);
            PeekFromCoverSpeed = settings.GetFloat("ANIMATION SPEED", "PeekFromCover", 1.0f);
            CoverTurnSpeed = settings.GetFloat("ANIMATION SPEED", "CoverTurn", 1.0f);
            GetInCoverSpeed = settings.GetFloat("ANIMATION SPEED", "GetInCover", 1.0f);
            EnterExitVehSpeed = settings.GetFloat("ANIMATION SPEED", "EnterandExitVehicles", 1.0f);
            CrouchingSpeed = settings.GetFloat("ANIMATION SPEED", "Crouching", 1.0f);
            HolsterSpeed = settings.GetFloat("ANIMATION SPEED", "Holstering", 1.0f);

            // OtherShit
            ClimbDownKey = settings.GetKey("EXPERIMENTAL FEATURES", "ClimbDownKey", Keys.J);
            GrabKey = (GameKey)settings.GetInteger("EXPERIMENTAL FEATURES", "GrabKey", 23);
            NumOfWeapIDs = settings.GetInteger("MAIN", "NumOfWeaponIDs", 60);
        }
    }
}
