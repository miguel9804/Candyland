using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDoor : MonoBehaviour
{
    [Header("Game Boards")]
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject HUD;

    [Header("Victory Stars")]
    [SerializeField] GameObject[] stars;

    //Controladores
    WinController winController;
    Animator[] anim;

    void Awake()
    {
        winController = GetComponent<WinController>();
        anim = new Animator[3];

        anim[0] = stars[0].GetComponent<Animator>();
        anim[1] = stars[1].GetComponent<Animator>();
        anim[2] = stars[2].GetComponent<Animator>();

        victoryScreen.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            winController.AddStar(true);
            victoryScreen.gameObject.SetActive(true);
            HUD.gameObject.SetActive(false);

            for (int i = 0; i < winController.starsCount; i++)
            {
                if (i >= 0)
                    anim[i].SetTrigger("NewStar");
            }
        }
    }
}
