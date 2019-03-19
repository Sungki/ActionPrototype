using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("enemy"))
        {
            AudioManager.audioManager.PlayHitCollision();
            collision.GetComponent<Enemy>().EnemyHit();
            collision.GetComponent<LeaperBehavior>().enemyHit = true;
        }
        if (collision.CompareTag("Archer"))
        {
            AudioManager.audioManager.PlayHitCollision();
            collision.GetComponent<Enemy>().EnemyHit();
            collision.GetComponent<ArcherBehavior>().enemyHit = true;
        }

    }
}
