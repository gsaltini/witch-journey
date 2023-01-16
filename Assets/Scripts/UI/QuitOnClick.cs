using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour {

    public void Quit()
    {
#if UNITY_EDITOR
        //GameObject.FindObjectOfType<LifetimeTotals>().ResetData();
        UnityEditor.EditorApplication.isPlaying = false;

#else
        //GameObject.FindObjectOfType<LifetimeTotals>().ResetData();
        Application.Quit ();
        
#endif
    }
}
