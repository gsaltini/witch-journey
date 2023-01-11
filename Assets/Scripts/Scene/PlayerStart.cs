using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStart : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private int leftRoom;  
    [SerializeField] private int RightRoom;
    [SerializeField] private int BottomRoom;
    [SerializeField] private int TopRoom;
    [SerializeField] private int ExtraRoom;
    [SerializeField] private Vector3 LeftPosition;
    [SerializeField] private Vector3 RightPosition;
    [SerializeField] private Vector3 BottomPosition;
    [SerializeField] private Vector3 TopPosition;
    [SerializeField] private Vector3 ExtraPosition;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("prevScene"));
        int lastScene = PlayerPrefs.GetInt("prevScene");

        // Determine where player starts based on last scene
        if (lastScene == leftRoom)
        {
            player.transform.position = LeftPosition;
        }
        else if (lastScene == RightRoom)
        {
            player.transform.position = RightPosition;
        }
        else if (lastScene == BottomRoom)
        {
            player.transform.position = BottomPosition;
        }
        else if (lastScene == TopRoom)
        {
            player.transform.position = TopPosition;
        }
        else if (lastScene == ExtraRoom)
        {
            player.transform.position = ExtraPosition;
        }
    }


}
