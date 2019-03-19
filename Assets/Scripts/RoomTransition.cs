using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public Vector2 cameraChange;
    public Vector3 playerPosChange;
    private followPosition cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<followPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            cam.minPosition += cameraChange;
            cam.maxPosition += cameraChange;

            other.transform.position += playerPosChange;
        }
    }

}
