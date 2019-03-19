using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager cameraManager;

    public Animator camAnim;

    public void CamShake()
    {
        camAnim.SetTrigger("Shake");
    }

}
