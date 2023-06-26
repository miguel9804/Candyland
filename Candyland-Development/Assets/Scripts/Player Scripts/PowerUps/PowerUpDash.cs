using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject player = collision.gameObject;
            PlayerDash playerDash = player.GetComponent<PlayerDash>();

            if (playerDash)
            {
                playerDash.enabled = true;
                Destroy(gameObject);
            }
        }
    }


}
