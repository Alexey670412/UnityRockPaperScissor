using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public void LoadSceneMain(){
        SceneManager.LoadScene("Main");
    }

    public void LoadScenePlay(){
        Debug.Log("loadPlay");
        if(PlayerController.GetInGameState()){
            SceneManager.LoadSceneAsync(1);
        }
    }
}
