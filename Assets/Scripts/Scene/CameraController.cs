using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // Allows attaching our player to the camera
    public float offset; // Allows offsetting the position of the camera depending on player direction
    public float offsetSmoothing; // Allows smoothing of offset
    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If we want the camera to follow the player up and down (like jumping), add player.transform.position.y
        playerPosition = new Vector3(Mathf.Clamp(player.transform.position.x, -35.0f, 28.0f),
            player.transform.position.y + 3.0f, transform.position.z);

        if (player.transform.localScale.x > 0f)
        {
            playerPosition = new Vector3(Mathf.Clamp(playerPosition.x + offset, -35.0f, 28.0f),
                playerPosition.y, playerPosition.z);
        }
        else
        {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, playerPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }
}