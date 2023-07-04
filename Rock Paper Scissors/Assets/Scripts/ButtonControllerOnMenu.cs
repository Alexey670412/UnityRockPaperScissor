using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonControllerOnMenu : MonoBehaviour
{
    public InputField name;
    public InputField roomcode;
 
    RoomController RoomController;
    
    void Start()
    {
        RoomController = GameObject.Find("RoomController").GetComponent<RoomController>();
        if(name.text==""){
            name.text=PlayerController.GetPlayerName();
        }
    }
    void OnMouseUpAsButton()
    {
        switch(gameObject.name){
            case "Play":  
                if(name.text=="" || roomcode.text=="")
                {
                    Debug.Log("Error. You need write name");
                    return;
                }else{
                    PlayerController.SetPlayerName(name.text);
                    RoomController.FindRoom(roomcode.text);
                }       
                break;
            default:
                break;
        }
    }
    void Update()
    {
        if(PlayerController.GetInGameState()){
            SceneManager.LoadScene("Play");  
        }
    }
}