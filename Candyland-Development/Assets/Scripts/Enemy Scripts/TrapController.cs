using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    //AudioSource audioHit;

    bool itsStun;
    float stunTime = 3, actualTime;

    void Start()
    {
        //audioHit = GetComponent<AudioSource>();
        itsStun = false;
    }

    //Detección de Colision con El JUgador
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !itsStun)
        {
            collision.GetComponent<PlayerDie>().Die();
            //audioHit.Play();
        }
    }

    public void BeStun()
    {
        itsStun = true;
    }

    void Update()
    {
        if (itsStun)
        {
            actualTime += Time.deltaTime;

            if (stunTime < actualTime)
            {
                actualTime = 0;
                itsStun = false;
            }
        }
    }
}
