using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyIA : MonoBehaviour
{
    //Referencia a los puntos en el mapa
    [SerializeField] List<Transform> points;
    //El valor int para el siguiente punto en el indice
    [SerializeField] private int nextID;
    //El valor que se le aplica al ID para el cambio
    int idChangeValue = 1;
    //La velocidad a la que se va mover el enemigo
    [SerializeField] private float speed = 2;
    [SerializeField] private float stunTime = 3;
    private float currentTime = 0;
    private bool itsStuned = false;

    private void Reset()
    {
        Init();
    }


    void Init()
    {
        //Hace que el collider sea un trigger
        GetComponent<BoxCollider2D>().isTrigger = true;

        //Crea un objeto raiz 
        GameObject root = new GameObject(name + "Raiz");

        //Reinicia la poscion del objeto raiz a la del objeto enemigo
        root.transform.position = transform.position;

        //Coloca el objeto enemigo como un hijo del objeto raiz
        transform.SetParent(root.transform);

        //Crea los objetos puntos de guia
        GameObject waypoints = new GameObject("Waypoints");

        //Reinicia los puntos de guia a la posicion del objeto raiz


        //Hace los puntos guia hijos del objeto raiz
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;

        //Crea dos puntos (GameObjects) y reinicia sus posisicones a los puntos de guia 
        GameObject p1 = new GameObject("Point1");
        p1.transform.SetParent(waypoints.transform);
        p1.transform.position = root.transform.position;

        GameObject p2 = new GameObject("Point2");
        p2.transform.SetParent(waypoints.transform);
        p2.transform.position = root.transform.position;

        //Inicia la lista de puntos y luego los agrega a ella
        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }

    private void Update()
    {
        MoveToNextPoint();

        if (itsStuned)
        {
            currentTime += Time.deltaTime;

            if (currentTime > stunTime)
            {
                itsStuned = false;
                currentTime = 0;
            }
        }

    }

    void MoveToNextPoint()
    {
        //Obtiene el siguiente punto del transform
        Transform goalPoint = points[nextID];

        //Voltea el transform del enemigo para que mire hacia el punto objetivo
        if (goalPoint.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(0.6f, 0.6f, 1);
        else
            transform.localScale = new Vector3(-0.6f, 0.6f, 1);

        //Mueve el enemigo hacia el punto objetivo
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);

        //Verifica la distancia entre el enemigo y el punto objetivo para activar el siguiente punto objetivo
        if (Vector2.Distance(transform.position, goalPoint.position) < 0.5f)
        {
            //Verifica si estamos al final de la linea (hace el cambio a -1)
            if (nextID == points.Count - 1)
                idChangeValue = -1;

            //Verifica si estamos al comienzo de la linea (hace el cambio a 1)
            if (nextID == 0)
                idChangeValue = 1;

            //Se aplica el cambio en nextID
            nextID += idChangeValue;
        }
    }

    public void BeStun()
    {
        if (!itsStuned)
        {
            speed = 0;
            itsStuned = true;
        }
    }
}
