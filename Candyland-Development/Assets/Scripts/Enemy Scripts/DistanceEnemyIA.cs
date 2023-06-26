using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceEnemyIA : MonoBehaviour
{
    bool playerNearvy;
    bool playerDetected;

    Transform playerPosition;

    Animator squirelAnim;

    [SerializeField] GameObject nutProjectile;
    [SerializeField] float fireRate;
    [SerializeField] float visionRadius;
    float time;
    GameObject projectile;

    void Awake()
    {
        squirelAnim = GetComponent<Animator>();
    }

    void Start()
    {
        playerNearvy = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerNearvy = true;
            Debug.Log("Jugador Cerca");
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerPosition = collider.transform;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerNearvy = false;
            playerDetected = false;
            Debug.Log("Jugador Escapo");
        }
    }

    void FixedUpdate()
    {
        if (playerNearvy)
        {
            RaycastHit2D hit = Physics2D.Raycast
                (transform.position,
                playerPosition.position - transform.position,
                visionRadius,
                1 << LayerMask.NameToLayer("Default"));

            Vector3 forward = transform.TransformDirection(playerPosition.position - transform.position);
            Debug.DrawRay(transform.position, forward, Color.red);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    playerDetected = true;
                    Debug.Log("Jugador Visto");
                }
                else
                {
                    playerDetected = false;
                    Debug.Log("Jugador Escondido");
                }
            }
        }
    }

    void Update()
    {
        if (playerDetected)
        {
            time += Time.deltaTime;

            if (time >= fireRate)
            {
                squirelAnim.SetBool("LaunchNut", true);
                time = 0;
            }
        }
    }

    public void ShootNut()
    {
        projectile = Instantiate(nutProjectile, transform.position, transform.rotation);
        projectile.gameObject.GetComponent<ParabolicProjectile>().target = playerPosition;
        squirelAnim.SetBool("LaunchNut", false);
    }
}
