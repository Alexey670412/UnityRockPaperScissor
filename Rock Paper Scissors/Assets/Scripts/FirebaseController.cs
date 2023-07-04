using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Analytics;

public class FirebaseController
{
    private FirebaseApp app;
    private FirebaseDatabase dbRef;
    private DatabaseReference refRoom;   
    GameContoller gameContollerFirebase; 
   

    void Awake()
    {
        //инициализация Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                app = FirebaseApp.DefaultInstance;
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    //запись в базу данных выбора игрока Камень, ножницы или бумага
    public void SendChoose(int choose){
        refRoom = FirebaseDatabase.DefaultInstance.RootReference.Child("rooms/"+PlayerController.roomcode);
        string strchoose = choose == 0 ? "r" : choose == 1 ? "p" : "s";
        refRoom.Child("player"+PlayerController.GetPlayerIdInGame()+"choose").SetValueAsync(strchoose);
    }

    //Подписка на обновлнеия Firebase playroom
    public void FirebaseDatabaseSubscribeOn(GameContoller gameContoller, string roomcode)
    {
        Debug.Log("subscribing");
        gameContollerFirebase = gameContoller;
        refRoom = FirebaseDatabase.DefaultInstance.RootReference.Child("rooms/"+roomcode);
        refRoom.ValueChanged += PlayRoomValueChenged;
        
    }
    //Отписка на обновлнеия Firebase playroom
    public void FirebaseDatabaseSubscribeOff(string roomcode)
    {
        refRoom = FirebaseDatabase.DefaultInstance.RootReference.Child("rooms/"+roomcode);
        Debug.Log("SubscribeOff");
        refRoom.ValueChanged -= PlayRoomValueChenged;
    }

    //При изменении данных когда мы подписаны
    private void PlayRoomValueChenged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null){
            Debug.LogError(e.DatabaseError);
            return;
        }
        DataSnapshot snapshot = e.Snapshot;
        Debug.Log(snapshot.Child("player1").Value);
        if(snapshot.Child("player1").Value!=null && snapshot.Child("player2").Value!=null){
            EnemyController.SetPlayerName(snapshot.Child("player"+EnemyController.GetPlayerIdInGame()).Value.ToString());
            if((snapshot.Child("player1").Value.ToString()!="-") && (snapshot.Child("player2").Value.ToString()!="-")){
                gameContollerFirebase.SetGameState(true);
                string playerChoose = snapshot.Child("player"+PlayerController.GetPlayerIdInGame()+"choose").Value.ToString();
                string enemyChoose = snapshot.Child("player"+EnemyController.GetPlayerIdInGame()+"choose").Value.ToString();
                if((playerChoose !="-") && (enemyChoose !="-")){
                    int winnerId = 0;
                    winnerId = GetWinnerID(playerChoose, enemyChoose);
                    Debug.Log("WinnerId"+winnerId);
                    AnimationController handPlayer = GameObject.Find("HandPlayer").GetComponent<AnimationController>();
                    AnimationController handEnemy = GameObject.Find("HandEnemy").GetComponent<AnimationController>();
                    handPlayer.Choice(playerChoose);
                    handEnemy.Choice(enemyChoose);
                    gameContollerFirebase.SetWinnerId(winnerId);
                }
            }
        }
    }
    //алгоритм выбора победителя
    private int GetWinnerID(string playerchoose, string enemychoose){
        switch(playerchoose){
            case "r":
                switch(enemychoose){
                    case "s":
                        return 1;
                    case "p":
                        return 2;
                    case "r":
                        return 3;
                    default:
                        Debug.Log("Error cant get winner id");
                        return 0;
                }
            case "s":
                switch(enemychoose){
                    case "s":
                        return 3;
                    case "p":
                        return 1;
                    case "r":
                        return 2;
                    default:
                        Debug.Log("Error cant get winner id");
                        return 0;
                }
            case "p":
                switch(enemychoose){
                    case "s":
                        return 2;
                    case "p":
                        return 3;
                    case "r":
                        return 1;
                    default:
                        Debug.Log("Error cant get winner id");
                        return 0;
                }
        }
        return 0;
    }
    //проверить существует ли комната
    public void CheckRoom(RoomController room, string roomcode){
        FirebaseDatabase.DefaultInstance.GetReference("rooms/"+roomcode).GetValueAsync().ContinueWith(task =>{
            if(task.IsFaulted){
                Debug.LogError("Error in get info on firebase");
            }
            else if(task.IsCompleted){
                DataSnapshot snapshot = task.Result;
                if(snapshot.Child("player1").Value==null){
                    CreateRoom(room, roomcode);
                }else{
                    GetRoomStatus(room, roomcode);
                }
            }
        });
    }
    //создать комнату
    public void CreateRoom(RoomController room, string roomcode){
        refRoom = FirebaseDatabase.DefaultInstance.RootReference.Child("rooms/"+roomcode);    
        refRoom.Child("player1").SetValueAsync("-");
        refRoom.Child("player2").SetValueAsync("-");
        refRoom.Child("player1choose").SetValueAsync("-");
        refRoom.Child("player2choose").SetValueAsync("-");
        GetRoomStatus(room, roomcode);
    }
    //Алгоритм проверки есть ли свободные места в комнате
    public void GetRoomStatus(RoomController room, string roomcode)
    {   
        refRoom = FirebaseDatabase.DefaultInstance.RootReference.Child("rooms/"+roomcode);
        string playername = PlayerController.GetPlayerName();
        FirebaseDatabase.DefaultInstance.GetReference("rooms/"+roomcode).GetValueAsync().ContinueWith(task =>{
            if(task.IsFaulted){
                Debug.LogError("Error in get info on firebase");
            }
            else if(task.IsCompleted){
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Child("player1").Value.ToString());
                Debug.Log(snapshot.Child("player2").Value.ToString());
                if (snapshot.Child("player1").Value.ToString()=="-"){
                    PlayerController.SetPlayerIdInGame(1);
                    PlayerController.SetInGameState(true);
                    refRoom.Child("player1").SetValueAsync(playername);
                    Debug.Log("player1 on");
                    EnemyController.SetPlayerName(snapshot.Child("player"+"2").Value.ToString());
                    EnemyController.SetPlayerIdInGame(2);
                    room.EnterRoom(roomcode);
                }else if (snapshot.Child("player2").Value.ToString()=="-"){
                    PlayerController.SetPlayerIdInGame(2);
                    PlayerController.SetInGameState(true);
                    refRoom.Child("player2").SetValueAsync(playername);
                    Debug.Log("player2 on");
                    EnemyController.SetPlayerName(snapshot.Child("player"+"1").Value.ToString());
                    EnemyController.SetPlayerIdInGame(1);
                    room.EnterRoom(roomcode);
                }else {
                    Debug.Log("Room is Full");
                }
            }
        });
    }

    //Удаление из комнаты
    public void DeleteFromRoom()
    {
        refRoom = FirebaseDatabase.DefaultInstance.RootReference.Child("rooms/"+PlayerController.roomcode);
        refRoom.Child("player"+PlayerController.GetPlayerIdInGame()+"choose").SetValueAsync("-");
        refRoom.Child("player"+PlayerController.GetPlayerIdInGame()).SetValueAsync("-");
    }
    //удалить стратегию игрока
    public void DeleteChoose(){
        refRoom = FirebaseDatabase.DefaultInstance.RootReference.Child("rooms/"+PlayerController.roomcode);
        refRoom.Child("player"+PlayerController.GetPlayerIdInGame()+"choose").SetValueAsync("-");
        refRoom.Child("player"+EnemyController.GetPlayerIdInGame()+"choose").SetValueAsync("-");
    }
    //проверка пустоты комнаты если комната пуста, то запускается функция удаления комнаты
    public void CheckZeroRoom(string roomcode){
        FirebaseDatabase.DefaultInstance.GetReference("rooms/"+roomcode).GetValueAsync().ContinueWith(task =>{
            if(task.IsFaulted){
                Debug.LogError("Error in get info on firebase");
            }
            else if(task.IsCompleted){
                DataSnapshot snapshot = task.Result;
                if(snapshot.Child("player1").Value.ToString()=="-" && snapshot.Child("player2").Value.ToString()=="-")
                {
                    DeleteRoom();
                }
            }
        });
    }
    //удалить комнату
    public void DeleteRoom(){
        refRoom = FirebaseDatabase.DefaultInstance.RootReference.Child("rooms/"+PlayerController.roomcode);
        refRoom.Child("player1choose").SetValueAsync(null);
        refRoom.Child("player1").SetValueAsync(null);
        refRoom.Child("player2choose").SetValueAsync(null);
        refRoom.Child("player2").SetValueAsync(null);
    }
}
