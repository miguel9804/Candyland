using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject player = collision.gameObject;
            PlayerDie playerDie = player.GetComponent<PlayerDie>();

            if (playerDie)
            {
                playerDie.GetArmor();
                Destroy(gameObject);
            }
        }
    }
}
