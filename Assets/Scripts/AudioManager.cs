using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager audioManager;
    public Player player;
    public PlayerStats playerStats;
    public GameObject playerObject;

    public LevelManager levelManager;
    public GameObject theLevelManager;

    #region - Music -
    public AudioSource forest;
    public AudioSource tower;
    public AudioSource forestAmb;
    public AudioSource towerAmb;
    #endregion

    [Space(15)]
    #region - Player Sounds - 
    public AudioSource walkForest;
    public AudioSource walkConcrete;
    public AudioSource dash;
    public AudioSource hurt;
    public AudioSource slash1;
    public AudioSource slash2;
    public AudioSource slash3;
    public AudioSource throwSword;
    public AudioSource plantSword;
    public AudioSource pullSword;
    public AudioSource warp;
    public AudioSource death;
    public AudioSource heal;
    public AudioSource critical;
    #endregion

    #region - Enemy Sounds - 
    [Space(15)]
    public AudioSource enemyShoot;
    public AudioSource enemyAttack;
    public AudioSource enemyHurt;
    public AudioSource enemyDeath;
    public AudioSource enemyGrowl;
    #endregion

    #region - Universal Sounds -
    [Space(15)]
    public AudioSource unlock;
    public AudioSource openDoor;
    public AudioSource failBuzz;
    public AudioSource success;
    public AudioSource hitCollision;
    public AudioSource hurtCollision;

    [Space(15)]
    public AudioSource hover;
    public AudioSource select;
    public AudioSource pause;
    #endregion


    #region - Start & Update Functions -

    public void Awake()
    {
        audioManager = gameObject.GetComponent<AudioManager>();
    }

    public void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null) {
            player = playerObject.GetComponent<Player>();
            playerStats = playerObject.GetComponent<PlayerStats>();
        }

        theLevelManager = GameObject.FindGameObjectWithTag("LevelManager");

        if (theLevelManager != null)
            levelManager = theLevelManager.GetComponent<LevelManager>();

        if (theLevelManager == null)
        {
            return;
        }

        if (levelManager.level == "Forest" )
        {
            walkForest.Play();
            PlayForest();
        }

        if (levelManager.level == "Tower")
        {
            walkConcrete.Play();
            PlayTower();
        }

    }

    public void Update()
    {
            PlayCritical();

        if (theLevelManager == null)
        {
            return;
        }

        if (levelManager.level == "Forest") {
            PlayWalkForest();
        }

        if (levelManager.level == "Tower")
        {
            PlayWalkTower();
        }
    }
    #endregion


    #region - Environment - 
    public void PlayForest()
    {
        forest.Play();
        forestAmb.Play();
    }


    public void PlayTower()
    {
        tower.Play();
        towerAmb.Play();
    }
    #endregion

    #region - Character Sounds -

    public void PlayWalkForest()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && player.isWalking == true) {
            walkForest.volume = 0.75f;
        }

        else {
            walkForest.volume = 0f;
        }
    }

    public void PlayWalkTower()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && player.isWalking == true)
        {
            walkConcrete.volume = 0.75f;
        }

        else
        {
            walkConcrete.volume = 0f;
        }
    }


    public void PlayHurt()
    {
        hurt.Play();
    }
    public void PlayDeath()
    {
        death.Play();
    }

    public void PlayDash()
    {
        dash.Play();

    }
    public void PlayWarp()
    {
        warp.Play();
    }
    public void PlayPull()
    {
        pullSword.Play();
    }

    public void PlayThrow()
    {
        throwSword.Play();
    }
    public void PlayPlant()
    {
        plantSword.Play();
    }

    public void PlaySlash()
    {
        if (Input.GetButtonDown("Attack") && player.Attack == "First" && player.currentState == PlayerState.attack) {
            slash1.Play();
            print("Attack 1");
        }
        else if (Input.GetButtonDown("Attack") && player.Attack == "Second" && player.currentState == PlayerState.attack) {
            slash2.Play();
            print("Attack 2");
        }

        else if (Input.GetButtonDown("Attack") && player.Attack == "Third" && player.currentState == PlayerState.attack)
        {
            slash3.Play();
            print("Attack 3");
        }

    }

    public void PlayHeal()
    {
        heal.Play();
    }

    public void PlayCritical()
    {
        if (playerStats.hp <= 2)
        {
            critical.volume = 1f;
        }

        else
        {
            critical.volume = 0f;
        }
    }
    
    #endregion

    #region - Enemy Sounds - 
    public void EnemyDeath()
    {
        enemyDeath.Play();
    }

    public void EnemyHurt()
    {
        enemyHurt.Play();
    }

    public void EnemyAttack()
    {
        enemyAttack.Play();
    }

    public void EnemyGrowl()
    {
        enemyGrowl.Play();
    }

    public void EnemyShoot() {
        enemyShoot.Play();
    }
    #endregion


    #region - UI Sounds -

    public void PlayHover()
    {
        hover.Play();

    }
    public void PlaySelect()
    {
        select.Play();
    }
    public void PlayPause()
    {
        pause.Play();
    }

    #endregion

    #region - Collisions -
    public void PlayHitCollision()
    {
        hitCollision.Play();
    }

    public void PlayHurtCollision()
    {
        hurtCollision.Play();
    }

    #endregion
}