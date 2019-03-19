using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfalls : MonoBehaviour {
    public GameObject target;
    public PlayerStats playerStats;
    public GameObject theLevelManager;
    public LevelManager levelManager;

    public float respawnDelay;



    void Start()
    {
        theLevelManager = GameObject.FindGameObjectWithTag("LevelManager");
        levelManager = theLevelManager.GetComponent<LevelManager>();
    }

    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D other) {
       if (other.tag == "Player") {
            target = other.gameObject;
            playerStats = target.GetComponent<PlayerStats>();
            playerStats.Fall();
            target.SetActive(false);
            StartCoroutine(Respawn());
        }
    }

    public IEnumerator Respawn() {
        yield return new WaitForSeconds(respawnDelay);
        target.transform.position = levelManager.respawnPos;
        target.SetActive(true);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy" || other.tag == "Archer")
        {
            print("Enemy Entered");
            target = other.gameObject; 
            Destroy(target);
            AudioManager.audioManager.EnemyDeath();
        }
    }
}
