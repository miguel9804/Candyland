using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public static bool gameIsPaused;//No importa la instancia
    public GameObject PauseMenu;//El menú

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;//Altero la variable de pausa para verificar luego e impedir movimiento y otras cosas. Siempre a lo que NO está.

        if (gameIsPaused)
        {
            Time.timeScale = 0f;//Pauso el juego si el juego está pausado
            AudioListener.pause = true;//Muteado el juego
            //Para ignorar algun audio source: AudioSource.ignoreListenerPause=true;
            if (PauseMenu != null)
            {
                bool isActive = PauseMenu.activeSelf;
                PauseMenu.SetActive(!isActive);
            }
        }
        else
        {
            Time.timeScale = 1;//si no, lo resumo.
            AudioListener.pause = false;//Desmuteado el juego
            if (PauseMenu != null)
            {
                bool isActive = PauseMenu.activeSelf;
                PauseMenu.SetActive(!isActive);
                Debug.Log("Esta linea está después de desactivar el menú (se supone)");
            }
            
        }
    }
}
