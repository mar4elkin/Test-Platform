using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPointDirection : MonoBehaviour
{
    // Класс для изменения позиции "пулевого отверстия"
    void Update()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        else if (hDirection > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

