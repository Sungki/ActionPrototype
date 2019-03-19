using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public GameObject player;
    public PlayerStats playerStats;

    private GameObject theLevelManager;
    private CameraManager cameraManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        theLevelManager = GameObject.FindGameObjectWithTag("LevelManager");
        cameraManager = theLevelManager.GetComponent<CameraManager>();
    }

    void Update()
    {
        if (hp <= 0)
        {
            Die();
        }
    }

    public void EnemyHit()
    {
        AudioManager.audioManager.EnemyHurt();
        hp -= 1;
    }

    public void Die()
    {
        playerStats.HealSP();
        cameraManager.CamShake();
        AudioManager.audioManager.EnemyDeath();
        hp = 0;
        Destroy(this.gameObject);
    }

}
