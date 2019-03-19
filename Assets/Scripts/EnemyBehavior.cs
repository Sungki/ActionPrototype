using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    none,
    idle,
    walk,
    hurt,
    attack,
}

public class EnemyBehavior : MonoBehaviour {

    public string type;

    [Space]
    public EnemyState currentState;
    public Transform hitboxChild;
    public BoxCollider2D hitbox;


    [Space]
    public float speed;
    public float chasingRange;
    public float stoppingRange;
    public float retreatRange;

    [Space]
    public float attackStartup;
    public float attackDelay;
    public float activeFrames;
    public float attackRecovery;
    [Space]
    public float attackSpeed;
    public float attackDamping;

    [Space]
    public float shootDelay;

    [Space]
    public Transform target;
    [SerializeField]
    private Vector2 targetPosition;
    [SerializeField]
    private Vector2 lastTargetPosition;

    private Transform myPos;

    [Space]
    //public GameObject weapon;

    private Animator anim;
    private AnimatorStateInfo animState;
    [SerializeField]
    private float myAnimNormalTime = 0.0f;

    void Start() {

        hitboxChild = gameObject.transform.Find("EnemyHitbox");
        hitbox = hitboxChild.GetComponent<BoxCollider2D>();
 
        myPos = GetComponent<Transform>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();

        currentState = EnemyState.none;
    }

    void Update()
    {

        RunStates();
    }



    #region - State Functions - 
    void RunStates()
    {
        print(currentState);

        if (Vector3.Distance(myPos.position, target.transform.position) <= stoppingRange
        && Vector3.Distance(myPos.position, target.transform.position) > retreatRange
        && currentState == EnemyState.idle
        && currentState != EnemyState.walk
        && currentState != EnemyState.attack
        && currentState != EnemyState.hurt)
        {
            print("StartAttack");
            lastTargetPosition = target.transform.position;
            StartAttack(); }


        else if (Vector3.Distance(myPos.position, target.transform.position) > chasingRange
            && currentState != EnemyState.attack 
            && currentState != EnemyState.hurt
            && currentState != EnemyState.walk)
        {   StartWalk(); }



        else if (Vector3.Distance(myPos.position, target.transform.position) < retreatRange
            && currentState != EnemyState.attack
            && currentState != EnemyState.hurt
            && currentState != EnemyState.walk)
        { Retreat(); }

        else if(currentState == EnemyState.none)
        {
            StartIdle();
        }

        if (currentState == EnemyState.idle) Idle();

        if (currentState == EnemyState.walk) Walk();

        if (currentState == EnemyState.attack) Attack();

        animState = anim.GetCurrentAnimatorStateInfo(0);
        myAnimNormalTime = animState.normalizedTime;
    }

    void ResetStates()
    {
        StopIdle();
        StopHurt();
        StopWalk();
        StopAttack();
    }

    #endregion

    #region - Flip Sprite Function - 
    void FlipSprite()
    {
        if (target.position.x > transform.position.x && currentState != EnemyState.hurt)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        else
        {
            transform.localScale = new Vector2(1f, 1f);
        }
    }
    #endregion

    #region - Idle Functions -
    void StartIdle()
    {
        FlipSprite();
        ResetStates();
        currentState = EnemyState.idle;
        anim.ResetTrigger("isIdling");
    }

    void Idle()
    {
        //nothing yet
    }

    void StopIdle()
    {
        currentState = EnemyState.none;
        anim.ResetTrigger("isIdling");
    }
    #endregion

    #region - Walk Functions - 
    void StartWalk()
    {
        FlipSprite();
        ResetStates();

        currentState = EnemyState.walk;
        anim.SetTrigger("isWalking");
        Walk();
    }

    void Walk()
    {
        targetPosition = target.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(myPos.position, target.transform.position) <= stoppingRange)
        {
            StopWalk();
        }
    }

    void StopWalk()
    {
        currentState = EnemyState.idle;
        anim.ResetTrigger("isWalking");
    }
    #endregion

    #region - Attack & Shoot Functions - 
    void StartAttack()
    {
        FlipSprite();
        ResetStates();

        myAnimNormalTime = 0;
        anim.SetBool("isAttacking", true);
        currentState = EnemyState.attack;


        if (type == "Leaper") {
            print("Enemy Leaper");
            Attack();
        }

        if (type == "Archer")
        {
            ShootAttack();
        }
    }

    void Attack()
    {

        StartCoroutine(AttackCo());
    }

    public IEnumerator AttackCo()
    {
        yield return new WaitForSeconds(attackStartup);

        print("Activate hitbox");
        hitbox.enabled = true;

        print("Perform Leap");
        transform.position = Vector2.Lerp(transform.position, lastTargetPosition, attackDamping * attackSpeed);
        yield return new WaitForSeconds(activeFrames);

        print("Disable hitbox");
        hitbox.enabled = false;


        if (myAnimNormalTime >= 1 && animState.IsName("Attack"))
        {
            yield return new WaitForSeconds(attackRecovery);
            StopAttack();
        }

    }

    void ShootAttack()
    {
        //Instantiate(Arrow)
        if (myAnimNormalTime >= 1 && animState.IsName("Attack"))
        {
//            StopAttack();
        }
    }

    void StopAttack()
    {
        hitbox.enabled = false;
        currentState = EnemyState.idle;
        anim.SetBool("isAttacking", false);
        print("2: StopAttack");
        target.gameObject.GetComponent<PlayerStats>().isHurt = false;
    }

    #endregion

    #region - Hurt Function - 
    void StartHurt()
    {
        FlipSprite();
        ResetStates();
        currentState = EnemyState.hurt;
        myAnimNormalTime = 0;
        anim.SetBool("isHurt", true);
    }

    void Hurt()
    {
        if (myAnimNormalTime >= 1 && animState.IsName("Hurt"))
        {
            StopHurt();
        }
    }

    void StopHurt()
    {
        currentState = EnemyState.idle;
        anim.SetBool("isHurt", false);
    }
    #endregion



    void Retreat()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", true);
        currentState = EnemyState.walk;
        targetPosition = target.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, -speed * Time.deltaTime);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "AttackHitbox")
        {
            Hurt();
        }
    }




}
