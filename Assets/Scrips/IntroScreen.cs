using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour {

    void Start()
    {
        Invoke("GoPlay", 5);
    }

    public void GoPlay()
    {
        SceneManager.LoadScene(2);
    }
}