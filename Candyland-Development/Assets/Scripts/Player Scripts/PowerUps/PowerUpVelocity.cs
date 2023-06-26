using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpVelocity : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject player = collision.gameObject;
            PlayerMovement playerMove = player.GetComponent<PlayerMovement>();

            if (playerMove)
            {
                playerMove.GainVelocity();
                Destroy(gameObject);
            }
        }
    }
}
