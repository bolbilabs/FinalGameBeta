using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NameTest : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(System.Environment.UserName);
        Debug.Log(System.Environment.UserDomainName);
        Debug.Log(System.Environment.MachineName);

        text.text = "UserName: " + System.Environment.UserName + "\nUserDomainName: " + System.Environment.UserDomainName
            + "\nMachineName: " + System.Environment.MachineName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
