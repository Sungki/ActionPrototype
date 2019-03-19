using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehavior : MonoBehaviour
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
    public GameObject arrowPrefab;
    public float speed;
    public bool enemyHit;

    Animator ani;
    bool _isNewState = false;
    private AnimatorStateInfo myAnimatorStateInfo;
    private float myAnimatorNormalizedTime = 0.0f;

    public float ShootDelayTime;
    float startTime = 0f;
    float currentTime = 0f;

    private void Start()
    {
        enemyHit = false;
        ani = GetComponent<Animator>();
        SetState(EnemyState.Idle);
        StartCoroutine(FSMMain());
        startTime = Time.time;
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
            currentTime = Time.time;

            if (Vector3.Distance(transform.position, player.transform.position) <= startChasing && e == EnemyState.Idle && (currentTime - startTime) > ShootDelayTime)
            {
                startTime = Time.time;
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
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
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

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, player.transform.position) >= stopChasing)
            {
                SetState(EnemyState.Idle);
            }
            if (Vector3.Distance(transform.position, player.transform.position) <= startAttack)
            {
                AudioManager.audioManager.EnemyShoot();
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

        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Vector2 dir = (Vector2)player.transform.position - new Vector2(arrow.transform.position.x, arrow.transform.position.y);
        arrow.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 950f);

        float angle = Mathf.Atan2(arrow.transform.position.y - player.transform.position.y, arrow.transform.position.x - player.transform.position.x) * 180f / Mathf.PI;
        arrow.transform.Rotate(0, 0, angle);

        do
        {
            yield return null;
            if (_isNewState) break;
            // Action
            if (myAnimatorNormalizedTime >= 1 && myAnimatorStateInfo.IsName("Attack"))
            {
                SetState(EnemyState.Idle);
            }
        } while (!_isNewState);

        //End
        ani.SetBool("attacking", false);
        if(player != null)
            player.GetComponent<PlayerStats>().isHurt = false;

//        yield return new WaitForSeconds(0.5f);
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
