using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoomTransition : MonoBehaviour
{
    //CameraControlller camCont;

    private void Start()
    {
        //camCont = GetComponent<CameraControlller>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraControlller.instance.SetPosition
                (new Vector2(transform.position.x, transform.position.y));
        }
        //else
        //{
        //    Debug.Log(other.name);
        //}
    }
}
