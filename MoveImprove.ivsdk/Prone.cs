using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;
using CCL;
using CCL.GTAIV;

namespace MoveImprove.ivsdk
{
    internal class Prone
    {
        private static bool keyPress;
        private static bool isProne;
        private static bool isRolling;
        private static float animTime;

        /*public static void Init()
        {
            REQUEST_ANIMS("misskbtruck");
            REQUEST_ANIMS("get_up");
        }*/
        public static void Tick()
        {
            if (NativeControls.IsGameKeyPressed(0, GameKey.SoundHorn) && !IS_PED_IN_COVER(Main.PlayerHandle) && !keyPress)
            {
                keyPress = true;
                if (!isProne)
                {
                    isProne = true;
                    //IVGame.ShowSubtitleMessage("ass");
                    if (!HAVE_ANIMS_LOADED("misskbtruck"))
                        REQUEST_ANIMS("misskbtruck");
                    //_TASK_PLAY_ANIM_NON_INTERRUPTABLE(Main.PlayerHandle, "crawl_idle", "misskbtruck", 4.0f, 1, 0, 0, 0, -1);
                    _TASK_PLAY_ANIM(Main.PlayerHandle, "crawl_idle", "misskbtruck", 4.0f, 1, 1, 1, 0, -1);
                }
                else
                {
                    isProne = false;
                    //IVGame.ShowSubtitleMessage("tit");
                    if (!HAVE_ANIMS_LOADED("get_up"))
                        REQUEST_ANIMS("get_up");
                    _TASK_PLAY_ANIM(Main.PlayerHandle, "get_up_fast", "get_up", 4.0f, 0, 0, 1, 0, -1);
                    SET_CHAR_DUCKING_TIMED(Main.PlayerHandle, 1);
                }
            }
            else if (isProne)
            {
                if (isRolling)
                {
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "misskbtruck", "crawl_roll_left", 1.75f);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "misskbtruck", "crawl_roll_right", 1.75f);
                    if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_roll_left"))
                        GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "misskbtruck", "crawl_roll_left", out animTime);
                    else if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_roll_right"))
                        GET_CHAR_ANIM_CURRENT_TIME(Main.PlayerHandle, "misskbtruck", "crawl_roll_right", out animTime);
                }
                if (NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.NavLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.NavRight) && !isRolling)
                {
                    //IVGame.ShowSubtitleMessage("fwd");
                    if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_fwd_loop"))
                        _TASK_PLAY_ANIM(Main.PlayerHandle, "crawl_fwd_loop", "misskbtruck", 4.0f, 1, 1, 1, 0, -1);
                    SET_CHAR_ANIM_SPEED(Main.PlayerHandle, "misskbtruck", "crawl_fwd_loop", 2.0f);
                }
                else if (NativeControls.IsGameKeyPressed(0, GameKey.NavLeft) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.NavRight) && !isRolling)
                {
                    isRolling = true;
                    //IVGame.ShowSubtitleMessage("rollLeft");
                    if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_roll_left"))
                        _TASK_PLAY_ANIM(Main.PlayerHandle, "crawl_roll_left", "misskbtruck", 4.0f, 0, 0, 1, 0, -1);
                }
                else if (NativeControls.IsGameKeyPressed(0, GameKey.NavRight) && !NativeControls.IsGameKeyPressed(0, GameKey.MoveForward) && !NativeControls.IsGameKeyPressed(0, GameKey.NavLeft) && !isRolling)
                {
                    isRolling = true;
                    //IVGame.ShowSubtitleMessage("rollRight");
                    if (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_roll_right"))
                        _TASK_PLAY_ANIM(Main.PlayerHandle, "crawl_roll_right", "misskbtruck", 4.0f, 0, 0, 1, 0, -1);
                }
                else if (animTime > 0.9 || (!IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_roll_left") && !IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_roll_right")))
                {
                    isRolling = false;
                    _TASK_PLAY_ANIM(Main.PlayerHandle, "crawl_idle", "misskbtruck", 4.0f, 1, 1, 1, 0, -1);
                }
            }
            if (!NativeControls.IsGameKeyPressed(0, GameKey.SoundHorn))
            {
                keyPress = false;
            }
        }
        /*public static bool IsProne()
        {
            if (IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_idle") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_fwd_loop") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_roll_left") || IS_CHAR_PLAYING_ANIM(Main.PlayerHandle, "misskbtruck", "crawl_roll_right"))
                return true;
            else
                return false;
        }*/
    }
}
