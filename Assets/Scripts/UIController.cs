using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        map.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            /*
            if (!pauseMenu.activeSelf)
            {
                InventoryManager.Instance.cleanInventory();
            }*/
            InventoryManager.Instance.ListItem();

            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);

            //togglePause();

        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            map.SetActive(!map.activeInHierarchy);
        }
    }

    public bool togglePause()
    {

        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }
}
