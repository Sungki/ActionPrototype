using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaperBehavior : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Run,
        Attack,
        Hurt
    }

    public EnemyState e;
    public float startChasing;
    public float stopChasing;
    public float startAttack;
    public GameObject player;
    public float speed;
    public bool enemyHit;

    Animator ani;
    bool _isNewState = false;
    private AnimatorStateInfo myAnimatorStateInfo;
    private float myAnimatorNormalizedTime = 0.0f;

    private void Start()
    {
        enemyHit = false;
        ani = GetComponent<Animator>();
        SetState(EnemyState.Idle);
        StartCoroutine(FSMMain());
    }

    void SetState(EnemyState newState)
    {
        _isNewState = true;
        e = newState;
    }

    private void Update()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= startChasing && e == EnemyState.Idle)
            {
                SetState(EnemyState.Run);
            }
        }

        if(enemyHit && e != EnemyState.Hurt)
        {
            SetState(EnemyState.Hurt);
        }

        myAnimatorStateInfo = ani.GetCurrentAnimatorStateInfo(0);
        myAnimatorNormalizedTime = myAnimatorStateInfo.normalizedTime;
    }

    IEnumerator FSMMain()
    {
        while(true)
        {
            _isNewState = false;
            yield return StartCoroutine(e.ToString());
        }
    }

    void FlipSprite()
    {
        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        else
        {
            transform.localScale = new Vector2(1f, 1f);
        }
    }


    IEnumerator Idle()
    {
        // Starta
        do
        {
            yield return null;
            if (_isNewState) break;
            // Action
        } while (!_isNewState);

        //End
    }

    IEnumerator Run()
    {
        // Start
        ani.SetBool("moving", true);

        do
        {
            yield return null;
            if (_isNewState) break;

            // Action
            FlipSprite();
            AudioManager.audioManager.EnemyGrowl();
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, player.transform.position) >= stopChasing)
            {
                SetState(EnemyState.Idle);
            }
            if (Vector3.Distance(transform.position, player.transform.position) <= startAttack)
            {
                SetState(EnemyState.Attack);

            }
        } while (!_isNewState);

        //End
        ani.SetBool("moving", false);
    }

    IEnumerator Attack()
    {
        // Start
        ani.SetBool("attacking", true);
        myAnimatorNormalizedTime = 0;
        Vector2 lastTargetPosition = player.transform.position;
        AudioManager.audioManager.EnemyAttack();

        do
       {

            yield return null;
            if (_isNewState) break;
            // Action
            transform.position = Vector2.MoveTowards(transform.position, lastTargetPosition, speed *2f* Time.deltaTime);
            if (myAnimatorNormalizedTime >= 1 && myAnimatorStateInfo.IsName("Attack"))
            {
                SetState(EnemyState.Idle);
            }
        } while (!_isNewState);

        //End
        ani.SetBool("attacking", false);
        if(player != null)
            player.GetComponent<PlayerStats>().isHurt = false;
    }

    IEnumerator Hurt()
    {
        // Start
        ani.SetBool("isHurt", true);
        myAnimatorNormalizedTime = 0;

        do
        {
            yield return null;
            if (_isNewState) break;
            // Action
            if (myAnimatorNormalizedTime >= 1 && myAnimatorStateInfo.IsName("Hurt"))
            {
                SetState(EnemyState.Idle);
            }
        } while (!_isNewState);

        //End
        ani.SetBool("isHurt", false);
        enemyHit = false;
    }
}
