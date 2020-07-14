using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSystemScripts : MonoBehaviour
{
    public Transform[] points;//массив
    public Transform obj;
    public float speed;
    public bool cycle;

    private Transform targetPoint;//точки куда мы стремимся
    private int сurrentPoint; //текущий индекс массива точки
    private bool forward;

    void Start()
    {
        forward = true;
        сurrentPoint = 0;
        targetPoint = points[сurrentPoint];
    }

    
    void Update()
    {
        if (obj.position == targetPoint.position)
        {
            if(forward)
                сurrentPoint++;
            else
                сurrentPoint--;

            if (сurrentPoint >= points.Length && cycle)
                сurrentPoint = 0;
            else if(сurrentPoint >= points.Length && !cycle)
            {
                forward = false;
                сurrentPoint = points.Length - 2;
            }
            else if(сurrentPoint < 0)
            {
                forward = true;
                сurrentPoint = 1;
            }

            targetPoint = points[сurrentPoint];
        }

        obj.position = Vector3.MoveTowards(obj.position, targetPoint.position, speed * Time.deltaTime);
    }
}
