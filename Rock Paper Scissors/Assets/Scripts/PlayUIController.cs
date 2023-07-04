using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUIController : MonoBehaviour
{
    public Text winnerText;
    public int winner = 0;
    public Text Scissor;
    public Text Rock;
    public Text Paper;
    public Text Menu;
    public Text Replay;
    public Text PlayerName;
    public Text EnemyName;
    public Text Leave;
    public float scissor;
    public float rock;
    public float paper;
    public float menu;
    public float replay;
    public float leave;

    void Start()
    {
        scissor = Scissor.transform.localPosition.y;
        rock = Rock.transform.localPosition.y;
        paper = Paper.transform.localPosition.y;
        menu = Menu.transform.localPosition.y;
        replay = Replay.transform.localPosition.y;
        leave = Leave.transform.localPosition.y;
    }
    
    //Кто выйграл
    public void WinnerVisable(){
        switch(winner){
            case 1:
                winnerText.text = "You Win!";
                break;
            case 2:
                winnerText.text = "You Lose";
                break;
            case 3:
                winnerText.text = "It's a tie";
                break;
            default:
                winnerText.text ="asdasd";
                break;
        }  
    }
    public void SetWinner(int win){
        winner = win;
    }

    //Убирает выборы знаков
    public void StopVisableChoose(){
        Scissor.transform.localPosition  = new Vector3(Scissor.transform.localPosition.x, Scissor.transform.localPosition .y-100, Scissor.transform.localPosition .z);
        Rock.transform.localPosition  = new Vector3(Rock.transform.localPosition.x, Rock.transform.localPosition .y-100, Rock.transform.localPosition .z);
        Paper.transform.localPosition  = new Vector3(Paper.transform.localPosition.x, Paper.transform.localPosition .y-100, Paper.transform.localPosition .z); 
        Leave.transform.localPosition  = new Vector3(Leave.transform.localPosition.x, Leave.transform.localPosition .y+100, Leave.transform.localPosition .z);
    }
    //Показывает кнопки меню и реплей
    public void VisableButtonMenu(){
        Menu.transform.localPosition  = new Vector3(Menu.transform.localPosition.x, Menu.transform.localPosition.y+50, Menu.transform.localPosition .z);
        Replay.transform.localPosition  = new Vector3(Replay.transform.localPosition.x, Replay.transform.localPosition.y+50, Replay.transform.localPosition .z);  
    }
    //Востановаление на изначальную сцену
    public void SetDefault(){
        winnerText.text="";
        Leave.transform.localPosition  = new Vector3(Leave.transform.localPosition.x, leave, Leave.transform.localPosition .z);
        Scissor.transform.localPosition  = new Vector3(Scissor.transform.localPosition.x, scissor, Scissor.transform.localPosition .z);
        Rock.transform.localPosition  = new Vector3(Rock.transform.localPosition.x, rock, Rock.transform.localPosition .z);
        Paper.transform.localPosition  = new Vector3(Paper.transform.localPosition.x, paper, Paper.transform.localPosition .z); 
        Menu.transform.localPosition  = new Vector3(Menu.transform.localPosition.x, menu, Menu.transform.localPosition .z);
        Replay.transform.localPosition  = new Vector3(Replay.transform.localPosition.x, replay, Replay.transform.localPosition .z);
    }
    //изменение имена игроков
    void Update()
    {
        PlayerName.text = PlayerController.GetPlayerName().ToString();
        EnemyName.text = EnemyController.GetPlayerName().ToString();
    } 
}
