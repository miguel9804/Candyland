using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    [SerializeField] private float dashSpeed;
    [SerializeField] private float startDashTime;
    

    private float dashTime;
    private float moveInput;
    private bool checkDash; // Detecta cuando oprime el boton y cuando lo suelta
    private int direction;
    private Rigidbody2D rb2d;
    private PlayerMovement mov; // Verifica la direccion del movimiento

    [Header("Efectos")]
    [SerializeField] GameObject dashEffect;

    AudioSource audioSource;
    [SerializeField] AudioClip dashSound;
    float randomPitch;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        mov = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = mov.dirDash;

        if (direction == 0)
        {
            if (checkDash || Input.GetKeyDown(KeyCode.Z)) 
            {
                if (moveInput < 0)
                {
                    direction = 1;
                    checkDash = false;
                }
                else if (moveInput > 0)
                {
                    direction = 2;
                    checkDash = false;
                }
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb2d.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;

                Instantiate(dashEffect, transform.position, transform.rotation);

                audioSource.clip = dashSound;

                randomPitch = Random.Range(0.7f, 1.7f);
                audioSource.pitch = randomPitch;
                audioSource.Play();

                if (direction == 1)
                {
                    rb2d.velocity = Vector2.left * dashSpeed;
                }
                else if (direction == 2)
                {
                    rb2d.velocity = Vector2.right * dashSpeed;
                }
            }
        }
    }

    public void ClickDownDash()
    {
        checkDash = true;
    }
    public void ClickUpDash()
    {
        checkDash = false;
    }
}
