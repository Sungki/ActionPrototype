using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    walk,
    attack,
    interact
}

public class Player : MonoBehaviour
{
    public Image throwPoint;

    public GameObject swordPrefab;
    GameObject sword;

    public float aimingDistance;
    public float throwingDistance;
    public float maxRangeSword;

    public PlayerState currentState;
    public float speed;
    public float dashSpeed;
    Rigidbody2D rb;
    Vector3 movement;
    Animator ani;
    PlayerStats playerStats;

    public string Attack;
    public bool isDone;
    bool handSword = true;
    public bool isWalking = false;
    public float attackDelay;
    public float attackRecovery;

    Vector2 mousePos;
    Vector3 worldMousePos;
    Vector2 attackDirection;
    public float attackForwardStep;

    public bool throwFlag;
    SpriteRenderer m_SpriteRenderer;
    bool isWarping = false;

    #region Puzzle 1 Hint Player Movement Variables
    // Disable player movement when interacting with Puzzle Hint.
    public bool canMove;
    #endregion

    void Start()
    {
        canMove = true;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        throwFlag = false;
        throwPoint.enabled = false;
    }

    private void OnEnable()
    {
        currentState = PlayerState.walk;
        Attack = "First";
        isDone = true;
    }
    
    void Update()
    {
        #region Player Movement Disabler (Puzzle Hint)
        if (!canMove)
        {
            return;
        }
        #endregion

        isWalking = false;
        movement = Vector3.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        UpdateInput();
    }

    void UpdateInput()
    {
        if (Input.GetButtonDown("Attack") && currentState != PlayerState.attack)
        {
            isDone = false;
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk && isDone == true)
        {
            UpdateAnimationMove();
        }

        if (Input.GetButtonDown("Dash") && isDone == true)
        {
            isDone = false;
            StartCoroutine(DashCo());
        }

        if (Input.GetButtonDown("Heal") && playerStats.kit != 0 && isDone == true)
        {

            playerStats.Heal();
            isDone = false;
            StartCoroutine(HealCo());
        }

        if (Input.GetButtonDown("Warp") && throwFlag && isDone == true)
        {
            isDone = false;
            StartCoroutine(WarpCo());
            //            transform.position = sword.GetComponent<Sword>().target;
        }

        if (Input.GetButtonDown("Special")&&throwFlag && isDone == true)
        {
            AudioManager.audioManager.PlayPull();
            sword.GetComponent<Sword>().target = transform.position;
            sword.GetComponent<Sword>().Callingback = true;
        }

        if(throwFlag&& Vector3.Distance(transform.position, sword.transform.position) > maxRangeSword)
        {
            AudioManager.audioManager.PlayPull();
            sword.GetComponent<Sword>().target = transform.position;
            sword.GetComponent<Sword>().Callingback = true;
        }

        if (Input.GetButton("Aim"))
        {
            mousePos = Input.mousePosition;
            worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 direction = (Vector2)((worldMousePos - transform.position));
            direction.Normalize();

            Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            throwPoint.rectTransform.position = playerScreenPos + new Vector2(direction.x * aimingDistance, direction.y * aimingDistance);
            throwPoint.enabled = true;

            if (Input.GetButtonDown("Special") && playerStats.sp > 0)
            {
                if (!throwFlag)
                {
                    AudioManager.audioManager.PlayThrow();
                    playerStats.UseSP();
                    throwFlag = true;

                    sword = Instantiate(swordPrefab, transform.position + (Vector3)(direction * 2.5f), Quaternion.identity);
                    float angle = Mathf.Atan2(direction.y, direction.x) * 180f / Mathf.PI;
                    sword.transform.Rotate(0, 0, angle);

                    Vector3 wrapPos = transform.position + (Vector3)(direction * throwingDistance);
                    sword.GetComponent<Sword>().target = wrapPos;
                }
            }
        }

        if (Input.GetButtonUp("Aim"))
        {
            throwPoint.enabled = false;
        }

        if(isWarping)
        {
            if(m_SpriteRenderer.color == Color.yellow)
                m_SpriteRenderer.color = Color.blue;
            else
                m_SpriteRenderer.color = Color.yellow;
        }
    }

    private IEnumerator DashCo()
    {
        AudioManager.audioManager.PlayDash();
        ani.SetBool("dash", true);
//        transform.Translate(movement * dashSpeed * Time.deltaTime);
        rb.MovePosition(transform.position + movement * dashSpeed * Time.deltaTime);
        yield return new WaitForSeconds(0.2f);
        ani.SetBool("dash", false);
        isDone = true;
    }

    private IEnumerator HealCo()
    {
        AudioManager.audioManager.PlayHeal();
        ani.SetBool("moving", false);
        currentState = PlayerState.interact;
        isDone = false;
        ani.SetBool("heal", true);
        yield return new WaitForSeconds(0.5f);
        ani.SetBool("heal", false);
        isDone = true;
        currentState = PlayerState.walk;
    }

