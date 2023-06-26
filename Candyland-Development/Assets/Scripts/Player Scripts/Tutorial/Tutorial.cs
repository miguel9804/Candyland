using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header ("Botones")]
    [SerializeField] private Button[] move;
    [SerializeField] private Button textB;



    [Header("Textos")]
    [SerializeField] private GameObject[] text;
    [SerializeField] private GameObject hello;
    [SerializeField] private GameObject muerte;
    [SerializeField] private GameObject candies;
    [SerializeField] private GameObject time;
    [SerializeField] private GameObject panel;

    [Header("Otros")]
    [SerializeField] private bool cambiar;
    [SerializeField] private int count;

    private PlayerMovement player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cambiar)
        {
            cambiar = false;
            count++;
        }
        if (count == 1)
        {
            hello.SetActive(false);
            text[0].SetActive(true);
        }
        if (count == 2)
        {
            text[1].SetActive(true);
            text[0].SetActive(false);
            time.SetActive(true);
        }
        if (count == 3)
        {
            text[2].SetActive(true);
            text[1].SetActive(false);
            candies.SetActive(true);
        }
        if (count == 4)
        {
            text[3].SetActive(true);
            text[2].SetActive(false);
        }
        if (count >= 5)
        {
            panel.SetActive(true);
            muerte.SetActive(true);
            for(int i=0; i<move.Length;i++)
            {
                move[i].gameObject.SetActive(true);
            }
            textB.gameObject.SetActive(false);
        }

    }

    public void CambiarText()
    {
        cambiar = true;
    }
}
