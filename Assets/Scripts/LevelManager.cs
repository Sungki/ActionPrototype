using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager levelManager;

    public Vector3 respawnPos;
    public GameObject player;
    public float delay;
    public string level;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CheckLevel();
    }

    public void CheckLevel ()
    {
        if (level == "Forest")
        {
            AudioManager.audioManager.PlayForest();
            AudioManager.audioManager.forestAmb.Play();

        }

        if (level == "Tower")
        {
            //AudioManager.audioManager.PlayTower();
            AudioManager.audioManager.towerAmb.Play();
        }
    }

    public void UpdateRespawn() {
        respawnPos = player.transform.position;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCo());
    }

    public IEnumerator GameOverCo()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Main");
    }

    public void Level2()
    {
        StartCoroutine(Level2Co());
    }

    public IEnumerator Level2Co()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Level 2");
    }
}
