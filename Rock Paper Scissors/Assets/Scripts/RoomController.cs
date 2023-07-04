using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class RoomController : MonoBehaviour
{
    GameContoller gamecontroller = new GameContoller();
    FirebaseController firebaseController = new FirebaseController();
    //Инициализации алгоритма проверки комнаты
    public void FindRoom(string room){
        Debug.Log(room);
        firebaseController.CheckRoom(this,room); 
    }
    //если комната свободна и мы туда зашли
    public void EnterRoom(string roomcode){
        Debug.Log("Play");
        gamecontroller.GameStart(roomcode);        
    }
}
