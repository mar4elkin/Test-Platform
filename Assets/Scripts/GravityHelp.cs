using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GravityHelp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnityEngine.Debug.Log("kek");
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerControl>().enabled = false;
        }
    }
}
