using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPosition : MonoBehaviour
{
    public Transform target;
    public float damping;
    public Vector2 maxPosition;
    public Vector2 minPosition;

    void LateUpdate()
    {
        if (target != null)
        {
            if (transform.position != target.position)
            {
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

                //targetPosition.x = Mathf.Clamp(target.position.x, minPosition.x, maxPosition.x);
                //targetPosition.y = Mathf.Clamp(target.position.y, minPosition.y, maxPosition.y);

                transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
            }
        }
    }
}
