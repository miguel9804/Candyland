using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Script Del Jugador
    Rigidbody2D rb;
    public Vector3 movement;

    [Header("Movimiento Básico")]
    [SerializeField] public float speed; //Velocidad del Jugador
    [SerializeField] private float extraSpeed; //Velocidad adicional que se le da al jugador cuando coge el PowerUpVelocity;
    [SerializeField] private float extraSpeedTime; //Tiempo que va durar el power up de velocidad.
    [SerializeField] private bool canMove;
    private bool facingRight, left, right; //Falso = Mirando a la Izquierda, Verdadero = Mirando a la Derecha. Flip del Sprite del Jugador. Left y Right sirven para verificar el movimiento de los lados
    public float dirDash; // direccion del dash
    private bool hasGainSpeed = false; //Variable para verificar que se activo el poder de velocidad.
    private float currentTime;
    private float x = 0f;
    private Vector2 dir;

    [Header("Control del Salto")]
    [SerializeField] float jumpPower;
    [SerializeField] public bool canJump;
    bool checkJump; // CheckJump comprueba cuando le da click al boton de salto y ahi lo realiza si esta en verdadero

    [Header("Control de salto en pared")]
    [SerializeField] private LayerMask isGround; // Verificar el layer en el que se encuentra el personaje, con un raycast, asi se sabra si esta cerca de una pared
    [SerializeField] private float senseRay, wallJumpLerp, slideSpeed; // jumpWPower = Poder de Salto en la pared. tempSpeed = es un guardado para la variable speed
    private Vector2 dirRay;
   [SerializeField] private bool jumpW, wallGrab;

    [Header("Control de Velocidad de Caida")]
    [SerializeField] float fallMultiplier; //Aumenta La Velocidad de Caida
    [SerializeField] float lowJumpMultiplier;

    [Header("Particulas")]
    [SerializeField] private ParticleSystem jumpParticle;
    [SerializeField] private ParticleSystem slideParticle;
    [SerializeField] private ParticleSystem jumpWallParticle;

    [Header("Sonidos")]
    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private AudioClip soundJump;
    [SerializeField] private AudioClip soundLand;





    private Animator anim;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        soundPlayer = GetComponent<AudioSource>();

    }

    void Update()
    {
        
        //Debug.DrawRay(this.transform.position + new Vector3(0.2f * senseRay, -0.1f, 0f), dirRay * 0.2f, Color.cyan);
        if (!PauseControl.gameIsPaused && !WinController.winLevel)
        {
            anim.SetFloat("Speed", rb.velocity.x);
            anim.SetFloat("VelocityJump", rb.velocity.y);
            x += movement.x * Time.deltaTime*3f;
            WallParticle();
            //Debug.Log(stop);
            if(Input.GetKeyDown(KeyCode.D))
            {
                ClickDownR();
            }
            if(Input.GetKeyUp(KeyCode.D))
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
            if(Input.GetKeyDown(KeyCode.Space))
            {
                ClickDownJump();
            }
            if(Input.GetKeyUp(KeyCode.Space))
            {
                ClickUpJump();
            }
            if (right)
            {
                float limit = Mathf.Clamp(x, 0.1f, 1f);
                dir = new Vector2(limit, 0f);
                Move(dir); // Ya no recibe input PD: Hecho por Miguel
                //Debug.Log(dir);
                anim.SetFloat("Speed", 1);
            }
            else if (left)
            {
                float limit = Mathf.Clamp(x, -1f, -0.1f);
                dir = new Vector2(limit, 0f);
                Move(dir);
                //Debug.Log(dir);
                anim.SetFloat("Speed", 1);
            }


            if (IsWall() && !canJump)
            {
                
                if(x!=0)
                {
                    anim.SetBool("WallGrab", true);
                   
                    WallSlide();
                }
                if(checkJump)
                {
                    WallJump();
                    checkJump = false;
                }
            }
            else if(!checkJump && IsWall() && !canJump)
            {
                anim.SetBool("WallGrab", false);
                wallGrab = false;
                jumpW =false;
            }
            if (!IsWall() && canJump)
            {
                anim.SetBool("WallGrab", false);
                wallGrab = false;
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

        if (!PauseControl.gameIsPaused && !WinController.winLevel)
        {
            if (checkJump)
            {
                if (canJump) //Verifica que pueda saltar
                {
                    
                    Jump(Vector2.up, false);
                    
                }
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
            anim.SetBool("OnGround", true);
            jumpParticle.Play();
            soundPlayer.PlayOneShot(soundLand, 0.5f);
        }
        if (collider.CompareTag("PlatformMob"))
        {
            transform.parent = collider.transform;
            checkJump = false;
            canJump = true;
            anim.SetBool("OnGround", true);
            jumpParticle.Play();
            soundPlayer.PlayOneShot(soundLand, 0.5f);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        //VERFICAR SI EL JUGADOR DEJA DE TOCAR EL SUELO O UNA PLATAFORMA
        if (collider.CompareTag("Floor"))
        {
            canJump = false;
            anim.SetBool("OnGround", false);
        }
        if (collider.CompareTag("PlatformMob"))
        {
            transform.parent = null;
            canJump = false;
            anim.SetBool("OnGround", false);
        }
    }

    //Controles de las flechas PD: Hecho por miguel
    public void ClickDownR() //Cuando se presiona la flecha de Right
    {
        movement = new Vector3(1f, 0f, 0f);
        x = 0.1f;
        right = true;
        dirRay = Vector2.right;
        senseRay = 1f;
        dirDash = 1f;
    }

    public void ClickUpR() //Cuando se suelta la flecha de Right
    {
        x = 0f;
        movement.x = 0;
        dirDash = 0f;
        right = false;
    }

    public void ClickDownL() //Cuando se presiona la flecha de Left
    {
        movement = new Vector3(-1f, 0f, 0f);
        x = -0.1f;
        left = true;
        dirRay = Vector2.left;
        senseRay = -1f;
        dirDash = -1f;
    }

    public void ClickUpL() //Cuando se suelta la flecha de Left
    {
        x = 0f;
        movement.x = 0;
        dirDash = 0f;
        left = false;
    }

    public void Move(Vector2 dir) 
    {
        if(!canMove)
        {
            return;
        }
        if(!jumpW)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        }
        else
        {
            //Debug.Log("Entro");
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
            //Debug.Log(rb.velocity);
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
    }

    public void Jump(Vector2 dir, bool wall)
    {
        anim.SetTrigger("Jump");
        ParticleSystem particle = wall ? jumpWallParticle : jumpParticle;
        //SALTO UTILIZANDO EL RIGIDBODY
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpPower;
        particle.Play();
        soundPlayer.PlayOneShot(soundJump, 0.5f);
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    private void WallParticle()
    {
        var main = slideParticle.main;
        if(wallGrab)
        {
            
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }
    private int ParticleSide ()
    {
        int particleSide = senseRay == 1 ? 1 : -1;
        return particleSide;
    }

    private void WallJump()
    {
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(0.1f));

        Vector2 wallDir = senseRay == 1 ? Vector2.left/2 : Vector2.right/2;
        Jump(Vector2.up / 0.5f + wallDir / 1.5f, true);
        jumpW = true;
    }

    private void WallSlide()
    {
        if (!canMove)
        {
            return;
        }
        wallGrab = true;
        bool pushingWall = false;
        if ((rb.velocity.x > 0 && senseRay == 1 && IsWall()) || (rb.velocity.x < 0 && senseRay == -1 && IsWall()))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;
        rb.velocity = new Vector2(push, -slideSpeed);

    }

    private bool IsWall()
    {
        if (Physics2D.Raycast(this.transform.position + new Vector3(0.2f * senseRay, -0.1f, 0f), dirRay, 0.2f, isGround))
        {
            anim.SetBool("IsOnWall", true);
            return true;
        }
        else
        {
            wallGrab = false;
            anim.SetBool("WallGrab", false);
            anim.SetBool("IsOnWall", false);
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
