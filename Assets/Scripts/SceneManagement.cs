using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour {
    public float delay;


    public void StartGame() {
        StartCoroutine(StartCo());
    }

    public IEnumerator StartCo()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Level 1");
    }


    public void HowToPlay() {
        StartCoroutine(How2PlayCo());
    }

    public IEnumerator How2PlayCo()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("How2play");
    }



    public void Exit2Main() {
        StartCoroutine(Exit2MainCo());
    }

    public IEnumerator Exit2MainCo()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Main");
    }



    public void QuitGame() {
        StartCoroutine(QuitCo());
    }

    public IEnumerator QuitCo()
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }

}
