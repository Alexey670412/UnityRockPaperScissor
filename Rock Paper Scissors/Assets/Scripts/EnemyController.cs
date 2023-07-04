using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController
{
    //Класс противника
    public static string playername;
    private static int playerIdInGame;

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
