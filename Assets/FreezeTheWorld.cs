using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTheWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameControl.FreezeOverworld();
    }
}
