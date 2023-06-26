using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class WinController : MonoBehaviour
{
    //El Script se Coloca en la "Puerta" del siguiente Nivel
    public static bool winLevel;
    bool allCandiesCollected;
    float playerWinTime;
    public int starsCount;

    [SerializeField] float limitTime;
    [SerializeField] TextMeshProUGUI timerText;
    float minutes, seconds;

    public static int[] obtainedStars = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    void Start()
    {
        allCandiesCollected = false;
        winLevel = false;
        minutes = limitTime / 60;
        seconds = (limitTime % 60);
        starsCount = 0;

        timerText.text = minutes.ToString() + ":" + seconds.ToString();
    }

    void Update()
    {
        //Incrementa el tiempo desde que empieza un nivel
        if (!winLevel)
        {
            playerWinTime += Time.deltaTime;

            if (playerWinTime <= limitTime)
            {
                minutes = (limitTime - playerWinTime - 30) / 60;
                seconds = ((limitTime - playerWinTime) % 60);

                if (seconds >= 10)
                    timerText.text = minutes.ToString("F0") + ":" + seconds.ToString("F0");
                else if (seconds <= 9)
                    timerText.text = minutes.ToString("F0") + ":0" + seconds.ToString("F0");
            }

            else
            {
                timerText.text = "0:00";
                timerText.color = Color.red;
            }
        }
    }

    public void AddStar()
    {
        //Añade una estrella cuando el jugador toma todos los Dulces.
        //Este metodo es llamado en el Script "PickUpController"
        if (!allCandiesCollected)
        {
            starsCount++;
            allCandiesCollected = true;
        }       
    }

    public void AddStar(bool levelComplete)
    {
        //Añade una o dos estrellas adicionales cuando se completa el nivel.
        //Este metodo es llamado en el Script "LevelDoor"
        if(playerWinTime <= limitTime && levelComplete && !winLevel)
        {
            starsCount = starsCount + 2;
            winLevel = true;
        }
        else if (levelComplete && !winLevel)
        {
            starsCount = starsCount + 1;
            winLevel = true;
        }

        Pass();
    }

    public void Pass()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel >= ES3.Load<int>("levelsUnlocked", 1))
        {
            ES3.Save<int>("levelsUnlocked", currentLevel);
        }

        //Revisa que la cantidad de estrellas obtenidas no sea menor a las que ya obtuvo en un momento anterior
        if (obtainedStars[currentLevel - 2] <= starsCount)
        {
            obtainedStars[currentLevel - 2] = starsCount;
        }

        ES3.Save<int[]>("obtainedStars", obtainedStars);



    }
}
