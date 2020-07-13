using UnityEngine;

public class CamMove : MonoBehaviour
{
    public GameObject player;

    //Commit
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -100f);
    }
}
