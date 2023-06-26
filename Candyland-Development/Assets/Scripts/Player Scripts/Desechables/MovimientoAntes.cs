using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoAntes : MonoBehaviour
{
    //Script Del Jugador
    Rigidbody2D rb;
    public Vector3 movement;

    [Header("Movimiento Básico")]
    [SerializeField] private float speed; //Velocidad del Jugador
    [SerializeField] private float extraSpeed; //Velocidad adicional que se le da al jugador cuando coge el PowerUpVelocity;
    [SerializeField] private float extraSpeedTime; //Tiempo que va durar el power up de velocidad.
    private bool facingRight, left, right; //Falso = Mirando a la Izquierda, Verdadero = Mirando a la Derecha. Flip del Sprite del Jugador. Left y Right sirven para verificar el movimiento de los lados
    public float dirDash; // direccion del dash
    private bool hasGainSpeed = false; //Variable para verificar que se activo el poder de velocidad.
    private float currentTime;

    [Header("Control del Salto")]
    [SerializeField] float jumpPower;
    [SerializeField] public bool canJump;
    bool checkJump; // CheckJump comprueba cuando le da click al boton de salto y ahi lo realiza si esta en verdadero

    [Header("Control de salto en pared")]
    [SerializeField] private LayerMask isGround; // Verificar el layer en el que se encuentra el personaje, con un raycast, asi se sabra si esta cerca de una pared
    [SerializeField] private float jumpWPower, tempSpeed, senseRay, tempTime; // jumpWPower = Poder de Salto en la pared. tempSpeed = es un guardado para la variable speed
    private Vector2 dirRay;
    private bool jumpW;

    [Header("Control de Velocidad de Caida")]
    [SerializeField] float fallMultiplier; //Aumenta La Velocidad de Caida
    [SerializeField] float lowJumpMultiplier;




    public Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tempSpeed = speed;

    }

    void Update()
    {

        Debug.DrawRay(this.transform.position + new Vector3(0.2f * senseRay, -0.1f, 0f), dirRay * 0.2f, Color.cyan);
        if (!PauseControl.gameIsPaused)
        {

            if (Input.GetKeyDown(KeyCode.D))
            {
                ClickDownR();
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                ClickUpR();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                ClickDownL();
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                ClickUpL();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickDownJump();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ClickUpJump();
            }

            if (right || left)
            {
                Move(); // Ya no recibe input PD: Hecho por Miguel
            }
            if (checkJump && IsWall() && !canJump)
            {
                jumpW = true;
                tempTime += 1f * Time.deltaTime;
                if (tempTime > 0.2f)
                {
                    checkJump = false;

                }
            }
            else if (!checkJump && IsWall() && !canJump)
            {
                jumpW = false;
            }
            if (!IsWall())
            {
                jumpW = false;
            }

        }
        else
        {

        }

        //Cuando transcurre el tiempo del power up vuelve la velocidad a la normalidad
        if (hasGainSpeed)
        {
            currentTime += Time.deltaTime;

            if (currentTime > extraSpeedTime)
            {
                speed -= extraSpeed;
                hasGainSpeed = false;
                currentTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {

        if (!PauseControl.gameIsPaused)
        {
            if (checkJump)
            {
                if (canJump) //Verifica que pueda saltar
                {
                    Jump(Vector2.up);
                }
            }

            if (jumpW)
            {
                JumpWall(Vector2.up);
                Physics2D.gravity = new Vector2(0f, -9.8f);
            }
            if (IsWall() && !canJump)
            {
                if (rb.velocity.y < 0f)
                {
                    Physics2D.gravity = new Vector2(0f, 0f);
                    rb.velocity = new Vector2(0f, 0f);
                    speed = 0f;
                }
            }
            if (Physics2D.gravity.y == 0f && !IsWall())
            {
                speed = tempSpeed;
                Physics2D.gravity = new Vector2(0f, -20f);
                transform.Translate(Vector3.down * 10f * Time.deltaTime);
            }
        }



        //CAIDA DEL PERSONAJE
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !checkJump) //Salto Corto
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //VERFICAR SI EL JUGADOR TOCA EL SUELO O UNA PLATAFORMA
        if (collider.CompareTag("Floor"))
        {
            canJump = true;
            checkJump = false;
            Physics2D.gravity = new Vector2(0f, -9.8f);
            animator.SetBool("IsJumping", false);
        }
        if (collider.CompareTag("PlatformMob"))
        {
            transform.parent = collider.transform;
            checkJump = false;
            Physics2D.gravity = new Vector2(0f, -9.8f);
            canJump = true;
            animator.SetBool("IsJumping", false);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        //VERFICAR SI EL JUGADOR DEJA DE TOCAR EL SUELO O UNA PLATAFORMA
        if (collider.CompareTag("Floor"))
        {
            canJump = false;
            animator.SetBool("IsJumping", true);
        }
        if (collider.CompareTag("PlatformMob"))
        {
            transform.parent = null;
            canJump = false;
            animator.SetBool("IsJumping", true);
        }
    }

    //Controles de las flechas PD: Hecho por miguel
    public void ClickDownR() //Cuando se presiona la flecha de Right
    {
        right = true;
        dirRay = Vector2.right;
        senseRay = 1f;
        dirDash = 1f;
    }

    public void ClickUpR() //Cuando se suelta la flecha de Right
    {
        movement.x = 0;
        animator.SetFloat("Speed", 0);
        dirDash = 0f;
        right = false;
    }

    public void ClickDownL() //Cuando se presiona la flecha de Left
    {
        left = true;
        dirRay = Vector2.left;
        senseRay = -1f;
        dirDash = -1f;
    }

    public void ClickUpL() //Cuando se suelta la flecha de Left
    {
        movement.x = 0;
        dirDash = 0f;
        animator.SetFloat("Speed", 0);
        left = false;
    }

    public void Move()
    {
        //CONTROLES DE MOVIMIENTO BASADOS EN TRANSFORM
        if (right)
        {
            movement = new Vector3(1, 0f, 0f);

            transform.position += movement * Time.deltaTime * speed;

            animator.SetFloat("Speed", 1);
        }
        if (left)
        {
            movement = new Vector3(-1, 0f, 0f);

            transform.position += movement * Time.deltaTime * speed;

            animator.SetFloat("Speed", 1);
        }

        if (movement.x < 0 && !facingRight)
        {
            Flip();

        }
        else if (movement.x > 0 && facingRight)
        {
            Flip();
        }

    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    //Control para el salto segun el boton 
    public void ClickDownJump()
    {
        checkJump = true;
    }
    public void ClickUpJump()
    {
        checkJump = false;
        tempTime = 0f;
    }

    public void Jump(Vector2 dir)
    {
        //SALTO UTILIZANDO EL RIGIDBODY
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpPower;
    }

    private void JumpWall(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpWPower;
    }

    private bool IsWall()
    {
        if (Physics2D.Raycast(this.transform.position + new Vector3(0.2f * senseRay, -0.1f, 0f), dirRay, 0.2f, isGround))
        {
            Debug.Log("Pared");
            return true;
        }
        else
        {
            //Debug.Log("No Pared");
            return false;
        }
    }

    public void GainVelocity()
    {
        if (!hasGainSpeed)
        {
            speed += extraSpeed;
            hasGainSpeed = true;
        }
    }
}
