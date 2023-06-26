using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    //SCRIPT ADAPTABLE PARA SER REUTILIZADO SI SE NECESITAN MÁS PROYECTILES
    [SerializeField] float speed;
    [SerializeField] Vector2 direction;

    void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Floor") | collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
