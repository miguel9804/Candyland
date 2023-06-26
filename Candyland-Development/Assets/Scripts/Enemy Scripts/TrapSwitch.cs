using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSwitch : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] GameObject trap; //GameObject principal de la Trampa.
    [SerializeField] Transform hidePosition; //Posición a la que se moverá la trampa al ser desactivada, sea oculta o fuera del camino del jugador.

    [Header("Velocidad")]
    [SerializeField] float hideSpeed;
    Transform trapPosition;

    bool active, moveTrap;

    void Awake()
    {
        if (trap != null)
        {
            trapPosition = trap.transform;
            active = false;
            moveTrap = false;
        }
    }

    void FixedUpdate()
    {
        if (moveTrap)
        {
            float fixedSpeed = hideSpeed * Time.deltaTime;
            trap.transform.position = Vector3.MoveTowards(trapPosition.position, hidePosition.position, fixedSpeed);
        }

        if (trap.transform.position == hidePosition.position)
            moveTrap = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !active)
        {
            active = true;
            moveTrap = true;
        }
    }
}
