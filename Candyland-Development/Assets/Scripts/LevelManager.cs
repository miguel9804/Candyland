using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int levelsUnlocked;
    int[] obtainedStars;
    int[] defaultStars = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    [SerializeField] Button[] buttons;

    void Start()
    {

        levelsUnlocked = ES3.Load("levelsUnlocked", 1);
        obtainedStars = ES3.Load("obtainedStars", defaultStars);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false; //Desactiva todos los botones

            //Activa el sprite del candado y desactiva el del numero
            buttons[i].transform.GetChild(0).gameObject.SetActive(false);
            buttons[i].transform.GetChild(1).gameObject.SetActive(true); 
        }

        for (int i = 0; i < levelsUnlocked; i++)
        {
            //Sistema de casos para elegir la cantidad de estrellas a mostrar en cada boton.
            switch (obtainedStars[i])
            {
                case 1: //Una sola estrela
                    buttons[i].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                    break;

                case 2: //Dos estrellas
                    buttons[i].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                    buttons[i].transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(true);
                    break;

                case 3: //Tres estrellas
                    buttons[i].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                    buttons[i].transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(true);
                    buttons[i].transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(true);
                    break;

                default: //Cero estrellas
                    buttons[i].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false);
                    buttons[i].transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(false);
                    buttons[i].transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(false);
                    break;
            }

            buttons[i].interactable = true; //Desbloquea el boton

            //Desactiva el sprite del candado y activa el del numero
            buttons[i].transform.GetChild(0).gameObject.SetActive(true);
            buttons[i].transform.GetChild(1).gameObject.SetActive(false);
        }

    }

    void Update()
    {
        //Sirve para bloquear todos los niveles hasta el uno (Testing)
        if (Input.GetKeyDown(KeyCode.E))
        {
            ES3.Save<int>("levelsUnlocked", 1);
            ES3.Save("obtainedStars", defaultStars);
        }
    }

    //Metodo para cargar las escenas con los botones
    public void LoadLevel(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void Restart()
    {
        //1- Reinicia la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
