using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSettings {

    public static int[] player_settings = new int[4];
    public static bool[] buttonSelected = new bool[4];

    public static bool CharactersSelectedByPlayers()
    {
        foreach (int value in player_settings)
        {
            if (value != 0)
                return true;
        }
        return false;
    }
}
