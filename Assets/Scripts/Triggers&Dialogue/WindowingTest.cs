using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEngine.UI;

public class WindowingTest : MonoBehaviour
{
    void Awake()
    {

        Screen.SetResolution(640, 480, false, 60);
    }

    //public Text text;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("RW " + Display.main.renderingWidth);
        //Debug.Log("RH " + Display.main.renderingHeight);

        //Debug.Log("SW " + Display.main.systemWidth);
        //Debug.Log("SH " + Display.main.systemHeight);

        //Debug.Log("WW " + Screen.width);
        //Debug.Log("WH " + Screen.height);


    }

    // Update is called once per frame
    void LateUpdate()
    {

        //text.text = "RW " + Display.main.renderingWidth + "\nRH" + Display.main.renderingHeight + "\nSW" + Display.main.systemWidth + "\nSH" + Display.main.systemHeight + "\nWW" + Screen.width + "\nWH" + Screen.height;


        //if (Input.GetKey("p"))
        //{
        //    Screen.SetResolution(Screen.width + 100, Screen.height, false, 60);
        //}

        //if (Input.GetKey("["))
        //{
        //    Screen.SetResolution(640, 480, true, 60);
        //}

        //Screen.SetResolution(Screen.width, 480, false, 60);
    }

}
