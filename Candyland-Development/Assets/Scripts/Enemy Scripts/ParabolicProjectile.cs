using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicProjectile : MonoBehaviour
{
    public Transform target;

    [SerializeField] float firingAngle = 45.0f;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] Transform projectile;

    private Transform myTransform;
    Vector2 projectileVelocityVector;

    //Sistemas de Particulas
    ParticleSystem nutSystem, explodeSystem;

    //Variables Para Simular la Fisica:
    float targetDistance;
    float projectileVelocity;
    float Vx, Vy;
    float flightDuration;
    float flightTime;
    float elapseTime = 0;

    void Awake()
    {
        myTransform = transform;

        nutSystem = GetComponent<ParticleSystem>();
        explodeSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        StartCoroutine(SimulateProjectile());

        var starExplode = explodeSystem.main.startDelay;
        starExplode = flightDuration;
    }

    IEnumerator SimulateProjectile()
    {
        // Desplazamiento del Proyectil
        projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

        // Calcula la distancia hacia el objetivo
        targetDistance = Vector3.Distance(projectile.position, target.position);

        // Calcula la velocidad necesaria para llegar al objetivo en un determinado angulo
        projectileVelocity = targetDistance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Separa los componentes X y Y de la Velocidad
        Vx = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        Vy = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Tiempo de Vuelo
        flightDuration = targetDistance / Vx;

        // Rota el proyectil para que mire al objetivo
        projectile.rotation = Quaternion.LookRotation(target.position - projectile.position);

        elapseTime = 0;

        while (elapseTime < flightDuration)
        {
            projectile.Translate(0, (Vy - (gravity * elapseTime)) * Time.deltaTime, Vx * Time.deltaTime);

            elapseTime += Time.deltaTime;

            yield return null;
        }
    }

    void FixedUpdate()
    {
        flightTime += Time.deltaTime;

        if (flightTime >= flightDuration - 0.4)
        {
            var nutEmission = nutSystem.emission;
            nutEmission.enabled = false;

            Destroy(gameObject, 3f);
        }
    }
}
