using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {
    [SerializeField]
    public GameObject PauseUI;
    [SerializeField]
    private bool paused;

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject thePlayer;

    void Start() {

        PauseUI.SetActive(false);
        paused = false;
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        player = thePlayer.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (paused == false) {
                PauseGame();
                AudioManager.audioManager.PlayPause();
            }

            else if (paused == true) {
                ResumeGame();
                AudioManager.audioManager.PlaySelect();
            }
        }
    }

    public void PauseGame() {
        PauseUI.SetActive(true);
        paused = true;
        player.canMove = false;
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        PauseUI.SetActive(false);
        paused = false;
        player.canMove = true;
        Time.timeScale = 1;
    }
}
