using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class How2play : MonoBehaviour {
    public GameObject controller;
    public GameObject keyboard;


    void Update() {
       
    }

    public void controlPad() {
        controller.SetActive(true);
    }

    public void keyBoard() {
        keyboard.SetActive(true);
    }

    public void exitPad() {
        controller.SetActive(false);
    }

    public void exitKeyboard() {
        keyboard.SetActive(false);
    }

}
