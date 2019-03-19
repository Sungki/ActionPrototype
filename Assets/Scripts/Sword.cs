using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Vector2 target;
    public float speed;
    public bool Callingback = false;
    GameObject player;
    Animator ani;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        if(Callingback)
        {
            ani.SetBool("isPull", true);
            transform.position = Vector2.Lerp(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, target, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Callingback)
        {
            target = transform.position;
            AudioManager.audioManager.PlayPlant();
        }

        if(collision.gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().EnemyHit();
            collision.gameObject.GetComponent<LeaperBehavior>().enemyHit = true;
        }

        if (collision.gameObject.CompareTag("Archer"))
        {
            collision.gameObject.GetComponent<Enemy>().EnemyHit();
            collision.gameObject.GetComponent<ArcherBehavior>().enemyHit = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().throwFlag = false;
            Callingback = false;
            ani.SetBool("isPull", false);
            Destroy(this.gameObject);
        }
    }
}
