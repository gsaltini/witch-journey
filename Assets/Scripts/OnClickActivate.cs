using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnClickActivate : MonoBehaviour
{
    //UI Elements
    [SerializeField] private GameObject menuArea;
    [SerializeField] private GameObject otherButton;
    [SerializeField] private bool activeOnOpen;
    [SerializeField] private GameObject active;
    [SerializeField] private GameObject inactive;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        if (!activeOnOpen)
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        menuArea.SetActive(true);
        inactive.SetActive(false);
        active.SetActive(true);
        text.color = Color.black;

        otherButton.GetComponent<OnClickActivate>().Deactivate();
    }

    public void Deactivate()
    {
        menuArea.SetActive(false);
        inactive.SetActive(true);
        active.SetActive(false);
        text.color = Color.white;
    }
}
