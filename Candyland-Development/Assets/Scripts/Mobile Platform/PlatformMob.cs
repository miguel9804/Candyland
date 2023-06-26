using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMob : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float speed, delay;

    private Vector3 start, end;

    private float temp = 0f;

    private bool wait= true;

    bool itsStun;
    float stunTime = 3, actualTime;

    // Start is called before the first frame update
    void Start()
    {
        if(target!=null)
        {
            target.parent = null;
            start = transform.position;
            end = target.position;
        }
    }
    private void Update()
    {
        if (itsStun)
        {
            actualTime += Time.deltaTime;

            if (stunTime < actualTime)
            {
                actualTime = 0;
                itsStun = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (!itsStun)
        {
            if (wait)
            {
                temp += 1f * Time.fixedDeltaTime;
                Debug.Log(temp);
            }


            if (temp > delay)
            {
                wait = false;
                if (target != null)
                {
                    float fixedSpeed = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, target.position, fixedSpeed);
                }
            }


            if (transform.position == target.position)
            {
                temp = 0f;
                wait = true;
                target.position = (target.position == start) ? end : start;
            }
        }
    }

    public void BeStun()
    {
        itsStun = true;
    }
}
