using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //Проигрывание анимации
    public void Choice(string choice){
        switch(choice){
            case "r":
                animator.SetBool("choiceRock", true);
                animator.SetBool("choicePaper", false);
                animator.SetBool("choiceScissor", false);
                break;
            case "s":
                animator.SetBool("choiceRock", false);
                animator.SetBool("choicePaper", false);
                animator.SetBool("choiceScissor", true);
                break;
            case "p":
                animator.SetBool("choiceRock", false);
                animator.SetBool("choicePaper", true);
                animator.SetBool("choiceScissor", false);
                break;
        }
    }
    //При остановки анимации
    public void StopAnimation(){
        Debug.Log("stop animation");
        PlayUIController playUIController;
        playUIController = GameObject.Find("UI").GetComponent<PlayUIController>();
        playUIController.WinnerVisable();
        if(GameContoller.winnerId==3){
            SetDefault();
            playUIController.SetDefault();
            playUIController.WinnerVisable();
            FirebaseController firebaseController = new FirebaseController();
            firebaseController.DeleteChoose();
            return;
        }
        playUIController.VisableButtonMenu();
    }
    //Востановаление анимации в состояние idle
    public void SetDefault(){
        animator.SetBool("choiceRock", false);
        animator.SetBool("choicePaper", false);
        animator.SetBool("choiceScissor", false);
    }
}
