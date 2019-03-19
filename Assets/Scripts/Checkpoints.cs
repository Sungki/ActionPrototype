using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public LevelManager levelManager;
    public GameObject theLevelManager;

    void Start()
    {
        theLevelManager = GameObject.FindGameObjectWithTag("LevelManager");
        levelManager = theLevelManager.GetComponent<LevelManager>();
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            levelManager.UpdateRespawn();
        }

    }

    
}
