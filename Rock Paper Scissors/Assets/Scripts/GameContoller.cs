using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameContoller : MonoBehaviour
{
    FirebaseController firebaseController = new FirebaseController();
    private bool GameState;
    private bool winnerstate;
    public static int winnerId;
    public Text winnerText;
    AnimationController handPlayer;
    AnimationController handEnemy;
    PlayUIController playUIController;

    
    void Start()
    {
        //Поиск компонентов
        handPlayer = GameObject.Find("HandPlayer").GetComponent<AnimationController>();
        handEnemy = GameObject.Find("HandEnemy").GetComponent<AnimationController>();
        playUIController = GameObject.Find("UI").GetComponent<PlayUIController>();
    }
    
    void OnMouseUpAsButton()
    {
        switch(gameObject.name){
            case "Rock":
                firebaseController.SendChoose(0);
                break;
            case "Paper":
                firebaseController.SendChoose(1);
                break;
            case "Scissor":
                firebaseController.SendChoose(2);
                break;
            case "Menu":
                firebaseController.DeleteFromRoom();
                PlayerController.SetInGameState(false); 
                firebaseController.FirebaseDatabaseSubscribeOff(PlayerController.roomcode);
                firebaseController.CheckZeroRoom(PlayerController.roomcode);
                SceneManager.LoadScene("Main");
                break;
            case "Replay":
                firebaseController.DeleteChoose();
                handEnemy.SetDefault();
                handPlayer.SetDefault();
                playUIController.SetDefault();
                break;
            case "LeaveFromRoom":
                firebaseController.DeleteFromRoom();
                PlayerController.SetInGameState(false); 
                firebaseController.FirebaseDatabaseSubscribeOff(PlayerController.roomcode);
                firebaseController.CheckZeroRoom(PlayerController.roomcode);
                SceneManager.LoadScene("Main");
                break;
            default:
                break;
        }
    }

    public void GameStart(string roomcode){
        //включение "подписки" на комнату
        PlayerController.roomcode = roomcode;
        firebaseController.FirebaseDatabaseSubscribeOn(this, roomcode);  
    }
    public void SetGameState(bool state){
        GameState = state;
    }
    public bool GetGameState(){
        return GameState;
    }
    //Алгоритм при нахождении победителя
    public void SetWinnerId(int id){
        winnerId = id;
        Debug.Log("winnerID"+id);
        winnerstate = true;
        firebaseController.FirebaseDatabaseSubscribeOff(PlayerController.roomcode);
        Debug.Log("winnerstate" + winnerstate.ToString());
        playUIController = GameObject.Find("UI").GetComponent<PlayUIController>();
        playUIController.winner = winnerId;
        playUIController.StopVisableChoose();
    }
}
