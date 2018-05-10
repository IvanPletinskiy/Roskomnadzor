using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferences {

    private static string MULTIPLAYER = "multiplayer";
    private static string SCORE = "score";
    private static string MUSIC = "music";

    public static int getBaseMultiplayer()
    {
        return PlayerPrefs.GetInt(MULTIPLAYER, 1);
    }

    /// <summary>
    /// Учти, что нужно сохранять базовый множитель
    /// </summary>
    /// <returns></returns>
    public static void setBaseMultiplayer(int multiplayer)
    {
        PlayerPrefs.SetInt(MULTIPLAYER, multiplayer);
    }

    public static long getScore()
    {
        return long.Parse(PlayerPrefs.GetString(SCORE));
    }

    public static void setScore(long score)
    {
        PlayerPrefs.SetString(SCORE, score.ToString());
    }

    public static bool isMusic()
    {
        if (PlayerPrefs.GetInt(MUSIC) != 1)
            return true;
        else
            return false;
    }

    public static void setMusic(bool isMus)
    {
        if (isMus)
            PlayerPrefs.SetInt(MUSIC, 1);
        else
            PlayerPrefs.SetInt(MUSIC, 0);
    }

}
