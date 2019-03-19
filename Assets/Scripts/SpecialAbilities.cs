using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbilities : MonoBehaviour {
    public GameObject sword;
    public GameObject parent;
    public Vector2 swordPos;
    public Vector2 actualPos;

    [Space]
    public float callSpeed;
    public float warpSpeed;

    public bool calling;
    public bool warping;

    // Start is called before the first frame update
    void Start() {
        parent = GameObject.FindGameObjectWithTag("Player");
        sword = GameObject.FindGameObjectWithTag("Sword");
        calling = false;
    }

    // Update is called once per frame
    void Update()
    {
        swordPos = sword.transform.position;
        actualPos = parent.transform.position;

        if (Input.GetButtonDown("Special")) {
            calling = true;
        }
        if (calling == true) {
            Callback();
        }



        if (Input.GetButtonDown("Warp")) {
            warping = true;
        }
        if (warping == true) {
            Warp();
        }

    }

    void Callback() {
        sword.transform.position = Vector2.Lerp(swordPos, actualPos, callSpeed * Time.deltaTime);
    }

    void Warp() {
        parent.transform.position = Vector2.Lerp(actualPos, swordPos, warpSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Sword") {
            calling = false;
            warping = false;
            sword.SetActive(false);

        }
    }
}
