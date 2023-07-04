using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerController
{
    //класс игрока
    public static string playername;
    private static int playerIdInGame;
    private static bool inGame;
    public static string roomcode;

    static void Start()
    {
        inGame = false;
    }

    static public bool GetInGameState(){
        return inGame;
    }
    static public void SetInGameState(bool state){
        inGame = state;
    }
    static public string GetPlayerName(){
        return playername;
    }
    static public void SetPlayerName(string name){
        playername = name;
    }
    static public void SetPlayerIdInGame(int id){
        playerIdInGame = id;
    }
    static public int GetPlayerIdInGame(){
        return playerIdInGame;
    }
    
}
