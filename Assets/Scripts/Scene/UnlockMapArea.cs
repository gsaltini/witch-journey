using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockMapArea : MonoBehaviour
{
    public GameObject displayed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        displayed.SetActive(false);
    }
}
