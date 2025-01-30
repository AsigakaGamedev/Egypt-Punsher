using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseEditorOnKey : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Break();
        }       
    }
}
