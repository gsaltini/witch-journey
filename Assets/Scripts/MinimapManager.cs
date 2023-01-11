using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinimapManager : MonoBehaviour
{
    static MinimapManager instance;
    [SerializeField] public List<GameObject> areas;
    [SerializeField] private List<int> visitedAreas;

    private void Awake()
    {
        //Create singleton so progress will be preserved across scenes
        SetUpSingleton();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //get current scene number
        int sceneNum = scene.buildIndex;
        //add to visitedAreas if not present
        if (!visitedAreas.Contains(sceneNum))
        {
            visitedAreas.Add(sceneNum);
        }

        //re-attach areas
        areas[0] = GameObject.Find("Graveyard");
        areas[1] = GameObject.Find("Well");
        areas[2] = GameObject.Find("Underground (1)");
        areas[3] = GameObject.Find("Underground (2)");
        areas[4] = GameObject.Find("Underground (3)");
        areas[5] = GameObject.Find("Underground (4)");
        areas[6] = GameObject.Find("Underground (5)");
        areas[7] = GameObject.Find("Underground (6)");
        areas[8] = GameObject.Find("Underground (7)");
        areas[9] = GameObject.Find("Underground (8)");
        areas[10] = GameObject.Find("Underground Shrine");
        areas[11] = GameObject.Find("Underground Boss");
        areas[12] = GameObject.Find("Dead Forest (1)");
        areas[13] = GameObject.Find("Dead Forest (2)");
        areas[14] = GameObject.Find("Dead Forest (3)");
        areas[15] = GameObject.Find("Dead Forest (4)");
        areas[16] = GameObject.Find("Dead Forest Shrine");
        areas[17] = GameObject.Find("Dead Forest Boss");

        UnHideVisitedScenes();
    }

    private void SetUpSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void UnHideVisitedScenes()
    {
        for (int i = 0; i < areas.Count; i++)
        {
            if (visitedAreas.Contains(i+2))
            {
                areas[i].SetActive(false);
            }
        }
    }
}
