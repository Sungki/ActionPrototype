using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public GameObject player;
    public PlayerStats playerStats;
    public Player playerScript;

    #region Variables 
    [Space]
    public int currentHp;

    // Sprites used for different HP states
    public Sprite maxHp;
    public Sprite fourHp;
    public Sprite threeHp;
    public Sprite twoHp;
    public Sprite oneHp;
    public Sprite noHp;
    // This is the slot that cycles sprites depending on the level of HP/SP

    [Space]
    public int currentSp;

    // Sprites used for different SP states
    public Sprite maxSp;
    public Sprite fourSp;
    public Sprite threeSp;
    public Sprite twoSp;
    public Sprite oneSp;
    public Sprite noSp;

    [Space]
    public int currentMedkit;

    [Space]
    public Sprite threeKit;
    public Sprite twoKit;
    public Sprite oneKit;
    public Sprite noKit;


    [Space]
    public bool swordThrown;

    [Space]
    public Sprite thrown;
    public Sprite notThrown;


    [Space]
    // This is the slot that cycles sprites depending on the level of HP/SP
    public Image hpState;
    public Image spState;
    public Image swordState;
    public Image medkitState;


    #endregion



    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        playerScript = player.GetComponent<Player>();
        UpdateHpUI(); // Update HP and HP UI to make sure it works properly at first
        UpdateSpUI();
    }

    void Update() {
        UpdateHpUI();
        UpdateSpUI();
        UpdateSwordUI();
        UpdateMedkitUI();
    }



    #region - HP sprites swap -
    public void UpdateHpUI()
    {
        currentHp = playerStats.hp;
        // function to update hp value & then UI according to hp
        ; // update currentHP to playerController's HP value
        switch (currentHp)
        { // switch the image sprite from one case to another depending on hp levels (image.sprite = sprite)
            case 5:
                hpState.sprite = maxHp;
                return;

            case 4:
                hpState.sprite = fourHp;
                return;

            case 3:
                hpState.sprite = threeHp;
                return;

            case 2:
                hpState.sprite = twoHp;
                return;

            case 1:
                hpState.sprite = oneHp;
                return;

            case 0:
                hpState.sprite = noHp;
                return;

            default:
                hpState.sprite = noHp;
                return;
        }

    }
    #endregion

    #region - Sp sprites swap -
    public void UpdateSpUI()
    {
        currentSp = playerStats.sp;
        // function to update hp value & then UI according to hp
        // update currentHP to playerController's HP value
        switch (currentSp)
        { // switch the image sprite from one case to another depending on hp levels (image.sprite = sprite)
            case 5:
                spState.sprite = maxSp;
                return;

            case 4:
                spState.sprite = fourSp;
                return;

            case 3:
                spState.sprite = threeSp;
                return;

            case 2:
                spState.sprite = twoSp;
                return;

            case 1:
                spState.sprite = oneSp;
                return;

            case 0:
                spState.sprite = noSp;
                return;

            default:
                spState.sprite = noSp;
                return;
        }
    }
    #endregion

    #region - Sword State - 
    void UpdateSwordUI()
    {
        swordThrown = playerScript.throwFlag;

        switch (swordThrown)
        {
            case true:
                swordState.sprite = thrown;
                return;

            case false:
                swordState.sprite = notThrown;
                return;

            default:
                swordState.sprite = notThrown;
                return;
        }
    }
    #endregion

    #region - Medkit State -
    public void UpdateMedkitUI()
    {
        currentMedkit = playerStats.kit;
        // function to update hp value & then UI according to hp
        // update currentHP to playerController's HP value
        switch (currentMedkit)
        { 
            case 3:
                medkitState.sprite = threeKit;
                return;

            case 2:
                medkitState.sprite = twoKit;
                return;

            case 1:
                medkitState.sprite = oneKit;
                return;

            case 0:
                medkitState.sprite = noKit;
                return;

            default:
                medkitState.sprite = noKit;
                return;
        }
    }
    #endregion

}