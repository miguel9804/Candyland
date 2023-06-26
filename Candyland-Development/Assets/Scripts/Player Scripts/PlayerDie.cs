using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDie : MonoBehaviour
{
    [Header("Muerte y Efectos")]
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject blood;
    [SerializeField] SpriteRenderer shield;

    Animator playerAnimator;
    public bool canDie = true;

    [Header("Sonido de Muerte y perdida de escudo")]
    AudioSource audioSource;
    [SerializeField] AudioClip dieSound, shieldLossSound;

    [Header("Contador de Muertes")]
    [SerializeField] TextMeshProUGUI deathCountText;
    Animator deathCountAnimator;
    int deathCount;

    void Start()
    {
        //playerAnimator = player.GetComponent<Animator>();
        spawnPosition.position = new Vector3(spawnPosition.position.x, spawnPosition.position.y, transform.position.z);
        audioSource = GetComponent<AudioSource>();
        deathCountAnimator = deathCountText.GetComponent<Animator>();

        deathCount = 0;
        deathCountText.text = deathCount.ToString();
    }

    public void Die()
    {
        if (canDie)
        {
            //Activar Animacion de Muerte
            Instantiate(blood, transform.position, Quaternion.identity);

            audioSource.clip = dieSound;
            audioSource.Play();

            AddDeath();
        }
        else
        {
            audioSource.clip = shieldLossSound;
            audioSource.Play();
            canDie = true;
            shield.enabled = false;
        }
    }

    public void Respawn()
    {
        transform.position = spawnPosition.position;
        deathCountAnimator.SetTrigger("AddDeathCount");
    }

    public void GetArmor()
    {
        shield.enabled = true;
        canDie = false;
    }

    void AddDeath()
    {
        deathCount ++;
        deathCountAnimator.SetTrigger("AddDeathCount");
        deathCountText.text = deathCount.ToString();

        Respawn();
    }
}
