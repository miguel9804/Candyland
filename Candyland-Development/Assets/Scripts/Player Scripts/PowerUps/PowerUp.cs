using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
   
    enum EPowerUp
    {
        Velocity,
        Armor,
        Net
    }

    [SerializeField] EPowerUp currentPowerUp;

    [SerializeField] SpriteRenderer gVelocity;
    [SerializeField] SpriteRenderer gArmor;
    [SerializeField] SpriteRenderer gNet;

    [SerializeField] float changeTime;
    [SerializeField] float currentTime;

    GameObject player;
    PlayerMovement playerMove;
    PlayerDie playerDie;
    WeaponNet weaponNet;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= changeTime)
        {
            if (currentPowerUp == EPowerUp.Armor)
            {
                currentPowerUp = EPowerUp.Net;
                currentTime = 0;
            }
            else if (currentPowerUp == EPowerUp.Net)
            {
                currentPowerUp = EPowerUp.Velocity;
                currentTime = 0;
            }
            else if (currentPowerUp == EPowerUp.Velocity)
            {
                currentPowerUp = EPowerUp.Armor;
                currentTime = 0;
            }

            //currentPowerUp = (EPowerUp)Random.Range(0, 3);
        }



        switch (currentPowerUp)
        {
            case EPowerUp.Velocity:
                Clean();
                gVelocity.enabled = true;
                if (playerMove)
                {
                    playerMove.GainVelocity();
                    Destroy(gameObject);
                }
                break;
            case EPowerUp.Armor:
                Clean();
                gArmor.enabled = true;
                if (playerDie)
                {
                    playerDie.GetArmor();
                    Destroy(gameObject);
                }
                break;
            case EPowerUp.Net:
                Clean();
                gNet.enabled = true;
                if (weaponNet)
                {
                    weaponNet.AddNets();
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject;
            playerMove = player.GetComponent<PlayerMovement>();
            playerDie = player.GetComponent<PlayerDie>();
            weaponNet = player.GetComponent<WeaponNet>();
        }
    }

    public void Clean()
    {
        gVelocity.enabled = false;
        gArmor.enabled = false;
        gNet.enabled = false;
    }
}
