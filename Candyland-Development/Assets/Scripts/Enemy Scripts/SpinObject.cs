using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] float angSpeed; //Velocidad Angular
    bool itsStun;
    float stunTime = 3, actualTime;

    void Update()
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

        if (!itsStun)
        {
            transform.Rotate(Vector3.forward * angSpeed * Time.deltaTime);
        }
    }

    public void BeStun()
    {
        itsStun = true;
    }
}
