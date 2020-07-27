using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{

    private void OnTriggerEnter2D (Collider2D collision)
    {
        //If player fall under the scene and touched spezial "bottom object", reload the game
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reload the game
        }
    }
}
