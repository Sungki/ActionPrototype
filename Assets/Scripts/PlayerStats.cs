using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    #region - Variables -
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int maxSp;
    [SerializeField]
    private int maxKit;

    [Space]
    public int hp;
    public int sp;
    public int kit;
    public int key;


    [Space]
    public bool isHurt;

    [Space]
    private Player player;
    private GameObject theUI;
    private UI ui;

    private LevelManager levelManager;
    private GameObject theLevelManager;
    private CameraManager cameraManager;
    

    #endregion

    void Start() {
        theUI = GameObject.FindGameObjectWithTag("UI");
        ui = theUI.GetComponent<UI>();
        player = GetComponent<Player>();

        theLevelManager = GameObject.FindGameObjectWithTag("LevelManager");
        levelManager = theLevelManager.GetComponent<LevelManager>();
        cameraManager = theLevelManager.GetComponent<CameraManager>();

        maxHp = 5;
        maxSp = 5;
        maxKit = 3;
        hp = maxHp;
        sp = maxSp;
        kit = maxKit;
        isHurt = false;
    }

    void Update() {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyHitbox" && isHurt == false) {
            isHurt = true;
            player.Hurt();
            Hurt();
        }

        if (other.tag == "EnemyArrow" && isHurt == false)
        {
            Destroy(other.gameObject);
            isHurt = true;
            player.Hurt();
            Hurt();
        }

        if (other.tag == "Mana")
        {
            HealSP();
        }
    }


    #region - HP functions -
    public void Hurt(){
        hp -= 1;
        AudioManager.audioManager.PlayHurtCollision();
        cameraManager.CamShake();
        if (hp != 0)
        {
            AudioManager.audioManager.PlayHurt();
        }

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }
    }

    public void Fall()
    {
        AudioManager.audioManager.PlayHurtCollision();
        cameraManager.CamShake();
    }

    public void Die()
    {
        AudioManager.audioManager.PlayDeath();
        this.gameObject.SetActive(false);
        levelManager.GameOver();
    }

    #endregion

    #region - SP functions - 
    public void UseSP() {
        sp -= 1;
        if (sp <= 0)
        {
            sp = 0;
        }

    }

    public void HealSP() {
        sp += 1;
        if (sp >= maxSp)
        {
            sp = maxSp;
        }
    }
    #endregion


    #region - Healthkit Functions - 
    public void Heal()
    {
        if (kit > 0) {
            hp = maxHp;
            kit -= 1;

            if (kit <= 0)
            {
                kit = 0;
            }
        }
    }

    public void AddKit()
    {
        kit += 1;
        
        if (kit >= maxKit)
        {
            kit = maxKit;
        }
    }

    #endregion

}
