using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferences {

    private static string MULTIPLAYER = "multiplayer";
    private static string SCORE = "score";
    private static string MUSIC = "music";

    public static int getMultiplayer()
    {
        return PlayerPrefs.GetInt(MULTIPLAYER);
    }

    /// <summary>
    /// Учти, что нужно сохранять базовый множитель
    /// </summary>
    /// <returns></returns>
    public static void setMultiplayer(int multiplayer)
    {
        PlayerPrefs.SetInt(MULTIPLAYER, multiplayer);
    }

    public static int getScore()
    {
        return PlayerPrefs.GetInt(SCORE);
    }

    public static void setScore(int score)
    {
        PlayerPrefs.SetInt(SCORE, score);
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