    private IEnumerator WarpCo()
    {
        isWarping = true;
        m_SpriteRenderer.color = Color.yellow;
        AudioManager.audioManager.PlayWarp();
        ani.SetBool("moving", false);
        currentState = PlayerState.interact;
        yield return new WaitForSeconds(0.5f);
        transform.position = sword.GetComponent<Sword>().target;
        yield return new WaitForSeconds(0.5f);
        isDone = true;
        currentState = PlayerState.walk;
        isWarping = false;
        m_SpriteRenderer.color = Color.white;
    }

    public void Hurt()
    {
        StartCoroutine(HurtCo());
    }

    private IEnumerator HurtCo()
    {
        ani.SetBool("hurt", true);
        yield return new WaitForSeconds(0.3f);
        ani.SetBool("hurt", false);
    }

    private IEnumerator AttackCo()
    {
        currentState = PlayerState.attack;

        if (!throwFlag)
        {
            if (Attack == "First")
            {
                AudioManager.audioManager.PlaySlash();
                ani.SetBool("attacking", true);
                transform.Translate(attackDirection * attackForwardStep);
//                transform.position = Vector2.Lerp(transform.position, attackDirection * 50f, 2f * Time.deltaTime);
                yield return null;
                ani.SetBool("attacking", false);
//                yield return new WaitForSeconds(0.3f);
                Attack = "Second";
                //yield return new WaitForSeconds(0.2f);
            }
            else if (Attack == "Second")
            {
                AudioManager.audioManager.PlaySlash();
                ani.SetBool("attacking2", true);
                transform.Translate(attackDirection * attackForwardStep);
//                transform.position = Vector2.Lerp(transform.position, attackDirection * 50f, 2f * Time.deltaTime);
                yield return null;
                ani.SetBool("attacking2", false);
//                yield return new WaitForSeconds(attackDelay);
                Attack = "Third";
//                yield return new WaitForSeconds(0.2f);
            }
            else if (Attack == "Third")
            {
                AudioManager.audioManager.PlaySlash();
                ani.SetBool("attacking3", true);
                transform.Translate(attackDirection * attackForwardStep);
                yield return null;
                ani.SetBool("attacking3", false);
                yield return new WaitForSeconds(attackDelay);
                Attack = "First";
            }

        }
        else
        {
            if (Attack == "First")
            {
                AudioManager.audioManager.PlaySlash();
                ani.SetBool("fistAttacking", true);
                transform.Translate(attackDirection * attackForwardStep);
//                transform.position = Vector2.Lerp(transform.position, attackDirection * 50f, 2f * Time.deltaTime);
                yield return null;
                ani.SetBool("fistAttacking", false);
                //yield return new WaitForSeconds(0.2f);
                Attack = "Second";

            }
            else
            {
                AudioManager.audioManager.PlaySlash();
                ani.SetBool("fistAttacking2", true);
                transform.Translate(attackDirection * attackForwardStep);
//                transform.position = Vector2.Lerp(transform.position, attackDirection * 50f, 2f * Time.deltaTime);
                yield return null;
                ani.SetBool("fistAttacking2", false);
                yield return new WaitForSeconds(attackDelay);
                Attack = "First";
            }
        }
        currentState = PlayerState.walk;
        yield return new WaitForSeconds(attackRecovery);
        isDone = true;
    }

    void UpdateAnimationMove()
    {
        if (movement != Vector3.zero)
        {

            MoveCharacter();

            ani.SetFloat("moveX", movement.x);
            ani.SetFloat("moveY", movement.y);
            ani.SetBool("moving", true);

            if (movement.x < 0 && isDone == true)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                attackDirection = Vector2.left;
            }

            if (movement.x > 0 && isDone == true)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                attackDirection = Vector2.right;
            }

            if(movement.y < 0)
                attackDirection = Vector2.down;

            if (movement.y > 0)
                attackDirection = Vector2.up;
        }
        else
        {
            ani.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
//        if (Attack != "First" && attackDirection != new Vector2(movement.x, movement.y))
//        {
//            Attack = "First";
//        }

//        if (isDone == true&&Attack=="First")
        if (isDone == true)
        {
            Attack = "First";
            movement.Normalize();
            rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
            isWalking = true;
        }

               /*if (Input.GetAxisRaw("Horizontal") != 0) {
                    transform.Translate((new Vector3(movement.x * speed  * Time.deltaTime, 0f, 0f))); 
                }

                if (Input.GetAxisRaw("Vertical") != 0) {
                    transform.Translate((new Vector3(0f, movement.y * speed  * Time.deltaTime, 0f)));
                }*/
    }
}
