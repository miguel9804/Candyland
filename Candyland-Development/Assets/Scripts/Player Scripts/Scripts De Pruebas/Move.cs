using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    bool  left, right, reflected, jump; //Falso = Mirando a la Izquierda, Verdadero = Mirando a la Derecha. Flip del Sprite del Jugador.

    [SerializeField] float speed; //Velocidad del Jugador

    public bool canJump;
    Rigidbody2D rb;
    bool jumpW;

    [SerializeField] float jumpPower, jumpWPower, maxspeed; // jumpWPower = Podre de Salto en la pared

    [SerializeField] LayerMask is_ground; // Verificar el layer en el que se encuentra el personaje, con el raycast


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpW = false;
        reflected = true;
    }

    void Update()
    {
        Debug.Log(canJump);
        if (jump && canJump)
        {
            Jump(Vector2.up);
            jump = false;
        }

        if (Input.GetButtonDown("Jump") && IsWallRight() && !canJump || Input.GetButtonDown("Jump") && IsWallLeft() && !canJump)
        {
            jumpW = true;
        }
        else
        {
            jumpW = false;
        }
        
    }

    private void FixedUpdate()
    {
       
        Moven();
        JumpWall();
    }

    public void Moven()
    {
        if(right)
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
            if (reflected)
            {
                transform.localScale = new Vector3(1f, 1f, 0);
                reflected = false;
            }
 
        }
        if(left)
        {
            transform.position += Vector3.right * Time.deltaTime * -speed;
            if (!reflected)
            {
                transform.localScale = new Vector3(-1f, 1f, 0);
                reflected = true;
            }
        }
        float limitespeed = Mathf.Clamp(rb.velocity.x, -maxspeed, maxspeed);
        rb.velocity = new Vector2(limitespeed, rb.velocity.y);

    }
    
    public void ClickDownR()
    {
        right = true;
    }

    public void ClickUpR()
    {
        right = false;
    }

    public void ClickDownL()
    {
        left = true;
    }

    public void ClickUpL()
    {
        left = false;
    }


    public void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpPower;
    }

    public void ClickDownJump()
    {
        jump = true;
    }
    public void ClickUpJump()
    {
       jump = false;
    }


    private void JumpWall()
    {
        if (jumpW)
        {
            rb.AddForce(Vector2.up * jumpWPower, ForceMode2D.Impulse);
            float limitejump = Mathf.Clamp(rb.velocity.y, -0f, 12f);
            //rb.AddForce(-Vector2.right * 100f * 1f);
            rb.velocity = new Vector2(rb.velocity.x, limitejump);
        }
        jumpW = false;

    }

    private bool IsWallRight()
    {
        if (reflected)
        {
            if (Physics2D.Raycast(this.transform.position + new Vector3(0.2f, -0.1f, 0f), Vector2.right, 0.3f, is_ground))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;

    }

    private bool IsWallLeft()
    {
        if (!reflected)
        {
            if (Physics2D.Raycast(this.transform.position + new Vector3(-0.2f, -0.1f, 0f), -Vector2.right, 0.3f, is_ground))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;

    }
}
