using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timerTime; //timerTime va en segundos!

    private int minutes, seconds, cents;
    
    void Update(){
        timerTime -= Time.deltaTime;

        if(timerTime < 0){
            timerTime = 0;
        }

        minutes = (int)(timerTime / 60f);
        seconds = (int)(timerTime - minutes * 60f);
        cents = (int)((timerTime - (int)timerTime) * 100f);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);

        if(timerTime <= 0f){
            //Aca va el GAME OVER (fade to black + sad music)
            SceneManager.LoadScene(3);
        }
    }
}