using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpeedChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Time.timeScale = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            Time.timeScale = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            Time.timeScale = 3;
        }
    }
}
