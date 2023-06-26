using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField] float speed = 20;
    [SerializeField] Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyIA enemy = hitInfo.GetComponent<EnemyIA>();
        if (enemy != null)
        {
            enemy.BeStun();
        }

        TrapController trapController = hitInfo.GetComponent<TrapController>();
        if (trapController != null)
        {
            trapController.BeStun();
        }

        SpinObject spinObject = hitInfo.GetComponentInChildren<SpinObject>();
        if (spinObject != null)
        {
            spinObject.BeStun();
        }

        PlatformMob platformMob = hitInfo.GetComponent<PlatformMob>();
        if (platformMob != null)
        {
            platformMob.BeStun();
        }

        Destroy(gameObject);
    }
}
