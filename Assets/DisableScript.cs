using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableScript : MonoBehaviour
{
    public MonoBehaviour thisScript;

    // Start is called before the first frame update
    void Start()
    {
        thisScript.enabled = false; 
    }

}
