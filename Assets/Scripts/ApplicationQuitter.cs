using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuitter : MonoBehaviour
{
    void Update()
    {
        QuitApplication();
    }

    void QuitApplication()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
